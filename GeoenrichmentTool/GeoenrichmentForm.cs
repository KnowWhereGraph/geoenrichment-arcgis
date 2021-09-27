﻿using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KWG_Geoenrichment
{
    public partial class GeoenrichmentForm : Form
    {
        //Array that maps the formatted feature type label to the respective URI
        protected Dictionary<string, string> featureTypeArray;
        protected List<string> commonPropertyUrls;
        protected List<string> sosaObsPropertyUrls;
        protected List<string> inversePropertyUrls;

        public GeoenrichmentForm()
        {
            InitializeComponent();

            KwgGeoModule.Current.activeGeoenrichmentForm = this;
            knowledgeGraph.Text = KwgGeoModule.Current.GetQueryClass().GetActiveEndPoint();
            PopulateFeatureTypes();
        }

        private void PopulateFeatureTypes()
        {
            var entityTypeQuery = "select distinct ?entityType ?entityTypeLabel where { ?entity rdf:type ?entityType . ?entity geo:hasGeometry ?aGeom . ?entityType rdfs:label ?entityTypeLabel }";
            QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();

            JToken entityTypeJson = queryClass.SubmitQuery(entityTypeQuery);

            featureTypeArray = new Dictionary<string, string>();
            List<object> termsList = new List<object>();

            foreach (var jsonResult in entityTypeJson)
            {
                string fURI = jsonResult["entityType"]["value"].ToString();
                string fType = jsonResult["entityTypeLabel"]["value"].ToString();
                string featureTypeFormatted = fType + " (" + queryClass.MakeIRIPrefix(fURI) + ")";
                featureTypeArray[featureTypeFormatted] = fURI;
                termsList.Add(featureTypeFormatted);
            }

            featureType.Items.AddRange(termsList.ToArray());
        }

        private void RefreshFeatureTypes(object sender, EventArgs e)
        {
            featureType.Items.Clear();
            featureType.ResetText();
            KwgGeoModule.Current.GetQueryClass().UpdateActiveEndPoint(knowledgeGraph.Text);

            try
            {
                PopulateFeatureTypes();
                MessageBox.Show($@"Feature types updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Failed to connect to endpoint!");
            }
        }

        private void getPropertiesForFeature(object sender, EventArgs e)
        {
            commonPropertyUrls = new List<string>() { };
            sosaObsPropertyUrls = new List<string>() { };
            inversePropertyUrls = new List<string>() { };

            string selectedType = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(featureTypeArray[featureType.Text]);
            PropertyEnrichment propEnrichClass = new PropertyEnrichment(selectedType);

            List<string>[] commonProperties = propEnrichClass.CommonProperties;
            List<string>[] sosaObsProperties = propEnrichClass.SosaObsProperties;
            List<string>[] inverseProperties = propEnrichClass.InverseProperties;

            for (var i = 0; i < commonProperties[0].Count(); i++)
            {
                string url = commonProperties[0][i];
                string name = commonProperties[1][i];

                commonPropertiesBox.Rows.Add(false, name, null, url);
                //We want to keep track of the fact its a common value
                commonPropertyUrls.Add(url);
            }

            for (var i = 0; i < sosaObsProperties[0].Count(); i++)
            {
                string url = sosaObsProperties[0][i];
                string name = sosaObsProperties[1][i];

                //We want to add the value to main user selection list for common properties
                commonPropertiesBox.Rows.Add(false, name, null, url);
                //But we also want to keep track of the fact its a sosa observation value
                sosaObsPropertyUrls.Add(url);
            }

            for (var i = 0; i < inverseProperties[0].Count(); i++)
            {
                string url = inverseProperties[0][i];
                string name = inverseProperties[1][i];

                //We want to add the value to main user selection list for common properties
                commonPropertiesBox.Rows.Add(false, name, null, url);
                //But we also want to keep track of the fact its an inverse value
                inversePropertyUrls.Add(url);
            }
        }

        private bool HasFormError()
        {
            if (knowledgeGraph.Text == "" | spatialRelation.Text == "" | saveLayerAs.Text == "")
            {
                MessageBox.Show("Required fields missing!");
                return true;
            }
            else
            {
                //validation for property rows
                DataGridViewRowCollection propertyRows = commonPropertiesBox.Rows;
                foreach (DataGridViewRow row in propertyRows)
                {
                    DataGridViewCellCollection rowCells = row.Cells;
                    var useProperty = rowCells[0].Value.ToString();

                    if (useProperty == "True")
                    {
                        if (rowCells[2].Value == null)
                        {
                            MessageBox.Show(rowCells[1].Value.ToString() + " is missing a merge rule!");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void DrawAreaOfInterest(object sender, EventArgs e)
        {
            if(!HasFormError())
            {
                Close();
                FrameworkApplication.SetCurrentToolAsync("KWG_Geoenrichment_DrawPolygon");
            }
        }

        public void SubmitGeoenrichmentForm(string polygonString)
        {
            //We need to update our global query module to point at the specified knowledge graph endpoint
            QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();
            queryClass.UpdateActiveEndPoint(knowledgeGraph.Text);

            string featureTypeURI = (featureType.Text != "") ? featureTypeArray[featureType.Text] : "";

            string[] geoFunctions = new string[] { };
            switch (spatialRelation.Text)
            {
                case "Contain or Intersect":
                    geoFunctions = new string[] { "geo:sfContains", "geo:sfIntersects" };
                    break;
                case "Contain":
                    geoFunctions = new string[] { "geo:sfContains" };
                    break;
                case "Within":
                    geoFunctions = new string[] { "geo:sfWithin" };
                    break;
                case "Intersect":
                default:
                    geoFunctions = new string[] { "geo:sfIntersects" };
                    break;
            }

            //Build proper WKT value
            string polygonWKT = "'''<http://www.opengis.net/def/crs/OGC/1.3/CRS84> " + polygonString + " '''";

            var geoQueryResult = TypeAndGeoSPARQLQuery(polygonWKT, featureTypeURI, ignoreSubclasses.Checked, geoFunctions);

            string layerName = saveLayerAs.Text.Replace(" ", "_");
            FeatureClassHelper.CreateClassFromSPARQL(geoQueryResult, layerName, featureTypeURI, ignoreSubclasses.Checked);

            //TODO:: Integrate merge code
            DataGridViewRowCollection propertyRows = commonPropertiesBox.Rows;
            List<List<string>> propertiesToMerge = new List<List<string>>() { };
            foreach (DataGridViewRow row in propertyRows)
            {
                DataGridViewCellCollection rowCells = row.Cells;
                var useProperty = rowCells[0].Value.ToString();

                if (useProperty == "True")
                {
                    propertiesToMerge.Add(new List<string>() { rowCells[1].Value.ToString(), rowCells[3].Value.ToString(), rowCells[2].Value.ToString() });
                }
            }

            EnrichData(propertiesToMerge);

            //TODO::Enable the property enrichment tool since we have a layer for it to use
            FrameworkApplication.State.Activate("kwg_query_layer_added");
        }

        /**
         * Format GeoSPARQL query
         * 
         * polygonWKT: the wkt literal for the user polygon
         * featureTypeUri: the user spercified feature type
         * ignoreSubclasses: ignore use of subclasses for feature type
         * geoFunctions: a list of geosparql functions
         **/
        private JToken TypeAndGeoSPARQLQuery(string polygonWKT, string featureTypeURI, bool ignoreSubclasses, string[] geoFunctions)
        {
            string query = "select distinct ?place ?placeLabel ?placeFlatType ?wkt " +
                "where" +
                "{" +
                "?place geo:hasGeometry ?geometry . " +
                "?place rdfs:label ?placeLabel . " +
                "?geometry geo:asWKT ?wkt . " +
                "?place rdf:type ?placeFlatType ." +
                "{ " + polygonWKT + "^^geo:wktLiteral " +
                geoFunctions[0] + "  ?geometry .}";

            if (geoFunctions.Length == 2)
            {
                query += " union" +
                    "{ " + polygonWKT + "^^geo:wktLiteral " +
                    geoFunctions[1] + "   ?geometry . }";
            }

            if (featureTypeURI != "")
            {
                if (!ignoreSubclasses)
                {
                    query += "?place rdf:type ?placeFlatType ." +
                    "?placeFlatType rdfs:subClassOf* <" + featureTypeURI + ">.";
                }
                else
                {
                    query += "?place rdf:type  <" + featureTypeURI + ">.";
                }

            }

            query += "}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(query);
        }

        private async void EnrichData(List<List<string>> properties)
        {
            Close();

            BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();

            foreach(var property in properties)
            {
                if (commonPropertyUrls.Contains(property[1]))
                {
                    List<string> commonURIs = new List<string>() { };
                    List<string> sosaObsURIs = new List<string>() { };
                    char[] delimiterChars = { '|' };

                    //Now that we have the selected common property values, separate the sosa observation values so that we can process them separately
                    //While were at it, get rid of the name and isolate to just the URIs
                    foreach (var checkedItem in commonCheckBox.CheckedItems)
                    {
                        string checkedVal = checkedItem.ToString();
                        string checkedURI = checkedVal.Split(delimiterChars)[1].Trim();
                        if (soList.Contains(checkedVal))
                        {
                            sosaObsURIs.Add(checkedURI);
                        }
                        else
                        {
                            commonURIs.Add(checkedURI);
                        }
                    }

                    //Determine which selected URIs are functional and create a function and non-functional list
                    JToken functionPropsResult = FunctionalPropertyQuery(commonURIs);
                    List<string> functionProps = new List<string> { };
                    foreach (var result in functionPropsResult)
                    {
                        functionProps.Add((string)result["property"]["value"]);
                    }
                    var noFunctionProps = commonURIs.Except(functionProps);

                    foreach (var propURI in functionProps)
                    {
                        JToken propertyVal = PropertyValueQuery(propURI, false);
                        await FeatureClassHelper.AddFieldInTableByMapping(propURI, propertyVal, "wikidataSub", "o", "URL", false);
                    }

                    foreach (var propURI in noFunctionProps)
                    {
                        JToken propertyVal = PropertyValueQuery(propURI, false);

                        Table tableResult = null;
                        string tableName = "";
                        await QueuedTask.Run(() =>
                        {
                            tableResult = FeatureClassHelper.CreateMappingTableFromJSON(propURI, propertyVal, "wikidataSub", "o", "URL", false, false).Result;
                            Project.Current.SaveEditsAsync();
                            tableName = tableResult.GetName();
                            string relationshipClassName = tableName + "_RelClass";
                            FeatureClassHelper.CreateRelationshipClass(mainLayer, tableResult, relationshipClassName, propURI, "features from Knowledge Graph").Wait();
                        });
                        //string currentValuePropertyName = tableResult[2];

                        JToken geoCheckJSON = CheckGeoPropertyQuery(propURI, false);

                        if ((int)geoCheckJSON[0]["cnt"]["value"] > 0)
                        {
                            JToken geoQueryResult = TwoDegreePropertyValueWKTquery(propURI, false);

                            string propName = FeatureClassHelper.GetPropertyName(propURI);

                            string outFeatureClassName = mainLayer.Name + "_" + propName;
                            await QueuedTask.Run(() =>
                            {
                                FeatureClassHelper.CreateClassFromSPARQL(geoQueryResult, outFeatureClassName);
                                var geoLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == outFeatureClassName).FirstOrDefault() as BasicFeatureLayer;
                                string outRelationshipClassName = outFeatureClassName + "_" + tableName + "_RelClass";
                                string currentValuePropertyName = tableResult.GetDefinition().GetFields().Last().Name;
                                FeatureClassHelper.CreateRelationshipClass(geoLayer, tableResult, outRelationshipClassName, "is " + propURI + " Of", "features from Knowledge Graph", currentValuePropertyName).Wait();
                            });
                        }
                    }

                    foreach (var propURI in sosaObsURIs)
                    {
                        JToken propertyVal = null;
                        try
                        {
                            propertyVal = SosaObsPropertyValueQuery(propURI);
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Sosa property query error for: " + propURI);
                            continue;
                        }

                        await QueuedTask.Run(() =>
                        {
                            Table tableResult = FeatureClassHelper.CreateMappingTableFromJSON(propURI, propertyVal, "wikidataSub", "o", "URL", false, false).Result;
                            Project.Current.SaveEditsAsync();
                            string sosaRelationshipClassName = tableResult.GetName() + "_RelClass";
                            FeatureClassHelper.CreateRelationshipClass(mainLayer, tableResult, sosaRelationshipClassName, propURI, "features from Knowledge Graph").Wait();
                        });
                    }
                }
                else if (sosaObsPropertyUrls.Contains(property[1]))
                {

                }
                else if (inversePropertyUrls.Contains(property[1]))
                {
                    List<string> inverseURIs = new List<string>() { };
                    char[] delimiterChars = { '|' };

                    //Get rid of the name and isolate to just the URIs
                    foreach (var checkedItem in commonCheckBox.CheckedItems)
                    {
                        string checkedVal = checkedItem.ToString();
                        string checkedURI = checkedVal.Split(delimiterChars)[1].Trim();
                        inverseURIs.Add(checkedURI);
                    }

                    //Determine which selected URIs are functional and create a function and non-functional list
                    JToken functionPropsResult = FunctionalPropertyQuery(inverseURIs, true);
                    List<string> functionProps = new List<string> { };
                    foreach (var result in functionPropsResult)
                    {
                        functionProps.Add((string)result["property"]["value"]);
                    }
                    List<string> noFunctionProps = (List<string>)inverseURIs.Except(functionProps);

                    foreach (var propURI in functionProps)
                    {
                        JToken propertyVal = InversePropertyValueQuery(propURI, false);
                        await FeatureClassHelper.AddFieldInTableByMapping(propURI, propertyVal, "wikidataSub", "o", "URL", true);
                    }

                    foreach (var propURI in noFunctionProps)
                    {
                        JToken propertyVal = InversePropertyValueQuery(propURI, false);

                        await QueuedTask.Run(() =>
                        {
                            Table tableResult = FeatureClassHelper.CreateMappingTableFromJSON(propURI, propertyVal, "wikidataSub", "o", "URL", true, false).Result;
                            Project.Current.SaveEditsAsync();
                            string relationshipClassName = tableResult.GetName() + "_RelClass";
                            FeatureClassHelper.CreateRelationshipClass(mainLayer, tableResult, relationshipClassName, propURI, "features from wikidata").Wait();
                        });
                    }
                }
            }
        }
    }
}

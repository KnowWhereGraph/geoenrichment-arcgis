using ArcGIS.Desktop.Framework.Threading.Tasks;
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

namespace GeoenrichmentTool
{
    public partial class PropertyEnrichment : Form
    {
        private List<string> soList;
        private List<string> uriList;

        public PropertyEnrichment(List<string>[] commonProperties, List<string>[] sosaObsProperties, List<string>[] inverseProperties, List<string> uris)
        {
            InitializeComponent();
            soList = new List<string>() { };

            for (var i=0; i < commonProperties[0].Count(); i++)
            {
                string url = commonProperties[0][i];
                string name = commonProperties[1][i];

                string value = name + " | " + url;

                commonCheckBox.Items.Add(value);
            }

            for (var i = 0; i < sosaObsProperties[0].Count(); i++)
            {
                ///TODO::DEV CODE///
                break;
                ////////////////////
                
                string url = sosaObsProperties[0][i];
                string name = sosaObsProperties[1][i];

                string value = name + " | " + url;

                //We want to add the value to main user selection list for common properties
                commonCheckBox.Items.Add(value);
                //But we also want to keep track of the fact its a sosa observation value
                soList.Add(value);
            }

            for (var i = 0; i < inverseProperties[0].Count(); i++)
            {
                string url = inverseProperties[0][i];
                string name = inverseProperties[1][i];

                string value = name + " | " + url;

                inverseCheckBox.Items.Add(value);
            }

            uriList = uris;
        }

        private void SelectAllProperties(object sender, EventArgs e)
        {
            for (int i = 0; i < commonCheckBox.Items.Count; i++) // loop to set all items checked or unchecked
            {
                commonCheckBox.SetItemChecked(i, true);
            }
        }

        private void InverseSelectAllProperties(object sender, EventArgs e)
        {
            for (int i = 0; i < commonCheckBox.Items.Count; i++) // loop to set all items checked or unchecked
            {
                inverseCheckBox.SetItemChecked(i, true);
            }
        }

        private async void EnrichData(object sender, EventArgs e)
        {
            Close();

            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();

            if (commonCheckBox.CheckedItems.Count > 0)
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
                foreach(var result in functionPropsResult)
                {
                    functionProps.Add((string)result["property"]["value"]);
                }
                var noFunctionProps = commonURIs.Except(functionProps);

                foreach(var propURI in functionProps)
                {
                    JToken propertyVal = PropertyValueQuery(propURI, false);
                    await FeatureClassHelper.AddFieldInTableByMapping(propURI, propertyVal, "wikidataSub", "o", "URL", false);
                }

                foreach (var propURI in noFunctionProps)
                {
                    JToken propertyVal = PropertyValueQuery(propURI, false);

                    string[] tableResult = null;
                    await QueuedTask.Run(() =>
                    {
                        tableResult = FeatureClassHelper.CreateMappingTableFromJSON(propURI, propertyVal, "wikidataSub", "o", "URL", false, false).Result;
                    });
                    string tableName = tableResult[0];
                    string keyPropertyFieldName = tableResult[1];
                    string currentValuePropertyName = tableResult[2];

                    string relationshipClassName = tableName + "_RelClass";

                    await FeatureClassHelper.CreateRelationshipClass(mainLayer.Name, tableName, relationshipClassName, propURI, "features from Knowledge Graph");

                    JToken geoCheckJSON = CheckGeoPropertyQuery(propURI, false);

                    if((int)geoCheckJSON[0]["cnt"]["value"] > 0)
                    {
                        JToken geoQueryResult = TwoDegreePropertyValueWKTquery(propURI, false);

                        string propName = FeatureClassHelper.GetPropertyName(propURI);

                        string outFeatureClassName = mainLayer.Name + "_" + propName;
                        string outFeatureClassPath = "";
                        await QueuedTask.Run(() =>
                        {
                            outFeatureClassPath = mainLayer.GetPath().ToString() + "_" + propName;
                        });

                        FeatureClassHelper.CreateClassFromSPARQL(geoQueryResult, outFeatureClassName);

                        string outRelationshipClassName = outFeatureClassName + "_" + tableName + "_RelClass";

                        await FeatureClassHelper.CreateRelationshipClass(outFeatureClassName, tableName, outRelationshipClassName, "is " + propURI + " Of", "features from Knowledge Graph", currentValuePropertyName);
                    }
                }

                foreach (var propURI in sosaObsURIs)
                {
                    JToken propertyVal = null;
                    try
                    {
                        propertyVal = SosaObsPropertyValueQuery(propURI);
                    } catch(Exception ex)
                    {
                        //MessageBox.Show("Sosa property query error for: " + propURI);
                        continue;
                    }

                    string[] tableResult = null;
                    await QueuedTask.Run(() =>
                    {
                        tableResult = FeatureClassHelper.CreateMappingTableFromJSON(propURI, propertyVal, "wikidataSub", "o", "URL", false, false).Result;
                    });
                    string sosaTableName = tableResult[0];

                    string sosaRelationshipClassName = mainLayer.Name + "_" + sosaTableName + "_RelClass";

                    await FeatureClassHelper.CreateRelationshipClass(mainLayer.Name, sosaTableName, sosaRelationshipClassName, propURI, "features from Knowledge Graph");
                }
            }

            if (inverseCheckBox.CheckedItems.Count > 0)
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

                    string[] tableResult = null;
                    await QueuedTask.Run(() =>
                    {
                        tableResult = FeatureClassHelper.CreateMappingTableFromJSON(propURI, propertyVal, "wikidataSub", "o", "URL", true, false).Result;
                    });
                    string tableName = tableResult[0];
                    string keyPropertyFieldName = tableResult[1];
                    string currentValuePropertyName = tableResult[2];

                    string relationshipClassName = mainLayer.Name + "_" + tableName + "_RelClass";

                    await FeatureClassHelper.CreateRelationshipClass(mainLayer.Name, tableName, relationshipClassName, propURI, "features from wikidata");
                }
            }
        }

        private JToken FunctionalPropertyQuery(List<string> properties, bool inverse=false)
        {
            string owlProp = (inverse) ? "owl:InverseFunctionalProperty": "owl:FunctionalProperty";

            string funcQuery = "select ?property where { ?property a "+ owlProp+". VALUES ?property {";

            foreach (string propURI in properties)
            {
                funcQuery += "<" + propURI + "> \n";
            }

            funcQuery += "}}";

            return GeoModule.Current.GetQueryClass().SubmitQuery(funcQuery);
        }

        private JToken PropertyValueQuery(string property, bool doSameAs = true)
        {
            string propValQuery = "";

            if (doSameAs)
            {
                propValQuery = "select ?wikidataSub ?o where { ?s owl:sameAs ?wikidataSub. ?s <" + property + "> ?o. VALUES ?wikidataSub {";
            }
            else
            {
                propValQuery = "select ?wikidataSub ?o where { ?wikidataSub <" + property + "> ?o. VALUES ?wikidataSub {";
            }

            foreach(var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return GeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }

        private JToken SosaObsPropertyValueQuery(string property)
        {
            string propValQuery = "select ?wikidataSub ?o where { ?wikidataSub sosa:isFeatureOfInterestOf ?obscol . ?obscol sosa:hasMember ?obs. " +
                "?obs sosa:observedProperty <" + property + "> . ?obs sosa:hasSimpleResult ?o. VALUES ?wikidataSub {";

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return GeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }

        private JToken InversePropertyValueQuery(string property, bool doSameAs = true)
        {
            string propValQuery = "";

            if (doSameAs)
            {
                propValQuery = "select ?wikidataSub ?o where { ?s owl:sameAs ?wikidataSub. ?o <" + property + "> ?s. VALUES ?wikidataSub {";
            }
            else
            {
                propValQuery = "select ?wikidataSub ?o where { ?o <" + property + "> ?wikidataSub. VALUES ?wikidataSub {";
            }

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return GeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }

        private JToken CheckGeoPropertyQuery(string propertyURL, bool doSameAs = true)
        {
            string propValQuery = "select (count(?geometry) as ?cnt) where {";

            if(doSameAs)
            {
                propValQuery += " ?s owl:sameAs ?wikidataSub. ?s <" + propertyURL + "> ?place.";
            }
            else
            {
                propValQuery += " ?wikidataSub <" + propertyURL + "> ?place.";
            }

            propValQuery += " ?place geo:hasGeometry ?geometry . VALUES ?wikidataSub {";

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return GeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }

        private JToken TwoDegreePropertyValueWKTquery(string propertyURL, bool doSameAs = true)
        {
            string propValQuery = "select distinct ?place ?placeLabel ?placeFlatType ?wkt where {";

            if (doSameAs)
            {
                propValQuery += " ?s owl:sameAs ?wikidataSub. ?s <" + propertyURL + "> ?place.";
            }
            else
            {
                propValQuery += " ?wikidataSub <" + propertyURL + "> ?place.";
            }

            propValQuery += " ?place geo:hasGeometry ?geometry . ?place rdfs:label ?placeLabel . " +
                "?geometry geo:asWKT ?wkt. ?place rdf:type ?placeFlatType. VALUES ?wikidataSub {";

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return GeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }
    }
}

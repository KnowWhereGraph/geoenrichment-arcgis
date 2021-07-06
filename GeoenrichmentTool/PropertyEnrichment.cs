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

        private async void EnrichData(object sender, EventArgs e)
        {   
            if(commonCheckBox.CheckedItems.Count > 0)
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
                    /*
                    # create a seperate table to store one-to-many property value, return the created table name
                    tableName, keyPropertyFieldName, currentValuePropertyName = Json2Field.createMappingTableFromJSON(noFunctionalPropertyJSON, "wikidataSub", "o", 
                                        noFunctionalProperty, inputFeatureClassName, "URL", False, False)
                    # creat relationship class between the original feature class and the created table
                    
                    relationshipClassName = featureClassName + "_" + tableName + "_RelClass"
                    arcpy.CreateRelationshipClass_management(featureClassName, tableName, relationshipClassName, "SIMPLE",
                        noFunctionalProperty, "features from Knowledge Graph",
                                            "FORWARD", "ONE_TO_MANY", "NONE", "URL", "URL")

                    # check whether the object of propertyURL is geo-entity
                    # if it is create new feature class
                    # for propertyURL in selectPropertyURLList:
                    propertyURL = noFunctionalProperty
                    geoCheckJSON = SPARQLQuery.checkGeoPropertyquery(inplaceIRIList, propertyURL, 
                                                        sparql_endpoint = sparql_endpoint,
                                                        doSameAs = False)
                    geometry_cnt = int(geoCheckJSON[0]["cnt"]["value"])
                    if geometry_cnt > 0:
                        # OK, propertyURL is a property whose value is geo-entities
                        # get their geometries, create a feature layer
                        GeoQueryResult = SPARQLQuery.twoDegreePropertyValueWKTquery(inplaceIRIList, propertyURL, 
                                                        sparql_endpoint = sparql_endpoint,
                                                        doSameAs = False)

                        in_place_IRI_desc = arcpy.Describe(in_place_IRI)

                        arcpy.AddMessage("input feature class: {}".format(in_place_IRI_desc.name))
                        arcpy.AddMessage("input feature class: {}".format(in_place_IRI_desc.path))

                        prop_name = UTIL.getPropertyName(propertyURL)

                        out_geo_feature_class_name = "{}_{}".format(featureClassName, prop_name)
                        out_geo_feature_class_path = os.path.join(in_place_IRI_desc.path, out_geo_feature_class_name)
                        Json2Field.createFeatureClassFromSPARQLResult(GeoQueryResult = GeoQueryResult, 
                                                        out_path = out_geo_feature_class_path)

                        out_relationshipClassName = out_geo_feature_class_name + "_" + tableName + "_RelClass"
                        arcpy.CreateRelationshipClass_management(origin_table = out_geo_feature_class_name, 
                                                                destination_table = tableName, 
                                                                out_relationship_class = out_relationshipClassName, 
                                                                relationship_type = "SIMPLE",
                                                                forward_label = "is " + noFunctionalProperty + " Of", 
                                                                backward_label = "features from Knowledge Graph",
                                                                message_direction = "FORWARD", 
                                                                cardinality = "ONE_TO_MANY", 
                                                                attributed = "NONE", 
                                                                origin_primary_key = "URL", 
                                                                origin_foreign_key = currentValuePropertyName)
                    */
                }

                foreach (var propURI in sosaObsURIs)
                {
                    /*                    
                    # sosa property value query
                    for p_url in selectSosaObsPropURLList:
                        sosaPropValJSON = SPARQLQuery.sosaObsPropertyValueQuery(inplaceIRIList, p_url,
                                                                                    sparql_endpoint = sparql_endpoint,
                                                                                    doSameAs = False)

                        sosaTableName, _, _ = Json2Field.createMappingTableFromJSON(sosaPropValJSON, 
                                                        keyPropertyName = "wikidataSub", 
                                                        valuePropertyName = "o", 
                                                        valuePropertyURL = p_url, 
                                                        inputFeatureClassName = inputFeatureClassName, 
                                                        keyPropertyFieldName = "URL", 
                                                        isInverse = False, 
                                                        isSubDivisionTable = False)

                        sosaRelationshipClassName = featureClassName + "_" + sosaTableName + "_RelClass"
                        arcpy.CreateRelationshipClass_management(featureClassName, sosaTableName, 
                                            sosaRelationshipClassName, "SIMPLE",
                                            p_url, "features from Knowledge Graph",
                                                "FORWARD", "ONE_TO_MANY", "NONE", "URL", "URL")
                     */
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
                    /*
                    # create a seperate table to store one-to-many property-subject pair, return the created table name
                    tableName, _, _ = Json2Field.createMappingTableFromJSON(noFunctionalInversePropertyJSON, "wikidataSub", "o", noFunctionalInverseProperty, inputFeatureClassName, "URL", True, False)
                    # creat relationship class between the original feature class and the created table

                    relationshipClassName = featureClassName + "_" + tableName + "_RelClass"
                    arcpy.AddMessage("featureClassName: {0}".format(featureClassName))
                    arcpy.AddMessage("tableName: {0}".format(tableName))
                    arcpy.CreateRelationshipClass_management(featureClassName, tableName, relationshipClassName, "SIMPLE",
                        noFunctionalInverseProperty, "features from wikidata",
                                            "FORWARD", "ONE_TO_MANY", "NONE", "URL", "URL")
                    */
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
    }
}

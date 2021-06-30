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
        public PropertyEnrichment(List<string>[] commonProperties, List<string>[] sosaObsProperties, List<string>[] inverseProperties)
        {
            InitializeComponent();

            for(var i=0; i < commonProperties[0].Count(); i++)
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

                commonCheckBox.Items.Add(value);
            }

            for (var i = 0; i < inverseProperties[0].Count(); i++)
            {
                string url = inverseProperties[0][i];
                string name = inverseProperties[1][i];

                string value = name + " | " + url;

                inverseCheckBox.Items.Add(value);
            }
        }

        private void EnrichData(object sender, EventArgs e)
        {
            /*
            """The source code of the tool."""
            in_sparql_endpoint = parameters[0]
            in_place_IRI = parameters[1]
            in_com_property = parameters[2]
            in_boolean_inverse_com = parameters[3]
            in_inverse_com_property = parameters[4]
            */
            
            /*
            # get all the IRI from input point feature class of wikidata places
            inplaceIRIList = []
            cursor = arcpy.SearchCursor(inputFeatureClassName)
            for row in cursor:
                inplaceIRIList.append(row.getValue("URL"))

            arcpy.AddMessage(in_com_property.valueAsText)
            propertySelect = in_com_property.valueAsText
            selectPropertyURLList = []
            selectSosaObsPropURLList = []
            arcpy.AddMessage("propertySelect: {0}".format(propertySelect))
            if propertySelect != None:
                propertySplitList = re.split("[;]", propertySelect.replace("'", ""))
                arcpy.AddMessage("propertySplitList: {0}".format(propertySplitList))
                for propertyItem in propertySplitList:
                    if propertyItem in LinkedDataPropertyEnrichGeneral.propertyURLDict:
                        selectPropertyURLList.append(LinkedDataPropertyEnrichGeneral.propertyURLDict[propertyItem])
                    elif propertyItem in LinkedDataPropertyEnrichGeneral.sosaObsPropURLDict:
                        selectSosaObsPropURLList.append(LinkedDataPropertyEnrichGeneral.sosaObsPropURLDict[propertyItem])

                # send a SPARQL query to DBpedia endpoint to test whether the properties are functionalProperty
                isFuncnalPropertyJSON = SPARQLQuery.functionalPropertyQuery(selectPropertyURLList,
                                                                            sparql_endpoint = sparql_endpoint)

                FunctionalPropertySet = set()

                for jsonItem in isFuncnalPropertyJSON:
                    functionalPropertyURL = jsonItem["property"]["value"]
                    FunctionalPropertySet.add(functionalPropertyURL)



                arcpy.AddMessage("FunctionalPropertySet: {0}".format(FunctionalPropertySet))

                # get the value for each functionalProperty
                FuncnalPropertyList = list(FunctionalPropertySet)
                # add these functionalProperty value to feature class table
                for functionalProperty in FuncnalPropertyList:
                    functionalPropertyJSON = SPARQLQuery.propertyValueQuery(inplaceIRIList, functionalProperty,
                                                                            sparql_endpoint = sparql_endpoint,
                                                                            doSameAs = False)

                    Json2Field.addFieldInTableByMapping(functionalPropertyJSON, "wikidataSub", "o", inputFeatureClassName, "URL", functionalProperty, False)
                    
                selectPropertyURLSet = set(selectPropertyURLList)
                noFunctionalPropertySet = selectPropertyURLSet.difference(FunctionalPropertySet)
                noFunctionalPropertyList = list(noFunctionalPropertySet)

                for noFunctionalProperty in noFunctionalPropertyList:
                    noFunctionalPropertyJSON = SPARQLQuery.propertyValueQuery(inplaceIRIList, noFunctionalProperty,
                                                                                sparql_endpoint = sparql_endpoint,
                                                                                doSameAs = False)
                    # noFunctionalPropertyJSON = noFunctionalPropertyJSONObj["results"]["bindings"]
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

            # if the user want the inverse properties
            if isInverse == True:
                inversePropertySelect = in_inverse_com_property.valueAsText
                arcpy.AddMessage("LinkedDataPropertyEnrichGeneral.inversePropertyURLDict: {0}".format(
                    LinkedDataPropertyEnrichGeneral.inversePropertyURLDict))
                arcpy.AddMessage("inversePropertySelect: {0}".format(inversePropertySelect))

                selectInversePropertyURLList = []
                if inversePropertySelect is not None:
                    inversePropertySplitList = re.split("[;]", inversePropertySelect.replace("'", ""))
                    for propertyItem in inversePropertySplitList:

                        selectInversePropertyURLList.append(
                            LinkedDataPropertyEnrichGeneral.inversePropertyURLDict[propertyItem.split("\'")[1]])
                    

                    # send a SPARQL query to DBpedia endpoint to test whether the properties are InverseFunctionalProperty
                    isInverseFuncnalPropertyJSON = SPARQLQuery.inverseFunctionalPropertyQuery(selectInversePropertyURLList,
                                                                                    sparql_endpoint = sparql_endpoint)
                    # isFuncnalPropertyJSON = isFuncnalPropertyJSONObj["results"]["bindings"]

                    inverseFunctionalPropertySet = set()

                    for jsonItem in isInverseFuncnalPropertyJSON:
                        inverseFunctionalPropertyURL = jsonItem["property"]["value"]
                        inverseFunctionalPropertySet.add(inverseFunctionalPropertyURL)

                    arcpy.AddMessage("inverseFunctionalPropertySet: {0}".format(inverseFunctionalPropertySet))

                    # get the value for each functionalProperty
                    inverseFuncnalPropertyList = list(inverseFunctionalPropertySet)
                    # add these inverseFunctionalProperty subject value to feature class table
                    for inverseFunctionalProperty in inverseFuncnalPropertyList:
                        inverseFunctionalPropertyJSON = SPARQLQuery.inversePropertyValueQuery(inplaceIRIList, 
                                                                                                inverseFunctionalProperty,
                                                                                                sparql_endpoint = sparql_endpoint,
                                                                                                doSameAs = False)
                        # functionalPropertyJSON = functionalPropertyJSONObj["results"]["bindings"]

                        Json2Field.addFieldInTableByMapping(inverseFunctionalPropertyJSON, "wikidataSub", "o", inputFeatureClassName, "URL", inverseFunctionalProperty, True)

                    selectInversePropertyURLSet = set(selectInversePropertyURLList)
                    noFunctionalInversePropertySet = selectInversePropertyURLSet.difference(inverseFunctionalPropertySet)
                    noFunctionalInversePropertyList = list(noFunctionalInversePropertySet)

                    for noFunctionalInverseProperty in noFunctionalInversePropertyList:
                        noFunctionalInversePropertyJSON = SPARQLQuery.inversePropertyValueQuery(inplaceIRIList, 
                                                                                                noFunctionalInverseProperty,
                                                                                                sparql_endpoint = sparql_endpoint,
                                                                                                doSameAs = False)
                        # create a seperate table to store one-to-many property-subject pair, return the created table name
                        tableName, _, _ = Json2Field.createMappingTableFromJSON(noFunctionalInversePropertyJSON, "wikidataSub", "o", noFunctionalInverseProperty, inputFeatureClassName, "URL", True, False)
                        # creat relationship class between the original feature class and the created table

                        relationshipClassName = featureClassName + "_" + tableName + "_RelClass"
                        arcpy.AddMessage("featureClassName: {0}".format(featureClassName))
                        arcpy.AddMessage("tableName: {0}".format(tableName))
                        arcpy.CreateRelationshipClass_management(featureClassName, tableName, relationshipClassName, "SIMPLE",
                            noFunctionalInverseProperty, "features from wikidata",
                                                "FORWARD", "ONE_TO_MANY", "NONE", "URL", "URL")

            return
             */
        }
    }
}

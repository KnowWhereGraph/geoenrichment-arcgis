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
    public partial class RelationshipFinder : Form
    {
        public RelationshipFinder()
        {
            InitializeComponent();

            relationshipDegree.SelectedItem = 1;
        }

        private void FindRelatedLinkedData(object sender, EventArgs e)
        {
            /*
            in_sparql_endpoint = parameters[0]
            in_wikiplace_IRI = parameters[1]
            in_do_single_ent_start = parameters[2]
            in_single_ent = parameters[3]
            in_relation_degree = parameters[4]
            in_first_property_dir = parameters[5]
            in_first_property = parameters[6]
            in_second_property_dir = parameters[7]
            in_second_property = parameters[8]
            in_third_property_dir = parameters[9]
            in_third_property = parameters[10]
            in_fourth_property_dir = parameters[11]
            in_fourth_property = parameters[12]
            out_location = parameters[13]
            out_table_name = parameters[14]
            out_points_name = parameters[15]


            sparql_endpoint = in_sparql_endpoint.valueAsText


            if in_do_single_ent_start.value:
                in_wikiplace_IRI.enabled = False
                in_single_ent.enabled = True
            else:
                in_wikiplace_IRI.enabled = True
                in_single_ent.enabled = False
    
            */
            if (relationshipDegree.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                /*
                relationDegree = int(in_relation_degree.valueAsText)
                in_first_property.enabled = False
                in_first_property_dir.enabled = False
                in_second_property.enabled = False
                in_second_property_dir.enabled = False
                in_third_property.enabled = False
                in_third_property_dir.enabled = False
                in_fourth_property.enabled = False
                in_fourth_property_dir.enabled = False
                if relationDegree == 1:
                    in_first_property.enabled = True
                    in_first_property_dir.enabled = True
                    # in_second_property.enabled = False
                    # in_second_property_dir.enabled = False
                    # in_third_property.enabled = False
                    # in_third_property_dir.enabled = False
                    # in_fourth_property.enabled = False
                    # in_fourth_property_dir.enabled = False
                elif relationDegree == 2:
                    in_first_property.enabled = True
                    in_first_property_dir.enabled = True
                    in_second_property.enabled = True
                    in_second_property_dir.enabled = True
                    # in_third_property.enabled = False
                    # in_third_property_dir.enabled = False
                    # in_fourth_property.enabled = False
                    # in_fourth_property_dir.enabled = False
                elif relationDegree == 3:
                    in_first_property.enabled = True
                    in_first_property_dir.enabled = True
                    in_second_property.enabled = True
                    in_second_property_dir.enabled = True
                    in_third_property.enabled = True
                    in_third_property_dir.enabled = True
                    # in_fourth_property.enabled = False
                    # in_fourth_property_dir.enabled = False
                elif relationDegree == 4:
                    in_first_property.enabled = True
                    in_first_property_dir.enabled = True
                    in_second_property.enabled = True
                    in_second_property_dir.enabled = True
                    in_third_property.enabled = True
                    in_third_property_dir.enabled = True
                    in_fourth_property.enabled = True
                    in_fourth_property_dir.enabled = True

                if in_wikiplace_IRI.value or in_single_ent.value:
                    if in_wikiplace_IRI.value and not in_do_single_ent_start.value:
                        inputFeatureClassName = in_wikiplace_IRI.valueAsText
                        lastIndexOFGDB = inputFeatureClassName.rfind("\\")
                        featureClassName = inputFeatureClassName[(lastIndexOFGDB+1):]
                        currentWorkspace = inputFeatureClassName[:lastIndexOFGDB]

                        arcpy.env.workspace = currentWorkspace
                        out_location.value = currentWorkspace



                        # get all the IRI from input point feature class of wikidata places
                        inplaceIRIList = []
                        cursor = arcpy.SearchCursor(inputFeatureClassName)
                        for row in cursor:
                            inplaceIRIList.append(row.getValue("URL"))

                    elif in_single_ent.value and in_do_single_ent_start.value:
                        inplaceIRIList = [in_single_ent.valueAsText]
                        featureClassName = "entity"


                    if not out_table_name.value:
                        out_table_name.value = featureClassName + "PathQueryTripleStore"
                    if not out_points_name.value:
                        out_points_name.value = featureClassName + "PathQueryGeographicEntity"


                    outLocation = out_location.valueAsText
                    outTableName = out_table_name.valueAsText
                    outputTableName = os.path.join(outLocation,outTableName)
                    # if arcpy.Exists(outputTableName):
                    #     arcpy.AddError("The output table already exists in current workspace!")
                    #     raise arcpy.ExecuteError

                    outFeatureClassName = out_points_name.valueAsText
                    outputFeatureClassName = os.path.join(outLocation,outFeatureClassName)
                    # if arcpy.Exists(outputFeatureClassName):
                    #     arcpy.AddError("The output Feature Class already exists in current workspace!")
                    #     raise arcpy.ExecuteError



                    # get the first property URL list and label list
                    if in_first_property_dir.value and len(in_first_property.filter.list) == 0:
                        fristDirection = in_first_property_dir.valueAsText
                        # get the first property URL list
                        firstPropertyURLListJsonBindingObject = SPARQLQuery.relFinderCommonPropertyQuery(inplaceIRIList, 
                                                                                    relationDegree = relationDegree, 
                                                                                    propertyDirectionList = [fristDirection], 
                                                                                    selectPropertyURLList = ["", "", ""], 
                                                                                    sparql_endpoint = sparql_endpoint)
                        firstPropertyURLList = []
                        for jsonItem in firstPropertyURLListJsonBindingObject:
                            firstPropertyURLList.append(jsonItem["p1"]["value"])

                        if sparql_endpoint == SPARQLUtil._WIKIDATA_SPARQL_ENDPOINT:
                            firstPropertyLabelJSON = SPARQLQuery.locationCommonPropertyLabelQuery(firstPropertyURLList, 
                                                    sparql_endpoint = sparql_endpoint)
                            # firstPropertyLabelJSON = firstPropertyLabelJSONObj["results"]["bindings"]

                            # get the first property label list
                            firstPropertyURLList = []
                            firstPropertyLabelList = []
                            for jsonItem in firstPropertyLabelJSON:
                                propertyURL = jsonItem["p"]["value"]
                                firstPropertyURLList.append(propertyURL)
                                propertyName = jsonItem["propertyLabel"]["value"]
                                firstPropertyLabelList.append(propertyName)
                        else:
                            firstPropertyLabelList = SPARQLUtil.make_prefixed_iri_batch(firstPropertyURLList)

                        RelFinder.firstPropertyLabelURLDict = dict(zip(firstPropertyLabelList, firstPropertyURLList))

                        in_first_property.filter.list = firstPropertyLabelList

                    # get the second property URL list and label list
                    #   when we pick the first property
                    if in_first_property_dir.value and in_first_property.value and \
                        in_second_property_dir.value and len(in_second_property.filter.list) == 0:

                        fristDirection = in_first_property_dir.valueAsText
                        firstProperty = in_first_property.valueAsText

                        if firstProperty == None:
                            firstProperty = ""
                        else:
                            firstProperty = RelFinder.firstPropertyLabelURLDict[firstProperty]

                        secondDirection = in_second_property_dir.valueAsText

                        # get the second property URL list
                        secondPropertyURLListJsonBindingObject = SPARQLQuery.relFinderCommonPropertyQuery(inplaceIRIList, 
                                                                    relationDegree = relationDegree, 
                                                                    propertyDirectionList = [fristDirection, secondDirection], 
                                                                    selectPropertyURLList = [firstProperty, "", ""],
                                                                    sparql_endpoint = sparql_endpoint)
                        secondPropertyURLList = []
                        for jsonItem in secondPropertyURLListJsonBindingObject:
                            secondPropertyURLList.append(jsonItem["p2"]["value"])

                        if sparql_endpoint == SPARQLUtil._WIKIDATA_SPARQL_ENDPOINT:
                            secondPropertyLabelJSON = SPARQLQuery.locationCommonPropertyLabelQuery(secondPropertyURLList,
                                                            sparql_endpoint = sparql_endpoint)
                            # secondPropertyLabelJSON = secondPropertyLabelJSONObj["results"]["bindings"]

                            # get the second property label list
                            secondPropertyURLList = []
                            secondPropertyLabelList = []
                            for jsonItem in secondPropertyLabelJSON:
                                propertyURL = jsonItem["p"]["value"]
                                secondPropertyURLList.append(propertyURL)
                                propertyName = jsonItem["propertyLabel"]["value"]
                                secondPropertyLabelList.append(propertyName)
                        else:
                            secondPropertyLabelList = SPARQLUtil.make_prefixed_iri_batch(secondPropertyURLList)

                        RelFinder.secondPropertyLabelURLDict = dict(zip(secondPropertyLabelList, secondPropertyURLList))

                        in_second_property.filter.list = secondPropertyLabelList

                    # get the third property URL list and label list
                    if in_first_property_dir.value and in_first_property.value and \
                        in_second_property_dir.value and in_second_property.value and \
                        in_third_property_dir.value and len(in_third_property.filter.list) == 0:

                        fristDirection = in_first_property_dir.valueAsText
                        firstProperty = in_first_property.valueAsText

                        secondDirection = in_second_property_dir.valueAsText
                        secondProperty = in_second_property.valueAsText

                        if firstProperty == None:
                            firstProperty = ""
                        else:
                            firstProperty = RelFinder.firstPropertyLabelURLDict[firstProperty]
                        if secondProperty == None:
                            secondProperty = ""
                        else:
                            secondProperty = RelFinder.secondPropertyLabelURLDict[secondProperty]

                        thirdDirection = in_third_property_dir.valueAsText

                        # get the third property URL list
                        thirdPropertyURLListJsonBindingObject = SPARQLQuery.relFinderCommonPropertyQuery(inplaceIRIList, 
                                                                    relationDegree = relationDegree, 
                                                                    propertyDirectionList = [fristDirection, secondDirection, thirdDirection], 
                                                                    selectPropertyURLList = [firstProperty, secondProperty, ""],
                                                                    sparql_endpoint = sparql_endpoint)
                        thirdPropertyURLList = []
                        for jsonItem in thirdPropertyURLListJsonBindingObject:
                            thirdPropertyURLList.append(jsonItem["p3"]["value"])

                        if sparql_endpoint == SPARQLUtil._WIKIDATA_SPARQL_ENDPOINT:
                            thirdPropertyLabelJSON = SPARQLQuery.locationCommonPropertyLabelQuery(thirdPropertyURLList,
                                                        sparql_endpoint = sparql_endpoint)
                            # thirdPropertyLabelJSON = thirdPropertyLabelJSONObj["results"]["bindings"]

                            # get the third property label list
                            thirdPropertyURLList = []
                            thirdPropertyLabelList = []
                            for jsonItem in thirdPropertyLabelJSON:
                                propertyURL = jsonItem["p"]["value"]
                                thirdPropertyURLList.append(propertyURL)
                                propertyName = jsonItem["propertyLabel"]["value"]
                                thirdPropertyLabelList.append(propertyName)
                        else:
                            thirdPropertyLabelList = SPARQLUtil.make_prefixed_iri_batch(thirdPropertyURLList)

                        RelFinder.thirdPropertyLabelURLDict = dict(zip(thirdPropertyLabelList, thirdPropertyURLList))

                        in_third_property.filter.list = thirdPropertyLabelList

                    # get the fourth property URL list and label list
                    if in_first_property_dir.value and in_first_property.value and \
                        in_second_property_dir.value and in_second_property.value and \
                        in_third_property_dir.value and in_third_property.value and \
                        in_fourth_property_dir.value and len(in_fourth_property.filter.list) == 0:

                        fristDirection = in_first_property_dir.valueAsText
                        firstProperty = in_first_property.valueAsText

                        secondDirection = in_second_property_dir.valueAsText
                        secondProperty = in_second_property.valueAsText

                        thirdDirection = in_third_property_dir.valueAsText
                        thirdProperty = in_third_property.valueAsText

                        if firstProperty == None:
                            firstProperty = ""
                        else:
                            firstProperty = RelFinder.firstPropertyLabelURLDict[firstProperty]
                        if secondProperty == None:
                            secondProperty = ""
                        else:
                            secondProperty = RelFinder.secondPropertyLabelURLDict[secondProperty]
                        if thirdProperty == None:
                            thirdProperty = ""
                        else:
                            thirdProperty = RelFinder.thirdPropertyLabelURLDict[thirdProperty]

                        fourthDirection = in_fourth_property_dir.valueAsText

                        # get the fourth property URL list
                        fourthPropertyURLListJsonBindingObject = SPARQLQuery.relFinderCommonPropertyQuery(inplaceIRIList, 
                                                                    relationDegree = relationDegree, 
                                                                    propertyDirectionList = [fristDirection, secondDirection, thirdDirection, fourthDirection], 
                                                                    selectPropertyURLList = [firstProperty, secondProperty, thirdProperty],
                                                                    sparql_endpoint = sparql_endpoint)
                        fourthPropertyURLList = []
                        for jsonItem in fourthPropertyURLListJsonBindingObject:
                            fourthPropertyURLList.append(jsonItem["p4"]["value"])

                        if sparql_endpoint == SPARQLUtil._WIKIDATA_SPARQL_ENDPOINT:
                            fourthPropertyLabelJSON = SPARQLQuery.locationCommonPropertyLabelQuery(fourthPropertyURLList,
                                                        sparql_endpoint = sparql_endpoint)
                            # fourthPropertyLabelJSON = fourthPropertyLabelJSONObj["results"]["bindings"]

                            # get the fourth property label list
                            fourthPropertyURLList = []
                            fourthPropertyLabelList = []
                            for jsonItem in fourthPropertyLabelJSON:
                                propertyURL = jsonItem["p"]["value"]
                                fourthPropertyURLList.append(propertyURL)
                                propertyName = jsonItem["propertyLabel"]["value"]
                                fourthPropertyLabelList.append(propertyName)
                        else:
                            fourthPropertyLabelList = SPARQLUtil.make_prefixed_iri_batch(fourthPropertyURLList)

                        RelFinder.fourthPropertyLabelURLDict = dict(zip(fourthPropertyLabelList, fourthPropertyURLList))

                        in_fourth_property.filter.list = fourthPropertyLabelList
                */
            }
        }
    }
}

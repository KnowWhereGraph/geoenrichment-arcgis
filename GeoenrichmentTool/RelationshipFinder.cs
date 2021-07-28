using ArcGIS.Desktop.Mapping;
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

            firstPropDirection.SelectedItem = 1;
        }

        private async void firstPropDirectionChanged(object sender, EventArgs e)
        {
            firstProp.Enabled = true; firstProp.Text = "";
            secondPropDirection.Enabled = false; secondPropDirection.Text = "";
            secondProp.Enabled = false; secondProp.Text = "";
            thirdPropDirection.Enabled = false; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";

            //populate first box
            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();

            List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
            string featureClassName = mainLayer.Name;
            /*
            elif in_single_ent.value and in_do_single_ent_start.value:
                inplaceIRIList = [in_single_ent.valueAsText]
                featureClassName = "entity"
            */

            string outTableName = featureClassName + "PathQueryTripleStore";
            string outFeatureClassName = featureClassName + "PathQueryGeographicEntity";

            /*
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

        private void firstPropChanged(object sender, EventArgs e)
        {
            secondPropDirection.Enabled = true; secondPropDirection.Text = "";
            secondProp.Enabled = false; secondProp.Text = "";
            thirdPropDirection.Enabled = false; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";
        }

        private async void secondPropDirectionChanged(object sender, EventArgs e)
        {
            secondProp.Enabled = true; secondProp.Text = "";
            thirdPropDirection.Enabled = false; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";

            //populate second box
        }

        private void secondPropChanged(object sender, EventArgs e)
        {
            thirdPropDirection.Enabled = true; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";
        }

        private async void thirdPropDirectionChanged(object sender, EventArgs e)
        {
            thirdProp.Enabled = true; thirdProp.Text = "";

            //populate third box
        }

        private void thirdPropChanged(object sender, EventArgs e)
        {
        }

        private async void FindRelatedLinkedData(object sender, EventArgs e)
        {
            if (firstPropDirection.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                
            }
        }
    }
}

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
    public partial class RelationshipFinder : Form
    {

        /*
        # propertyDirectionList: the list of property direction, it has at most 4 elements, the length is the path degree. The element value is from ["BOTH", "ORIGIN", "DESTINATION"]
        # selectPropertyURLList: the selected peoperty URL list, it always has three elements, "" if no property has been selected
         */
        List<string> propertyDirectionList;
        List<string> selectPropertyURLList;

        public RelationshipFinder()
        {
            InitializeComponent();

            propertyDirectionList = new List<string>() { };
            selectPropertyURLList = new List<string>() { };
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

            propertyDirectionList = new List<string>() { firstPropDirection.Text };
            selectPropertyURLList = new List<string>() { };

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, 1);

            foreach (var item in finderJSON)
            {
                string itemVal = item["p1"]["value"].ToString();
                string itemLabel = GeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                firstProp.Items.Add(itemLabel + " | " + itemVal);
            }
        }

        private void firstPropChanged(object sender, EventArgs e)
        {
            secondPropDirection.Enabled = true; secondPropDirection.Text = "";
            secondProp.Enabled = false; secondProp.Text = "";
            thirdPropDirection.Enabled = false; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";

            propertyDirectionList = new List<string>() { propertyDirectionList[0] };
            selectPropertyURLList = new List<string>() { firstProp.Text };
        }

        private async void secondPropDirectionChanged(object sender, EventArgs e)
        {
            secondProp.Enabled = true; secondProp.Text = "";
            thirdPropDirection.Enabled = false; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";

            //populate second box
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

            propertyDirectionList = new List<string>() { propertyDirectionList[0], secondPropDirection.Text };
            selectPropertyURLList = new List<string>() { selectPropertyURLList[0] };

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, 2);

            foreach (var item in finderJSON)
            {
                string itemVal = item["p2"]["value"].ToString();
                string itemLabel = GeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                secondProp.Items.Add(itemLabel + " | " + itemVal);
            }
        }

        private void secondPropChanged(object sender, EventArgs e)
        {
            thirdPropDirection.Enabled = true; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";

            propertyDirectionList = new List<string>() { propertyDirectionList[0], propertyDirectionList[1] };
            selectPropertyURLList = new List<string>() { selectPropertyURLList[0], secondProp.Text };
        }

        private async void thirdPropDirectionChanged(object sender, EventArgs e)
        {
            thirdProp.Enabled = true; thirdProp.Text = "";

            //populate third box
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

            propertyDirectionList = new List<string>() { propertyDirectionList[0], propertyDirectionList[1], thirdPropDirection.Text };
            selectPropertyURLList = new List<string>() { selectPropertyURLList[0], selectPropertyURLList[1] };

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, 3);

            foreach (var item in finderJSON)
            {
                string itemVal = item["p3"]["value"].ToString();
                string itemLabel = GeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                thirdProp.Items.Add(itemLabel + " | " + itemVal);
            }
        }

        private void thirdPropChanged(object sender, EventArgs e)
        {
            selectPropertyURLList.Add(thirdProp.Text);

            propertyDirectionList = new List<string>() { propertyDirectionList[0], propertyDirectionList[1], propertyDirectionList[2] };
            selectPropertyURLList = new List<string>() { selectPropertyURLList[0], selectPropertyURLList[1], thirdProp.Text };
        }

        /*
        # get the property URL list in the specific degree path from the inplaceIRIList
        # inplaceIRIList: the URL list of wikidata locations
        # relationDegree: the degree of the property on the path the current query wants to get
         */
        private JToken RelationshipFinderCommonPropertyQuery(List<string> inplaceIRIList, int relationDegree)
        {
            string selectParam = "?p" + relationDegree.ToString();
            char[] delimPipe = { '|' };

            string relationFinderQuery = "SELECT distinct " + selectParam +  " WHERE { ";

            for(int index=0; index<relationDegree; index++)
            {
                string currDirection = propertyDirectionList[index];
                int currDegree = index + 1;
                string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                string oValHigh = "?o" + currDegree.ToString();
                string pVal = (currDegree == relationDegree) ? "?p" + currDegree.ToString() : "<" + selectPropertyURLList[0].Split(delimPipe).Last().Trim() + ">";

                switch (currDirection)
                {
                    case "Both":
                        relationFinderQuery += "{" + oValLow + " " + pVal + " " + oValHigh + ".} UNION {" + oValHigh + " " + pVal + " " + oValLow + ".}\n";
                        break;
                    case "Origin":
                        relationFinderQuery += oValLow + " " + pVal + " " + oValHigh + ".\n";
                        break;
                    case "Destination":
                        relationFinderQuery += oValHigh + " " + pVal + " " + oValLow + ".\n";
                        break;
                    default:
                        break;
                }
            }

            relationFinderQuery += " VALUES ?place { ";

            foreach(var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "} }";

            return GeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);
        }

        private async void FindRelatedLinkedData(object sender, EventArgs e)
        {
            if (firstPropDirection.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
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

                relationDegree = int(in_relation_degree.valueAsText)
                outLocation = out_location.valueAsText
                outTableName = out_table_name.valueAsText
                outFeatureClassName = out_points_name.valueAsText
                outputTableName = os.path.join(outLocation,outTableName)
                outputFeatureClassName = os.path.join(outLocation,outFeatureClassName)

                # check whether outputTableName or outputFeatureClassName already exist
                if arcpy.Exists(outputTableName) or arcpy.Exists(outputFeatureClassName):
                    messages.addErrorMessage("The output table or feature class already exists in current workspace!")
                    raise arcpy.ExecuteError
                    return
                */
                
                /*
                if not in_do_single_ent_start
                */
                BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();

                List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
                /*
                else:
                    # we use single iri as the start node
                    inplaceIRIList = [in_single_ent.valueAsText]
                */

                //# for the direction list, change "BOTH" to "OROIGIN" and "DESTINATION"
                List<List<string>> directionExpendedLists = DirectionListFromBoth2OD();

                List<Dictionary<string, string>> tripleStore = new List<Dictionary<string, string>>() { };

                foreach (var currentDirectionList in directionExpendedLists)
                {
                    //# get a list of triples for curent specified property path
                    List<Dictionary<string, string>> newTripleStore = RelFinderTripleQuery(inplaceIRIList, currentDirectionList);
                    tripleStore = tripleStore.Union(newTripleStore).ToList();
                }

                List<string> triplePropertyURLList = new List<string>() { };
                List<string> triplePropertyLabelList = new List<string>() { };
                foreach(var triple in tripleStore)
                {
                    if(!triplePropertyURLList.Contains(triple["p"]))
                    {
                        triplePropertyURLList.Add(triple["p"]);
                        triplePropertyLabelList.Add(GeoModule.Current.GetQueryClass().MakeIRIPrefix(triple["p"]));
                    }
                }

                FeatureClassHelper.CreateRelationshipFinderTable(tripleStore, triplePropertyURLList, triplePropertyLabelList, ""); //TODO

                List<string> entitySet = new List<string>() { };
                foreach(var triple in tripleStore)
                {
                    entitySet.Add(triple["s"]);
                    entitySet.Add(triple["o"]);
                }

                JToken placeJSON = EndPlaceInformationQuery(entitySet);

                FeatureClassHelper.CreateRelationshipFinderFeatureClass(placeJSON);
            }
        }

        //# given a list of direction, return a list of lists which change a list with "BOTH" to two list with "ORIGIN" and "DESTINATION"
        //# e.g. ["BOTH", "ORIGIN", "DESTINATION", "ORIGIN"] -> ["ORIGIN", "ORIGIN", "DESTINATION", "ORIGIN"] and ["DESTINATION", "ORIGIN", "DESTINATION", "ORIGIN"]
        //# propertyDirectionList: a list of direction from ["BOTH", "ORIGIN", "DESTINATION"], it has at most 4 elements
        private List<List<string>> DirectionListFromBoth2OD()
        {
            List<List<string>> propertyDirectionExpandedLists = new List<List<string>>() { };
            propertyDirectionExpandedLists.Add(propertyDirectionList);

            List<List<string>> resultList = new List<List<string>>() { };

            foreach(var currentPropertyDirectionList in propertyDirectionExpandedLists)
            {
                int i = 0;
                int indexOfBOTH = -1;
                while(i < currentPropertyDirectionList.Count)
                {
                    if(currentPropertyDirectionList[i] == "BOTH")
                    {
                        indexOfBOTH = i;
                        break;
                    }

                    i++;
                }

                if(indexOfBOTH != -1)
                {
                    List<string> newList1 = currentPropertyDirectionList;
                    newList1[indexOfBOTH] = "ORIGIN";
                    propertyDirectionExpandedLists.Add(newList1);

                    List<string> newList2 = currentPropertyDirectionList;
                    newList2[indexOfBOTH] = "DESTINATION";
                    propertyDirectionExpandedLists.Add(newList2);
                }
                else
                {
                    if(!resultList.Contains(currentPropertyDirectionList))
                    {
                        resultList.Add(currentPropertyDirectionList);
                    }
                }
            }

            return resultList;
        }

        //# get the triple set in the specific degree path from the inplaceIRIList
        //# inplaceIRIList: the URL list of wikidata locations
        //# propertyDirectionList: the list of property direction, it has at most 4 elements, the length is the path degree. The element value is from ["ORIGIN", "DESTINATION"]
        //# selectPropertyURLList: the selected peoperty URL list, it always has 4 elements, "" if no property has been selected
        private List<Dictionary<string, string>> RelFinderTripleQuery(List<string> inplaceIRIList, List<string> currentDirectionList)
        {
            string selectParam = "?place ";
            int relationDegree = selectPropertyURLList.Count();

            for(int i=0; i< relationDegree; i++)
            {
                int currDegree = i + 1;
                //if selectPropertyURLList[0] == "":
                //    selectParam += "?p"+ currDegree.ToString() +" "

                selectParam += "?o" + currDegree.ToString() + " ";
            }

            string relationFinderQuery = "SELECT distinct " + selectParam + " WHERE {";
            char[] delimPipe = { '|' };

            for (int index = 0; index < relationDegree; index++)
            {
                string currDirection = currentDirectionList[index];
                int currDegree = index + 1;
                string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                string oValHigh = "?o" + currDegree.ToString();
                string pVal = (currDegree == relationDegree) ? "?p" + currDegree.ToString() : "<" + selectPropertyURLList[0].Split(delimPipe).Last().Trim() + ">";

                switch (currDirection)
                {
                    case "Both":
                        relationFinderQuery += "{" + oValLow + " " + pVal + " " + oValHigh + ".} UNION {" + oValHigh + " " + pVal + " " + oValLow + ".}\n";
                        break;
                    case "Origin":
                        relationFinderQuery += oValLow + " " + pVal + " " + oValHigh + ".\n";
                        break;
                    case "Destination":
                        relationFinderQuery += oValHigh + " " + pVal + " " + oValLow + ".\n";
                        break;
                    default:
                        break;
                }
            }

            relationFinderQuery += " VALUES ?place { ";

            foreach (var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "} }";

            JToken resultsJSON = GeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);

            List<Dictionary<string, string>> tripleStore = new List<Dictionary<string, string>>() { };
            foreach (var jsonItem in resultsJSON)
            {
                for (int index = 0; index < relationDegree; index++)
                {
                    string currDirection = currentDirectionList[index];
                    int currDegree = index + 1;
                    string oValLow = (index == 0) ? "place" : "o" + index.ToString();
                    string oValHigh = "o" + currDegree.ToString();

                    Dictionary<string, string> currentTriple = new Dictionary<string, string>() { };
                    switch (currDirection)
                    {
                        case "Origin":
                            currentTriple = new Dictionary<string, string>() { 
                                { "s", jsonItem[oValLow]["value"].ToString() }, { "p", selectPropertyURLList[index] }, { "o", jsonItem[oValHigh]["value"].ToString() } 
                            };
                            break;
                        case "Destination":
                            currentTriple = new Dictionary<string, string>() {
                                { "s", jsonItem[oValHigh]["value"].ToString() }, { "p", selectPropertyURLList[index] }, { "o", jsonItem[oValLow]["value"].ToString() }
                            };
                            break;
                        default:
                            break;
                    }

                    tripleStore.Add(currentTriple);
                }
            }

            return tripleStore;
        }

        private JToken EndPlaceInformationQuery(List<string> endPlaceIRIList)
        {
            string endPlaceQuery = "SELECT distinct ?place ?placeLabel ?placeFlatType ?wkt WHERE { " +
                "?place geo:hasGeometry ?geometry . ?place rdfs:label ?placeLabel . ?geometry geo:asWKT ?wkt. " +
                "VALUES ?place {";

            foreach (var iri in endPlaceIRIList)
            {
                endPlaceQuery += "<" + iri + "> \n";
            }
            endPlaceQuery += "} }";

            return GeoModule.Current.GetQueryClass().SubmitQuery(endPlaceQuery);
        }
    }
}

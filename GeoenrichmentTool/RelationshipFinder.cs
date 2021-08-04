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

            propertyDirectionList.Add(firstPropDirection.Text);

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

            selectPropertyURLList.Add(firstProp.Text);
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

            propertyDirectionList.Add(secondPropDirection.Text);

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, 2);

            foreach (var item in finderJSON)
            {
                string itemVal = item["p2"]["value"].ToString();
                string itemLabel = GeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                firstProp.Items.Add(itemLabel + " | " + itemVal);
            }
        }

        private void secondPropChanged(object sender, EventArgs e)
        {
            thirdPropDirection.Enabled = true; thirdPropDirection.Text = "";
            thirdProp.Enabled = false; thirdProp.Text = "";

            selectPropertyURLList.Add(secondProp.Text);
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

            propertyDirectionList.Add(thirdPropDirection.Text);

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, 3);

            foreach (var item in finderJSON)
            {
                string itemVal = item["p3"]["value"].ToString();
                string itemLabel = GeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                firstProp.Items.Add(itemLabel + " | " + itemVal);
            }
        }

        private void thirdPropChanged(object sender, EventArgs e)
        {
            selectPropertyURLList.Add(thirdProp.Text);
        }

        /*
        # get the property URL list in the specific degree path from the inplaceIRIList
        # inplaceIRIList: the URL list of wikidata locations
        # relationDegree: the degree of the property on the path the current query wants to get
         */
        private JToken RelationshipFinderCommonPropertyQuery(List<string> inplaceIRIList, int relationDegree)
        {
            string selectParam = "?p" + relationDegree.ToString();

            string relationFinderQuery = "SELECT distinct " + selectParam +  "WHERE { ";

            for(int index=0; index<relationDegree; index++)
            {
                string currDirection = propertyDirectionList[index];
                int currDegree = index + 1;
                string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                string oValHigh = "?o" + currDegree.ToString();
                string pVal = (currDegree == relationDegree) ? "?p" + currDegree.ToString() : "<" + selectPropertyURLList[0] + ">";

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

            relationFinderQuery += "VALUES ?place { ";

            foreach(var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "} }";

            return GeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);
        }

        private void FindRelatedLinkedData(object sender, EventArgs e)
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

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
    public partial class TraverseKnowledgeGraph : Form
    {
        /*
        # selectPropertyURLList: the selected peoperty URL list
         */
        private List<string> selectPropertyURLList;
        private List<List<string>> permutations;

        private BasicFeatureLayer mainLayer;
        private string outTableName;
        private string outFeatureClassName;

        public TraverseKnowledgeGraph()
        {
            InitializeComponent();

            mainLayer = KwgGeoModule.Current.GetLayers().First();
            string featureClassName = mainLayer.Name;

            Random gen = new Random();
            string identifier = gen.Next(999999).ToString(); //TODO::4 digits?
            outTableName = featureClassName + "PathQueryTripleStore_" + identifier;
            outFeatureClassName = featureClassName + "PathQueryGeographicEntity_" + identifier;

            selectPropertyURLList = new List<string>() { };
            populatePropertyBox(1);
        }

        private async void populatePropertyBox(int degree)
        {
            //TODO::reset things

            List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
            /*
            elif in_single_ent.value and in_do_single_ent_start.value:
                inplaceIRIList = [in_single_ent.valueAsText]
                featureClassName = "entity"
            */

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, 1);

            ComboBox propBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
            propBox.Enabled = true;
            propBox.Items.Clear();
            foreach (var item in finderJSON)
            {
                string itemVal = item["p" + degree.ToString()]["value"].ToString();
                string itemLabel = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                propBox.Items.Add(itemLabel + " | " + itemVal);
            }
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

            string relationFinderQuery = "SELECT distinct " + selectParam + " WHERE { ";

            for (int index = 0; index < relationDegree; index++)
            {
                int currDegree = index + 1;
                string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                string oValHigh = "?o" + currDegree.ToString();
                string pVal = (currDegree == relationDegree) ? "?p" + currDegree.ToString() : "<" + selectPropertyURLList[0].Split(delimPipe).Last().Trim() + ">";

                relationFinderQuery += "{" + oValLow + " " + pVal + " " + oValHigh + ".} UNION {" + oValHigh + " " + pVal + " " + oValLow + ".}\n";
            }

            relationFinderQuery += " VALUES ?place { ";

            foreach (var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "} }";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);
        }
    }
}

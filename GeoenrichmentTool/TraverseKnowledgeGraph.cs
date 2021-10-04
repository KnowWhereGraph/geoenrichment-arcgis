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
        private List<List<string>> permutations;

        private BasicFeatureLayer mainLayer;
        private string outTableName;
        private string outFeatureClassName;

        protected int maxDegree = 1;
        private int propertySpacing = 75;

        public TraverseKnowledgeGraph()
        {
            InitializeComponent();

            mainLayer = KwgGeoModule.Current.GetLayers().First();
            string featureClassName = mainLayer.Name;

            Random gen = new Random();
            string identifier = gen.Next(999999).ToString(); //TODO::4 digits?
            outTableName = featureClassName + "PathQueryTripleStore_" + identifier;
            outFeatureClassName = featureClassName + "PathQueryGeographicEntity_" + identifier;

            PopulatePropertyBox(1);
        }

        private async void PopulatePropertyBox(int degree)
        {
            //Clear boxes that have a higher degree
            for(int i=degree+1; i<=maxDegree; i++)
            {
                ComboBox otherBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                otherBox.Enabled = false;
                otherBox.Text = "";
                otherBox.Items.Clear();
            }

            List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
            /*
            elif in_single_ent.value and in_do_single_ent_start.value:
                inplaceIRIList = [in_single_ent.valueAsText]
                featureClassName = "entity"
            */

            JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, degree);

            ComboBox propBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
            propBox.Enabled = true;
            propBox.Text = "";
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
                ComboBox currentBox = (ComboBox)this.Controls.Find("prop" + currDegree.ToString(), true).First();
                string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                string oValHigh = "?o" + currDegree.ToString();
                string pVal = (currDegree == relationDegree) ? "?p" + currDegree.ToString() : "<" + currentBox.Text.Split(delimPipe).Last().Trim() + ">";

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

        private void PropertyChanged(object sender, EventArgs e)
        {
            ComboBox propBox = (ComboBox)sender;
            int degree = int.Parse(propBox.Name.Replace("prop", ""));

            if (degree < maxDegree)
            {
                PopulatePropertyBox(degree + 1);
            }
        }

        private void AddNewProperty(object sender, MouseEventArgs e)
        {
            int newDegree = maxDegree + 1;

            //Expand the form and move down the button elements
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
            this.addPropertyBtn.Location = new System.Drawing.Point(this.addPropertyBtn.Location.X, this.addPropertyBtn.Location.Y + propertySpacing);
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);

            //Create new property elements based on old ones
            Label propRequired = (Label)this.Controls.Find("prop" + maxDegree.ToString() + "Req", true).First();
            Label propRequired_copy = new Label();
            propRequired_copy.AutoSize = propRequired.AutoSize;
            propRequired_copy.BackColor = propRequired.BackColor;
            propRequired_copy.Font = propRequired.Font;
            propRequired_copy.ForeColor = propRequired.ForeColor;
            propRequired_copy.Location = new System.Drawing.Point(45, propRequired.Location.Y + propertySpacing);
            propRequired_copy.Name = "prop" + newDegree.ToString() + "Req";
            propRequired_copy.Size = propRequired.Size;
            propRequired_copy.Text = propRequired.Text;
            this.Controls.Add(propRequired_copy);

            Label propLabel = (Label)this.Controls.Find("prop" + maxDegree.ToString() + "Label", true).First();
            Label propLabel_copy = new Label();
            propLabel_copy.AutoSize = propLabel.AutoSize;
            propLabel_copy.BackColor = propLabel.BackColor;
            propLabel_copy.Font = propLabel.Font;
            propLabel_copy.ForeColor = propLabel.ForeColor;
            propLabel_copy.Location = new System.Drawing.Point(60, propLabel.Location.Y + propertySpacing);
            propLabel_copy.Margin = propLabel.Margin;
            propLabel_copy.Name = "prop" + newDegree.ToString() + "Label";
            propLabel_copy.Size = propLabel.Size;
            propLabel_copy.Text = "More Content";
            this.Controls.Add(propLabel_copy);

            ComboBox propBox = (ComboBox)this.Controls.Find("prop" + maxDegree.ToString(), true).First();
            ComboBox propBox_copy = new ComboBox();
            propBox_copy.Enabled = false;
            propBox_copy.Font = propBox.Font;
            propBox_copy.FormattingEnabled = propBox.FormattingEnabled;
            propBox_copy.Location = new System.Drawing.Point(50, propBox.Location.Y + propertySpacing);
            propBox_copy.Name = "prop" + newDegree.ToString();
            propBox_copy.Size = propBox.Size;
            propBox_copy.SelectedValueChanged += new System.EventHandler(this.PropertyChanged);
            this.Controls.Add(propBox_copy);

            //Populate list options and enable if previous property is selected
            if (propBox.Text != "")
            {
                propBox_copy.Enabled = true;
                PopulatePropertyBox(newDegree);
            }

            maxDegree++;
        }
    }
}

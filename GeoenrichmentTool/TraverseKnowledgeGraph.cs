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
        private string currentEndpoint;
        private string entityVals;

        protected int maxDegree = 1;

        private int propertySpacing = 115;
        private int helpSpacing = 440;
        private bool helpOpen = true;

        public TraverseKnowledgeGraph(string endpoint, List<string> entities)
        {
            InitializeComponent();
            ToggleHelpMenu();

            currentEndpoint = endpoint;
            entityVals = "values ?entity {" + String.Join(" ", entities) + "}";

            PopulateClassBox(1);
        }

        private void PopulateClassBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            ComboBox classBox = (ComboBox)this.Controls.Find("subject" + degree.ToString(), true).First();

            //Lets get our list of entities
            var typeQuery = "select distinct ?type ?label where { " +
                "?entity a ?type. " +
                "?type rdfs:label ?label. " +
                entityVals +
            "}";

            JToken typeResults = queryClass.SubmitQuery(currentEndpoint, typeQuery);

            Dictionary<string, string> classes = new Dictionary<string, string>() { {"", ""} };
            foreach (var item in typeResults)
            {
                string cType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                string cLabel = queryClass.IRIToPrefix(item["label"]["value"].ToString());

                if (!classes.ContainsKey(cType))
                {
                    classes[cType] = cLabel;
                }
            }

            classBox.DataSource = new BindingSource(classes.OrderBy(key => key.Value), null);
            classBox.Enabled = true;
        }

        private void PopulatePropertyBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            ComboBox classBox = (ComboBox)this.Controls.Find("subject" + degree.ToString(), true).First();
            var classVal = classBox.SelectedValue.ToString();
            ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + degree.ToString(), true).First();

            //Lets get our list of entities
            var propQuery = "select distinct ?p ?label where { " +
                "?entity a "+ classVal + "; " +
                    "?p ?o. " +
                    "optional {?p rdfs:label ?label}" +
                entityVals +
            "}";

            JToken propResults = queryClass.SubmitQuery(currentEndpoint, propQuery);

            Dictionary<string, string> properties = new Dictionary<string, string>() { { "", "" } };
            foreach (var item in propResults)
            {
                string predicate = queryClass.IRIToPrefix(item["p"]["value"].ToString());
                string pLabel = (item["label"] != null) ? queryClass.IRIToPrefix(item["label"]["value"].ToString()) : predicate;

                if (!properties.ContainsKey(predicate))
                {
                    properties[predicate] = pLabel;
                }
            }

            propBox.DataSource = new BindingSource(properties.OrderBy(key => key.Value), null);
            propBox.Enabled = true;
        }

        private void OnClassBoxChange(object sender, EventArgs e)
        {
            ComboBox classBox = (ComboBox)sender;
            if (classBox.SelectedValue.ToString() != "")
            {
                int degree = int.Parse(classBox.Name.Replace("subject", ""));

                PopulatePropertyBox(degree);
            }
        }

        private void RunTraverseGraph(object sender, EventArgs e)
        {
            if (object1.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                Close();

                //Do stuff
            }
        }

        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            ToggleHelpMenu();
        }

        private void ToggleHelpMenu()
        {
            if (helpOpen)
            {
                this.Size = new System.Drawing.Size(this.Size.Width - helpSpacing, this.Size.Height);
                helpOpen = false;
            }
            else
            {
                this.Size = new System.Drawing.Size(this.Size.Width + helpSpacing, this.Size.Height);
                helpOpen = true;
            }
        }
    }
}

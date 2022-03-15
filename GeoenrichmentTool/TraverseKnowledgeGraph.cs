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
        private GeoenrichmentForm originalWindow;
        
        private string currentEndpoint;
        private string entityVals;

        protected int maxDegree = 1;

        private int propertySpacing = 30;
        private int helpSpacing = 440;
        private bool helpOpen = true;

        public TraverseKnowledgeGraph(GeoenrichmentForm gf, string endpoint, List<string> entities)
        {
            InitializeComponent();
            ToggleHelpMenu();

            originalWindow = gf;
            currentEndpoint = endpoint;
            entityVals = "values ?entity {" + String.Join(" ", entities) + "}";

            PopulateClassBox(1);
        }

        private void PopulateClassBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            ComboBox classBox = (ComboBox)this.Controls.Find("subject" + degree.ToString(), true).First();
            Dictionary<string, string> classes;

            if (degree == 1)
            {
                var typeQuery = "select distinct ?type ?label where { " +
                    "?entity a ?type. " +
                    "?type rdfs:label ?label. " +
                    entityVals +
                "}";

                JToken typeResults = queryClass.SubmitQuery(currentEndpoint, typeQuery);

                classes = new Dictionary<string, string>() { { "", "" } };
                foreach (var item in typeResults)
                {
                    string cType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                    string cLabel = queryClass.IRIToPrefix(item["label"]["value"].ToString());

                    if (!classes.ContainsKey(cType))
                    {
                        classes[cType] = cLabel;
                    }
                }

                classBox.Enabled = true;
            }
            else
            {
                ComboBox valueBox = (ComboBox)this.Controls.Find("object" + (degree-1).ToString(), true).First();
                string objectVal = valueBox.SelectedValue.ToString();
                string objectLabel = valueBox.Text;

                classes = new Dictionary<string, string>() { { objectVal, objectLabel } };
            }

            classBox.DataSource = new BindingSource(classes.OrderBy(key => key.Value), null);
        }

        private void PopulatePropertyBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();

            var propQuery = "select distinct ?p ?label where {  ";

            for(int i=1; i < degree + 1; i++)
            {
                ComboBox classBox = (ComboBox)this.Controls.Find("subject" + i.ToString(), true).First();
                ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + i.ToString(), true).First();
                string subjectStr = (i == 1) ? "?entity" : "?o" + (i - 1).ToString();
                string classStr = classBox.SelectedValue.ToString();
                string predicateStr = (i == degree) ? "?p" : propBox.SelectedValue.ToString();
                string objectStr = "?o" + i.ToString();

                propQuery += subjectStr + " a " + classStr + "; " + predicateStr + " " + objectStr + ". ";
            }        
            propQuery += "optional {?p rdfs:label ?label} " + entityVals + "}";

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

            ComboBox currPropBox = (ComboBox)this.Controls.Find("predicate" + degree.ToString(), true).First();
            currPropBox.DataSource = new BindingSource(properties.OrderBy(key => key.Value), null);
            currPropBox.Enabled = true;
        }

        private void PopulateValueBox(int degree)
        {
            //    "?entity a " + classVal + "; " + propVal + " ?o. " + "?o a ?type. " +

            var queryClass = KwgGeoModule.Current.GetQueryClass();

            var valueQuery = "select distinct ?type ?label where {  ";

            for (int i = 1; i < degree + 1; i++)
            {
                ComboBox classBox = (ComboBox)this.Controls.Find("subject" + i.ToString(), true).First();
                ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + i.ToString(), true).First();
                string subjectStr = (i == 1) ? "?entity" : "?o" + (i - 1).ToString();
                string classStr = classBox.SelectedValue.ToString();
                string predicateStr = propBox.SelectedValue.ToString();
                string objectStr = (i == degree) ? "?o. ?o a ?type" : "?o" + i.ToString();

                valueQuery += subjectStr + " a " + classStr + "; " + predicateStr + " " + objectStr + ". ";
            }
            valueQuery += "?type rdfs:label ?label. " + entityVals + "}";

            JToken valueResults = queryClass.SubmitQuery(currentEndpoint, valueQuery);
            ComboBox currValueBox = (ComboBox)this.Controls.Find("object" + degree.ToString(), true).First();

            Dictionary<string, string> values;
            if (valueResults.HasValues) {
                values = new Dictionary<string, string>() { { "", "" } };
                foreach (var item in valueResults)
                {
                    string oType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                    string oLabel = queryClass.IRIToPrefix(item["label"]["value"].ToString());

                    if (!values.ContainsKey(oType))
                    {
                        values[oType] = oLabel;
                    }
                }
                currValueBox.Enabled = true;
            }
            else
            {
                values = new Dictionary<string, string>() { { "LiteralDataFound", "Literal Data Found" } };
            }

            currValueBox.DataSource = new BindingSource(values.OrderBy(key => key.Value), null);
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

        private void OnPropBoxChange(object sender, EventArgs e)
        {
            ComboBox propBox = (ComboBox)sender;
            if (propBox.SelectedValue.ToString() != "")
            {
                int degree = int.Parse(propBox.Name.Replace("predicate", ""));

                PopulateValueBox(degree);
            }
        }

        private void OnValueBoxChange(object sender, EventArgs e)
        {
            ComboBox valueBox = (ComboBox)sender;
            if (valueBox.SelectedValue.ToString() != "")
            {
                addPropertyBtn.Enabled = true;
            }
        }

        private void LearnMore(object sender, EventArgs e)
        {
            addPropertyBtn.Enabled = false;
            int newDegree = maxDegree + 1;

            //Expand the form and move down the button elements
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
            this.addPropertyBtn.Location = new System.Drawing.Point(this.addPropertyBtn.Location.X, this.addPropertyBtn.Location.Y + propertySpacing);
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);
            this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y + propertySpacing);
            this.helpPanel.Size = new System.Drawing.Size(this.helpPanel.Size.Width, this.helpPanel.Size.Height + propertySpacing);

            ComboBox classBox = (ComboBox)this.Controls.Find("subject" + maxDegree.ToString(), true).First();
            ComboBox classBox_copy = new ComboBox();
            classBox_copy.Enabled = false;
            classBox_copy.Font = classBox.Font;
            classBox_copy.FormattingEnabled = classBox.FormattingEnabled;
            classBox_copy.Location = new System.Drawing.Point(classBox.Location.X, classBox.Location.Y + propertySpacing);
            classBox_copy.Name = "subject" + newDegree.ToString();
            classBox_copy.Size = classBox.Size;
            classBox_copy.DisplayMember = "Value";
            classBox_copy.ValueMember = "Key";
            this.Controls.Add(classBox_copy);

            ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + maxDegree.ToString(), true).First();
            ComboBox propBox_copy = new ComboBox();
            propBox_copy.Enabled = false;
            propBox_copy.Font = propBox.Font;
            propBox_copy.FormattingEnabled = propBox.FormattingEnabled;
            propBox_copy.Location = new System.Drawing.Point(propBox.Location.X, propBox.Location.Y + propertySpacing);
            propBox_copy.Name = "predicate" + newDegree.ToString();
            propBox_copy.Size = propBox.Size;
            propBox_copy.DisplayMember = "Value";
            propBox_copy.ValueMember = "Key";
            propBox_copy.SelectedIndexChanged += new System.EventHandler(this.OnPropBoxChange);
            this.Controls.Add(propBox_copy);

            ComboBox valueBox = (ComboBox)this.Controls.Find("object" + maxDegree.ToString(), true).First();
            ComboBox valueBox_copy = new ComboBox();
            valueBox_copy.Enabled = false;
            valueBox_copy.Font = valueBox.Font;
            valueBox_copy.FormattingEnabled = valueBox.FormattingEnabled;
            valueBox_copy.Location = new System.Drawing.Point(valueBox.Location.X, valueBox.Location.Y + propertySpacing);
            valueBox_copy.Name = "object" + newDegree.ToString();
            valueBox_copy.Size = valueBox.Size;
            valueBox_copy.DisplayMember = "Value";
            valueBox_copy.ValueMember = "Key";
            valueBox_copy.SelectedIndexChanged += new System.EventHandler(this.OnValueBoxChange);
            this.Controls.Add(valueBox_copy);

            //Populate list options and enable
            PopulateClassBox(newDegree);
            PopulatePropertyBox(newDegree);

            maxDegree++;
        }

        private void RunTraverseGraph(object sender, EventArgs e)
        {
            if (subject1.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                List<string> uriList = new List<string>();
                List<string> labelList = new List<string>();
                for (int i = 1; i < maxDegree + 1; i++)
                {
                    ComboBox classBox = (ComboBox)this.Controls.Find("subject" + i.ToString(), true).First();
                    if (classBox.Text != null && classBox.Text != "")
                    {
                        uriList.Add(classBox.SelectedValue.ToString());
                        labelList.Add(classBox.Text);
                    }

                    ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + i.ToString(), true).First();
                    if (propBox.Text != null && propBox.Text != "")
                    {
                        uriList.Add(propBox.SelectedValue.ToString());
                        labelList.Add(propBox.Text);
                    }

                    ComboBox valueBox = (ComboBox)this.Controls.Find("object" + i.ToString(), true).First();
                    if (valueBox.Text != null && valueBox.Text != "")
                    {
                        uriList.Add(valueBox.SelectedValue.ToString());
                        labelList.Add(valueBox.Text);
                    } 
                }

                originalWindow.AddSelectedContent(uriList, labelList);
                Close();
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

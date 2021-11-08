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
        private Dictionary<int, Dictionary<string, List<string>>> classesAndProperties;

        private BasicFeatureLayer mainLayer;

        protected int maxDegree = 1;

        private int propertySpacing = 115;
        private int helpSpacing = 440;
        private bool helpOpen = true;

        public TraverseKnowledgeGraph()
        {
            InitializeComponent();
            ToggleHelpMenu();

            classesAndProperties = new Dictionary<int, Dictionary<string, List<string>>>() { };
            mainLayer = KwgGeoModule.Current.GetLayers().First();

            PopulateClassBox(1);
        }

        private async void PopulateClassBox(int degree)
        {
            //Clear boxes that have a higher degree
            ComboBox matchingPropBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
            matchingPropBox.Enabled = false;
            matchingPropBox.Text = "";
            matchingPropBox.Items.Clear();
            for (int i = degree + 1; i <= maxDegree; i++)
            {
                ComboBox otherClassBox = (ComboBox)this.Controls.Find("class" + i.ToString(), true).First();
                otherClassBox.Enabled = false;
                otherClassBox.Text = "";
                otherClassBox.Items.Clear();

                ComboBox otherPropBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                otherPropBox.Enabled = false;
                otherPropBox.Text = "";
                otherPropBox.Items.Clear();
            }

            List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);

            JToken finderJSON = RelationshipFinderClassQuery(inplaceIRIList, degree);

            ComboBox classBox = (ComboBox)this.Controls.Find("class" + degree.ToString(), true).First();
            classBox.Enabled = true;
            classBox.Text = "";
            classBox.Items.Clear();

            Dictionary<string, List<string>> classToProperties = new Dictionary<string, List<string>>() { };
            foreach (var item in finderJSON)
            {
                string itemClass = item["classLabel"]["value"].ToString();
                string itemVal = item["p" + degree.ToString()]["value"].ToString();
                string itemLabel = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);

                if(!classBox.Items.Contains(itemClass))
                    classBox.Items.Add(itemClass);

                if(classToProperties.ContainsKey(itemClass))
                {
                    List<string> currProps = classToProperties[itemClass];
                    if(!currProps.Contains(itemVal))
                    {
                        currProps.Add(itemVal);
                        classToProperties[itemClass] = currProps;
                    }
                }
                else
                {
                    classToProperties[itemClass] = new List<string>() { itemVal };
                }
            }

            classesAndProperties[degree] = classToProperties;
        }

        private async void PopulatePropertyBox(int degree)
        {
            //Clear boxes that have a higher degree
            for (int i = degree + 1; i <= maxDegree; i++)
            {
                ComboBox otherClassBox = (ComboBox)this.Controls.Find("class" + i.ToString(), true).First();
                otherClassBox.Enabled = false;
                otherClassBox.Text = "";
                otherClassBox.Items.Clear();

                ComboBox otherPropBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                otherPropBox.Enabled = false;
                otherPropBox.Text = "";
                otherPropBox.Items.Clear();
            }

            List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);

            ComboBox propBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
            propBox.Enabled = true;
            propBox.Text = "";
            propBox.Items.Clear();

            ComboBox classBox = (ComboBox)this.Controls.Find("class" + degree.ToString(), true).First();
            string selectedClass = (string)classBox.SelectedItem;
            propBox.Items.AddRange(classesAndProperties[degree][selectedClass].ToArray());
        }

        private JToken RelationshipFinderClassQuery(List<string> inplaceIRIList, int relationDegree)
        {
            string selectParam = "?p" + relationDegree.ToString();
            char[] delimPipe = { '|' };

            string relationFinderQuery = "SELECT distinct ?classLabel " + selectParam + " WHERE { ";

            if (relationDegree == 1)
            {
                relationFinderQuery += "{ ?place ?p1 ?o1. ?place a ?class. ?class rdfs:label ?classLabel }";
                relationFinderQuery += " UNION ";
                relationFinderQuery += "{ ?o1 ?p1 ?place. ?o1 a ?oclass. ?oclass rdfs:label ?classLabel }";
            }
            else
            {
                for (int index = 0; index < relationDegree-1; index++)
                {
                    int currDegree = index + 1;
                    ComboBox currentBox = (ComboBox)this.Controls.Find("prop" + currDegree.ToString(), true).First();
                    string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                    string oValHigh = "?o" + currDegree.ToString();

                    relationFinderQuery += "{ " + oValLow + " <" + currentBox.Text.Split(delimPipe).Last().Trim() + "> " + oValHigh + ". }";
                    relationFinderQuery += " UNION ";
                    relationFinderQuery += "{ " + oValHigh + " <" + currentBox.Text.Split(delimPipe).Last().Trim() + "> " + oValLow + ". }";
                }

                string oValLowCurr = "?o" + (relationDegree-1).ToString();
                string oValHighCurr = "?o" + relationDegree.ToString();
                string pVal = "?p" + relationDegree.ToString();

                relationFinderQuery += " { " + oValLowCurr + " " + pVal + " " + oValHighCurr + ". " + oValLowCurr + " a ?oclass. ?oclass rdfs:label ?classLabel }";
            }

            relationFinderQuery += " VALUES ?place { ";

            foreach (var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "} }";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);
        }

        private void ClassChanged(object sender, EventArgs e)
        {
            ComboBox classBox = (ComboBox)sender;
            int degree = int.Parse(classBox.Name.Replace("class", ""));

            PopulatePropertyBox(degree);
        }

        private void PropertyChanged(object sender, EventArgs e)
        {
            ComboBox propBox = (ComboBox)sender;
            int degree = int.Parse(propBox.Name.Replace("prop", ""));

            if (degree < maxDegree)
            {
                PopulateClassBox(degree + 1);
            }
        }

        private void AddNewProperty(object sender, MouseEventArgs e)
        {
            int newDegree = maxDegree + 1;

            //Expand the form and move down the button elements
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
            this.addPropertyBtn.Location = new System.Drawing.Point(this.addPropertyBtn.Location.X, this.addPropertyBtn.Location.Y + propertySpacing);
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);
            this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y + propertySpacing);
            this.helpPanel.Size = new System.Drawing.Size(this.helpPanel.Size.Width, this.helpPanel.Size.Height + propertySpacing);

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

            ComboBox classBox = (ComboBox)this.Controls.Find("class" + maxDegree.ToString(), true).First();
            ComboBox classBox_copy = new ComboBox();
            classBox_copy.Enabled = false;
            classBox_copy.Font = classBox.Font;
            classBox_copy.FormattingEnabled = classBox.FormattingEnabled;
            classBox_copy.Location = new System.Drawing.Point(50, classBox.Location.Y + propertySpacing);
            classBox_copy.Name = "class" + newDegree.ToString();
            classBox_copy.Size = classBox.Size;
            classBox_copy.SelectedValueChanged += new System.EventHandler(this.ClassChanged);
            this.Controls.Add(classBox_copy);

            //Populate list options and enable if previous property is selected
            if (propBox.Text != "")
            {
                classBox_copy.Enabled = true;
                PopulateClassBox(newDegree);
            }

            maxDegree++;
        }

        private void RunTraverseGraph(object sender, EventArgs e)
        {
            if (class1.Text == "")
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

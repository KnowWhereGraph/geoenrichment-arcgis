using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KWG_Geoenrichment
{
    public partial class TraverseKnowledgeGraph : Form
    {
        private GeoenrichmentForm originalWindow;
        private string currentEndpoint;
        private List<string> entityVals;
        private int returnIndex; //TODO

        protected int maxDegree = 1;

        private int propertySpacing = 50;

        private readonly string helpText = "Select a geography feature from the first box.You may use successive boxes to explore additional information about that feature.\n\n" +
            "Select \"Explore Further\" to expand your exploration, or \"Add Content\" to add the feature to your new Feature Class.\n\n" +
            "You can return to this menu multiple times to either learn more about your selected feature, or to explore additional feature types."; //TODO

        //Initializes the form
        public TraverseKnowledgeGraph(GeoenrichmentForm gf, string endpoint, List<string> entities, string entityClassLabel, int originalIndex)
        {
            InitializeComponent();

            originalWindow = gf;
            currentEndpoint = endpoint;
            entityVals = entities;
            returnIndex = originalIndex;

            exploreProperties.Text = "Explore " + entityClassLabel + " Properties";

            PopulatePropertyBox(1);
        }

        //Populates the property lists at each level of exploration
        private async void PopulatePropertyBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            Dictionary<string, string> properties = new Dictionary<string, string>() { { "", "" } };

            for (int j = 0; j < entityVals.Count; j++)
            {
                var propQuery = "select distinct ?prop ?label where {  ";

                for (int i = 1; i < degree + 1; i++)
                {
                    ComboBox prevValueBox = (i > 1) ? (ComboBox)this.Controls.Find("value" + (i - 1).ToString(), true).First() : null;
                    ComboBox propBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();

                    if (i == 1)
                    {
                        string propVal = (i == degree) ? "?prop" : propBox.SelectedValue.ToString();

                        propQuery += "?entity " + propVal + " ?val1. ";
                    }
                    else
                    {
                        string prevVal = "?val" + (i - 1).ToString();
                        string classVal = prevValueBox.SelectedValue.ToString();
                        string propVal = (i == degree) ? "?prop" : propBox.SelectedValue.ToString();
                        string newVal = "?val" + i.ToString();

                        propQuery += prevVal + " a " + classVal + "; " + propVal + " " + newVal + ". ";
                    }
                }
                propQuery += "optional {?prop rdfs:label ?label} " + entityVals[j] + "}";

                //TODO::Lock buttons
                propLoading.Visible = true;
                string error = await QueuedTask.Run(() =>
                {
                    try
                    {
                        JToken propResults = queryClass.SubmitQuery(currentEndpoint, propQuery);

                        foreach (var item in propResults)
                        {
                            string predicate = queryClass.IRIToPrefix(item["prop"]["value"].ToString());
                            string pLabel = (item["label"] != null) ? queryClass.IRIToPrefix(item["label"]["value"].ToString()) : predicate;

                            if (!properties.ContainsKey(predicate))
                            {
                                properties[predicate] = pLabel;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return "prp";
                    }

                    return "";
                });

                if (error != "")
                {
                    if (degree > 1)
                    {
                        //Reset the value that produced the property error
                        ComboBox prevValueBox = (ComboBox)this.Controls.Find("value" + (degree - 1).ToString(), true).First();
                        prevValueBox.SelectedValue = "";
                    }
                    queryClass.ReportGraphError(error);

                    if (degree == 1)
                    {
                        //If the original feature of interest class caused the error, exit out of Traverse window
                        originalWindow.Show();
                        Close();
                    }

                    propLoading.Visible = false;
                    //TODO::Unlock buttons

                    return;
                }
            }

            propLoading.Visible = false;
            //TODO::Unlock buttons

            ComboBox currPropBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
            currPropBox.DataSource = new BindingSource(properties.OrderBy(key => key.Value), null);
            currPropBox.DropDownWidth = properties.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, currPropBox.Font).Width);
            currPropBox.Enabled = true;
        }

        //Populates the value lists at each level of exploration
        private async void PopulateValueBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            ComboBox currValueBox = (ComboBox)this.Controls.Find("value" + degree.ToString(), true).First();
            Dictionary<string, string> values = new Dictionary<string, string>() { { "", "" } };

            for (int j = 0; j < entityVals.Count; j++)
            {
                var valueQuery = "select distinct ?type ?label where {  ";

                for (int i = 1; i < degree + 1; i++)
                {
                    ComboBox prevValueBox = (i > 1) ? (ComboBox)this.Controls.Find("value" + (i - 1).ToString(), true).First() : null;
                    ComboBox propBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();

                    if (i == 1)
                    {
                        string propVal = propBox.SelectedValue.ToString();

                        valueQuery += "?entity " + propVal + " ?val1. ";
                    }
                    else
                    {
                        string prevVal = "?val" + (i - 1).ToString();
                        string classVal = prevValueBox.SelectedValue.ToString();
                        string propVal = propBox.SelectedValue.ToString();
                        string newVal = "?val" + i.ToString();

                        valueQuery += prevVal + " a " + classVal + "; " + propVal + " " + newVal + ". ";
                    }
                }
                valueQuery += "?val" + degree.ToString() + " a ?type. ";
                valueQuery += "optional {?type rdfs:label ?label} " + entityVals[j] + "}";

                //TODO::Lock buttons
                valueLoading.Visible = true;
                string error = await QueuedTask.Run(() =>
                {
                    try
                    {
                        JToken valueResults = queryClass.SubmitQuery(currentEndpoint, valueQuery);

                        foreach (var item in valueResults)
                        {
                            string oType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                            string oLabel = (item["label"] != null) ? queryClass.IRIToPrefix(item["label"]["value"].ToString()) : oType;

                            if (!values.ContainsKey(oType))
                            {
                                values[oType] = oLabel;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return "val";
                    }

                    return "";
                });

                if (error != "")
                {
                    ComboBox propBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
                    propBox.SelectedValue = "";
                    queryClass.ReportGraphError(error);

                    valueLoading.Visible = false;
                    //TODO::Unlock buttons

                    return;
                }
            }

            bool keepBoxEnabled = true;
            if (values.Count == 1)
            {
                keepBoxEnabled = false;
                values = new Dictionary<string, string>() { { "LiteralDataFound", "Literal Data Found" } };
            }

            valueLoading.Visible = false;
            //TODO::Unlock buttons

            currValueBox.DataSource = new BindingSource(values.OrderBy(key => key.Value), null);
            currValueBox.DropDownWidth = values.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, currValueBox.Font).Width);
            currValueBox.Enabled = keepBoxEnabled;
            if (values.Count == 1)
                this.BeginInvoke(new Action(() => { currValueBox.Select(0, 0); })); //This unhighlights the text after the box is disabled so Literal Data Found can actually be read
        }

        //Determines what happens when a user selects a property box value
        private void OnPropBoxChange(object sender, EventArgs e)
        {
            ComboBox propBox = (ComboBox)sender;
            int degree = int.Parse(propBox.Name.Replace("prop", ""));

            //We need to clear out any boxes later in the chain
            ComboBox valueBox = (ComboBox)this.Controls.Find("value" + degree.ToString(), true).First();
            valueBox.SelectedValue = "";
            valueBox.Enabled = false;
            if (degree < maxDegree)
                RemoveRows(degree);

            if (propBox.SelectedValue != null && propBox.SelectedValue.ToString() != "")
            {
                PopulateValueBox(degree);
            }
        }

        //Determines what happens when a user selects a value box value
        private void OnValueBoxChange(object sender, EventArgs e)
        {
            ComboBox valueBox = (ComboBox)sender;
            int degree = int.Parse(valueBox.Name.Replace("value", ""));

            //We need to clear out any boxes later in the chain
            if (degree < maxDegree)
                RemoveRows(degree);

            if (valueBox.SelectedValue != null && valueBox.SelectedValue.ToString() != "")
            {
                exploreFurtherBtn.Enabled = (valueBox.SelectedValue.ToString() != "LiteralDataFound") ? true : false;
            }
        }

        //If a user changes a box on an earlier level, we remove the property and value boxes at all levels above that
        private void RemoveRows(int newMax)
        {
            for (int i = newMax + 1; i <= maxDegree; i++)
            {
                //resize window and move main UI up
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - propertySpacing);
                this.exploreFurtherBtn.Location = new System.Drawing.Point(this.exploreFurtherBtn.Location.X, this.exploreFurtherBtn.Location.Y - propertySpacing);
                this.addValueBtn.Location = new System.Drawing.Point(this.addValueBtn.Location.X, this.addValueBtn.Location.Y - propertySpacing);
                this.propertyValueLabel.Location = new System.Drawing.Point(this.propertyValueLabel.Location.X, this.propertyValueLabel.Location.Y - propertySpacing);
                //TODO::Move properties that were added
                this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y - propertySpacing);
                this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y - propertySpacing);

                //delete property boxes for this degree
                ComboBox propBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                ComboBox valueBox = (ComboBox)this.Controls.Find("value" + i.ToString(), true).First();

                this.Controls.Remove(propBox);
                this.Controls.Remove(valueBox);
            }

            maxDegree = newMax;
        }

        //Expands the exploration of the selected value box by adding a new row of property/value boxes
        private void LearnMore(object sender, EventArgs e)
        {
            exploreFurtherBtn.Enabled = false;
            int newDegree = maxDegree + 1;

            //Expand the form and move down the button elements
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
            this.exploreFurtherBtn.Location = new System.Drawing.Point(this.exploreFurtherBtn.Location.X, this.exploreFurtherBtn.Location.Y + propertySpacing);
            this.addValueBtn.Location = new System.Drawing.Point(this.addValueBtn.Location.X, this.addValueBtn.Location.Y + propertySpacing);
            this.propertyValueLabel.Location = new System.Drawing.Point(this.propertyValueLabel.Location.X, this.propertyValueLabel.Location.Y + propertySpacing);
            //TODO::Move properties that were added
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);
            this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y + propertySpacing);

            ComboBox propBox = (ComboBox)this.Controls.Find("prop" + maxDegree.ToString(), true).First();
            ComboBox propBox_copy = new ComboBox();
            propBox_copy.Enabled = false;
            propBox_copy.Font = propBox.Font;
            propBox_copy.FormattingEnabled = propBox.FormattingEnabled;
            propBox_copy.Location = new System.Drawing.Point(propBox.Location.X, propBox.Location.Y + propertySpacing);
            propBox_copy.Name = "prop" + newDegree.ToString();
            propBox_copy.Size = propBox.Size;
            propBox_copy.DisplayMember = "Value";
            propBox_copy.ValueMember = "Key";
            propBox_copy.SelectedIndexChanged += new System.EventHandler(this.OnPropBoxChange);
            this.Controls.Add(propBox_copy);

            ComboBox valueBox = (ComboBox)this.Controls.Find("value" + maxDegree.ToString(), true).First();
            ComboBox valueBox_copy = new ComboBox();
            valueBox_copy.Enabled = false;
            valueBox_copy.Font = valueBox.Font;
            valueBox_copy.FormattingEnabled = valueBox.FormattingEnabled;
            valueBox_copy.Location = new System.Drawing.Point(valueBox.Location.X, valueBox.Location.Y + propertySpacing);
            valueBox_copy.Name = "value" + newDegree.ToString();
            valueBox_copy.Size = valueBox.Size;
            valueBox_copy.DisplayMember = "Value";
            valueBox_copy.ValueMember = "Key";
            valueBox_copy.SelectedIndexChanged += new System.EventHandler(this.OnValueBoxChange);
            this.Controls.Add(valueBox_copy);

            //Populate list options and enable
            PopulatePropertyBox(newDegree);

            maxDegree++;
        }

        private void AddValueToList(object sender, EventArgs e)
        {

        } //TODO

        private void RunTraverseGraph(object sender, EventArgs e)
        {
            /*if (subject1.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }*/
            //else
            //{
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
            //}
        } //TODO

        //Toggles the help menu pop up
        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            var helpWindow = new KWGHelp(helpText);
            helpWindow.Show();
        }

        //Closes the Traverse window and returns the user to the original window
        private void CloseWindow(object sender, EventArgs e)
        {
            originalWindow.Show();
            foreach (Control ctrl in originalWindow.Controls)
            {
                ctrl.Enabled = true;
            }
            originalWindow.CheckCanRunGeoenrichment();
            Close();
        }

        //This is a catch all in case the window gets closed prematurely
        private void TraverseKnowledgeGraph_FormClosing(object sender, FormClosingEventArgs e)
        {
            originalWindow.Show();
            foreach (Control ctrl in originalWindow.Controls)
            {
                ctrl.Enabled = true;
            }
            originalWindow.CheckCanRunGeoenrichment();
        }
    }
}

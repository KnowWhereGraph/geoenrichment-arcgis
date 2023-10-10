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
        private int returnIndex;

        protected int maxDegree = 1;

        private int propertySpacing = 50;
        private int propertyMargins = 25;
        private Dictionary<string, List<string>> selectedProperties;
        private readonly Dictionary<string, string> mergeRules = new Dictionary<string, string>() {
            { "concat", "Concatenate values together with a \" | \"" },
            { "first", "Get the first value found" },
            { "count", "Get the number of values found" },
            { "total", "Get the total of all values (numeric)" },
            { "high", "Get the highest value (numeric)" },
            { "low", "Get the lowest value (numeric)" },
            { "avg", "Get the average of all values (numeric)" },
            { "stdev", "Get the standard deviation of all values (numeric)" },
        };

        private readonly string helpText = "Use the 'Select Property' dropdown to choose a property for your Feature of Interest, to learn additional information about it. Then use the 'Select Value' dropdown to choose a value for that property.\n\n" +
            "If the return data from the value is expected to result in textual/numerical data, the 'Select Value' dropdown will auto-populate with a note explaining this.\n\n" +
            "If the return data results in an object, then the 'Explore Further' button can be used to add new property/value dropdowns to further explore the properties of that object. The 'Explore Further' button can be used indefinitely, or until textual/numerical data is reached.\n\n" +
            "'Add Value' will take the value of the last 'Select Value' dropdown and add it to the 'Selected Property Values' list. Once added you can specify a column name for that value which will be used in the final Feature Layer table. There is also a dropdown to decide how data will be merged in the final table (i.e. if you expect the value to have multiple data points per specific entity of your Feature of Interest).\n\n" +
            "To complete this form and submit your properties back to the main Geoenrichment window, select 'Add Properties'.";

        //Initializes the form
        public TraverseKnowledgeGraph(GeoenrichmentForm gf, string endpoint, List<string> entities, string entityClassLabel, int originalIndex, List<List<string>> previouslySelected)
        {
            InitializeComponent();
            originalWindow = gf;
            currentEndpoint = endpoint;
            entityVals = entities;
            returnIndex = originalIndex;
            selectedProperties = new Dictionary<string, List<string>>();

            //Load previous selected properties when returning if applicable
            foreach (var prop in previouslySelected)
            {
                string label = prop[0];
                List<string> uris = prop[1].Split("||").ToList();
                string column = prop[2];
                string merge = prop[3];
                AddPrevValueToList(label, uris, column, merge);
            }
            exploreProperties.Text = "Explore " + entityClassLabel + " Properties";

            PopulatePropertyBox(1);
        }

        //Populates the property lists at each level of exploration
        private async void PopulatePropertyBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            Dictionary<string, string> properties = new Dictionary<string, string>() { { "", "" } };

            DisableActionButtons();
            propLoading.Visible = true;
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
                    EnableActionButtons();

                    return;
                }
            }

            propLoading.Visible = false;
            EnableActionButtons();

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

            DisableActionButtons();
            valueLoading.Visible = true;
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
                    EnableActionButtons();

                    return;
                }
            }

            bool keepBoxEnabled = true;
            if (values.Count == 1)
            {
                keepBoxEnabled = false;
                values = new Dictionary<string, string>() { { "LiteralDataFound", "Textual or Numerical data found" } };
            }

            valueLoading.Visible = false;
            EnableActionButtons();

            currValueBox.DataSource = new BindingSource(values.OrderBy(key => key.Value), null);
            currValueBox.DropDownWidth = values.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, currValueBox.Font).Width);
            currValueBox.Enabled = keepBoxEnabled;
            if (values.Count == 1)
                this.BeginInvoke(new Action(() => { currValueBox.Select(0, 0); })); //This unhighlights the text after the box is disabled so Literal Data Found can actually be read
        }

        //Disable buttons while data is loading
        private void DisableActionButtons()
        {
            exploreFurtherBtn.Enabled = false;
            addValueBtn.Enabled = false;
        }

        //Check if buttons should be enabled
        private void EnableActionButtons()
        {
            ComboBox lastValuebox = (ComboBox)this.Controls.Find("value" + maxDegree, true).First();
            if (lastValuebox.SelectedValue != null && lastValuebox.SelectedValue.ToString() != "")
            {
                exploreFurtherBtn.Enabled = (lastValuebox.SelectedValue.ToString() != "LiteralDataFound") ? true : false;
                addValueBtn.Enabled = true;
            }
            else
            {
                DisableActionButtons();
            }
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

            EnableActionButtons();
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
                for (int j = 1; j <= selectedProperties.Count(); j++)
                {
                    Label propertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + j.ToString(), true).First();
                    TextBox propertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + j.ToString(), true).First();
                    ComboBox propertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + j.ToString(), true).First();
                    Button removeProperty = (Button)this.Controls.Find("removeAddedProperty" + j.ToString(), true).First();

                    propertyLabel.Location = new System.Drawing.Point(propertyLabel.Location.X, propertyLabel.Location.Y - propertySpacing);
                    propertyColumn.Location = new System.Drawing.Point(propertyColumn.Location.X, propertyColumn.Location.Y - propertySpacing);
                    propertyMerge.Location = new System.Drawing.Point(propertyMerge.Location.X, propertyMerge.Location.Y - propertySpacing);
                    removeProperty.Location = new System.Drawing.Point(removeProperty.Location.X, removeProperty.Location.Y - propertySpacing);
                }
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
            for (int j = 1; j <= selectedProperties.Count(); j++)
            {
                Label propertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + j.ToString(), true).First();
                TextBox propertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + j.ToString(), true).First();
                ComboBox propertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + j.ToString(), true).First();
                Button removeProperty = (Button)this.Controls.Find("removeAddedProperty" + j.ToString(), true).First();

                propertyLabel.Location = new System.Drawing.Point(propertyLabel.Location.X, propertyLabel.Location.Y + propertySpacing);
                propertyColumn.Location = new System.Drawing.Point(propertyColumn.Location.X, propertyColumn.Location.Y + propertySpacing);
                propertyMerge.Location = new System.Drawing.Point(propertyMerge.Location.X, propertyMerge.Location.Y + propertySpacing);
                removeProperty.Location = new System.Drawing.Point(removeProperty.Location.X, removeProperty.Location.Y + propertySpacing);
            }
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

        //Add a selected property value to the final list
        private void AddValueToList(object sender, EventArgs e)
        {
            //Get the full chain of Key Value pairs
            List<string> uriList = new List<string>();
            List<string> labelList = new List<string>();
            for (int i = 1; i < maxDegree + 1; i++)
            {
                ComboBox propBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                if (propBox.Text != null && propBox.Text != "")
                {
                    uriList.Add(propBox.SelectedValue.ToString());
                    labelList.Add(propBox.Text);
                }

                ComboBox valueBox = (ComboBox)this.Controls.Find("value" + i.ToString(), true).First();
                if (valueBox.Text != null && valueBox.Text != "")
                {
                    uriList.Add(valueBox.SelectedValue.ToString());
                    labelList.Add(valueBox.Text);
                }
            }
            string labelJoined = String.Join(" -> ", labelList);

            if (!selectedProperties.ContainsKey(labelJoined))
            {
                //Expand the form and move down the button elements
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
                for (int j = 1; j <= selectedProperties.Count(); j++)
                {
                    Label propertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + j.ToString(), true).First();
                    TextBox propertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + j.ToString(), true).First();
                    ComboBox propertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + j.ToString(), true).First();
                    Button removeProperty = (Button)this.Controls.Find("removeAddedProperty" + j.ToString(), true).First();

                    propertyLabel.Location = new System.Drawing.Point(propertyLabel.Location.X, propertyLabel.Location.Y + propertySpacing);
                    propertyColumn.Location = new System.Drawing.Point(propertyColumn.Location.X, propertyColumn.Location.Y + propertySpacing);
                    propertyMerge.Location = new System.Drawing.Point(propertyMerge.Location.X, propertyMerge.Location.Y + propertySpacing);
                    removeProperty.Location = new System.Drawing.Point(removeProperty.Location.X, removeProperty.Location.Y + propertySpacing);
                }
                this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);
                this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y + propertySpacing);

                //Add new property value
                selectedProperties.Add(labelJoined, uriList);

                //Add chain label to property list
                Label labelObj = new Label();
                labelObj.AutoSize = propLabel.AutoSize;
                labelObj.BackColor = Color.FromName("ActiveCaption");
                labelObj.Font = propLabel.Font;
                labelObj.ForeColor = propLabel.ForeColor;
                labelObj.Margin = propLabel.Margin;
                labelObj.Name = "addedPropertyLabel" + selectedProperties.Count.ToString();
                labelObj.Size = propLabel.Size;
                labelObj.MaximumSize = new Size(300, 26);
                labelObj.AutoEllipsis = true;
                labelObj.Text = labelJoined;
                Controls.Add(labelObj);

                //Since the chain label can be very long, lets make a tooltip for it
                ToolTip labelFullTooltip = new ToolTip();
                labelFullTooltip.ToolTipIcon = ToolTipIcon.Info;
                labelFullTooltip.IsBalloon = true;
                labelFullTooltip.ShowAlways = true;
                labelFullTooltip.SetToolTip(labelObj, labelJoined);

                //Add column name textbox
                TextBox columnText = new TextBox();
                columnText.Font = prop1.Font;
                columnText.Name = "addedPropertyColumn" + selectedProperties.Count.ToString();
                columnText.Text = (uriList.Last() == "LiteralDataFound") ? labelList[labelList.Count - 2] : labelList.Last();
                columnText.Size = new System.Drawing.Size(300, 26);
                Controls.Add(columnText);

                //Add the merge dropdown
                ComboBox mergeBox = new ComboBox();
                mergeBox.Font = prop1.Font;
                mergeBox.FormattingEnabled = prop1.FormattingEnabled;
                mergeBox.Name = "addedPropertyMerge" + selectedProperties.Count.ToString();
                mergeBox.Size = new System.Drawing.Size(300, 26);
                mergeBox.DisplayMember = "Value";
                mergeBox.ValueMember = "Key";
                mergeBox.DataSource = new BindingSource(mergeRules, null);
                mergeBox.DropDownWidth = mergeRules.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, mergeBox.Font).Width);
                Controls.Add(mergeBox);

                //Add the remove property button
                Button removeContent = new Button();
                removeContent.BackColor = System.Drawing.Color.Transparent;
                removeContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                removeContent.Cursor = System.Windows.Forms.Cursors.Hand;
                removeContent.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                removeContent.FlatAppearance.BorderSize = 0;
                removeContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                removeContent.Image = global::KWG_Geoenrichment.Properties.Resources.x;
                removeContent.Name = "removeAddedProperty" + selectedProperties.Count.ToString();
                removeContent.Size = new System.Drawing.Size(26, 26);
                removeContent.UseVisualStyleBackColor = false;
                removeContent.Click += new System.EventHandler(this.RemoveValueFromList);
                Controls.Add(removeContent);

                //Move the label, add property button, and remove class button
                labelObj.Location = new System.Drawing.Point(propertyValueLabel.Location.X, propertyValueLabel.Location.Y + propertySpacing);
                columnText.Location = new System.Drawing.Point(labelObj.Location.X + propertyMargins + labelObj.MaximumSize.Width, labelObj.Location.Y);
                mergeBox.Location = new System.Drawing.Point(columnText.Location.X + propertyMargins + columnText.Width, labelObj.Location.Y);
                removeContent.Location = new System.Drawing.Point(mergeBox.Location.X + propertyMargins + mergeBox.Width, labelObj.Location.Y);
            }
            else
            {
                MessageBox.Show($@"Selected property value is already in the list!");
            }

            //Clear all rows
            ComboBox propBoxOne = (ComboBox)this.Controls.Find("prop1", true).First();
            ComboBox valueBoxOne = (ComboBox)this.Controls.Find("value1", true).First();
            propBoxOne.SelectedValue = "";
            valueBoxOne.SelectedValue = "";
            valueBoxOne.Enabled = false;
            RemoveRows(1);
        }

        //Alt function for adding a previous selected property value to the final list
        //A lot of duplicate code
        private void AddPrevValueToList(string labelJoined, List<string> uriList, string column, string merge)
        {
            //Expand the form and move down the button elements
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
            for (int j = 1; j <= selectedProperties.Count(); j++)
            {
                Label propertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + j.ToString(), true).First();
                TextBox propertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + j.ToString(), true).First();
                ComboBox propertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + j.ToString(), true).First();
                Button removeProperty = (Button)this.Controls.Find("removeAddedProperty" + j.ToString(), true).First();

                propertyLabel.Location = new System.Drawing.Point(propertyLabel.Location.X, propertyLabel.Location.Y + propertySpacing);
                propertyColumn.Location = new System.Drawing.Point(propertyColumn.Location.X, propertyColumn.Location.Y + propertySpacing);
                propertyMerge.Location = new System.Drawing.Point(propertyMerge.Location.X, propertyMerge.Location.Y + propertySpacing);
                removeProperty.Location = new System.Drawing.Point(removeProperty.Location.X, removeProperty.Location.Y + propertySpacing);
            }
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);
            this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y + propertySpacing);

            //Add new property value
            selectedProperties.Add(labelJoined, uriList);

            //Add chain label to property list
            Label labelObj = new Label();
            labelObj.AutoSize = propLabel.AutoSize;
            labelObj.BackColor = Color.FromName("ActiveCaption");
            labelObj.Font = propLabel.Font;
            labelObj.ForeColor = propLabel.ForeColor;
            labelObj.Margin = propLabel.Margin;
            labelObj.Name = "addedPropertyLabel" + selectedProperties.Count.ToString();
            labelObj.Size = propLabel.Size;
            labelObj.MaximumSize = new Size(300, 26);
            labelObj.AutoEllipsis = true;
            labelObj.Text = labelJoined;
            Controls.Add(labelObj);

            //Since the chain label can be very long, lets make a tooltip for it
            ToolTip labelFullTooltip = new ToolTip();
            labelFullTooltip.ToolTipIcon = ToolTipIcon.Info;
            labelFullTooltip.IsBalloon = true;
            labelFullTooltip.ShowAlways = true;
            labelFullTooltip.SetToolTip(labelObj, labelJoined);

            //Add column name textbox
            TextBox columnText = new TextBox();
            columnText.Font = prop1.Font;
            columnText.Name = "addedPropertyColumn" + selectedProperties.Count.ToString();
            columnText.Text = column;
            columnText.Size = new System.Drawing.Size(300, 26);
            Controls.Add(columnText);

            //Add the merge dropdown
            ComboBox mergeBox = new ComboBox();
            mergeBox.Font = prop1.Font;
            mergeBox.FormattingEnabled = prop1.FormattingEnabled;
            mergeBox.Name = "addedPropertyMerge" + selectedProperties.Count.ToString();
            mergeBox.Size = new System.Drawing.Size(300, 26);
            mergeBox.DisplayMember = "Value";
            mergeBox.ValueMember = "Key";
            mergeBox.DataSource = new BindingSource(mergeRules, null);
            mergeBox.DropDownWidth = mergeRules.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, mergeBox.Font).Width);
            mergeBox.SelectedValue = merge;
            Controls.Add(mergeBox);

            //Add the remove property button
            Button removeContent = new Button();
            removeContent.BackColor = System.Drawing.Color.Transparent;
            removeContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            removeContent.Cursor = System.Windows.Forms.Cursors.Hand;
            removeContent.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            removeContent.FlatAppearance.BorderSize = 0;
            removeContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            removeContent.Image = global::KWG_Geoenrichment.Properties.Resources.x;
            removeContent.Name = "removeAddedProperty" + selectedProperties.Count.ToString();
            removeContent.Size = new System.Drawing.Size(26, 26);
            removeContent.UseVisualStyleBackColor = false;
            removeContent.Click += new System.EventHandler(this.RemoveValueFromList);
            Controls.Add(removeContent);

            //Move the label, add property button, and remove class button
            labelObj.Location = new System.Drawing.Point(propertyValueLabel.Location.X, propertyValueLabel.Location.Y + propertySpacing);
            columnText.Location = new System.Drawing.Point(labelObj.Location.X + propertyMargins + labelObj.MaximumSize.Width, labelObj.Location.Y);
            mergeBox.Location = new System.Drawing.Point(columnText.Location.X + propertyMargins + columnText.Width, labelObj.Location.Y);
            removeContent.Location = new System.Drawing.Point(mergeBox.Location.X + propertyMargins + mergeBox.Width, labelObj.Location.Y);
        }

        //Remove a selected property value to the final list
        private void RemoveValueFromList(object sender, EventArgs e)
        {
            //get index
            Button clickedButton = sender as Button;
            string buttonText = clickedButton.Name;
            string index = buttonText.Replace("removeAddedProperty", "");
            int idx = Int32.Parse(index);

            //remove content from ui
            Label oldPropertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + idx, true).First();
            TextBox oldPropertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + idx, true).First();
            ComboBox oldPropertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + idx, true).First();

            //remove content from array
            int oldSize = selectedProperties.Count;
            string oldLabelInx = oldPropertyLabel.Text;
            selectedProperties.Remove(oldLabelInx);

            this.Controls.Remove(oldPropertyLabel);
            this.Controls.Remove(oldPropertyColumn);
            this.Controls.Remove(oldPropertyMerge);
            this.Controls.Remove(clickedButton);

            //Resize window and move main UI up
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - propertySpacing);
            for (int j = 1; j <= oldSize; j++)
            {
                //This is the row we just deleted, so skip it
                if (j == idx)
                    continue;

                Label propertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + j.ToString(), true).First();
                TextBox propertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + j.ToString(), true).First();
                ComboBox propertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + j.ToString(), true).First();
                Button removeProperty = (Button)this.Controls.Find("removeAddedProperty" + j.ToString(), true).First();

                if (j < idx)
                {
                    //UI elements below the deleted row need to be moved up
                    propertyLabel.Location = new System.Drawing.Point(propertyLabel.Location.X, propertyLabel.Location.Y - propertySpacing);
                    propertyColumn.Location = new System.Drawing.Point(propertyColumn.Location.X, propertyColumn.Location.Y - propertySpacing);
                    propertyMerge.Location = new System.Drawing.Point(propertyMerge.Location.X, propertyMerge.Location.Y - propertySpacing);
                    removeProperty.Location = new System.Drawing.Point(removeProperty.Location.X, removeProperty.Location.Y - propertySpacing);
                }
                else
                {
                    //UI elements above the deleted row need to be reindexed
                    propertyLabel.Name = "addedPropertyLabel" + (j - 1).ToString();
                    propertyColumn.Name = "addedPropertyColumn" + (j - 1).ToString();
                    propertyMerge.Name = "addedPropertyMerge" + (j - 1).ToString();
                    removeProperty.Name = "removeAddedProperty" + (j - 1).ToString();
                }
            }
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y - propertySpacing);
            this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y - propertySpacing);
        }

        //Close window, and return to main window with the selected property data
        private void RunTraverseGraph(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> propertyDetails = new Dictionary<string, List<string>>();
            //For each property, gather the column name and merge rule, and store information into new List
            for (int j = 1; j <= selectedProperties.Count(); j++)
            {
                Label propertyLabel = (Label)this.Controls.Find("addedPropertyLabel" + j.ToString(), true).First();
                TextBox propertyColumn = (TextBox)this.Controls.Find("addedPropertyColumn" + j.ToString(), true).First();
                ComboBox propertyMerge = (ComboBox)this.Controls.Find("addedPropertyMerge" + j.ToString(), true).First();

                string labelStr = propertyLabel.Text;
                string columnStr = propertyColumn.Text;
                string mergeStr = propertyMerge.SelectedValue.ToString();

                propertyDetails[labelStr] = new List<string>() { columnStr, mergeStr };
            }

            originalWindow.AddSelectedProperties(returnIndex, selectedProperties, propertyDetails);
            Close();
        }

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

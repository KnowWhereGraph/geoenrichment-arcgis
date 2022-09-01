using ArcGIS.Desktop.Framework.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace KWG_Geoenrichment
{
    public partial class TraverseKnowledgeGraph : Form
    {
        private GeoenrichmentForm originalWindow;
        
        private string currentEndpoint;
        private List<string> entityVals;

        protected int maxDegree = 1;

        private int propertySpacing = 30;

        private readonly string helpText = "Select a geography feature from the first box.You may use successive boxes to explore additional information about that feature.\n\n" +
            "Select \"Explore Further\" to expand your exploration, or \"Add Content\" to add the feature to your new Feature Class.\n\n" +
            "You can return to this menu multiple times to either learn more about your selected feature, or to explore additional feature types.";

        public TraverseKnowledgeGraph(GeoenrichmentForm gf, string endpoint, List<string> entities)
        {
            InitializeComponent();

            originalWindow = gf;
            currentEndpoint = endpoint;
            entityVals = SplitEntityList(entities);

            PopulateClassBox(1);
        }

        private static List<string> SplitEntityList(List<string> originalList)
        {
            var newEntityList = new List<string>();

            for (int i = 0; i < originalList.Count; i += 1000)
            {
                List<string> subList = originalList.GetRange(i, Math.Min(1000, originalList.Count - i));
                newEntityList.Add("values ?entity {" + String.Join(" ", subList) + "}");
            }

            return newEntityList;
        }

        private async void PopulateClassBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            ComboBox classBox = (ComboBox)this.Controls.Find("subject" + degree.ToString(), true).First();
            Dictionary<string, string> classes = new Dictionary<string, string>() { { "", "" } };

            if (degree == 1)
            {
                for (int i = 0; i < entityVals.Count; i++)
                {
                    var typeQuery = "select distinct ?type ?label where { " +
                        "?entity a ?type. " +
                        "?type rdfs:label ?label. " +
                        entityVals[i] +
                    "}";

                    runTraverseBtn.Enabled = false;
                    edgeLoading.Visible = true;
                    string error = await QueuedTask.Run(() =>
                    {
                        try
                        {
                            JToken typeResults = queryClass.SubmitQuery(currentEndpoint, typeQuery);

                            foreach (var item in typeResults)
                            {
                                string cType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                                string cLabel = queryClass.IRIToPrefix(item["label"]["value"].ToString());

                                if (!classes.ContainsKey(cType))
                                {
                                    classes[cType] = cLabel;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return "typ";
                        }

                        return "";
                    });

                    if(error != "")
                    {
                        originalWindow.Show();
                        queryClass.ReportGraphError(error);
                        Close();
                        return;
                    }
                }

                edgeLoading.Visible = false;
                runTraverseBtn.Enabled = true;

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
            classBox.DropDownWidth = classes.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, classBox.Font).Width);
        }

        private async void PopulatePropertyBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            Dictionary<string, string> properties = new Dictionary<string, string>() { { "", "" } };

            for (int j = 0; j < entityVals.Count; j++)
            {
                var propQuery = "select distinct ?p ?label where {  ";

                for (int i = 1; i < degree + 1; i++)
                {
                    ComboBox classBox = (ComboBox)this.Controls.Find("subject" + i.ToString(), true).First();
                    ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + i.ToString(), true).First();
                    string subjectStr = (i == 1) ? "?entity" : "?o" + (i - 1).ToString();
                    string classStr = classBox.SelectedValue.ToString();
                    string predicateStr = (i == degree) ? "?p" : propBox.SelectedValue.ToString();
                    string objectStr = "?o" + i.ToString();

                    propQuery += subjectStr + " a " + classStr + "; " + predicateStr + " " + objectStr + ". ";
                }
                propQuery += "optional {?p rdfs:label ?label} " + entityVals[j] + "}";

                runTraverseBtn.Enabled = false;
                edgeLoading.Visible = true;
                string error = await QueuedTask.Run(() =>
                {
                    try
                    {
                        JToken propResults = queryClass.SubmitQuery(currentEndpoint, propQuery);

                        foreach (var item in propResults)
                        {
                            string predicate = queryClass.IRIToPrefix(item["p"]["value"].ToString());
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
                    ComboBox classBox = (ComboBox)this.Controls.Find("subject" + degree.ToString(), true).First();
                    classBox.SelectedValue = "";
                    if (degree > 1)
                    {
                        ComboBox valBox = (ComboBox)this.Controls.Find("object" + (degree - 1).ToString(), true).First();
                        valBox.SelectedValue = "";
                    }
                    queryClass.ReportGraphError(error);

                    edgeLoading.Visible = false;
                    runTraverseBtn.Enabled = true;

                    return;
                }
            }

            edgeLoading.Visible = false;
            runTraverseBtn.Enabled = true;

            ComboBox currPropBox = (ComboBox)this.Controls.Find("predicate" + degree.ToString(), true).First();
            currPropBox.DataSource = new BindingSource(properties.OrderBy(key => key.Value), null);
            currPropBox.DropDownWidth = properties.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, currPropBox.Font).Width);
            currPropBox.Enabled = true;
        }

        private async void PopulateValueBox(int degree)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            ComboBox currValueBox = (ComboBox)this.Controls.Find("object" + degree.ToString(), true).First();
            Dictionary<string, string> values = new Dictionary<string, string>() { { "", "" } };
            bool keepBoxEnabled = true;

            for (int j = 0; j < entityVals.Count; j++)
            {
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
                valueQuery += "?type rdfs:label ?label. " + entityVals[j] + "}";

                runTraverseBtn.Enabled = false;
                edgeLoading.Visible = true;
                string error = await QueuedTask.Run(() =>
                {
                    try
                    {
                        JToken valueResults = queryClass.SubmitQuery(currentEndpoint, valueQuery);

                        if (valueResults.HasValues)
                        {
                            foreach (var item in valueResults)
                            {
                                string oType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                                string oLabel = queryClass.IRIToPrefix(item["label"]["value"].ToString());

                                if (!values.ContainsKey(oType))
                                {
                                    values[oType] = oLabel;
                                }
                            }
                        }
                        else
                        {
                            keepBoxEnabled = false;
                            values = new Dictionary<string, string>() { { "LiteralDataFound", "Literal Data Found" } };
                        }
                    }
                    catch (Exception ex)
                    {
                        return "cls";
                    }

                    return "";
                });

                if (error != "")
                {
                    ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + degree.ToString(), true).First();
                    propBox.SelectedValue = "";
                    queryClass.ReportGraphError(error);

                    edgeLoading.Visible = false;
                    runTraverseBtn.Enabled = true;

                    return;
                }
            }

            edgeLoading.Visible = false;
            runTraverseBtn.Enabled = true;

            currValueBox.Enabled = keepBoxEnabled;
            currValueBox.DataSource = new BindingSource(values.OrderBy(key => key.Value), null);
            currValueBox.DropDownWidth = values.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, currValueBox.Font).Width);
        }

        private void OnClassBoxChange(object sender, EventArgs e)
        {
            ComboBox classBox = (ComboBox)sender;
            int degree = int.Parse(classBox.Name.Replace("subject", ""));

            //We need to clear out any boxes later in the chain
            ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + degree.ToString(), true).First();
            propBox.SelectedValue = "";
            propBox.Enabled = false;
            ComboBox valueBox = (ComboBox)this.Controls.Find("object" + degree.ToString(), true).First();
            valueBox.SelectedValue = "";
            valueBox.Enabled = false;
            if (degree < maxDegree)
                RemoveRows(degree);

            if (classBox.SelectedValue != null && classBox.SelectedValue.ToString() != "")
            {
                PopulatePropertyBox(degree);
            }
        }

        private void OnPropBoxChange(object sender, EventArgs e)
        {
            ComboBox propBox = (ComboBox)sender;
            int degree = int.Parse(propBox.Name.Replace("predicate", ""));

            //We need to clear out any boxes later in the chain
            ComboBox valueBox = (ComboBox)this.Controls.Find("object" + degree.ToString(), true).First();
            valueBox.SelectedValue = "";
            valueBox.Enabled = false;
            if (degree < maxDegree)
                RemoveRows(degree);

            if (propBox.SelectedValue != null && propBox.SelectedValue.ToString() != "")
            {
                PopulateValueBox(degree);
            }
        }

        private void OnValueBoxChange(object sender, EventArgs e)
        {
            ComboBox valueBox = (ComboBox)sender;
            int degree = int.Parse(valueBox.Name.Replace("object", ""));

            //We need to clear out any boxes later in the chain
            if (degree < maxDegree)
                RemoveRows(degree);

            if (valueBox.SelectedValue != null && valueBox.SelectedValue.ToString() != "")
            {
                addPropertyBtn.Enabled = (valueBox.SelectedValue.ToString() != "LiteralDataFound") ? true : false;
            }
        }

        private void RemoveRows(int newMax)
        {
            for(int i = newMax+1; i <= maxDegree; i++)
            {
                //resize window and move main UI down
                this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - propertySpacing);
                this.addPropertyBtn.Location = new System.Drawing.Point(this.addPropertyBtn.Location.X, this.addPropertyBtn.Location.Y - propertySpacing);
                this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y - propertySpacing);
                this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y - propertySpacing);

                //delete property boxes for this degree
                ComboBox classBox = (ComboBox)this.Controls.Find("subject" + i.ToString(), true).First();
                ComboBox propBox = (ComboBox)this.Controls.Find("predicate" + i.ToString(), true).First();
                ComboBox valueBox = (ComboBox)this.Controls.Find("object" + i.ToString(), true).First();

                this.Controls.Remove(classBox);
                this.Controls.Remove(propBox);
                this.Controls.Remove(valueBox);
            }

            maxDegree = newMax;
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
            var helpWindow = new KWGHelp(helpText);
            helpWindow.Show();
        }

        private void TraverseKnowledgeGraph_FormClosing(object sender, FormClosingEventArgs e)
        {
            //This is a catch all in case the window gets closed prematurely
            originalWindow.Show();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            originalWindow.Show();
            Close();
        }
    }
}

﻿using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComboBox = System.Windows.Forms.ComboBox;

namespace KWG_Geoenrichment
{
    public partial class GeoenrichmentForm : Form
    {
        private string currentRepository;

        private bool gdbFileUploaded = false;
        private GDBProjectItem selectedGDB;

        private bool areaOfInterestDrawn = false;
        private string areaOfInterestPolygon;

        private List<String> entities;
        private List<List<String>> content;
        private Dictionary<string, string> mergeRules = new Dictionary<string, string>() { 
            { "concat", "Concate values together with a \" | \"" },
            { "first", "Get the first value found" },
            { "count", "Get the number of values found" },
            { "total", "Get the total of all values (numeric)" },
            { "high", "Get the highest value (numeric)" },
            { "low", " Get the lowest value (numeric)" },
            { "avg", "Get the average of all values (numeric)" },
            { "stdev", "Get the standard deviation of all values (numeric)" },
        };

        private List<String> shapeTables = new List<String>() {
            "MapPoint",
            "Polyline",
            "Polygon"
        };

        private int contentTotalSpacing = 50;
        private int contentPadding = 11;
        private int helpSpacing = 400;
        private bool helpOpen = true;

        public GeoenrichmentForm()
        {
            InitializeComponent();
            ToggleHelpMenu();

            QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();
            foreach(var endpoint in queryClass.defaultEndpoints)
            {
                knowledgeGraph.Items.Add(endpoint.Key);
            }

            knowledgeGraph.SelectedIndex = 0;
            currentRepository = knowledgeGraph.Text;
            content = new List<List<String>>() { };
        }

        private void OnChangeGraph(object sender, EventArgs e)
        {
            currentRepository = knowledgeGraph.Text;
            CheckCanSelectContent();
            CheckCanRunGeoenrichment();
        }

        private async void UploadGDBFile(object sender, EventArgs e)
        {
            if (areaOfInterestDrawn)
                return;

            var bpf = new BrowseProjectFilter("esri_browseDialogFilters_geodatabases_file");
            bpf.Name = "File Geodatabases";
            var openItemDialog = new OpenItemDialog { BrowseFilter = bpf };

            if ((bool)openItemDialog.ShowDialog())
            {
                selectedGDB = (GDBProjectItem)openItemDialog.Items.First();
                gdbFileUploaded = true;
                openGDBBtn.Text = selectedGDB.Name;
                openGDBBtn.Enabled = false;
                selectAreaBtn.Enabled = false;

                Show();
                contentLoading.Visible = true;

                await QueuedTask.Run(() => SearchForEntities());

                CheckCanSelectContent();
                CheckCanRunGeoenrichment();
                contentLoading.Visible = false;
            }
        }

        private void DrawAreaOfInterest(object sender, EventArgs e)
        {
            if (gdbFileUploaded)
                return;

            FrameworkApplication.SetCurrentToolAsync("KWG_Geoenrichment_DrawPolygon");

            KwgGeoModule.Current.SetActiveForm(this);

            Hide();
        }

        public async void SetDrawnPolygon(string polygonString)
        {
            areaOfInterestPolygon = polygonString;
            areaOfInterestDrawn = true;
            selectAreaBtn.Text = "Area Drawn";
            openGDBBtn.Enabled = false;
            selectAreaBtn.Enabled = false;

            Show();
            contentLoading.Visible = true;

            await QueuedTask.Run(() => SearchForEntities());

            CheckCanSelectContent();
            CheckCanRunGeoenrichment();
            contentLoading.Visible = false;
        }

        public void SearchForEntities()
        {
            var s2CellList = new List<string>() { };
            entities = new List<string>() { };
            var queryClass = KwgGeoModule.Current.GetQueryClass();

            //Get s2Cells related to the polygon
            //TODO:: Change function base on spatial relation
            if (gdbFileUploaded)
            {

            }
            else if (areaOfInterestDrawn)
            {
                var s2Query = "select ?s2Cell where { " +
                    "?adminRegion2 a kwg-ont:AdministrativeRegion_3. " +
                    "?adminRegion2 geo:hasGeometry ?arGeo. " +
                    "?arGeo geo:asWKT ?arWKT. " +
                    "FILTER(geof:sfIntersects(\"" + areaOfInterestPolygon + "\"^^geo:wktLiteral, ?arWKT)). " +

                    "?adminRegion2 kwg-ont:sfContains ?s2Cell. " +
                    "?s2Cell a kwg-ont:KWGCellLevel13. " +
                    "?s2Cell geo:hasGeometry ?s2Geo. " +
                    "?s2Geo geo:asWKT ?s2WKT. " +
                    "FILTER(geof:sfIntersects(\"" + areaOfInterestPolygon + "\"^^geo:wktLiteral, ?s2WKT)). " +
                "}";

                JToken s2Results = queryClass.SubmitQuery(currentRepository, s2Query);

                foreach (var item in s2Results)
                {
                    s2CellList.Add(queryClass.IRIToPrefix(item["s2Cell"]["value"].ToString()));
                }
            }

            var s2CellVals = "values ?s2Cell {" + String.Join(" ", s2CellList) + "}";

            //Lets get our list of entities
            var entityQuery = "select distinct ?entity where { " +
                "{?entity ?p ?s2Cell.} union {?s2Cell ?p ?entity.} " +
                "?entity a geo:Feature. " +
                s2CellVals +
            "}";

            JToken entityResults = queryClass.SubmitQuery(currentRepository, entityQuery);

            foreach (var item in entityResults)
            {
                entities.Add(queryClass.IRIToPrefix(item["entity"]["value"].ToString()));
            }
        }

        public void CheckCanSelectContent()
        {
            if(
                knowledgeGraph.SelectedItem != null && knowledgeGraph.SelectedItem.ToString() != "" &&
                (gdbFileUploaded || areaOfInterestDrawn)
              )
            {
                selectContentBtn.Enabled = true;
            } else
            {
                selectContentBtn.Enabled = false;
            }
        }

        private void SelectContent(object sender, EventArgs e)
        {
            var exploreWindow = new TraverseKnowledgeGraph(this, currentRepository, entities);
            Hide();
            exploreWindow.Show();
        }

        public void AddSelectedContent(List<string> uris, List<string> labels)
        {
            Show();

            //We need to eliminate doubles since the value box in one line is the same as the class box in the next line
            var uniqueUris = new List<string>() { };
            var uniqueLabels = new List<string>() { };

            for(int i=0; i<uris.Count; i++)
            {
                if(
                    i == 0 || 
                    (uris[i] != uris[i-1] && uris[i] != "LiteralDataFound")
                )
                {
                    uniqueUris.Add(uris[i]);
                    uniqueLabels.Add(labels[i]);
                }
            }

            //Capture the data
            content.Add(uniqueUris);

            
            string labelString = String.Join(" -> ", uniqueLabels);
            string columnString = "NoAdditionalData";
            int labelCnt = uniqueLabels.Count;
            if (labelCnt > 1)
                columnString = (labelCnt % 2 == 0) ? uniqueLabels[labelCnt - 1] : uniqueLabels[labelCnt - 2];

            //Add the label
            Label labelObj = new Label();
            labelObj.AutoSize = knowledgeGraphLabel.AutoSize;
            labelObj.BackColor = Color.FromName("ActiveCaption");
            labelObj.Font = knowledgeGraphLabel.Font;
            labelObj.ForeColor = knowledgeGraphLabel.ForeColor;
            labelObj.Margin = knowledgeGraphLabel.Margin;
            labelObj.Name = "contentLabel";
            labelObj.Size = knowledgeGraphLabel.Size;
            labelObj.MaximumSize = new Size(780, 0);
            labelObj.Text = labelString;
            Controls.Add(labelObj);

            //Add column name textbox
            TextBox columnText = new TextBox();
            columnText.BorderStyle = saveLayerAs.BorderStyle;
            columnText.Font = saveLayerAs.Font;
            columnText.Name = "columnName" + content.Count.ToString();
            columnText.Text = columnString;
            columnText.Size = new System.Drawing.Size(200, 26);
            Controls.Add(columnText);

            //Add the merge dropdown
            ComboBox mergeBox = new ComboBox();
            mergeBox.Font = knowledgeGraph.Font;
            mergeBox.FormattingEnabled = knowledgeGraph.FormattingEnabled;
            mergeBox.Name = "mergeRule"+content.Count.ToString();
            mergeBox.Size = new System.Drawing.Size(400, 26);
            mergeBox.DisplayMember = "Value";
            mergeBox.ValueMember = "Key";
            mergeBox.DataSource = new BindingSource(mergeRules, null);
            Controls.Add(mergeBox);

            //Move the label
            labelObj.Location = new System.Drawing.Point(knowledgeGraph.Location.X, knowledgeGraph.Location.Y + contentTotalSpacing);
            int addedHeight = labelObj.Height + contentPadding;

            //Move the merge dropdown and the column text
            columnText.Location = new System.Drawing.Point(labelObj.Location.X, labelObj.Location.Y + labelObj.Height + contentPadding);
            mergeBox.Location = new System.Drawing.Point(labelObj.Location.X + 206, labelObj.Location.Y + labelObj.Height + contentPadding);
            addedHeight += mergeBox.Height + contentPadding;

            //Adjust the total amount of spacing we've moved
            contentTotalSpacing += addedHeight;

            //Move things down
            selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y + addedHeight);
            requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y + addedHeight);
            saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y + addedHeight);
            saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y + addedHeight);
            helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y + addedHeight);
            runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y + addedHeight);
            Height += addedHeight;

            //Disable boxes if needed
            if(columnString == "NoAdditionalData")
            {
                columnText.Enabled = false;
                mergeBox.Enabled = false;
            }


            CheckCanRunGeoenrichment();
        }

        private void OnFeatureNameChage(object sender, EventArgs e)
        {
            CheckCanRunGeoenrichment();
        }

        public void CheckCanRunGeoenrichment()
        {
            if (
                knowledgeGraph.SelectedItem != null && knowledgeGraph.SelectedItem.ToString() != "" &&
                (gdbFileUploaded || areaOfInterestDrawn) &&
                content.Count > 0 &&
                saveLayerAs.Text != ""
              )
            {
                runBtn.Enabled = true;
            }
            else
            {
                runBtn.Enabled = false;
            }
        }

        private async void RunGeoenrichment(object sender, EventArgs e)
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            bool layerFailed = false;
            Dictionary<string, BasicFeatureLayer> tables = new Dictionary<string, BasicFeatureLayer>() { }; //entityType -> layer

            //build the table and its columns
            foreach (var shape in shapeTables)
            {
                string tableName = FeatureClassHelper.ValidateTableName(saveLayerAs.Text + "_" + shape);
                await FeatureClassHelper.CreateFeatureClassLayer(tableName, shape);
                var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == tableName).FirstOrDefault() as BasicFeatureLayer;

                if (fcLayer == null)
                {
                    MessageBox.Show($@"Failed to create {tableName} in the active map");
                    layerFailed = true;
                } else
                {
                    await FeatureClassHelper.AddField(fcLayer, "Label", "TEXT");
                    await FeatureClassHelper.AddField(fcLayer, "URL", "TEXT");
                    tables[shape] = fcLayer;

                }
            }

            if(!layerFailed)
            {
                //Build and run query for base classes
                var finalContent = new Dictionary<string, Dictionary<string, List<string>>>() { }; //entity -> column label -> data
                var finalContentLabels = new Dictionary<string, string>() { }; //entity -> entityLabel
                var finalContentGeometry = new Dictionary<string, string>() { }; //entity -> wkt
                var labelToMergeRule = new Dictionary<string, string>() { }; //column label -> merge rule
                string entityName; string nextEntityName;
                string entityVals = "values ?entity {" + String.Join(" ", entities) + "}";
                for (int j = 0; j < content.Count; j++)
                {
                    //Add the column to our feature tables and track its merge rule
                    ComboBox mergeBox = (ComboBox)this.Controls.Find("mergeRule" + (j+1).ToString(), true).First();
                    string mergeRule = mergeBox.SelectedValue.ToString();
                    string columnLabel = this.Controls.Find("columnName" + (j + 1).ToString(), true).First().Text.Replace(' ', '_') + '_' + mergeRule;
                    labelToMergeRule[columnLabel] = mergeRule;
                    foreach (var shape in shapeTables)
                    {
                        if(!columnLabel.StartsWith("NoAdditionalData"))
                            await FeatureClassHelper.AddField(tables[shape], columnLabel, "TEXT");
                    }

                    var contentResultsQuery = "select distinct ?entity ?entityLabel ?o ?wkt where { ?entity rdfs:label ?entityLabel. ";

                    for (int i = 0; i < content[j].Count; i++)
                    {
                        //Even indices are classes, odd are predicates
                        if (i % 2 == 0)
                        {
                            if (i == 0)
                                entityName = "?entity";
                            else
                                entityName = (i + 1 == content[j].Count) ? "?o" : "?o" + (i / 2).ToString();
                            var className = content[j][i];
                            contentResultsQuery += entityName + " a " + className + ". ";
                        }
                        else
                        {
                            if (i == 1)
                            {
                                entityName = "?entity";
                                nextEntityName = (i + 1 == content[j].Count) ? "?o" : "?o1";
                            }
                            else
                            {
                                entityName = "?o" + (i / 2).ToString();
                                nextEntityName = (i + 1 == content[j].Count) ? "?o" : "?o" + (i / 2 + 1).ToString();
                            }
                            var propName = content[j][i];
                            contentResultsQuery += entityName + " " + propName + " " + nextEntityName + ". ";
                        }
                    }

                    contentResultsQuery += "optional {?entity geo:hasGeometry ?geo. ?geo geo:asWKT ?wkt} " + entityVals + "}";

                    JToken contentResults = queryClass.SubmitQuery(currentRepository, contentResultsQuery);

                    foreach (var item in contentResults)
                    {
                        var entityVal = item["entity"]["value"].ToString();

                        //Check to see if we this entity exists, if not, set it up
                        if (!finalContent.ContainsKey(entityVal))
                            finalContent[entityVal] = new Dictionary<string, List<string>>() { };
                        if (!finalContentLabels.ContainsKey(entityVal))
                            finalContentLabels[entityVal] = item["entityLabel"]["value"].ToString();
                        if (!finalContentGeometry.ContainsKey(entityVal))
                            finalContentGeometry[entityVal] = item["wkt"]["value"].ToString();

                        //Let's prep and store the result content
                        if (!finalContent[entityVal].ContainsKey(columnLabel))
                            finalContent[entityVal][columnLabel] = new List<string>() { };

                        if (item["o"] != null && item["o"]["value"].ToString() != "")
                            finalContent[entityVal][columnLabel].Add(item["o"]["value"].ToString());
                    }
                }

                //Use data to populate table
                await QueuedTask.Run(() =>
                {

                    foreach(var entityPair in finalContent)
                    {
                        string currEntity = entityPair.Key;

                        IGeometryEngine geoEngine = GeometryEngine.Instance;
                        SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(4326);
                        string wkt = finalContentGeometry[currEntity].Replace("<http://www.opengis.net/def/crs/OGC/1.3/CRS84>", "");
                        Geometry geo = geoEngine.ImportFromWKT(0, wkt, sr);

                        InsertCursor cursor = tables[geo.GetType().Name].GetTable().CreateInsertCursor();
                        RowBuffer buff = tables[geo.GetType().Name].GetTable().CreateRowBuffer();

                        buff["Label"] = finalContentLabels[currEntity];
                        buff["URL"] = currEntity;
                        buff["Shape"] = geo;

                        //Add column data based on merge rule
                        foreach(var dataPair in entityPair.Value)
                        {
                            string currLabel = dataPair.Key;
                            if (currLabel.StartsWith("NoAdditionalData"))
                                continue;

                            switch (labelToMergeRule[currLabel])
                            {
                                case "first":
                                    buff[currLabel] = dataPair.Value[0];
                                    break;
                                case "count":
                                    buff[currLabel] = dataPair.Value.Count;
                                    break;
                                case "total":
                                    float total = 0;
                                    foreach(string val in dataPair.Value)
                                    {
                                        total += float.Parse(val);
                                    }
                                    buff[currLabel] = total;
                                    break;
                                case "high":
                                    float high = float.Parse(dataPair.Value[0]);
                                    foreach (string val in dataPair.Value)
                                    {
                                        if (float.Parse(val) > high)
                                            high = float.Parse(val);
                                    }
                                    buff[currLabel] = high;
                                    break;
                                case "low":
                                    float low = float.Parse(dataPair.Value[0]);
                                    foreach (string val in dataPair.Value)
                                    {
                                        if (float.Parse(val) < low)
                                            low = float.Parse(val);
                                    }
                                    buff[currLabel] = low;
                                    break;
                                case "avg":
                                    List<float> avgValues = new List<float>() { };
                                    foreach (string val in dataPair.Value)
                                    {
                                        avgValues.Add(float.Parse(val));
                                    }
                                    buff[currLabel] = avgValues.Average();
                                    break;
                                case "stdev":
                                    List<float> stdDevValues = new List<float>() { };
                                    foreach (string val in dataPair.Value)
                                    {
                                        stdDevValues.Add(float.Parse(val));
                                    }
                                    buff[currLabel] = Math.Sqrt(stdDevValues.Average(v => Math.Pow(v - stdDevValues.Average(), 2)));
                                    break;
                                case "concat":
                                default:
                                    buff[currLabel] = String.Join(" | ", dataPair.Value);
                                    break;
                            }
                        }

                        cursor.Insert(buff);
                        cursor.Dispose();
                    }

                    MapView.Active.Redraw(false);
                });
            }

            //Check each table, and delete any empty ones
            foreach (var shape in shapeTables)
            {
                BasicFeatureLayer currentLayer = tables[shape];
                var tableSize = await FeatureClassHelper.GetFeatureLayerCount(tables[shape]);
                if(tableSize == 0)
                {
                    await FeatureClassHelper.DeleteFeatureClassLayer(tables[shape]);
                }
            }

            Close();
        }

        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            ToggleHelpMenu();
        }

        private void ToggleHelpMenu()
        {
            if(helpOpen)
            {
                Size = new System.Drawing.Size(Size.Width - helpSpacing, Size.Height);
                helpOpen = false;
            } else
            {
                Size = new System.Drawing.Size(Size.Width + helpSpacing, Size.Height);
                helpOpen = true;
            }
        }
    }
}

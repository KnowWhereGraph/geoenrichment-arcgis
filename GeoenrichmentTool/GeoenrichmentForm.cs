using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Catalog;
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
        private string currentRepository = "";
        private string currentLayer = "";

        private List<String> entities;
        private List<List<String>> content;
        private readonly Dictionary<string, string> mergeRules = new Dictionary<string, string>() { 
            { "concat", "Concatenate values together with a \" | \"" },
            { "first", "Get the first value found" },
            { "count", "Get the number of values found" },
            { "total", "Get the total of all values (numeric)" },
            { "high", "Get the highest value (numeric)" },
            { "low", " Get the lowest value (numeric)" },
            { "avg", "Get the average of all values (numeric)" },
            { "stdev", "Get the standard deviation of all values (numeric)" },
        };


        //List of supported geometries
        //On execution, we build one of each to 
        private readonly List<String> shapeTables = new List<String>() {
            "MapPoint",
            "Polyline",
            "Polygon"
        };

        private int contentTotalSpacing = 50;
        private readonly int contentPadding = 11;

        private readonly string helpText = "The entire KnowWhereGraph is available to explore. In the future, the Choose Knowledge Graph list will allow you to select specific repositories in the KnowWhereGraph to explore.\n\n" +
            "To begin exploring, select any polygonal feature layer as an area of interest.You can even use the "+ " button to manually draw a new polygon layer representing your area of interest.\n\n" +
            "After selecting an area of interest, choose \"Select Content\" to learn about what happened in that area.You may run this feature as many times as desired.\n\n" +
            "When you're ready to create your new Feature Class, provide a name for the new layer and hit \"RUN\".";

        //Initializes the form
        public GeoenrichmentForm()
        {
            InitializeComponent();

            content = new List<List<String>>() { };

            QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();
            foreach(var endpoint in queryClass.defaultEndpoints)
            {
                knowledgeGraph.Items.Add(endpoint.Key);
            }

            knowledgeGraph.SelectedIndex = 0;

            PopulateActiveLayers();

            ToolTip buttonToolTips = new ToolTip();
            buttonToolTips.SetToolTip(refreshLayersBtn, "Refresh Polygon Layer List");
            buttonToolTips.SetToolTip(addLayerBtn, "Draw New Polygon Layer");
            buttonToolTips.SetToolTip(openLayerBtn, "Open Polygon Layer File");
        }

        //Grabs each layer name from the active map layer
        public void PopulateActiveLayers()
        {
            selectedLayer.Items.Clear();

            foreach (Layer activeLayer in MapView.Active.Map.GetLayersAsFlattenedList())
            {
                if (activeLayer is BasicFeatureLayer)
                {
                    var geoLayer = activeLayer as BasicFeatureLayer;
                    if(geoLayer.ShapeType == ArcGIS.Core.CIM.esriGeometryType.esriGeometryPolygon)
                        selectedLayer.Items.Add(geoLayer.Name);
                }
            }
        }
        
        //Graph endpoint changed, so see if we can select content
        private void OnChangeGraph(object sender, EventArgs e)
        {
            //If we didn't actually change, don't do anything
            if (knowledgeGraph.Text == currentRepository)
                return;

            if (content.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Changing graph repositories will remove any selected content. Are you sure you want to continue?", "Reset Content", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ResetSelectedContent();
                }
                if (dialogResult == DialogResult.No)
                {
                    knowledgeGraph.Text = currentRepository;
                    return;
                }
            }

            currentRepository = knowledgeGraph.Text;
            UpdateEntityList();
        }

        //Selected layer changed, so see if we can select content
        private void OnChangeLayer(object sender, EventArgs e)
        {
            //If we didn't actually change, don't do anything
            if (selectedLayer.Text == currentLayer)
                return;

            if (content.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Changing layers will remove any selected content. Are you sure you want to continue?", "Reset Content", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ResetSelectedContent();
                }
                if (dialogResult == DialogResult.No)
                {
                    selectedLayer.Text = currentLayer;
                    return;
                }
            }

            currentLayer = selectedLayer.Text;
            UpdateEntityList();
        }

        //Refresh the layer list to fit the current map state
        private void RefreshLayerList(object sender, EventArgs e)
        {
            PopulateActiveLayers();
        }

        //We are adding a new layer, so open the custom draw tool to do so
        private void DrawAreaOfInterest(object sender, EventArgs e)
        {
            if (content.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Drawing a new layer will remove any selected content. Are you sure you want to continue?", "Reset Content", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ResetSelectedContent();
                }
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            FrameworkApplication.SetCurrentToolAsync("KWG_Geoenrichment_DrawPolygon");

            KwgGeoModule.Current.SetActiveForm(this);

            Hide();
        }

        //Once draw tool compeletes, the newly created layer is set as the active layer
        public void SetDrawnLayer(string layerName)
        {
            PopulateActiveLayers();
            selectedLayer.SelectedIndex = selectedLayer.FindStringExact(layerName);
        }

        //Once layer file is loaded, the layer is added to the map and is set as the active layer
        private async void UploadLayer(object sender, EventArgs e)
        {
            if (content.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Loading a layer will remove any selected content. Are you sure you want to continue?", "Reset Content", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ResetSelectedContent();
                }
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            var bpf = new BrowseProjectFilter("esri_browseDialogFilters_featureClasses_layerProperties_polygon");
            bpf.Name = "Polygon Feature Class";
            var openItemDialog = new OpenItemDialog { BrowseFilter = bpf };

            if ((bool)openItemDialog.ShowDialog())
            {
                Item fileItem = openItemDialog.Items.First();

                Uri uri = new Uri(fileItem.Path);
                await QueuedTask.Run(() => LayerFactory.Instance.CreateLayer(uri, MapView.Active.Map));

                PopulateActiveLayers();
                selectedLayer.SelectedIndex = selectedLayer.FindStringExact(fileItem.Name);
            }
        }


        //Search for entities using the selected knowledge graph and layer
        //If entities found AND no errors occured, enable Select Content
        public async void UpdateEntityList()
        {
            if (
                knowledgeGraph.SelectedItem != null && knowledgeGraph.SelectedItem.ToString() != "" &&
                selectedLayer.SelectedItem != null && selectedLayer.SelectedItem.ToString() != ""
              )
            {
                contentLoading.Visible = true;
                selectContentBtn.Text = "Searching area...";
                string error = await QueuedTask.Run(() => SearchForEntities());
                selectContentBtn.Text = "Select Content";
                contentLoading.Visible = false;
                if (error == "")
                {
                    if (entities.Count > 0)
                        selectContentBtn.Enabled = true;
                    else
                    {
                        selectContentBtn.Enabled = false;
                        MessageBox.Show($@"No content found in the selected Map Layer. Try a different feature layer!");
                    }
                }
                else
                {
                    selectedLayer.SelectedItem = null;
                    selectContentBtn.Enabled = false;
                    KwgGeoModule.Current.GetQueryClass().ReportGraphError(error);
                }
            }
            else
            {
                selectContentBtn.Enabled = false;
            }
        }

        public string SearchForEntities()
        {
            var s2CellList = new List<string>() { };
            entities = new List<string>() { };
            var queryClass = KwgGeoModule.Current.GetQueryClass();

            List<string> polygonWKTs = FeatureClassHelper.GetPolygonStringsFromActiveLayer(currentLayer);

            foreach (var wkt in polygonWKTs)
            {
                //Get s2Cells related to the polygon
                //TODO:: Change function base on spatial relation
                var entityQuery = "select distinct ?entity where { " +
                    "values ?userWKT {\"" + wkt + "\"^^geo:wktLiteral}. " +

                    "?adminRegion2 a kwg-ont:AdministrativeRegion_2. " +
                    "?adminRegion2 geo:hasGeometry ?arGeo2. " +
                    "?arGeo2 geo:asWKT ?arWKT2. " +
                    "FILTER(geof:sfIntersects(?userWKT, ?arWKT2) || geof:sfWithin(?userWKT, ?arWKT2)). " +

                    "?adminRegion3 kwg-ont:sfWithin ?adminRegion2." +
                    "?adminRegion3 a kwg-ont:AdministrativeRegion_3. " +
                    "?adminRegion3 geo:hasGeometry ?arGeo3. " +
                    "?arGeo3 geo:asWKT ?arWKT3. " +
                    "FILTER(geof:sfIntersects(?userWKT, ?arWKT3) || geof:sfWithin(?userWKT, ?arWKT3)). " +

                    "?adminRegion3 kwg-ont:sfContains ?s2Cell. " +
                    "?s2Cell a kwg-ont:KWGCellLevel13. " +
                    "?s2Cell geo:hasGeometry ?s2Geo. " +
                    "?s2Geo geo:asWKT ?s2WKT. " +
                    "FILTER(geof:sfIntersects(?userWKT, ?s2WKT) || geof:sfWithin(?userWKT, ?s2WKT)). " +

                    "{?entity ?p ?s2Cell.} union {?s2Cell ?p ?entity.} " +
                    "?entity a geo:Feature. " +
                "}";

                try
                {
                    JToken s2Results = queryClass.SubmitQuery(currentRepository, entityQuery);

                    foreach (var item in s2Results)
                    {
                        entities.Add(queryClass.IRIToPrefix(item["entity"]["value"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    return "ent";
                }
            }

            return "";
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
            labelObj.Name = "contentLabel" + content.Count.ToString();
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

            //Add the remove content button
            Button removeContent = new Button();
            removeContent.BackColor = System.Drawing.Color.Transparent;
            removeContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            removeContent.Cursor = System.Windows.Forms.Cursors.Hand;
            removeContent.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            removeContent.FlatAppearance.BorderSize = 0;
            removeContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            removeContent.Image = global::KWG_Geoenrichment.Properties.Resources.x;
            removeContent.Name = "removeContent" + content.Count.ToString();
            removeContent.Size = new System.Drawing.Size(26, 26);
            removeContent.UseVisualStyleBackColor = false;
            removeContent.Click += new System.EventHandler(this.RemoveSelectedContent);
            Controls.Add(removeContent);

            //Move the label
            labelObj.Location = new System.Drawing.Point(selectedLayer.Location.X, selectedLayer.Location.Y + contentTotalSpacing);
            int addedHeight = labelObj.Height + contentPadding;

            //Move the merge dropdown, the remove content button, and the column text
            columnText.Location = new System.Drawing.Point(labelObj.Location.X, labelObj.Location.Y + labelObj.Height + contentPadding);
            mergeBox.Location = new System.Drawing.Point(labelObj.Location.X + 206, labelObj.Location.Y + labelObj.Height + contentPadding);
            removeContent.Location = new System.Drawing.Point(labelObj.Location.X + 612, labelObj.Location.Y + labelObj.Height + contentPadding);
            addedHeight += mergeBox.Height + contentPadding;

            //Adjust the total amount of spacing we've moved
            contentTotalSpacing += addedHeight;

            //Move things down
            selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y + addedHeight);
            requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y + addedHeight);
            saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y + addedHeight);
            saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y + addedHeight);
            helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y + addedHeight);
            layerLoading.Location = new System.Drawing.Point(layerLoading.Location.X, layerLoading.Location.Y + addedHeight);
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

        private void RemoveSelectedContent(object sender, EventArgs e)
        {
            //get index
            Button clickedButton = sender as Button;
            string buttonText = clickedButton.Name;
            string index = buttonText.Replace("removeContent", "");
            int idx = Int32.Parse(index);

            //remove content from ui
            Label contentLabel = (Label)this.Controls.Find("contentLabel" + index, true).First();
            TextBox columnName = (TextBox)this.Controls.Find("columnName" + index, true).First();
            ComboBox mergeRule = (ComboBox)this.Controls.Find("mergeRule" + index, true).First();

            this.Controls.Remove(contentLabel);
            this.Controls.Remove(columnName);
            this.Controls.Remove(mergeRule);
            this.Controls.Remove(clickedButton);

            //remove content from array
            int oldSize = content.Count;
            content.RemoveAt(idx - 1);

            //remove the window height
            int addedHeight = contentLabel.Height + contentPadding;
            addedHeight += mergeRule.Height + contentPadding;

            contentTotalSpacing -= addedHeight;
            selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y - addedHeight);
            requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y - addedHeight);
            saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y - addedHeight);
            saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y - addedHeight);
            helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y - addedHeight);
            layerLoading.Location = new System.Drawing.Point(layerLoading.Location.X, layerLoading.Location.Y - addedHeight);
            runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y - addedHeight);
            Height -= addedHeight;

            //remove any content that is listed after, and relabel with new index 
            for (int i = idx+1; i <= oldSize; i++)
            {
                Label oldContentLabel = (Label)this.Controls.Find("contentLabel" + i.ToString(), true).First();
                TextBox oldColumnName = (TextBox)this.Controls.Find("columnName" + i.ToString(), true).First();
                ComboBox oldMergeRule = (ComboBox)this.Controls.Find("mergeRule" + i.ToString(), true).First();
                Button oldRemoveContent = (Button)this.Controls.Find("removeContent" + i.ToString(), true).First();

                oldContentLabel.Location = new System.Drawing.Point(oldContentLabel.Location.X, oldContentLabel.Location.Y - addedHeight);
                oldColumnName.Location = new System.Drawing.Point(oldColumnName.Location.X, oldColumnName.Location.Y - addedHeight);
                oldMergeRule.Location = new System.Drawing.Point(oldMergeRule.Location.X, oldMergeRule.Location.Y - addedHeight);
                oldRemoveContent.Location = new System.Drawing.Point(oldRemoveContent.Location.X, oldRemoveContent.Location.Y - addedHeight);

                oldContentLabel.Name = "contentLabel" + (i - 1).ToString();
                oldColumnName.Name = "columnName" + (i - 1).ToString();
                oldMergeRule.Name = "mergeRule" + (i - 1).ToString();
                oldRemoveContent.Name = "removeContent" + (i - 1).ToString();
            }

            CheckCanRunGeoenrichment();
        }

        private void ResetSelectedContent()
        {
            for (int i = 1; i <= content.Count; i++)
            {
                //remove content from ui
                Label contentLabel = (Label)this.Controls.Find("contentLabel" + i.ToString(), true).First();
                TextBox columnName = (TextBox)this.Controls.Find("columnName" + i.ToString(), true).First();
                ComboBox mergeRule = (ComboBox)this.Controls.Find("mergeRule" + i.ToString(), true).First();
                Button removeContent = (Button)this.Controls.Find("removeContent" + i.ToString(), true).First();

                this.Controls.Remove(contentLabel);
                this.Controls.Remove(columnName);
                this.Controls.Remove(mergeRule);
                this.Controls.Remove(removeContent);

                //remove the window height
                int addedHeight = contentLabel.Height + contentPadding;
                addedHeight += mergeRule.Height + contentPadding;

                contentTotalSpacing -= addedHeight;
                selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y - addedHeight);
                requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y - addedHeight);
                saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y - addedHeight);
                saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y - addedHeight);
                helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y - addedHeight);
                layerLoading.Location = new System.Drawing.Point(layerLoading.Location.X, layerLoading.Location.Y - addedHeight);
                runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y - addedHeight);
                Height -= addedHeight;
            }

            content = new List<List<String>>() { };
            CheckCanRunGeoenrichment();
        }

        private void OnFeatureNameChage(object sender, EventArgs e)
        {
            CheckCanRunGeoenrichment();
        }

        public void CheckCanRunGeoenrichment()
        {
            if (
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
            runBtn.Enabled = false;
            runBtn.Text = "Running...";
            layerLoading.Visible = true;

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

                    try
                    {
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
                    catch(Exception ex)
                    {
                        runBtn.Enabled = true;
                        runBtn.Text = "Run";
                        layerLoading.Visible = false;
                        queryClass.ReportGraphError("res");
                        return;
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

            layerLoading.Visible = false;
            Close();
        }

        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            var helpWindow = new KWGHelp(helpText);
            helpWindow.Show();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}

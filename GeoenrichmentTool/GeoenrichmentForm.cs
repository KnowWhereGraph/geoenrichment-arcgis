using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Catalog;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ArcGIS.Desktop.Internal.Core.PortalTrafficDataService.ServiceErrorResponse;
using ComboBox = System.Windows.Forms.ComboBox;

namespace KWG_Geoenrichment
{
    public partial class GeoenrichmentForm : Form
    {
        private string currentRepository = "";
        private string currentLayer = "";
        private List<string> currentLayerWKTs;

        private List<String> entities;
        private List<String> entitiesFormatted;
        Dictionary<string, string> entitiesClasses;

        private List<String> selectedClasses;
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
        private readonly int contentPadding = 50;

        private readonly string helpText = "The entire KnowWhereGraph is available to explore. In the future, the Choose Knowledge Graph list will allow you to select specific repositories in the KnowWhereGraph to explore.\n\n" +
            "To begin exploring, select any polygonal feature layer as an area of interest.You can even use the " + " button to manually draw a new polygon layer representing your area of interest.\n\n" +
            "After selecting an area of interest, choose \"Select Content\" to learn about what happened in that area.You may run this feature as many times as desired.\n\n" +
            "When you're ready to create your new Feature Class, provide a name for the new layer and hit \"RUN\".";

        //Initializes the form
        public GeoenrichmentForm()
        {
            InitializeComponent();

            content = new List<List<String>>() { };
            selectedClasses = new List<String>() { };

            QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();
            foreach (var endpoint in queryClass.defaultEndpoints)
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
                    if (geoLayer.ShapeType == ArcGIS.Core.CIM.esriGeometryType.esriGeometryPolygon)
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
                featuresOfInterest.Enabled = false;
                contentLoading.Visible = true;
                string error = await QueuedTask.Run(() => SearchForEntities());
                contentLoading.Visible = false;
                if (error == "")
                {
                    if (entities.Count > 0)
                    {
                        Dictionary<string, string> classes = new Dictionary<string, string>() { { "", "" } };
                        classes = entitiesClasses;

                        featuresOfInterest.Enabled = true;
                        featuresOfInterest.DataSource = new BindingSource(classes.OrderBy(key => key.Value), null);
                        featuresOfInterest.DropDownWidth = classes.Values.Cast<string>().Max(x => TextRenderer.MeasureText(x, featuresOfInterest.Font).Width);
                    }
                    else
                    {
                        MessageBox.Show($@"No content found in the selected Map Layer. Try a different feature layer!");
                    }
                }
                else
                {
                    selectedLayer.SelectedItem = null;
                    featuresOfInterest.Enabled = false;
                    KwgGeoModule.Current.GetQueryClass().ReportGraphError(error);
                }
            }
            else
            {
                featuresOfInterest.Enabled = false;
            }
        }

        //Split value lists into chunks to make smaller efficient queries
        private List<string> SplitValueList(List<string> originalList, string queryVar, int queryChunk)
        {
            var newEntityList = new List<string>();

            for (int i = 0; i < originalList.Count; i += queryChunk)
            {
                List<string> subList = originalList.GetRange(i, Math.Min(queryChunk, originalList.Count - i));
                newEntityList.Add("values ?" + queryVar + " {" + String.Join(" ", subList) + "} ");
            }

            return newEntityList;
        }

        //Creates sparql queries for finding s2Cells and all their entities in the search area
        public string SearchForEntities()
        {
            entities = new List<string>() { };
            entitiesFormatted = new List<string>() { };
            entitiesClasses = new Dictionary<string, string>() { { "", "" } };
            var queryClass = KwgGeoModule.Current.GetQueryClass();

            currentLayerWKTs = FeatureClassHelper.GetPolygonStringsFromActiveLayer(currentLayer);

            foreach (var wkt in currentLayerWKTs)
            {
                List<string> s2CellFourList = new List<string>() { };
                List<string> s2CellEightList = new List<string>() { };
                List<string> s2CellThirteenList = new List<string>() { };

                //Get s2 Cells at level 4
                bool pagination = true;
                int offset = 0;
                while (pagination)
                {
                    var s2CellQuery = "select distinct ?s2Cell4 where { " +
                        "values ?userWKT {\"" + wkt + "\"^^geo:wktLiteral}. " +

                        "?s2Cell1 a kwg-ont:KWGCellLevel1. " +
                        "?s2Cell1 geo:hasGeometry ?s2CellGeo1. " +
                        "?s2CellGeo1 geo:asWKT ?s2CellWKT1. " +
                        "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT1) || geof:sfWithin(?userWKT, ?s2CellWKT1)). " +

                        "?s2Cell1 kwg-ont:sfContains ?s2Cell2. " +
                        "?s2Cell2 a kwg-ont:KWGCellLevel2. " +
                        "?s2Cell2 geo:hasGeometry ?s2CellGeo2. " +
                        "?s2CellGeo2 geo:asWKT ?s2CellWKT2. " +
                        "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT2) || geof:sfWithin(?userWKT, ?s2CellWKT2)). " +

                        "?s2Cell2 kwg-ont:sfContains ?s2Cell3. " +
                        "?s2Cell3 a kwg-ont:KWGCellLevel3. " +
                        "?s2Cell3 geo:hasGeometry ?s2CellGeo3. " +
                        "?s2CellGeo3 geo:asWKT ?s2CellWKT3. " +
                        "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT3) || geof:sfWithin(?userWKT, ?s2CellWKT3)). " +

                        "?s2Cell3 kwg-ont:sfContains ?s2Cell4. " +
                        "?s2Cell4 a kwg-ont:KWGCellLevel4. " +
                        "?s2Cell4 geo:hasGeometry ?s2CellGeo4. " +
                        "?s2CellGeo4 geo:asWKT ?s2CellWKT4. " +
                        "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT4) || geof:sfWithin(?userWKT, ?s2CellWKT4)). " +
                    "} " +
                    "ORDER BY ?s2Cell4 " +
                    "LIMIT 10000 " +
                    "OFFSET " + offset.ToString();

                    try
                    {
                        JToken s2Results = queryClass.SubmitQuery(currentRepository, s2CellQuery);

                        foreach (var item in s2Results)
                        {
                            s2CellFourList.Add(queryClass.IRIToPrefix(item["s2Cell4"]["value"].ToString()));
                        }

                        if (s2Results.Count() == 10000)
                        {
                            offset += 10000;
                        }
                        else
                        {
                            pagination = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return "s2c4";
                    }
                }

                //Split the list by groups, the goal is to super chunk queries
                List<string> s24ValueChunks = SplitValueList(s2CellFourList, "s2Cell4", 10);

                //Get s2 Cells at level 8
                for (int i = 0; i < s24ValueChunks.Count; i++)
                {
                    pagination = true;
                    offset = 0;
                    while (pagination)
                    {
                        var s2CellQuery = "select distinct ?s2Cell8 where { " +
                            "values ?userWKT {\"" + wkt + "\"^^geo:wktLiteral}. " +

                            s24ValueChunks[i] +

                            "?s2Cell4 kwg-ont:sfContains ?s2Cell5. " +
                            "?s2Cell5 a kwg-ont:KWGCellLevel5. " +
                            "?s2Cell5 geo:hasGeometry ?s2CellGeo5. " +
                            "?s2CellGeo5 geo:asWKT ?s2CellWKT5. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT5) || geof:sfWithin(?userWKT, ?s2CellWKT5)). " +

                            "?s2Cell5 kwg-ont:sfContains ?s2Cell6. " +
                            "?s2Cell6 a kwg-ont:KWGCellLevel6. " +
                            "?s2Cell6 geo:hasGeometry ?s2CellGeo6. " +
                            "?s2CellGeo6 geo:asWKT ?s2CellWKT6. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT6) || geof:sfWithin(?userWKT, ?s2CellWKT6)). " +

                            "?s2Cell6 kwg-ont:sfContains ?s2Cell7. " +
                            "?s2Cell7 a kwg-ont:KWGCellLevel7. " +
                            "?s2Cell7 geo:hasGeometry ?s2CellGeo7. " +
                            "?s2CellGeo7 geo:asWKT ?s2CellWKT7. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT7) || geof:sfWithin(?userWKT, ?s2CellWKT7)). " +

                            "?s2Cell7 kwg-ont:sfContains ?s2Cell8. " +
                            "?s2Cell8 a kwg-ont:KWGCellLevel8. " +
                            "?s2Cell8 geo:hasGeometry ?s2CellGeo8. " +
                            "?s2CellGeo8 geo:asWKT ?s2CellWKT8. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT8) || geof:sfWithin(?userWKT, ?s2CellWKT8)). " +
                        "} " +
                        "ORDER BY ?s2Cell8 " +
                        "LIMIT 10000 " +
                        "OFFSET " + offset.ToString();

                        try
                        {
                            JToken s2Results = queryClass.SubmitQuery(currentRepository, s2CellQuery);

                            foreach (var item in s2Results)
                            {
                                s2CellEightList.Add(queryClass.IRIToPrefix(item["s2Cell8"]["value"].ToString()));
                            }

                            if (s2Results.Count() == 10000)
                            {
                                offset += 10000;
                            }
                            else
                            {
                                pagination = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            return "s2c8";
                        }
                    }
                }

                //Split the list by groups, the goal is to super chunk queries
                List<string> s28ValueChunks = SplitValueList(s2CellEightList, "s2Cell8", 10);

                //Get s2 Cells at level 13
                for (int i = 0; i < s28ValueChunks.Count; i++)
                {
                    pagination = true;
                    offset = 0;
                    while (pagination)
                    {
                        var s2CellQuery = "select distinct ?s2Cell13 where { " +
                            "values ?userWKT {\"" + wkt + "\"^^geo:wktLiteral}. " +

                            s28ValueChunks[i] +

                            "?s2Cell8 kwg-ont:sfContains ?s2Cell9. " +
                            "?s2Cell9 a kwg-ont:KWGCellLevel9. " +
                            "?s2Cell9 geo:hasGeometry ?s2CellGeo9. " +
                            "?s2CellGeo9 geo:asWKT ?s2CellWKT9. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT9) || geof:sfWithin(?userWKT, ?s2CellWKT9)). " +

                            "?s2Cell9 kwg-ont:sfContains ?s2Cell10. " +
                            "?s2Cell10 a kwg-ont:KWGCellLevel10. " +
                            "?s2Cell10 geo:hasGeometry ?s2CellGeo10. " +
                            "?s2CellGeo10 geo:asWKT ?s2CellWKT10. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT10) || geof:sfWithin(?userWKT, ?s2CellWKT10)). " +

                            "?s2Cell10 kwg-ont:sfContains ?s2Cell11. " +
                            "?s2Cell11 a kwg-ont:KWGCellLevel11." +
                            "?s2Cell11 geo:hasGeometry ?s2CellGeo11. " +
                            "?s2CellGeo11 geo:asWKT ?s2CellWKT11. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT11) || geof:sfWithin(?userWKT, ?s2CellWKT11)). " +

                            "?s2Cell11 kwg-ont:sfContains ?s2Cell12. " +
                            "?s2Cell12 a kwg-ont:KWGCellLevel12. " +
                            "?s2Cell12 geo:hasGeometry ?s2CellGeo12. " +
                            "?s2CellGeo12 geo:asWKT ?s2CellWKT12. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT12) || geof:sfWithin(?userWKT, ?s2CellWKT12)). " +

                            "?s2Cell12 kwg-ont:sfContains ?s2Cell13. " +
                            "?s2Cell13 a kwg-ont:KWGCellLevel13. " +
                            "?s2Cell13 geo:hasGeometry ?s2CellGeo13. " +
                            "?s2CellGeo13 geo:asWKT ?s2CellWKT13. " +
                            "FILTER(geof:sfIntersects(?userWKT, ?s2CellWKT13) || geof:sfWithin(?userWKT, ?s2CellWKT13))." +
                        "} " +
                        "ORDER BY ?s2Cell13 " +
                        "LIMIT 10000 " +
                        "OFFSET " + offset.ToString();

                        try
                        {
                            JToken s2Results = queryClass.SubmitQuery(currentRepository, s2CellQuery);

                            foreach (var item in s2Results)
                            {
                                s2CellThirteenList.Add(queryClass.IRIToPrefix(item["s2Cell13"]["value"].ToString()));
                            }

                            if (s2Results.Count() == 10000)
                            {
                                offset += 10000;
                            }
                            else
                            {
                                pagination = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            return "s2c13";
                        }
                    }
                }

                //Split the list by groups, the goal is to super chunk queries
                List<string> s213ValueChunks = SplitValueList(s2CellThirteenList, "s2Cell13", 10000);

                //Get entitites via s2Cells
                for (int i = 0; i < s213ValueChunks.Count; i++)
                {
                    pagination = true;
                    offset = 0;
                    while (pagination)
                    {
                        var entityQuery = "select distinct ?entity where { " +
                            s213ValueChunks[i] +

                            "{?entity ?p ?s2Cell13.} union {?s2Cell13 ?p ?entity.} " +
                            "?entity a geo:Feature. " +
                        "} " +
                        "ORDER BY ?entity " +
                        "LIMIT 10000 " +
                        "OFFSET " + offset.ToString();

                        try
                        {
                            JToken s2Results = queryClass.SubmitQuery(currentRepository, entityQuery);

                            foreach (var item in s2Results)
                            {
                                entities.Add(queryClass.IRIToPrefix(item["entity"]["value"].ToString()));
                            }

                            if (s2Results.Count() == 10000)
                            {
                                offset += 10000;
                            }
                            else
                            {
                                pagination = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            return "ent";
                        }
                    }
                }
            }

            entitiesFormatted = SplitValueList(entities, "entity", 10000);

            for (int i = 0; i < entitiesFormatted.Count; i++)
            {
                bool pagination = true;
                int offset = 0;
                while (pagination)
                {
                    var typeQuery = "select distinct ?type ?label where { " +
                        "?entity a ?type. " +
                        "?type rdfs:label ?label. " +
                        entitiesFormatted[i] +
                    "}";
                    try
                    {
                        JToken typeResults = queryClass.SubmitQuery(currentRepository, typeQuery);

                        foreach (var item in typeResults)
                        {
                            string cType = queryClass.IRIToPrefix(item["type"]["value"].ToString());
                            string cLabel = queryClass.IRIToPrefix(item["label"]["value"].ToString());

                            if (cType == "sosa:FeatureOfInterest" || cType == "geo:Feature")
                                continue;

                            if (!entitiesClasses.ContainsKey(cType))
                                entitiesClasses[cType] = cLabel;
                        }

                        if (typeResults.Count() == 10000)
                        {
                            offset += 10000;
                        }
                        else
                        {
                            pagination = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return "type";
                    }
                }
            }

            return "";
        }

        //When user selects a feature of interest, add feature to the 
        private void OnSelectFeature(object sender, EventArgs e)
        {
            String feature = featuresOfInterest.SelectedValue.ToString();
            String featureLabel = featuresOfInterest.Text;

            if (selectedClasses.Contains(feature))
            {
                MessageBox.Show($@"Selected Feature of Interest is already in the list!");
            }
            else if (feature != "")
            {
                selectedClasses.Add(feature);

                //Add the class label
                Label labelObj = new Label();
                labelObj.AutoSize = knowledgeGraphLabel.AutoSize;
                labelObj.BackColor = Color.FromName("ActiveCaption");
                labelObj.Font = knowledgeGraphLabel.Font;
                labelObj.ForeColor = knowledgeGraphLabel.ForeColor;
                labelObj.Margin = knowledgeGraphLabel.Margin;
                labelObj.Name = "classLabel" + selectedClasses.Count.ToString();
                labelObj.Size = knowledgeGraphLabel.Size;
                labelObj.MaximumSize = new Size(780, 0);
                labelObj.Text = featureLabel;
                Controls.Add(labelObj);

                //Add the Add Property button
                Button addProperty = new Button();
                addProperty.BackColor = System.Drawing.Color.FromArgb(66, 214, 237);
                addProperty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                addProperty.Cursor = System.Windows.Forms.Cursors.Hand;
                addProperty.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
                addProperty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                addProperty.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                addProperty.ForeColor = System.Drawing.Color.Black;
                addProperty.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
                addProperty.Name = "addProperties" + selectedClasses.Count.ToString();
                addProperty.Size = new System.Drawing.Size(100, 25);
                addProperty.Text = "Add Properties";
                addProperty.UseVisualStyleBackColor = false;
                addProperty.Click += new System.EventHandler(this.OpenTraverseWindow);
                Controls.Add(addProperty);

                //Add the remove class button
                Button removeClass = new Button();
                removeClass.BackColor = System.Drawing.Color.Transparent;
                removeClass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                removeClass.Cursor = System.Windows.Forms.Cursors.Hand;
                removeClass.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                removeClass.FlatAppearance.BorderSize = 0;
                removeClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                removeClass.Image = global::KWG_Geoenrichment.Properties.Resources.x;
                removeClass.Name = "removeClass" + selectedClasses.Count.ToString();
                removeClass.Size = new System.Drawing.Size(25, 25);
                removeClass.UseVisualStyleBackColor = false;
                removeClass.Click += new System.EventHandler(this.RemoveSelectedClass);
                Controls.Add(removeClass);

                //Move the label and remove class button
                labelObj.Location = new System.Drawing.Point(featuresOfInterest.Location.X, featuresOfInterest.Location.Y + contentTotalSpacing);
                addProperty.Location = new System.Drawing.Point(labelObj.Location.X + labelObj.Width + 20, labelObj.Location.Y - 1);
                removeClass.Location = new System.Drawing.Point(addProperty.Location.X + addProperty.Width + 20, addProperty.Location.Y - 1);

                //Adjust the total amount of spacing we've moved
                contentTotalSpacing += contentPadding;

                //Move things down
                requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y + contentPadding);
                saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y + contentPadding);
                saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y + contentPadding);
                helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y + contentPadding);
                layerLoading.Location = new System.Drawing.Point(layerLoading.Location.X, layerLoading.Location.Y + contentPadding);
                runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y + contentPadding);
                Height += contentPadding;

                //Reset feature box
                featuresOfInterest.SelectedValue = "";

                CheckCanRunGeoenrichment();
            }
        }

        private void OpenTraverseWindow(object sender, EventArgs e) //TODO
        {
            var queryClass = KwgGeoModule.Current.GetQueryClass();
            bool queryFailed = false;

            //get index
            Button clickedButton = sender as Button;
            string buttonText = clickedButton.Name;
            string index = buttonText.Replace("addProperties", "");
            int idx = Int32.Parse(index);

            string classToTraverse = selectedClasses[idx - 1];
            string classToTraverseLabel = entitiesClasses[classToTraverse];
            List<string> traverseEntities = new List<string>() { };

            //Grab only the entities that are of the class
            //We don't need to paginate this query because each entitiesFormatted value list is no more than 10000 elements
            for (int i = 0; i < entitiesFormatted.Count; i++)
            {
                var traverseEntitesQuery = "select ?entity where { " +
                    entitiesFormatted[i] +
                    "?entity a " + classToTraverse + ". " +
                "} ";

                try
                {
                    JToken s2Results = queryClass.SubmitQuery(currentRepository, traverseEntitesQuery);

                    foreach (var item in s2Results)
                    {
                        traverseEntities.Add(queryClass.IRIToPrefix(item["entity"]["value"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    queryFailed = true;
                    queryClass.ReportGraphError("trav");
                }
            }

            if (!queryFailed)
            {
                List<string> traverseEntitiesFormatted = SplitValueList(traverseEntities, "entity", 10000);

                //var exploreWindow = new TraverseKnowledgeGraph(this, currentRepository, traverseEntitiesFormatted, classToTraverseLabel);
                //Hide();
                //exploreWindow.Show();
            }
        }

        //Removes a class and its selected properties from the data and the UI
        private void RemoveSelectedClass(object sender, EventArgs e)
        {
            //get index
            Button clickedButton = sender as Button;
            string buttonText = clickedButton.Name;
            string index = buttonText.Replace("removeClass", "");
            int idx = Int32.Parse(index);

            //remove content from ui
            Label classLabel = (Label)this.Controls.Find("classLabel" + index, true).First();
            Button addPropBtn = (Button)this.Controls.Find("addProperties" + index, true).First();

            this.Controls.Remove(classLabel);
            this.Controls.Remove(addPropBtn);
            this.Controls.Remove(clickedButton);

            //remove content from array
            int oldSize = selectedClasses.Count;
            selectedClasses.RemoveAt(idx - 1);

            //TODO::Remove Properties content as well

            //remove the window height
            contentTotalSpacing -= contentPadding;
            requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y - contentPadding);
            saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y - contentPadding);
            saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y - contentPadding);
            helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y - contentPadding);
            layerLoading.Location = new System.Drawing.Point(layerLoading.Location.X, layerLoading.Location.Y - contentPadding);
            runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y - contentPadding);
            Height -= contentPadding;

            //remove any content that is listed after, and relabel with new index 
            for (int i = idx + 1; i <= oldSize; i++)
            {
                Label oldClassLabel = (Label)this.Controls.Find("classLabel" + i.ToString(), true).First();
                Button oldAddPropBtn = (Button)this.Controls.Find("addProperties" + i.ToString(), true).First();
                Button oldRemoveClass = (Button)this.Controls.Find("removeClass" + i.ToString(), true).First();

                oldClassLabel.Location = new System.Drawing.Point(oldClassLabel.Location.X, oldClassLabel.Location.Y - contentPadding);
                oldAddPropBtn.Location = new System.Drawing.Point(oldAddPropBtn.Location.X, oldAddPropBtn.Location.Y - contentPadding);
                oldRemoveClass.Location = new System.Drawing.Point(oldRemoveClass.Location.X, oldRemoveClass.Location.Y - contentPadding);

                oldClassLabel.Name = "classLabel" + (i - 1).ToString();
                oldAddPropBtn.Name = "addProperties" + (i - 1).ToString();
                oldRemoveClass.Name = "removeClass" + (i - 1).ToString();
            }

            CheckCanRunGeoenrichment();
        }

        public void AddSelectedContent(List<string> uris, List<string> labels) //TODO
        {
            Show();

            //We need to eliminate doubles since the value box in one line is the same as the class box in the next line
            var uniqueUris = new List<string>() { };
            var uniqueLabels = new List<string>() { };

            for (int i = 0; i < uris.Count; i++)
            {
                if (
                    i == 0 ||
                    (uris[i] != uris[i - 1] && uris[i] != "LiteralDataFound")
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
            mergeBox.Name = "mergeRule" + content.Count.ToString();
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
            //removeContent.Click += new System.EventHandler(this.RemoveSelectedContent);
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
            //selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y + addedHeight);
            requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y + addedHeight);
            saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y + addedHeight);
            saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y + addedHeight);
            helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y + addedHeight);
            layerLoading.Location = new System.Drawing.Point(layerLoading.Location.X, layerLoading.Location.Y + addedHeight);
            runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y + addedHeight);
            Height += addedHeight;

            //Disable boxes if needed
            if (columnString == "NoAdditionalData")
            {
                columnText.Enabled = false;
                mergeBox.Enabled = false;
            }


            CheckCanRunGeoenrichment();
        }

        private void ResetSelectedContent() //TODO
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
                //selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y - addedHeight);
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

        //Triggers form validation when Save Layer As name changes
        private void OnFeatureNameChage(object sender, EventArgs e)
        {
            CheckCanRunGeoenrichment();
        }

        //Checks to see if there is selected content before user is allowed to execute the form
        public void CheckCanRunGeoenrichment()
        {
            if (
                selectedClasses.Count > 0 &&
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

        private async void RunGeoenrichment(object sender, EventArgs e) //TODO
        {
            runBtn.Enabled = false;
            runBtn.Text = "Running...";
            layerLoading.Visible = true;

            var queryClass = KwgGeoModule.Current.GetQueryClass();
            bool layerFailed = false;
            Dictionary<string, Dictionary<string, BasicFeatureLayer>> tables = new Dictionary<string, Dictionary<string, BasicFeatureLayer>>() { }; //entityType -> shape -> layer
            var columnLabels = new List<string>() { };
            var labelToMergeRule = new Dictionary<string, string>() { }; //column label -> merge rule

            IGeometryEngine geoEngine = GeometryEngine.Instance;
            SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(4326);

            //Pull all geometries from the area of interest
            List<Geometry> userGeos = new List<Geometry>() { };
            for (int i = 0; i < currentLayerWKTs.Count; i++)
            {
                userGeos.Add(geoEngine.ImportFromWKT(0, currentLayerWKTs[i], sr));
            }

            //build the table and its columns
            for (int j = 0; j < content.Count; j++)
            {
                var firstShape = true;
                var className = content[j][0].Contains(':') ? content[j][0].Split(':')[1] : content[j][0];

                if (!tables.ContainsKey(className))
                    tables[className] = new Dictionary<string, BasicFeatureLayer>();

                foreach (var shape in shapeTables)
                {
                    //Make the table if it doesn't exist yet
                    if (!tables[className].ContainsKey(shape))
                    {
                        string tableName = FeatureClassHelper.ValidateTableName(saveLayerAs.Text + "_" + className + "_" + shape);
                        await FeatureClassHelper.CreateFeatureClassLayer(tableName, shape);
                        var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == tableName).FirstOrDefault() as BasicFeatureLayer;

                        if (fcLayer == null)
                        {
                            MessageBox.Show($@"Failed to create {tableName} in the active map");
                            layerFailed = true;
                        }
                        else
                        {
                            await FeatureClassHelper.AddField(fcLayer, "Label", "TEXT");
                            await FeatureClassHelper.AddField(fcLayer, "URL", "TEXT");
                            tables[className][shape] = fcLayer;
                        }
                    }

                    //Add the additional data column to our feature tables
                    ComboBox mergeBox = (ComboBox)this.Controls.Find("mergeRule" + (j + 1).ToString(), true).First();
                    string mergeRule = mergeBox.SelectedValue.ToString();
                    string columnLabel = this.Controls.Find("columnName" + (j + 1).ToString(), true).First().Text.Replace(' ', '_') + '_' + mergeRule;


                    if (firstShape)
                    {
                        //We are adding the same column to multiple shape type features, so only capture it for reference the first time
                        columnLabels.Add(columnLabel);
                        labelToMergeRule[columnLabel] = mergeRule;
                        firstShape = false;
                    }

                    if (!columnLabel.StartsWith("NoAdditionalData"))
                        await FeatureClassHelper.AddField(tables[className][shape], columnLabel, "TEXT");
                }
            }

            if (!layerFailed)
            {
                //Build and run query for base classes
                var finalContent = new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>() { }; //entityType -> entity -> column label -> data
                var finalContentLabels = new Dictionary<string, Dictionary<string, string>>() { }; //entityType -> entity -> entityLabel
                var finalContentGeometry = new Dictionary<string, Dictionary<string, string>>() { }; //entityType -> entity -> wkt
                string entityName; string nextEntityName;

                for (int j = 0; j < content.Count; j++)
                {
                    var classNameForTableArray = content[j][0].Contains(':') ? content[j][0].Split(':')[1] : content[j][0]; //We use this to divide all the content into class type so they go to the appropriate table later

                    for (int k = 0; k < entitiesFormatted.Count; k++)
                    {
                        var contentResultsQuery = "select distinct ?entity ?entityLabel ?o ?wkt where { ?entity rdfs:label ?entityLabel. ";

                        //Set entity type key for arrays if needed
                        if (!finalContent.ContainsKey(classNameForTableArray))
                            finalContent[classNameForTableArray] = new Dictionary<string, Dictionary<string, List<string>>>();
                        if (!finalContentLabels.ContainsKey(classNameForTableArray))
                            finalContentLabels[classNameForTableArray] = new Dictionary<string, string>();
                        if (!finalContentGeometry.ContainsKey(classNameForTableArray))
                            finalContentGeometry[classNameForTableArray] = new Dictionary<string, string>();

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

                        contentResultsQuery += "optional {?entity geo:hasGeometry ?geo. ?geo geo:asWKT ?wkt} " + entitiesFormatted[k] + "}";

                        try
                        {
                            JToken contentResults = queryClass.SubmitQuery(currentRepository, contentResultsQuery);

                            foreach (var item in contentResults)
                            {
                                var entityVal = item["entity"]["value"].ToString();

                                //Check to see if we this entity exists, if not, set it up
                                if (!finalContent[classNameForTableArray].ContainsKey(entityVal))
                                    finalContent[classNameForTableArray][entityVal] = new Dictionary<string, List<string>>() { };
                                if (!finalContentLabels[classNameForTableArray].ContainsKey(entityVal))
                                    finalContentLabels[classNameForTableArray][entityVal] = item["entityLabel"]["value"].ToString();
                                if (!finalContentGeometry[classNameForTableArray].ContainsKey(entityVal))
                                    finalContentGeometry[classNameForTableArray][entityVal] = item["wkt"]["value"].ToString();

                                //Let's prep and store the result content
                                if (!finalContent[classNameForTableArray][entityVal].ContainsKey(columnLabels[j]))
                                    finalContent[classNameForTableArray][entityVal][columnLabels[j]] = new List<string>() { };

                                if (item["o"] != null && item["o"]["value"].ToString() != "")
                                    finalContent[classNameForTableArray][entityVal][columnLabels[j]].Add(item["o"]["value"].ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            runBtn.Enabled = true;
                            runBtn.Text = "Run";
                            layerLoading.Visible = false;
                            queryClass.ReportGraphError("res");
                            return;
                        }
                    }
                }

                //Use data to populate table
                await QueuedTask.Run(() =>
                {
                    foreach (var entityType in finalContent)
                    {
                        foreach (var entityPair in finalContent[entityType.Key])
                        {
                            string currEntity = entityPair.Key;

                            string wkt = finalContentGeometry[entityType.Key][currEntity].Replace("<http://www.opengis.net/def/crs/OGC/1.3/CRS84>", "");
                            Geometry geo = geoEngine.ImportFromWKT(0, wkt, sr);

                            //Validate that our geometry actually intersects the original user area
                            //We do this because we originally used s2Cell approximation to find the entities
                            bool doesIntersectWithUserArea = false;
                            for (int i = 0; i < userGeos.Count; i++)
                            {
                                if (GeometryEngine.Instance.Intersects(geo, userGeos[i]))
                                    doesIntersectWithUserArea = true;
                            }
                            if (!doesIntersectWithUserArea)
                                continue;

                            InsertCursor cursor = tables[entityType.Key][geo.GetType().Name].GetTable().CreateInsertCursor();
                            RowBuffer buff = tables[entityType.Key][geo.GetType().Name].GetTable().CreateRowBuffer();

                            buff["Label"] = finalContentLabels[entityType.Key][currEntity];
                            buff["URL"] = currEntity;
                            buff["Shape"] = geo;

                            //Add column data based on merge rule
                            foreach (var dataPair in entityPair.Value)
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
                                        foreach (string val in dataPair.Value)
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
                    }

                    MapView.Active.Redraw(false);
                });
            }

            ////Check each table, and delete any empty ones
            foreach (var classLabel in tables)
            {
                foreach (var shape in tables[classLabel.Key])
                {
                    BasicFeatureLayer currLayer = tables[classLabel.Key][shape.Key];
                    var tableSize = await FeatureClassHelper.GetFeatureLayerCount(currLayer);
                    if (tableSize == 0)
                    {
                        await FeatureClassHelper.DeleteFeatureClassLayer(currLayer);
                    }
                }
            }

            layerLoading.Visible = false;
            Close();
        }

        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            var helpWindow = new KWGHelp(helpText);
            helpWindow.Show();
        } //TODO

        //Closes the Geoenrichment window, which closes the plugin entirely
        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}

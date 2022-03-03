using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KWG_Geoenrichment
{
    public partial class GeoenrichmentForm : Form
    {
        private bool gdbFileUploaded = false;
        private GDBProjectItem selectedGDB;

        private bool areaOfInterestDrawn = false;
        private string areaOfInterestPolygon;

        private List<String> entities;
        private List<List<String>> content;

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
            content = new List<List<String>>() { };
        }

        private void OnChangeGraph(object sender, EventArgs e)
        {
            CheckCanSelectContent();
        }

        private void OnChangeSpatialFilter(object sender, EventArgs e)
        {
            CheckCanSelectContent();
        }

        private void UploadGDBFile(object sender, EventArgs e)
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
                SearchForEntities();

                this.openGDBBtn.Text = selectedGDB.Name;
                this.openGDBBtn.Enabled = false;
                this.selectAreaBtn.Enabled = false;

                CheckCanSelectContent();
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

        public void SetDrawnPolygon(string polygonString)
        {
            areaOfInterestPolygon = polygonString;
            areaOfInterestDrawn = true;
            SearchForEntities();

            this.selectAreaBtn.Text = "Area Drawn";
            this.openGDBBtn.Enabled = false;
            this.selectAreaBtn.Enabled = false;

            CheckCanSelectContent();
            Show();
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

                    "?adminRegion2 geo:sfContains ?s2Cell. " +
                    "?s2Cell a kwg-ont:KWGCellLevel13. " +
                    "?s2Cell geo:hasGeometry ?s2Geo. " +
                    "?s2Geo geo:asWKT ?s2WKT. " +
                    "FILTER(geof:sfIntersects(\"" + areaOfInterestPolygon + "\"^^geo:wktLiteral, ?s2WKT)). " +
                "}";

                JToken s2Results = queryClass.SubmitQuery(this.knowledgeGraph.Text, s2Query);

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

            JToken entityResults = queryClass.SubmitQuery(this.knowledgeGraph.Text, entityQuery);

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
            var exploreWindow = new TraverseKnowledgeGraph(this, this.knowledgeGraph.Text, entities);
            this.Hide();
            exploreWindow.Show();
        }

        public void AddSelectedContent(List<string> uris, List<string> labels)
        {
            this.Show();

            //Add the label
            var uniqueLabels = labels.Distinct().ToList();
            string labelString = String.Join(" -> ", uniqueLabels);

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
            this.Controls.Add(labelObj);

            //Move the label
            labelObj.Location = new System.Drawing.Point(knowledgeGraph.Location.X, knowledgeGraph.Location.Y + contentTotalSpacing);
            int addedHeight = labelObj.Height + contentPadding;
            contentTotalSpacing += addedHeight;

            //Move things down
            selectContentBtn.Location = new System.Drawing.Point(selectContentBtn.Location.X, selectContentBtn.Location.Y + addedHeight);
            requiredSaveLayerAs.Location = new System.Drawing.Point(requiredSaveLayerAs.Location.X, requiredSaveLayerAs.Location.Y + addedHeight);
            saveLayerAsLabel.Location = new System.Drawing.Point(saveLayerAsLabel.Location.X, saveLayerAsLabel.Location.Y + addedHeight);
            saveLayerAs.Location = new System.Drawing.Point(saveLayerAs.Location.X, saveLayerAs.Location.Y + addedHeight);
            helpButton.Location = new System.Drawing.Point(helpButton.Location.X, helpButton.Location.Y + addedHeight);
            runBtn.Location = new System.Drawing.Point(runBtn.Location.X, runBtn.Location.Y + addedHeight);
            this.Height += addedHeight;

            //Capture the data
            content.Add(uris);
        }

        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            ToggleHelpMenu();
        }

        private void ToggleHelpMenu()
        {
            if(helpOpen)
            {
                this.Size = new System.Drawing.Size(this.Size.Width - helpSpacing, this.Size.Height);
                helpOpen = false;
            } else
            {
                this.Size = new System.Drawing.Size(this.Size.Width + helpSpacing, this.Size.Height);
                helpOpen = true;
            }
        }
    }
}

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
        private int helpSpacing = 400;
        private bool helpOpen = true;

        bool gdbFileUploaded = false;
        GDBProjectItem selectedGDB;

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
        }

        private void UploadGDBFile(object sender, EventArgs e)
        {
            var bpf = new BrowseProjectFilter("esri_browseDialogFilters_geodatabases_file");
            bpf.Name = "File Geodatabases";
            var openItemDialog = new OpenItemDialog { BrowseFilter = bpf };

            if ((bool)openItemDialog.ShowDialog())
            {
                selectedGDB = (GDBProjectItem)openItemDialog.Items.First();
                gdbFileUploaded = true;

                this.openGDBBtn.Text = selectedGDB.Name;
                this.selectAreaBtn.Enabled = false;
            }
        }

        private void DrawAreaOfInterest(object sender, EventArgs e)
        {
            /*if(!HasFormError())
            {
                Close();
                FrameworkApplication.SetCurrentToolAsync("KWG_Geoenrichment_DrawPolygon");
            }*/
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

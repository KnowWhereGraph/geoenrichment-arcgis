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
        private string currentEndpoint;
        private string entityVals;

        protected int maxDegree = 1;

        private int propertySpacing = 115;
        private int helpSpacing = 440;
        private bool helpOpen = true;

        public TraverseKnowledgeGraph(string endpoint, List<string> entities)
        {
            InitializeComponent();
            ToggleHelpMenu();

            currentEndpoint = endpoint;
            entityVals = "value ?entity {" + String.Join(" ", entities) + "}";

            PopulateBoxes(1);
        }

        private void PopulateBoxes(int lvl)
        {
            
        }

        private void RunTraverseGraph(object sender, EventArgs e)
        {
            if (object1.Text == "")
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

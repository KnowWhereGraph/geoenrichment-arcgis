﻿using ArcGIS.Core.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoenrichmentTool
{
    public partial class GeoSPARQL_Query : Form
    {
        protected string defaultNameSpace = "http://stko-kwg.geog.ucsb.edu/";
        protected string defaultEndpoint = "http://stko-roy.geog.ucsb.edu:7202/repositories/plume_soil_wildfire";
        protected string fileSavePath = "";

        public GeoSPARQL_Query()
        {
            InitializeComponent();
            this.endPoint.Text = defaultEndpoint;
        }

        private void submitGeoQueryForm(object sender, EventArgs e)
        {
            this.formError.Text = "";
            if (this.endPoint.Text == "" | this.className.Text == "")
            {
                this.formError.Text = "* Required fields missing!";
            }
            else
            {
                this.Close();
                ChooseFolder();
            }
        }

        public void ChooseFolder()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fileSavePath = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}

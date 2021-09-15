﻿using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KWG_Geoenrichment
{
    internal class KwgGeoModule : Module
    {
        private static KwgGeoModule _this = null;
        private readonly QuerySPARQL queryClass;
        protected List<BasicFeatureLayer> activeLayers;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static KwgGeoModule Current
        {
            get
            {
                return _this ?? (_this = (KwgGeoModule)FrameworkApplication.FindModule("KWG_Geoenrichment_Module"));
            }
        }

        KwgGeoModule()
        {
            queryClass = new QuerySPARQL();
            activeLayers = new List<BasicFeatureLayer> { };
        }

        public QuerySPARQL GetQueryClass()
        {
            return queryClass;
        }

        public void AddLayer(BasicFeatureLayer layerName)
        {
            activeLayers.Add(layerName);
        }

        public List<BasicFeatureLayer> GetLayers()
        {
            return activeLayers;
        }

        #region Overrides
        /// <summary>
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            //TODO - add your business logic
            //return false to ~cancel~ Application close
            return true;
        }

        #endregion Overrides
    }
}
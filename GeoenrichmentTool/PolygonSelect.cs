using ArcGIS.Core.CIM;
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
using System.Windows.Forms;

namespace GeoenrichmentTool
{
    internal class PolygonSelect : MapTool
    {
        public PolygonSelect()
        {
            IsSketchTool = true;
            SketchType = SketchGeometryType.Polygon;
            SketchOutputMode = SketchOutputMode.Map;
        }

        protected override Task OnToolActivateAsync(bool active)
        {
            return base.OnToolActivateAsync(active);
        }

        protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            MapView mv = MapView.Active;
            //bool zoom = mv.ZoomToAsync(geometry).Result;

            //TODO::Turn geometry into a feature class layer
            //GroupLayer groupLayer = QueuedTask.Run<GroupLayer>(() =>
            //{
            //    return LayerFactory.Instance.CreateGroupLayer(mv.Map, 0, "GeoSPARQL Layer");
            //}).Result;

            //var features = QueuedTask.Run<Dictionary<BasicFeatureLayer,List<long>>>(() =>
            //{
            //    return mv.GetFeatures(geometry);
            //}).Result;

            //var overlay = QueuedTask.Run<IDisposable>(() =>
            //{
            //    return mv.AddOverlay(geometry);
            //}).Result;

            //GDBProjectItem activeGDB = Project.Current.GetItems<GDBProjectItem>().First();
            //string path = activeGDB.Path;
            //Uri uri = new Uri(path);
            //FeatureClass fc = new FeatureClass();
            //FeatureLayer featureLayer = QueuedTask.Run<FeatureLayer>(() =>
            //{
            //return LayerFactory.Instance.CreateFeatureLayer("", groupLayer);
            //}).Result;

            //bool test = CreateFeatureAsync(mv, geometry).Result;
            

            Form geoForm = new GeoSPARQL_Query(geometry);
            geoForm.ShowDialog();
            return base.OnSketchCompleteAsync(geometry);
        }
    }
}

using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;

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

        private string defaultLayerPrefix = "GeoSPARQLQueryLayer_";

        protected override Task OnToolActivateAsync(bool active)
        {
            return base.OnToolActivateAsync(active);
        }

        protected async override Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            //We only support polygons at the moment
            //Some things might need more significant changes were other shapes allowed
            if (geometry.GetType().FullName == "ArcGIS.Core.Geometry.Polygon")
            {
                Polygon polyGeo = (Polygon)geometry;
                Form geoForm = new GeoSPARQL_Query(polyGeo);

                Random gen = new Random();
                string layerName = defaultLayerPrefix + gen.Next(999999).ToString();
                await FeatureClassHelper.CreatePolygonFeatureLayer(layerName);

                var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == layerName).FirstOrDefault() as BasicFeatureLayer;

                if (fcLayer == null)
                {
                    MessageBox.Show($@"Unable to find {layerName} in the active map");
                }
                else
                {
                    //TODO:: May need to figure out why layer isnt visible, but I think the polygon needs data first
                    // make 5 points
                    //var coordinates = polyGeo.Copy2DCoordinatesToList();

                    //List<MapPoint> mapPoints = new List<MapPoint>();
                    //foreach (Coordinate2D coor in coordinates)
                    //{
                    //    mapPoints.Add(coor.ToMapPoint());
                    //}
                    //  var editOp = new EditOperation
                    //  {
                    //    Name = "1. edit operation"
                    //  };
                    //  int iMap = 0;
                    //  foreach (var mp in mapPoints)
                    //  {
                    //    var attributes = new Dictionary<string, object>
                    //        {
                    //          { "Shape", mp.Clone() },
                    //          { "Description", $@"Map point: {++iMap}" }
                    //        };
                    //    editOp.Create(fcLayer, attributes);
                    //  }
                    //  var result1 = editOp.Execute();
                    //  if (result1 != true || editOp.IsSucceeded != true)
                    //    throw new Exception($@"Edit 1 failed: {editOp.ErrorMessage}");
                    //  MessageBox.Show("1. edit operation complete");

                    //editOp = new EditOperation
                    //{
                    //  Name = "2. edit operation"
                    //};
                    //foreach (var mp in mapPoints)
                    //{
                    //  var attributes = new Dictionary<string, object>
                    //      {
                    //        { "Shape", GeometryEngine.Instance.Buffer(mp, 50.0) },
                    //        { "Description", $@"Polygon: {iMap--}" }
                    //      };
                    //  editOp.Create(polyLayer, attributes);
                    //}
                    ////Execute the operations
                    //var result2 = editOp.Execute();
                    //if (result2 != true || editOp.IsSucceeded != true)
                    //  throw new Exception($@"Edit 2 failed: {editOp.ErrorMessage}");
                    //MessageBox.Show("2. edit operation complete");
                    geoForm.ShowDialog();
                }
            }

            return await base.OnSketchCompleteAsync(geometry);
        }
    }
}

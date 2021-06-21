using ArcGIS.Core.Data;
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
        private SpatialReference sr;

        protected override Task OnToolActivateAsync(bool active)
        {
            QueuedTask.Run(() =>
            {
                sr = SpatialReferenceBuilder.CreateSpatialReference(4326);
                MapView.Active.Map.SetSpatialReference(sr);
            });
            return base.OnToolActivateAsync(active);
        }

        protected async override Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            //We only support polygons at the moment
            //Some things might need more significant changes were other shapes allowed
            if (geometry.GetType().FullName == "ArcGIS.Core.Geometry.Polygon")
            {
                Polygon polyGeo = (Polygon)geometry;

                //Build the string Polygon value
                var coordinates = polyGeo.Copy2DCoordinatesToList();
                List<string> coorArray = new List<string>();
                foreach (Coordinate2D coor in coordinates)
                {
                    MapPoint geoCoor = coor.ToMapPoint();
                    coorArray.Add(coor.X.ToString() + " " + coor.Y.ToString());
                }
                string polyString = "Polygon((" + String.Join(", ", coorArray) + "))";

                //Create layer for the polygon
                Random gen = new Random();
                string layerName = defaultLayerPrefix + gen.Next(999999).ToString();
                await FeatureClassHelper.CreatePolygonFeatureLayer(layerName);

                var mainLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == layerName).FirstOrDefault() as BasicFeatureLayer;

                if (mainLayer == null)
                {
                    MessageBox.Show($@"Unable to find {layerName} in the active map");
                }
                else
                {
                    //Add polygon value to layer
                    await QueuedTask.Run(() =>
                    {
                        mainLayer.SetTransparency(50.0);

                        InsertCursor cursor = mainLayer.GetTable().CreateInsertCursor();

                        RowBuffer buff = mainLayer.GetTable().CreateRowBuffer();
                        IGeometryEngine geoEngine = GeometryEngine.Instance;
                        Geometry geo = geoEngine.ImportFromWKT(0, polyString, sr);

                        buff["Shape"] = geo;
                        cursor.Insert(buff);

                        cursor.Dispose();

                        MapView.Active.Redraw(false);
                    });

                    Form geoForm = new GeoSPARQL_Query(polyString);
                    geoForm.ShowDialog();
                }
            }

            return await base.OnSketchCompleteAsync(geometry);
        }
    }
}

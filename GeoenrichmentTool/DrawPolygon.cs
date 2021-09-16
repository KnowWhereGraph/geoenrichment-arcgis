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

namespace KWG_Geoenrichment
{
    internal class DrawPolygon : MapTool
    {
        private SpatialReference sr;
        private string layerPrefix = "KWG_AreaOfInterest_";

        public DrawPolygon()
        {
            IsSketchTool = true;
            SketchType = SketchGeometryType.Polygon;
            SketchOutputMode = SketchOutputMode.Map;
        }

        protected override Task OnToolActivateAsync(bool active)
        {
            QueuedTask.Run(() =>
            {
                sr = SpatialReferenceBuilder.CreateSpatialReference(4326);
                MapView.Active.Map.SetSpatialReference(sr);
            });

            return base.OnToolActivateAsync(active);
        }

        protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            //We only support polygons at the moment
            //Some things might need more significant changes were other shapes allowed
            if (geometry.GetType().FullName == "ArcGIS.Core.Geometry.Polygon")
            {
                CreateDrawnPolygonLayer((Polygon)geometry);
            }

            //TODO::Close the map tool

            return base.OnSketchCompleteAsync(geometry);
        }

        private async void CreateDrawnPolygonLayer(Polygon polyGeo)
        {
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
            string layerName = layerPrefix + gen.Next(999999).ToString();
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
            }
        }
    }
}

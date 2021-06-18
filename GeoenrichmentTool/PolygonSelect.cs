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

                var mainLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == layerName).FirstOrDefault() as BasicFeatureLayer;

                if (mainLayer == null)
                {
                    MessageBox.Show($@"Unable to find {layerName} in the active map");
                }
                else
                {
                    await QueuedTask.Run(() =>
                    {
                        InsertCursor cursor = mainLayer.GetTable().CreateInsertCursor();

                        RowBuffer buff = mainLayer.GetTable().CreateRowBuffer();

                        buff["Shape"] = polyGeo;
                        cursor.Insert(buff);

                        cursor.Dispose();

                        MapView.Active.Redraw(false);
                    });

                    geoForm.ShowDialog();
                }
            }

            return await base.OnSketchCompleteAsync(geometry);
        }
    }
}

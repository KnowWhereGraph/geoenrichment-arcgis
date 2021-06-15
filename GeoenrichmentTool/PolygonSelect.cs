using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
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
        public enum EnumFeatureClassType
        {
            POINT,
            MULTIPOINT,
            POLYLINE,
            POLYGON
        }

        protected override Task OnToolActivateAsync(bool active)
        {
            return base.OnToolActivateAsync(active);
        }

        protected async override Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            //We only support polygons at the moment
            //Some things might need more significant changes were other shapes allowed
            if(geometry.GetType().FullName == "ArcGIS.Core.Geometry.Polygon")
            {
                Polygon polyGeo = (Polygon)geometry;
                Form geoForm = new GeoSPARQL_Query(polyGeo);

                await CreatePolygonFeatureLayer(polyGeo.SpatialReference);

                var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == "GeoSPARQLQueryLayer").FirstOrDefault() as BasicFeatureLayer;
                if (fcLayer == null)
                {
                    //MessageBox.Show($@"Unable to find {fcName} in the active map");
                    string test = "";
                }
                {
                    geoForm.ShowDialog();
                }
            }

            return await base.OnSketchCompleteAsync(geometry);
        }

        public static async Task CreatePolygonFeatureLayer(SpatialReference polySR)
        {
            List<object> arguments = new List<object>
              {
                // store the results in the default geodatabase
                CoreModule.CurrentProject.DefaultGeodatabasePath,
                // name of the feature class
                "GeoSPARQLQueryLayer",
                // type of geometry
                EnumFeatureClassType.POLYGON.ToString(),
                // no template
                "",
                // no z values
                "DISABLED",
                // no m values
                "DISABLED"
              };

            await QueuedTask.Run(() =>
            {
                // spatial reference
                arguments.Add(polySR);
            });
            IGPResult result = await Geoprocessing.ExecuteToolAsync("CreateFeatureclass_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }
    }
}

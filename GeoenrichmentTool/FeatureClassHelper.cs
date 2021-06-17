using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoenrichmentTool
{
    public enum EnumFeatureClassType
    {
        POINT,
        MULTIPOINT,
        POLYLINE,
        POLYGON
    }

    class FeatureClassHelper
    {
        public static async Task CreatePolygonFeatureLayer(string layerName)
        {
            List<object> arguments = new List<object>
            {
                // store the results in the default geodatabase
                CoreModule.CurrentProject.DefaultGeodatabasePath,
                // name of the feature class
                layerName,
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
                arguments.Add(SpatialReferenceBuilder.CreateSpatialReference(4326));
            });
            IGPResult result = await Geoprocessing.ExecuteToolAsync("CreateFeatureclass_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }

        public static async Task AddField(BasicFeatureLayer featureLayer, string fieldName, string fieldType)
        {
            List<object> arguments = new List<object>
            {
                //feature layer
                featureLayer,
                // name of the data field
                fieldName,
                // type of data field
                fieldType
            };

            IGPResult result = await Geoprocessing.ExecuteToolAsync("AddField_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }
    }
}

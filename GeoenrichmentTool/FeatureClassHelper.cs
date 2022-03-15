using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;

namespace KWG_Geoenrichment
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
        public static async Task CreateFeatureClassLayer(string layerName, string shape = "Polygon")
        {
            //This is the only geo type where the class names don't match between libraries
            shape = (shape == "MapPoint") ? "Point" : shape;

            List<object> arguments = new List<object>
            {
                // store the results in the default geodatabase
                CoreModule.CurrentProject.DefaultGeodatabasePath,
                // name of the feature class
                layerName,
                // type of geometry
                shape,
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

        public static string ValidateTableName(string tableName)
        {
            char[] invalids = { '`', '~', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '|', ',', '<', '>', '?', '/', '{', '}', '.', '!', '\'', '[', ']', ':', ';', ' ' };

            foreach(char c in invalids)
            {
                tableName = tableName.Replace(c, '_');
            }

            return tableName;
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

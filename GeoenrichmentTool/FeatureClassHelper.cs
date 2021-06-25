using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static async Task<List<string>> GetURIs(BasicFeatureLayer featLayer)
        {
            List<string> uris = new List<string>() { };

            await QueuedTask.Run(() =>
            {
                using (var tbl = featLayer.GetTable())
                {
                    using (var datastore = tbl.GetDatastore())
                    {
                        if (datastore is UnknownDatastore) return;
                        var geodatabase = datastore as Geodatabase;
                        if (geodatabase == null) return;
                        var queryDef = new QueryDef
                        {
                            Tables = featLayer.Name,
                            SubFields = "URL"
                        };
                        var result = new DataTable("results");
                        using (var rowCursor = geodatabase.Evaluate(queryDef, false))
                        {
                            while (rowCursor.MoveNext())
                            {
                                using (Row row = rowCursor.Current)
                                {
                                    string uri = row[0].ToString();
                                    uris.Add(uri);
                                }
                            }
                        }
                    }
                }
            });

            return uris;
        }
    }
}

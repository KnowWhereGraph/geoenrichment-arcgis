﻿using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
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

        /*
         * according to the json object from sparql query which contains the mapping from keyProperty to valueProperty, add field in the Table
            # valuePropertyURL: the URL of valueProperty, we use it to get the field name of valueProperty, ex. functionalProperty
            # jsonBindingObject: the json object from sparql query which contains the mapping from keyProperty to valueProperty, ex. functionalPropertyJSON
            # keyPropertyName: the name of keyProperty in JSON object, ex. wikidataSub
            # valuePropertyName: the name of valueProperty in JSON object, ex. o
            # keyPropertyFieldName:  the name of the field which stores the value of keyProperty, ex. URL
            # isInverse: Boolean variable indicates whether the value we get is the subject value or object value of valuePropertyURL
         */
        public static async Task AddFieldInTableByMapping(string valuePropertyURL, JToken jsonBindingObject, string keyPropertyName, string valuePropertyName, string keyPropertyFieldName, bool isInverse)
        {
            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();

            List<string>[] keyValueDict = ProcessPropertyValueQueryResult(jsonBindingObject, keyPropertyName, valuePropertyName);

            string currentValuePropertyName = "";
            char[] delimSharp = { '#' };
            char[] delimSlash = { '/' };
            if(valuePropertyURL.Contains("#"))
            {
                currentValuePropertyName = valuePropertyURL.Split(delimSharp)[1];
            }
            else
            {
                currentValuePropertyName = valuePropertyURL.Split(delimSlash)[1];
            }

            string currentFieldName = (isInverse) ? "is_" + currentValuePropertyName + "_Of" : currentValuePropertyName;

            if(IsFieldNameInTable(currentValuePropertyName,mainLayer).Result)
            {
                Random gen = new Random();
                currentFieldName = currentFieldName + gen.Next(999999).ToString();
            }

            string fieldType = DetermineFieldDataType(jsonBindingObject, valuePropertyName);

            await AddField(mainLayer, currentFieldName, fieldType);

            /*            
            cursor = arcpy.UpdateCursor(inputFeatureClassName)
            
            for row in cursor:
                currentKeyPropertyValue = row.getValue(keyPropertyFieldName)
                if currentKeyPropertyValue in keyValueDict:
                    propertyValue = Json2Field.dataTypeCast(keyValueDict[currentKeyPropertyValue], fieldType)
                    # row[1] = propertyValue
                    row.setValue(currentFieldName, propertyValue)
                    cursor.updateRow(row)
             */
        }

        private static List<string>[] ProcessPropertyValueQueryResult(JToken propertyValues, string keyPropertyName, string valuePropertyName)
        {
            List<string> valueList = new List<string>() { };
            List<string> keyPropList = new List<string>() { };

            foreach (var item in propertyValues)
            {
                valueList.Add((string)item[valuePropertyName]["value"]);
                keyPropList.Add((string)item[keyPropertyName]["value"]);
            }

            return new List<string>[] { keyPropList, valueList };
        }

        private static async Task<bool> IsFieldNameInTable(string fieldName, BasicFeatureLayer featureClass)
        {
            IReadOnlyList<Field> fields = null;
            await QueuedTask.Run(() =>
            {
                fields = featureClass.GetTable().GetDefinition().GetFields();
            });

            foreach(var field in fields)
            {
                if(field.Name == fieldName)
                    return true;
            }

            return false;
        }

        /*
        # jsonBindingObject: a list object which is jsonObject.json()["results"]["bindings"]
        # fieldName: the name of the property/field in the JSON object thet what to evaluate
        # return the Field data type given a JSONItem for one property, return -1 if the field is about geometry and bnode
         */
        private static string DetermineFieldDataType(JToken propertyValues, string valuePropertyName)
        {
            Dictionary<string, int> types = new Dictionary<string, int>() { };
            foreach(var item in propertyValues)
            {
                string linkedType = GetLinkedDataType(item, valuePropertyName);
                if(types.ContainsKey(linkedType))
                {
                    types[linkedType] = types[linkedType] + 1;
                }
                else
                {
                    types[linkedType] = 1;
                }
            }

            string leader = "string";
            int leaderCnt = 0;
            foreach(var linkedType in types)
            {
                if(linkedType.Value > leaderCnt)
                {
                    leader = linkedType.Key;
                    leaderCnt = linkedType.Value;
                }
            }

            return leader;
        }

        private static string UrlDataType2FieldDataType(string urlDataType)
        {
            switch(urlDataType)
            {
                case "uri":
                case "string":
                    return "TEXT";
                case "date":
                    return "DATE";
                case "int":
                    return "LONG";
                case "double":
                    return "DOUBLE";
                case "float":
                    return "FLOAT";
                default:
                    return "TEXT";
            }
        }

        /*
        # according the the property name of this jsonBindingObjectItem, return the meaningful dataType
         */
        private static string GetLinkedDataType(JToken propertyValue, string valuePropertyName)
        {
            string rdfDataType = propertyValue[valuePropertyName]["type"].ToString();
            if (rdfDataType == "uri" | rdfDataType == "bnode")
            {
                return rdfDataType;
            }
            else if (rdfDataType.Contains("literal") && propertyValue[valuePropertyName]["datatype"] != null)
            {
                switch(propertyValue[valuePropertyName]["datatype"].ToString())
                {
                    case "http://www.w3.org/2001/XMLSchema#date":
                        return "date";
                    case "http://www.openlinksw.com/schemas/virtrdf#Geometry":
                        return "geometry";
                    case "http://www.w3.org/2001/XMLSchema#integer":
                    case "http://www.w3.org/2001/XMLSchema#nonNegativeInteger":
                        return "int";
                    case "http://www.w3.org/2001/XMLSchema#double":
                        return "double";
                    case "http://www.w3.org/2001/XMLSchema#float":
                        return "float";
                    default:
                        return "string";
                }
            }
            else
            {
                return "string";
            }
        }
    }
}

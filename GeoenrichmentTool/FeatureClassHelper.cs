﻿using ArcGIS.Core.Data;
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

        public static async Task CreateTable(string tableName)
        {
            List<object> arguments = new List<object>
            {
                // store the results in the default geodatabase
                CoreModule.CurrentProject.DefaultGeodatabasePath,
                // name of the table
                tableName
            };

            IGPResult result = await Geoprocessing.ExecuteToolAsync("CreateTable_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }

        public static string ValidateTableName(string tableName)
        {
            char[] invalids = { '`', '~', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '|', ',', '<', '>', '?', '/', '{', '}', '.', '!', '\'', '[', ']', ':', ';' };

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

        public static async Task AddField(Table geoTable, string fieldName, string fieldType)
        {
            List<object> arguments = new List<object>
            {
                // name of table in geodatabase
                geoTable,
                // name of the data field
                fieldName,
                // type of data field
                fieldType
            };

            IGPResult result = await Geoprocessing.ExecuteToolAsync("AddField_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }

        public static async Task CalculateField(BasicFeatureLayer featureLayer, string fieldName, string expression, string expressionType)
        {
            List<object> arguments = new List<object>
            {
                //feature layer
                featureLayer, 
                //field name
                fieldName, 
                //expression
                expression, 
                //expression type
                expressionType
            };

            IGPResult result = await Geoprocessing.ExecuteToolAsync("CalculateField_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }

        public static async Task CreateRelationshipClass(BasicFeatureLayer featureClass, Table dataTable, string relationshipClassName, string forwardLabel, string backwardLabel, string foreignKey = "URL")
        {
            List<object> arguments = new List<object>
            {
                featureClass, dataTable, CoreModule.CurrentProject.DefaultGeodatabasePath+"\\"+relationshipClassName, "SIMPLE", forwardLabel, backwardLabel, 
                "FORWARD", "ONE_TO_MANY", "NONE", "URL", foreignKey
            };

            IGPResult result = await Geoprocessing.ExecuteToolAsync("CreateRelationshipClass_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
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

        /**
         * Format GeoSPARQL query by given query_geo_wkt and type
         * 
         * geoQueryResult: a sparql query result json obj
         * layerName: Name of feature layer class
         * featureTypeURI: the user spercified feature type IRI
         * ignoreSubclasses: Do we ignore all subclasses of our feature type
         **/
        public static async Task CreateClassFromSPARQL(JToken geoQueryResult, string layerName, string featureTypeURI = "", bool ignoreSubclasses = false)
        {
            List<string> placeIRISet = new List<string>();
            List<string[]> placeList = new List<string[]>();
            //string geomType = "";

            int index = 0;
            foreach (var item in geoQueryResult)
            {
                /*
                 * Not sure if this block will be needed but will leave in case the error eventaully needs to be caught
                 * Basically this makes sure that returned results are also polygon objects
                string wktLiteral = item["wkt"]["value"].ToString();
                if(index == 0)
                {
                    geomType = GetShapeFromWKT(wktLiteral);
                }
                else
                {
                    if(wktLiteral!=geomType)
                    {
                        continue;
                    }
                }*/

                string placeType = (item["placeFlatType"] != null) ? item["placeFlatType"]["value"].ToString() : "";
                if (ignoreSubclasses)
                {
                    placeType = featureTypeURI;
                }

                string place = item["place"]["value"].ToString();
                if (!placeIRISet.Contains(place))
                {
                    placeIRISet.Add(place);
                    placeList.Add(new string[] { place, item["placeLabel"]["value"].ToString(), placeType, item["wkt"]["value"].ToString() });
                }

                index++;
            }

            if (placeList.Count != 0)
            {
                await FeatureClassHelper.CreatePolygonFeatureLayer(layerName);

                var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == layerName).FirstOrDefault() as BasicFeatureLayer;

                if (fcLayer == null)
                {
                    MessageBox.Show($@"Unable to find {layerName} in the active map");
                }
                else
                {
                    await Project.Current.SaveEditsAsync();

                    await FeatureClassHelper.AddField(fcLayer, "Label", "TEXT");
                    await FeatureClassHelper.AddField(fcLayer, "URL", "TEXT");
                    await FeatureClassHelper.AddField(fcLayer, "Class", "TEXT");


                    await QueuedTask.Run(() =>
                    {
                        InsertCursor cursor = fcLayer.GetTable().CreateInsertCursor();

                        foreach (string[] item in placeList)
                        {
                            RowBuffer buff = fcLayer.GetTable().CreateRowBuffer();
                            IGeometryEngine geoEngine = GeometryEngine.Instance;
                            SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(4326);
                            Geometry geo = geoEngine.ImportFromWKT(0, item[3].Replace("<http://www.opengis.net/def/crs/OGC/1.3/CRS84>", ""), sr);


                            buff["URL"] = item[0];
                            buff["Label"] = item[1];
                            buff["Class"] = item[2];
                            buff["Shape"] = geo;
                            cursor.Insert(buff);
                        }

                        cursor.Dispose();

                        MapView.Active.Redraw(false);

                        //Save layer name to main list of active layers so other tools can access them
                        //KwgGeoModule.Current.AddLayer(fcLayer);
                    });
                }
            }
        }

        private string GetShapeFromWKT(string WKT)
        {
            if (WKT.ToLower().Contains("point"))
            {
                return "POINT";
            }
            else if (WKT.ToLower().Contains("multipoint"))
            {
                return "MULTIPOINT";
            }
            else if (WKT.ToLower().Contains("linestring"))
            {
                return "POLYLINE";
            }
            else if (WKT.ToLower().Contains("polygon"))
            {
                return "polygon";
            }

            return "";
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
            /*BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();

            Dictionary<string, string> keyValueDict = ProcessPropertyValueQueryResult(jsonBindingObject, keyPropertyName, valuePropertyName);

            string currentValuePropertyName = GetPropertyName(valuePropertyURL);

            string currentFieldName = (isInverse) ? "is_" + currentValuePropertyName + "_Of" : currentValuePropertyName;

            if (IsFieldNameInTable(currentValuePropertyName, mainLayer))
            {
                Random gen = new Random();
                currentFieldName = currentFieldName + gen.Next(999999).ToString();
            }

            string fieldType = DetermineFieldDataType(jsonBindingObject, valuePropertyName);

            await AddField(mainLayer, currentFieldName, fieldType);

            string message = String.Empty;
            bool modificationResult = false;

            await QueuedTask.Run(() => {
                using (var featureTable = mainLayer.GetTable())
                {
                    EditOperation editOperation = new EditOperation();
                    editOperation.Callback(context => {
                        QueryFilter openCutFilter = new QueryFilter { };

                        using (RowCursor rowCursor = featureTable.Search(openCutFilter, false))
                        {
                            TableDefinition tableDefinition = featureTable.GetDefinition();

                            while (rowCursor.MoveNext())
                            {
                                using (Row row = rowCursor.Current)
                                {
                                    // In order to update the Map and/or the attribute table.
                                    // Has to be called before any changes are made to the row.
                                    context.Invalidate(row);

                                    string rowValue = row[keyPropertyFieldName].ToString();

                                    if (keyValueDict.ContainsKey(rowValue))
                                    {
                                        var propVal = keyValueDict[rowValue];
                                        //propertyValue = Json2Field.dataTypeCast(keyValueDict[currentKeyPropertyValue], fieldType)
                                        row[currentFieldName] = propVal;
                                    }

                                    //After all the changes are done, persist it.
                                    row.Store();

                                    // Has to be called after the store too.
                                    context.Invalidate(row);
                                }
                            }
                        }
                    }, featureTable);

                    try
                    {
                        modificationResult = editOperation.Execute();
                        if (!modificationResult) message = editOperation.ErrorMessage;
                    }
                    catch (GeodatabaseException exObj)
                    {
                        message = exObj.Message;
                    }
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);*/
        }

        /*
        # according to jsonBindingObject, create a sperate table to store the nofunctional property-value pairs 
        # OR store the transtive "isPartOf" relationship between location and its subDivision
        # return the name of the table without the full path
        # isInverse: Boolean variable indicates whether the value we get is the subject value or object value of valuePropertyURL
        # isSubDivisionTable: Boolean variable indicates whether the current table store the value of subdivision for the original location
         */
        //public static async Task<Table> CreateMappingTableFromJSON(string valuePropertyURL, JToken jsonBindingObject, string keyPropertyName, string valuePropertyName, string keyPropertyFieldName, bool isInverse, bool isSubDivisionTable)
        //{
            /*BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();

            Dictionary<string, string> keyValueDict = ProcessPropertyValueQueryResult(jsonBindingObject, keyPropertyName, valuePropertyName);

            string featureClassName = mainLayer.Name;

            string currentValuePropertyName = GetPropertyName(valuePropertyURL);
            currentValuePropertyName = ValidateTableName(currentValuePropertyName);
            if (isInverse)
                currentValuePropertyName = "is_" + currentValuePropertyName + "_Of";
            if (isSubDivisionTable)
                currentValuePropertyName = "subDivisionIRI";

            //if outputLocation.endswith(".gdb"):
            string tableName = featureClassName + "_" + keyPropertyFieldName + "_" + currentValuePropertyName;
            *//*
            else:
                lastIndexOFshp = featureClassName.rfind(".")
                featureClassName = featureClassName[:lastIndexOFshp]

                tableName =  featureClassName + "_" + keyPropertyFieldName + "_" + currentValuePropertyName + ".dbf"
            *//*

            var datastore = mainLayer.GetTable().GetDatastore();
            var geodatabase = datastore as Geodatabase;

            if (DoesTableNameExist(geodatabase, tableName))
            {
                Random gen = new Random();
                tableName = tableName + gen.Next(999999).ToString();
            }

            await CreateTable(tableName);
            var propertyTable = geodatabase.OpenDataset<Table>(tableName);

            await Project.Current.SaveEditsAsync();

            await AddField(propertyTable, keyPropertyFieldName, "TEXT");

            string valuePropertyFieldType = DetermineFieldDataType(jsonBindingObject, valuePropertyName);
            await AddField(propertyTable, currentValuePropertyName, valuePropertyFieldType);

            string message = String.Empty;
            bool creationResult = false;
            EditOperation editOperation = new EditOperation();

            await QueuedTask.Run(() => {
                editOperation.Callback(context =>
                {
                    using (RowBuffer rowBuffer = propertyTable.CreateRowBuffer())
                    {
                        foreach (var item in keyValueDict)
                        {
                            rowBuffer[keyPropertyFieldName] = item.Key;
                            rowBuffer[currentValuePropertyName] = item.Value;
                            using (Row row = propertyTable.CreateRow(rowBuffer))
                            {
                                // To Indicate that the attribute table has to be updated.
                                context.Invalidate(row);
                            }
                        }
                    }
                }, propertyTable);

                try
                {
                    creationResult = editOperation.Execute();
                    if (!creationResult) message = editOperation.ErrorMessage;
                }
                catch (GeodatabaseException exObj)
                {
                    message = exObj.Message;
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);

            return propertyTable;*/
        //}

        //public static async Task<Dictionary<string, List<string>>> BuildMultiValueDictFromNoFunctionalProperty(string fieldName, string tableName, string urlFieldName)
        //{
            /*Dictionary<string, List<string>> valueDict = new Dictionary<string, List<string>>() { };
            BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();

            await QueuedTask.Run(() =>
            {
                var datastore = mainLayer.GetTable().GetDatastore();
                var geodatabase = datastore as Geodatabase;

                var queryDef = new QueryDef
                {
                    Tables = tableName
                };
                var result = new DataTable("results");
                using (var rowCursor = geodatabase.Evaluate(queryDef, false))
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            var foreignKeyValue = row[urlFieldName].ToString();
                            var noFunctionalPropertyValue = row[fieldName].ToString();

                            if (!valueDict.ContainsKey(foreignKeyValue))
                            {
                                valueDict[foreignKeyValue] = new List<string>() { };
                            }

                            valueDict[foreignKeyValue].Add(noFunctionalPropertyValue);
                        }
                    }
                }
            });

            return valueDict;*/
        //}

        /*
        # append a new field in inputFeatureClassName which will install the merged no-functional property value
        # noFunctionalPropertyDict: the collections.defaultdict object which stores the no-functional property value for each URL
        # appendFieldName: the field name of no-functional property in the relatedTableName
        # mergeRule: the merge rule the user selected, one of ['SUM', 'MIN', 'MAX', 'STDEV', 'MEAN', 'COUNT', 'FIRST', 'LAST']
        # delimiter: the optional paramter which define the delimiter of the cancatenate operation
        */
        public static async Task AppendFieldInFeatureClassByMergeRule(Dictionary<string, List<string>> noFunctionalPropertyDict, string appendFieldName, 
            string relatedTableName, string mergeRule)
        {
            /*BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();

            string appendFieldType = "";
            //int appendFieldLength = 0;
            await QueuedTask.Run(() =>
            {
                var datastore = mainLayer.GetTable().GetDatastore();
                var geodatabase = datastore as Geodatabase;
                var propertyTable = geodatabase.OpenDataset<Table>(relatedTableName);

                foreach (var field in propertyTable.GetDefinition().GetFields())
                {
                    if (field.Name == appendFieldName) {
                        appendFieldType = field.FieldType.ToString();
                        //if field.type == "String":
                        //    appendFieldLength = field.length
                        break;
                    }
                }
            });

            Dictionary<string, string> mergeConvert = new Dictionary<string, string>() { { "STDEV","STD" }, { "MEAN", "MEN" }, { "CONCATENATE", "CONCAT" } };
            if(mergeConvert.ContainsKey(mergeRule))
            {
                mergeRule = mergeConvert[mergeRule];
            }

            *//*
            if appendFieldType != "String":
                cursor = arcpy.SearchCursor(relatedTableName)
                for row in cursor:
                    rowValue = row.getValue(appendFieldName)
                    if appendFieldLength < len(str(rowValue)):
                        appendFieldLength = len(str(rowValue))
            *//*

            string featureClassAppendFieldName = appendFieldName + "_" + mergeRule;
            if (IsFieldNameInTable(featureClassAppendFieldName, mainLayer))
            {
                Random gen = new Random();
                featureClassAppendFieldName = featureClassAppendFieldName + gen.Next(999999).ToString();
            }

            //string appendFieldType = "TEXT";
            if(mergeRule == "COUNT")
                appendFieldType = "SHORT";
            else if(mergeRule == "STDEV" | mergeRule == "MEAN")
                appendFieldType = "DOUBLE";
            else if (mergeRule == "CONCAT")
            {
                appendFieldType = "TEXT";
                *//*
                # get the maximum number of values for current property: maxNumOfValue
                maxNumOfValue = 1
                for key in noFunctionalPropertyDict:
                    if maxNumOfValue < len(noFunctionalPropertyDict[key]):
                        maxNumOfValue = len(noFunctionalPropertyDict[key])
                arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, 'TEXT', field_length=appendFieldLength * maxNumOfValue)
                 *//*
            }

            *//*   
            else:
                if appendFieldType == "String":
                    arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, appendFieldType, field_length=appendFieldLength)
            *//*

            await Project.Current.SaveEditsAsync();
            await AddField(mainLayer, featureClassAppendFieldName, appendFieldType);

            if (IsFieldNameInTable("URL", mainLayer))
            {
                string message = String.Empty;
                bool modificationResult = false;

                await QueuedTask.Run(() => {
                    using (var featureTable = mainLayer.GetTable())
                    {
                        EditOperation editOperation = new EditOperation();
                        editOperation.Callback(context => {
                            QueryFilter openCutFilter = new QueryFilter { };

                            using (RowCursor rowCursor = featureTable.Search(openCutFilter, false))
                            {
                                TableDefinition tableDefinition = featureTable.GetDefinition();

                                while (rowCursor.MoveNext())
                                {
                                    using (Row row = rowCursor.Current)
                                    {
                                        // In order to update the Map and/or the attribute table.
                                        // Has to be called before any changes are made to the row.
                                        context.Invalidate(row);

                                        string foreignKeyValue = row["URL"].ToString();
                                        if(!noFunctionalPropertyDict.ContainsKey(foreignKeyValue))
                                        {
                                            continue;
                                        }
                                        List<string> noFunctionalPropertyValueList = noFunctionalPropertyDict[foreignKeyValue];

                                        string rowValue = "";
                                        switch (mergeRule)
                                        {
                                            case "STDEV":
                                                List<double> stdevVals = new List<double>() { };
                                                foreach (var strVal in noFunctionalPropertyValueList)
                                                {
                                                    stdevVals.Add(Convert.ToDouble(strVal));
                                                }
                                                double average = stdevVals.Average();
                                                double sumOfSquaresOfDifferences = stdevVals.Select(val => (val - average) * (val - average)).Sum();
                                                rowValue = Math.Sqrt(sumOfSquaresOfDifferences / stdevVals.Count).ToString();
                                                break;
                                            case "MEAN":
                                                List<double> meanVals = new List<double>() { };
                                                foreach (var strVal in noFunctionalPropertyValueList)
                                                {
                                                    meanVals.Add(Convert.ToDouble(strVal));
                                                }
                                                rowValue = meanVals.Average().ToString();
                                                break;
                                            case "SUM":
                                                double sumVal = 0;
                                                foreach (var strVal in noFunctionalPropertyValueList)
                                                {
                                                    sumVal += Convert.ToDouble(strVal);
                                                }
                                                rowValue = sumVal.ToString();
                                                break;
                                            case "MIN":
                                                double minVal = 99999999999;
                                                foreach (var strVal in noFunctionalPropertyValueList)
                                                {
                                                    double numVal = Convert.ToDouble(strVal);
                                                    if (numVal < minVal)
                                                    {
                                                        minVal = numVal;
                                                    }
                                                }
                                                rowValue = minVal.ToString();
                                                break;
                                            case "MAX":
                                                double maxVal = -.99999999999;
                                                foreach (var strVal in noFunctionalPropertyValueList)
                                                {
                                                    double numVal = Convert.ToDouble(strVal);
                                                    if(numVal > maxVal)
                                                    {
                                                        maxVal = numVal;
                                                    }
                                                }
                                                rowValue = maxVal.ToString();
                                                break;
                                            case "COUNT":
                                                rowValue = noFunctionalPropertyValueList.Count().ToString();
                                                break;
                                            case "FIRST":
                                                rowValue = noFunctionalPropertyValueList.First();
                                                break;
                                            case "LAST":
                                                rowValue = noFunctionalPropertyValueList.Last();
                                                break;
                                            case "CONCAT":
                                                rowValue = String.Join(" | ", noFunctionalPropertyValueList.ToArray());
                                                break;
                                        }
                                        row[featureClassAppendFieldName] = rowValue;

                                        //After all the changes are done, persist it.
                                        row.Store();

                                        // Has to be called after the store too.
                                        context.Invalidate(row);
                                    }
                                }
                            }
                        }, featureTable);

                        try
                        {
                            modificationResult = editOperation.Execute();
                            if (!modificationResult) message = editOperation.ErrorMessage;
                        }
                        catch (GeodatabaseException exObj)
                        {
                            message = exObj.Message;
                        }
                    }
                });

                if (!string.IsNullOrEmpty(message))
                    MessageBox.Show(message);
            }*/
        }

        public static string GetPropertyName(string valuePropertyURL) {
            char[] delimSharp = { '#' };
            char[] delimSlash = { '/' };
            if (valuePropertyURL.Contains("#"))
            {
                return valuePropertyURL.Split(delimSharp).Last();
            }
            else
            {
                return valuePropertyURL.Split(delimSlash).Last();
            }
        }

        public async static void CreateRelationshipFinderTable(List<Dictionary<string, string>> tripleStore, List<string> triplePropertyURLList, List<string> triplePropertyLabelList, string tableName)
        {
            /*BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();
            Geodatabase geodatabase = await QueuedTask.Run(() =>
            {
                var datastore = mainLayer.GetTable().GetDatastore();
                geodatabase = datastore as Geodatabase;
                return geodatabase;
            });

            await CreateTable(tableName);

            Table tripleStoreTable = await QueuedTask.Run(() =>
            {
                var tripTbl = geodatabase.OpenDataset<Table>(tableName);

                Project.Current.SaveEditsAsync();

                AddField(tripTbl, "Subject", "TEXT").Wait();
                AddField(tripTbl, "Predicate", "TEXT").Wait();
                AddField(tripTbl, "Object", "TEXT").Wait();
                AddField(tripTbl, "Pred_Label", "TEXT").Wait();
                AddField(tripTbl, "Degree", "LONG").Wait();

                return tripTbl;
            });

            string message = String.Empty;
            bool creationResult = false;
            EditOperation editOperation = new EditOperation();

            await QueuedTask.Run(() => {
                editOperation.Callback(context =>
                {
                    using (RowBuffer rowBuffer = tripleStoreTable.CreateRowBuffer())
                    {
                        foreach (var triple in tripleStore)
                        {
                            rowBuffer["Subject"] = triple["s"];
                            rowBuffer["Predicate"] = triple["p"];
                            rowBuffer["Object"] = triple["o"];
                            rowBuffer["Pred_Label"] = triplePropertyLabelList[triplePropertyURLList.IndexOf(triple["p"])];
                            rowBuffer["Degree"] = tripleStore.IndexOf(triple);
                            using (Row row = tripleStoreTable.CreateRow(rowBuffer))
                            {
                                // To Indicate that the attribute table has to be updated.
                                context.Invalidate(row);
                            }
                        }
                    }
                }, tripleStoreTable);

                try
                {
                    creationResult = editOperation.Execute();
                    if (!creationResult) message = editOperation.ErrorMessage;
                }
                catch (GeodatabaseException exObj)
                {
                    message = exObj.Message;
                }

                MapView.Active.Redraw(false);
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);  */
        }

        public async static void CreateRelationshipFinderFeatureClass(JToken placeJSON, string outTableName, string outFeatureClassName)
        {
            //create a Shapefile/FeatuerClass for all geographic entities in the triplestore
            await QueuedTask.Run(() =>
            {
                CreateClassFromSPARQL(placeJSON, outFeatureClassName);
            });

            await Project.Current.SaveEditsAsync();

            var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == outFeatureClassName).FirstOrDefault() as BasicFeatureLayer;

            if (fcLayer == null)
            {
                MessageBox.Show($@"Unable to find {outFeatureClassName} in the active map");
            }
            else
            {
                //# add their centrold point of each geometry
                await AddField(fcLayer, "POINT_X", "DOUBLE");
                await AddField(fcLayer, "POINT_Y", "DOUBLE");
                await CalculateField(fcLayer, "POINT_X", "!SHAPE.CENTROID.X!", "PYTHON_9.3");
                await CalculateField(fcLayer, "POINT_Y", "!SHAPE.CENTROID.Y!", "PYTHON_9.3");

                await Project.Current.SaveEditsAsync();

                await QueuedTask.Run(() =>
                {
                    var datastore = fcLayer.GetTable().GetDatastore();
                    var geodatabase = datastore as Geodatabase;
                    Table outTable = geodatabase.OpenDataset<Table>(outTableName);

                    string originFeatureRelationshipClassName = outFeatureClassName + "_" + outTableName + "_Origin" + "_RelClass";
                    string endFeatureRelationshipClassName = outFeatureClassName + "_" + outTableName + "_Destination" + "_RelClass";

                    CreateRelationshipClass(fcLayer, outTable, originFeatureRelationshipClassName, "SPO Link", "Origin of SPO Link", "Subject").Wait();
                    CreateRelationshipClass(fcLayer, outTable, endFeatureRelationshipClassName, "SPO Link", "Destination of SPO Link", "Object").Wait();
                });
            }
        }

        private static Dictionary<string, string> ProcessPropertyValueQueryResult(JToken propertyValues, string keyPropertyName, string valuePropertyName)
        {
            Dictionary<string, string> keyPropList = new Dictionary<string, string>() { };

            foreach (var item in propertyValues)
            {
                var kpnValue = (string)item[keyPropertyName]["value"];
                var vpnValue = (string)item[valuePropertyName]["value"];
                keyPropList[kpnValue] = vpnValue;
            }

            return keyPropList;
        }

        private static bool IsFieldNameInTable(string fieldName, BasicFeatureLayer featureClass)
        {
            IReadOnlyList<Field> fields = null;
            return QueuedTask.Run(() =>
            {
                fields = featureClass.GetTable().GetDefinition().GetFields();

                foreach (var field in fields)
                {
                    if (field.Name == fieldName)
                        return true;
                }

                return false;
            }).Result;
        }

        private static bool DoesTableNameExist(Geodatabase geodatabase, string tableName)
        {
            try
            {
                TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableName);
                tableDefinition.Dispose();
                return true;
            }
            catch
            {
                // GetDefinition throws an exception if the definition doesn't exist
                return false;
            }
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

            return UrlDataType2FieldDataType(leader);
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

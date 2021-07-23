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
         * geoQueryResult: a sparql query result json obj serialized as a list of dict()
         *           SPARQL query like this:
         *           select distinct ?place ?placeLabel ?placeFlatType ?wkt
         *           where
         *           {...}
         * className: Name of feature layer class
         * inPlaceType: the label of user spercified type IRI
         * selectedURL: the user spercified type IRI
         * isDirectInstance: True: use placeFlatType as the type of geo-entity, False: use selectedURL as the type of geo-entity
         **/
        public static async void CreateClassFromSPARQL(JToken geoQueryResult, string className, string inPlaceType = "", string selectedURL = "", bool isDirectInstance = false)
        {
            List<string> placeIRISet = new List<string>();
            List<string[]> placeList = new List<string[]>();
            //string geomType = "";

            int index = 0;
            foreach (var item in geoQueryResult)
            {
                /*
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
                if (isDirectInstance)
                {
                    placeType = selectedURL;
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
                await FeatureClassHelper.CreatePolygonFeatureLayer(className);

                var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == className).FirstOrDefault() as BasicFeatureLayer;

                if (fcLayer == null)
                {
                    MessageBox.Show($@"Unable to find {className} in the active map");
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
                        GeoModule.Current.AddLayer(fcLayer);
                    });
                }
            }
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
                MessageBox.Show(message);
        }

        /*
        # according to jsonBindingObject, create a sperate table to store the nofunctional property-value pairs 
        # OR store the transtive "isPartOf" relationship between location and its subDivision
        # return the name of the table without the full path
        # isInverse: Boolean variable indicates whether the value we get is the subject value or object value of valuePropertyURL
        # isSubDivisionTable: Boolean variable indicates whether the current table store the value of subdivision for the original location
         */
        public static async Task<Table> CreateMappingTableFromJSON(string valuePropertyURL, JToken jsonBindingObject, string keyPropertyName, string valuePropertyName, string keyPropertyFieldName, bool isInverse, bool isSubDivisionTable)
        {
            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();

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
            /*
            else:
                lastIndexOFshp = featureClassName.rfind(".")
                featureClassName = featureClassName[:lastIndexOFshp]

                tableName =  featureClassName + "_" + keyPropertyFieldName + "_" + currentValuePropertyName + ".dbf"
            */

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

            return propertyTable;
        }

        /*
        # append a new field in inputFeatureClassName which will install the merged no-functional property value
        # noFunctionalPropertyDict: the collections.defaultdict object which stores the no-functional property value for each URL
        # appendFieldName: the field name of no-functional property in the relatedTableName
        # mergeRule: the merge rule the user selected, one of ['SUM', 'MIN', 'MAX', 'STDEV', 'MEAN', 'COUNT', 'FIRST', 'LAST']
        # delimiter: the optional paramter which define the delimiter of the cancatenate operation
        */
        public static async Task AppendFieldInFeatureClassByMergeRule(BasicFeatureLayer fcLayer, Dictionary<string, string> noFunctionalPropertyDict, string appendFieldName, 
            string relatedTableName, string mergeRule, bool useDelimiter)
        {
            /*
            appendFieldType = ''
            appendFieldLength = 0
            fieldList = arcpy.ListFields(relatedTableName)
            for field in fieldList:
                if field.name == appendFieldName:
                    appendFieldType = field.type
                    if field.type == "String":
                        appendFieldLength = field.length
                    break
            mergeRuleField = ''
            if mergeRule == 'SUM':
                mergeRuleField = 'SUM'
            elif mergeRule == 'MIN':
                mergeRuleField = 'MIN'
            elif mergeRule == 'MAX':
                mergeRuleField = 'MAX'
            elif mergeRule == 'STDEV':
                mergeRuleField = 'STD'
            elif mergeRule == 'MEAN':
                mergeRuleField = 'MEN'
            elif mergeRule == 'COUNT':
                mergeRuleField = 'COUNT'
            elif mergeRule == 'FIRST':
                mergeRuleField = 'FIRST'
            elif mergeRule == 'LAST':
                mergeRuleField = 'LAST'
            elif mergeRule == 'CONCATENATE':
                mergeRuleField = 'CONCAT'

            if appendFieldType != "String":
                cursor = arcpy.SearchCursor(relatedTableName)
                for row in cursor:
                    rowValue = row.getValue(appendFieldName)
                    if appendFieldLength < len(str(rowValue)):
                        appendFieldLength = len(str(rowValue))

            featureClassAppendFieldName = appendFieldName + "_" + mergeRuleField
            newAppendFieldName = UTIL.getFieldNameWithTable(featureClassAppendFieldName, inputFeatureClassName)
            if newAppendFieldName != -1:
                if mergeRule == 'COUNT':
                    arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, "SHORT")
                elif mergeRule == 'STDEV' or mergeRule == 'MEAN':
                    arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, "DOUBLE")
                elif mergeRule == 'CONCATENATE':
                    # get the maximum number of values for current property: maxNumOfValue
                    # maxNumOfValue * field.length = the length of new append field
                    maxNumOfValue = 1
                    for key in noFunctionalPropertyDict:
                        if maxNumOfValue < len(noFunctionalPropertyDict[key]):
                            maxNumOfValue = len(noFunctionalPropertyDict[key])
                
                    arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, 'TEXT', field_length=appendFieldLength * maxNumOfValue)
                
                
                else:
                    if appendFieldType == "String":
                        arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, appendFieldType, field_length=appendFieldLength)
                    else:
                        arcpy.AddField_management(inputFeatureClassName, newAppendFieldName, appendFieldType)

                if UTIL.isFieldNameInTable("URL", inputFeatureClassName):
                    urows = None
                    urows = arcpy.UpdateCursor(inputFeatureClassName)
                    for row in urows:
                        foreignKeyValue = row.getValue("URL")
                        noFunctionalPropertyValueList = noFunctionalPropertyDict[foreignKeyValue]
                        if len(noFunctionalPropertyValueList) != 0:
                            rowValue = ""
                            if mergeRule in ['STDEV', 'MEAN', 'SUM', 'MIN', 'MAX']:
                                if appendFieldType in ['Single', 'Double', 'SmallInteger', 'Integer']:
                                    if mergeRule == 'MEAN':
                                        rowValue = numpy.average(noFunctionalPropertyValueList)
                                    elif mergeRule == 'STDEV':
                                        rowValue = numpy.std(noFunctionalPropertyValueList)
                                    elif mergeRule == 'SUM':
                                        rowValue = numpy.sum(noFunctionalPropertyValueList)
                                    elif mergeRule == 'MIN':
                                        rowValue = numpy.amin(noFunctionalPropertyValueList)
                                    elif mergeRule == 'MAX':
                                        rowValue = numpy.amax(noFunctionalPropertyValueList)
                                else:
                                    arcpy.AddError("The {0} data type of Field {1} does not support {2} merge rule".format(appendFieldType, appendFieldName, mergeRule))
                            elif mergeRule in ['COUNT', 'FIRST', 'LAST']:
                                if mergeRule == 'COUNT':
                                    rowValue = len(noFunctionalPropertyValueList)
                                elif mergeRule == 'FIRST':
                                    rowValue = noFunctionalPropertyValueList[0]
                                elif mergeRule == 'LAST':
                                    rowValue = noFunctionalPropertyValueList[len(noFunctionalPropertyValueList)-1]
                            elif mergeRule == 'CONCATENATE':
                                value = ""
                                if appendFieldType in ['String']:
                                    rowValue = delimiter.join(sorted(set([val for val in noFunctionalPropertyValueList if not value is None])))
                                else:
                                    rowValue = delimiter.join(sorted(set([str(val) for val in noFunctionalPropertyValueList if not value is None])))

                            row.setValue(newAppendFieldName, rowValue)
                            urows.updateRow(row)
             */
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

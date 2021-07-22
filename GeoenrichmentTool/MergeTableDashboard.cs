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

namespace GeoenrichmentTool
{
    internal class MergeTableDashboard : Button
    {
        protected async override void OnClick()
        {
            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();

            await QueuedTask.Run(() =>
            {
                var datastore = mainLayer.GetTable().GetDatastore();
                var geodatabase = datastore as Geodatabase;

                List<string> tablPropValueList = new List<string>() { };
                Dictionary<string, List<string>> propMergeValueDict = new Dictionary<string, List<string>>() { };
                List<string> relatedTableList = GetRelatedTablesFromFeatureClass(mainLayer).Result;

                foreach (var tblName in relatedTableList)
                {
                    Table rTable = geodatabase.OpenDataset<Table>(tblName);
                    var rFields = rTable.GetDefinition().GetFields();
                    bool hasOriginEnd = false;
                    foreach(var rf in rFields)
                    {
                        if(rf.Name== "origin" | rf.Name == "end")
                        {
                            hasOriginEnd = true;
                        }
                    }

                    if(!hasOriginEnd)
                    {
                        string noFunctionalFieldName = rFields[2].Name;
                        tablPropValueList.Add(noFunctionalFieldName + " | " + tblName);

                        foreach (var rf in rFields)
                        {
                            if(rf.Name == noFunctionalFieldName)
                            {
                                string type = rf.FieldType.ToString();

                                List<string> mergeRules;
                                switch (type)
                                {
                                    case "Single":
                                    case "Double":
                                    case "SmallInteger":
                                    case "Integer":
                                        mergeRules = new List<string>() { "SUM", "MIN", "MAX", "STDEV", "MEAN", "COUNT", "FIRST", "LAST", "CONCATENATE" };
                                        break;
                                    default:
                                        mergeRules = new List<string>() { "COUNT", "FIRST", "LAST", "CONCATENATE" };
                                        break;
                                }
                                propMergeValueDict.Add(noFunctionalFieldName + " | " + tblName, mergeRules);
                            }
                        }
                    }
                }
            });

            /*
            if in_merge_rule.valueAsText == "CONCATENATE":
                in_cancatenate_delimiter.enabled = True
            */
        }

        private async Task<List<string>> GetRelatedTablesFromFeatureClass(BasicFeatureLayer fcLayer)
        {
            List<string> tables = new List<string>() { };
            await QueuedTask.Run(() =>
            {
                var datastore = fcLayer.GetTable().GetDatastore();
                var geodatabase = datastore as Geodatabase;
                var rcDefs = geodatabase.GetDefinitions<RelationshipClassDefinition>();
                foreach (var rcDef in rcDefs)
                {
                    if(rcDef.GetOriginClass() == fcLayer.Name)
                    {
                        tables.Add(rcDef.GetDestinationClass());
                    }
                }
            });

            return tables;
        }
    }
}

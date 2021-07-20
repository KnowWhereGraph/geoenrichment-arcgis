﻿using ArcGIS.Core.CIM;
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
            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First(); //in_wikiplace_IRI | inputFeatureClassName
            //in_no_functional_property_list
            //in_related_table_list
            //in_merge_rule
            //in_cancatenate_delimiter

            await QueuedTask.Run(() =>
            {
                var datastore = mainLayer.GetTable().GetDatastore();
                var geodatabase = datastore as Geodatabase;

                var rcDefs = geodatabase.GetDefinitions<RelationshipClassDefinition>();
                foreach(var rcDef in rcDefs)
                {
                    
                }

                var test = "";
            });

            /*
            if in_wikiplace_IRI.altered:
                MergeSingleNoFunctionalProperty.relatedTableFieldList = []
                MergeSingleNoFunctionalProperty.relatedTableList = []
                MergeSingleNoFunctionalProperty.relatedNoFunctionalPropertyURLList = []

                MergeSingleNoFunctionalProperty.relatedTableList = UTIL.getRelatedTableFromFeatureClass(inputFeatureClassName)
                in_related_table_list.filter.list = MergeSingleNoFunctionalProperty.relatedTableList
                
                # noFunctionalPropertyTable = []

                for relatedTable in MergeSingleNoFunctionalProperty.relatedTableList:
                    fieldList = arcpy.ListFields(relatedTable)
                    if "origin" not in fieldList and "end" not in fieldList:
                        noFunctionalFieldName = fieldList[2].name
                        arcpy.AddMessage("noFunctionalFieldName: {0}".format(noFunctionalFieldName))
                        MergeSingleNoFunctionalProperty.relatedTableFieldList.append(noFunctionalFieldName)
                        # get the no functioal property URL from the firt row of this table field "propURL"

                        TableRelationshipClassList = UTIL.getRelationshipClassFromTable(relatedTable)
                        propURL = arcpy.Describe(TableRelationshipClassList[0]).forwardPathLabel

                        MergeSingleNoFunctionalProperty.relatedNoFunctionalPropertyURLList.append(propURL)

                in_no_functional_property_list.filter.list = MergeSingleNoFunctionalProperty.relatedNoFunctionalPropertyURLList

            if in_no_functional_property_list.altered:
                selectPropURL = in_no_functional_property_list.valueAsText
                selectIndex = MergeSingleNoFunctionalProperty.relatedNoFunctionalPropertyURLList.index(selectPropURL)
                selectFieldName = MergeSingleNoFunctionalProperty.relatedTableFieldList[selectIndex]
                selectTableName = MergeSingleNoFunctionalProperty.relatedTableList[selectIndex]

                in_related_table_list.value = selectTableName

                currentDataType = UTIL.getFieldDataTypeInTable(selectFieldName, selectTableName)
                if currentDataType in ['Single', 'Double', 'SmallInteger', 'Integer']:
                    in_merge_rule.filter.list = ['SUM', 'MIN', 'MAX', 'STDEV', 'MEAN', 'COUNT', 'FIRST', 'LAST', 'CONCATENATE']
                # elif currentDataType in ['SmallInteger', 'Integer']:
                #   in_merge_rule.filter.list = ['SUM', 'MIN', 'MAX', 'COUNT', 'FIRST', 'LAST']
                else:
                    in_merge_rule.filter.list = ['COUNT', 'FIRST', 'LAST', 'CONCATENATE']

            if in_related_table_list.altered:
                selectTableName = in_related_table_list.valueAsText
                selectIndex = MergeSingleNoFunctionalProperty.relatedTableList.index(selectTableName)
                selectFieldName = MergeSingleNoFunctionalProperty.relatedTableFieldList[selectIndex]
                selectPropURL = MergeSingleNoFunctionalProperty.relatedNoFunctionalPropertyURLList[selectIndex]

                in_no_functional_property_list.value = selectPropURL

                currentDataType = UTIL.getFieldDataTypeInTable(selectFieldName, selectTableName)
                if currentDataType in ['Single', 'Double', 'SmallInteger', 'Integer']:
                    in_merge_rule.filter.list = ['SUM', 'MIN', 'MAX', 'STDEV', 'MEAN', 'COUNT', 'FIRST', 'LAST', 'CONCATENATE']
                # elif currentDataType in ['SmallInteger', 'Integer']:
                #   in_merge_rule.filter.list = ['SUM', 'MIN', 'MAX', 'COUNT', 'FIRST', 'LAST']
                else:
                    in_merge_rule.filter.list = ['COUNT', 'FIRST', 'LAST', 'CONCATENATE']
            

            if in_merge_rule.valueAsText == "CONCATENATE":
                in_cancatenate_delimiter.enabled = True
            */
        }
    }
}
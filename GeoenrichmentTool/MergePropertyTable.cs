using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoenrichmentTool
{
    public partial class MergePropertyTable : Form
    {
        protected List<string> propVals;
        protected Dictionary<string, List<string>> propMergeVals;

        public MergePropertyTable(List<string> tablePropValueList, Dictionary<string, List<string>> propMergeValueDict)
        {
            propVals = tablePropValueList;
            propMergeVals = propMergeValueDict;

            InitializeComponent();

            foreach (var prop in propVals)
            {
                relatedTables.Items.Add(prop);
            }

            //Set initial item
            relatedTables.SelectedItem = propVals.First();
            SetMergeRules(propVals.First());
        }

        private void SetMergeRules(string prop)
        {
            mergeRules.Items.Clear();

            var rules = propMergeVals[prop];

            foreach(var rule in rules)
            {
                mergeRules.Items.Add(rule);
            }
        }

        private async void MergeTables(object sender, EventArgs e)
        {
            if (mergeRules.Text == "" | relatedTables.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                Close();

                BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();
                char[] delimSharp = { '|' };
                string selectTableName = relatedTables.Text.Split(delimSharp)[1].Trim();
                string selectFieldName = relatedTables.Text.Split(delimSharp)[0].Trim();
                string selectMergeRule = mergeRules.Text;

                Dictionary<string, List<string>> noFunctionalPropertyDict = BuildMultiValueDictFromNoFunctionalProperty(selectFieldName, selectTableName, "URL").Result;

                if(noFunctionalPropertyDict.Count() > 0)
                {
                    await FeatureClassHelper.AppendFieldInFeatureClassByMergeRule(mainLayer, noFunctionalPropertyDict, selectFieldName, selectTableName, selectMergeRule);
                }
            }
        }

        private void UpdateMergeRules(object sender, EventArgs e)
        {
            SetMergeRules(relatedTables.SelectedItem.ToString());
        }

        private async Task<Dictionary<string, List<string>>> BuildMultiValueDictFromNoFunctionalProperty(string fieldName, string tableName, string urlFieldName)
        {
            Dictionary<string, List<string>> valueDict = new Dictionary<string, List<string>>() { };

            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();
            var datastore = mainLayer.GetTable().GetDatastore();
            var geodatabase = datastore as Geodatabase;

            await QueuedTask.Run(() =>
            {
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

                            valueDict[foreignKeyValue].Add(noFunctionalPropertyValue);
                        }
                    }
                }
            });

            return valueDict;
        }
    }
}

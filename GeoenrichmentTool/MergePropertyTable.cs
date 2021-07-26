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

                char[] delimSharp = { '|' };
                string selectTableName = relatedTables.Text.Split(delimSharp)[1].Trim();
                string selectFieldName = relatedTables.Text.Split(delimSharp)[0].Trim();
                string selectMergeRule = mergeRules.Text;

                Dictionary<string, List<string>> noFunctionalPropertyDict = await FeatureClassHelper.BuildMultiValueDictFromNoFunctionalProperty(selectFieldName, selectTableName, "URL");

                if(noFunctionalPropertyDict.Count() > 0)
                {
                    await FeatureClassHelper.AppendFieldInFeatureClassByMergeRule(noFunctionalPropertyDict, selectFieldName, selectTableName, selectMergeRule);
                }
            }
        }

        private void UpdateMergeRules(object sender, EventArgs e)
        {
            SetMergeRules(relatedTables.SelectedItem.ToString());
        }
    }
}

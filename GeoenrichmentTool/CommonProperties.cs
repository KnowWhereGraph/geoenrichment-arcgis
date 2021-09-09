using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Button = ArcGIS.Desktop.Framework.Contracts.Button;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;

namespace KWG_Geoenrichment
{
    internal class CommonProperties : Button
    {
        protected async override void OnClick()
        {
            BasicFeatureLayer mainLayer = KwgGeoModule.Current.GetLayers().First();
            List<string> uris = await FeatureClassHelper.GetURIs(mainLayer);

            if(uris.Count > 0)
            {
                List<string>[] commonProps = GetCommonProperties(uris);
                List<string>[] sosaObsProps = GetSosaObsProperties(uris);
                List<string>[] inverseProps = GetCommonProperties(uris, true);

                Form propForm = new PropertyEnrichment(commonProps, sosaObsProps, inverseProps, uris);
                propForm.ShowDialog();
            } 
            else
            {
                MessageBox.Show($@"No data to enrich for layer {mainLayer.Name}");
            }
        }

        private List<string>[] GetCommonProperties(List<string> uriList, bool inverse = false)
        {
            string uriString = "";
            foreach(var uri in uriList)
            {
                uriString += "<" + uri + "> \n";
            }

            string cpQuery = "";

            if(inverse)
            {
                cpQuery += "select distinct ?p (count(distinct ?s) as ?NumofSub) where { ?s owl:sameAs ?wikidataSub. ?s ?p ?o. VALUES ?wikidataSub { ";
            }
            else
            {
                cpQuery += "select distinct ?p (count(distinct ?s) as ?NumofSub) where { ?s ?p ?o. VALUES ?s { ";
            }

            cpQuery += uriString;

            cpQuery += "} } group by ?p order by DESC(?NumofSub)";

            var results = KwgGeoModule.Current.GetQueryClass().SubmitQuery(cpQuery, false);
            return ProcessProperties(results);
        }

        private List<string>[] GetSosaObsProperties(List<string> uriList)
        {
            string uriString = "";
            foreach (var uri in uriList)
            {
                uriString += "<" + uri + "> \n";
            }

            string cpQuery = "select distinct ?p ?pLabel (count(distinct ?s) as ?NumofSub) where " +
                "{ ?s sosa:isFeatureOfInterestOf ?obscol . ?obscol sosa:hasMember ?obs. ?obs sosa:observedProperty ?p . OPTIONAL {?p rdfs:label ?pLabel . } " +
                "VALUES ?s {";

            cpQuery += uriString;

            cpQuery += "} } group by ?p ?pLabel order by DESC(?NumofSub)";

            var results = KwgGeoModule.Current.GetQueryClass().SubmitQuery(cpQuery, false);
            return ProcessProperties(results);
        }

        private List<string>[] ProcessProperties(JToken properties)
        {
            List<string> urlList = new List<string>() { };
            List<string> nameList = new List<string>() { };

            foreach (var item in properties)
            {
                string propertyURL = (string)item["p"]["value"];

                if(!urlList.Contains(propertyURL))
                {
                    urlList.Add(propertyURL);

                    string label = "";
                    if (item["pLabel"] != null)
                    {
                        label = (string)item["pLabel"]["value"];
                    }
                    if(label.Trim() == "")
                    {
                        label = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(propertyURL);
                    }

                    string propertyName = label + " [" + item["NumofSub"]["value"] + "]";
                    nameList.Add(propertyName);
                }
            }

            return new List<string>[] { urlList, nameList };
        }
    }
}

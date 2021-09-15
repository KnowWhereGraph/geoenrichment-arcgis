using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWG_Geoenrichment
{
    class PropertyEnrichment
    {
        public List<string>[] CommonProperties { get; }
        public List<string>[] SosaObsProperties { get; }
        public List<string>[] InverseProperties { get; }

        public PropertyEnrichment(string selectedFeatureType)
        {
            CommonProperties = GenerateCommonProperties(selectedFeatureType);
            SosaObsProperties = GenerateSosaObsProperties(selectedFeatureType);
            InverseProperties = GenerateCommonProperties(selectedFeatureType, true);
        }

        private List<string>[] GenerateCommonProperties(string feature, bool inverse = false)
        {
            string cpQuery = "";

            if (inverse)
            {
                cpQuery += "select distinct ?p ?plabel where { ?s owl:sameAs ?wikidataSub. ?s ?p ?o. ?wikidataSub rdf:type " + feature +
                    ". OPTIONAL {?p rdfs:label ?plabel .} }";
            }
            else
            {
                cpQuery += "select distinct ?p ?plabel where { ?s ?p ?o. ?s rdf:type " + feature + ". OPTIONAL {?p rdfs:label ?plabel .} }";
            }

            var results = KwgGeoModule.Current.GetQueryClass().SubmitQuery(cpQuery, false);
            return ProcessProperties(results);
        }

        private List<string>[] GenerateSosaObsProperties(string feature)
        {
            string cpQuery = "select distinct ?p ?plabel where " +
                "{ ?s sosa:isFeatureOfInterestOf ?obscol . ?obscol sosa:hasMember ?obs. ?obs sosa:observedProperty ?p . ?s rdf:type " + feature + ". " +
                "OPTIONAL {?p rdfs:label ?plabel .} }";

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

                if (!urlList.Contains(propertyURL))
                {
                    urlList.Add(propertyURL);

                    string label = "";
                    if (item["plabel"] != null)
                    {
                        label = (string)item["plabel"]["value"];
                    }
                    if (label.Trim() == "")
                    {
                        label = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(propertyURL);
                    }

                    nameList.Add(label);
                }
            }

            return new List<string>[] { urlList, nameList };
        }
    }
}

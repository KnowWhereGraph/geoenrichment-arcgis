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
            /*CommonProperties = GenerateCommonProperties(selectedFeatureType);
            SosaObsProperties = GenerateSosaObsProperties(selectedFeatureType);
            InverseProperties = GenerateCommonProperties(selectedFeatureType, true);*/
        }

        /*private List<string>[] GenerateCommonProperties(string feature, bool inverse = false)
        {
            string cpQuery = "";

            if (inverse)
            {
                cpQuery += "select distinct ?inverse_p ?plabel where { ?s ?p ?o . ?o ?inverse_p ?s. ?o rdf:type " + feature + ". OPTIONAL {?inverse_p rdfs:label ?plabel .} }";
            }
            else
            {
                cpQuery += "select distinct ?p ?plabel where { ?s ?p ?o. ?s rdf:type " + feature + ". OPTIONAL {?p rdfs:label ?plabel .} }";
            }

            var results = KwgGeoModule.Current.GetQueryClass().SubmitQuery(cpQuery, false);
            return ProcessProperties(results, inverse);
        }*/

        /*private List<string>[] GenerateSosaObsProperties(string feature)
        {
            string cpQuery = "select distinct ?p ?plabel where " +
                "{ ?s sosa:isFeatureOfInterestOf ?obscol . ?obscol sosa:hasMember ?obs. ?obs sosa:observedProperty ?p . ?s rdf:type " + feature + ". " +
                "OPTIONAL {?p rdfs:label ?plabel .} }";

            var results = KwgGeoModule.Current.GetQueryClass().SubmitQuery(cpQuery, false);
            return ProcessProperties(results);
        }*/

        private List<string>[] ProcessProperties(JToken properties, bool inverse = false)
        {
            List<string> urlList = new List<string>() { };
            List<string> nameList = new List<string>() { };

            foreach (var item in properties)
            {
                string propertyURL = (inverse) ? (string)item["inverse_p"]["value"] : (string)item["p"]["value"];

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
                        label = KwgGeoModule.Current.GetQueryClass().IRIToPrefix(propertyURL);
                    }

                    nameList.Add(label);
                }
            }

            return new List<string>[] { urlList, nameList };
        }

        /*public static JToken FunctionalPropertyQuery(List<string> properties, bool inverse = false)
        {
            string owlProp = (inverse) ? "owl:InverseFunctionalProperty" : "owl:FunctionalProperty";

            string funcQuery = "select ?property where { ?property a " + owlProp + ". VALUES ?property {";

            foreach (string propURI in properties)
            {
                funcQuery += "<" + propURI + "> \n";
            }

            funcQuery += "}}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(funcQuery);
        }*/

        /*public static JToken PropertyValueQuery(List<string> uriList, string property, bool doSameAs = true)
        {
            string propValQuery = "";

            if (doSameAs)
            {
                propValQuery = "select ?wikidataSub ?o where { ?s owl:sameAs ?wikidataSub. ?s <" + property + "> ?o. VALUES ?wikidataSub {";
            }
            else
            {
                propValQuery = "select ?wikidataSub ?o where { ?wikidataSub <" + property + "> ?o. VALUES ?wikidataSub {";
            }

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }*/

        /*public static JToken SosaObsPropertyValueQuery(List<string> uriList, string property)
        {
            string propValQuery = "select ?wikidataSub ?o where { ?wikidataSub sosa:isFeatureOfInterestOf ?obscol . ?obscol sosa:hasMember ?obs. " +
                "?obs sosa:observedProperty <" + property + "> . ?obs sosa:hasSimpleResult ?o. VALUES ?wikidataSub {";

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }*/

        /*public static JToken InversePropertyValueQuery(List<string> uriList, string property, bool doSameAs = true)
        {
            string propValQuery = "";

            if (doSameAs)
            {
                propValQuery = "select ?wikidataSub ?o where { ?s owl:sameAs ?wikidataSub. ?o <" + property + "> ?s. VALUES ?wikidataSub {";
            }
            else
            {
                propValQuery = "select ?wikidataSub ?o where { ?o <" + property + "> ?wikidataSub. VALUES ?wikidataSub {";
            }

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }*/

        /*public static JToken CheckGeoPropertyQuery(List<string> uriList, string propertyURL, bool doSameAs = true)
        {
            string propValQuery = "select (count(?geometry) as ?cnt) where {";

            if (doSameAs)
            {
                propValQuery += " ?s owl:sameAs ?wikidataSub. ?s <" + propertyURL + "> ?place.";
            }
            else
            {
                propValQuery += " ?wikidataSub <" + propertyURL + "> ?place.";
            }

            propValQuery += " ?place geo:hasGeometry ?geometry . VALUES ?wikidataSub {";

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }*/

        /*public static JToken TwoDegreePropertyValueWKTquery(List<string> uriList, string propertyURL, bool doSameAs = true)
        {
            string propValQuery = "select distinct ?place ?placeLabel ?placeFlatType ?wkt where {";

            if (doSameAs)
            {
                propValQuery += " ?s owl:sameAs ?wikidataSub. ?s <" + propertyURL + "> ?place.";
            }
            else
            {
                propValQuery += " ?wikidataSub <" + propertyURL + "> ?place.";
            }

            propValQuery += " ?place geo:hasGeometry ?geometry . ?place rdfs:label ?placeLabel . " +
                "?geometry geo:asWKT ?wkt. ?place rdf:type ?placeFlatType. VALUES ?wikidataSub {";

            foreach (var uri in uriList)
            {
                propValQuery += "<" + uri + "> \n";
            }

            propValQuery += "}}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(propValQuery);
        }*/
    }
}

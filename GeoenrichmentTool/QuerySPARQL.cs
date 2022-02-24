using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace KWG_Geoenrichment
{
    class QuerySPARQL
    {
        public Dictionary<string, string> defaultEndpoints = new Dictionary<string, string>() { 
            { "KnowWhere Graph", "https://stko-kwg.geog.ucsb.edu/sparql" }
        };
        protected Dictionary<string, string> _PREFIX = new Dictionary<string, string>() {
            {"kwg-ont", "http://stko-kwg.geog.ucsb.edu/lod/ontology/"},
            {"kwgr", "http://stko-kwg.geog.ucsb.edu/lod/resource/"},
            {"geo", "http://www.opengis.net/ont/geosparql#"},
            {"geof", "http://www.opengis.net/def/function/geosparql/"},
            {"gnisf", "http://gnis-ld.org/lod/gnis/feature/"},
            {"cegis", "http://gnis-ld.org/lod/cegis/ontology/"},
            {"sosa", "http://www.w3.org/ns/sosa/"},
            {"owl", "http://www.w3.org/2002/07/owl#"},
            {"rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"},
            {"rdfs", "http://www.w3.org/2000/01/rdf-schema#"},
            //{"xvd", "http://www.w3.org/2001/XMLSchema#"},
            //{"dc", "http://purl.org/dc/elements/1.1/"},
            //{"dcterms", "http://purl.org/dc/terms/"},
            //{"foaf", "http://xmlns.com/foaf/0.1/"},
            //{"time", "http://www.w3.org/2006/time#"},
            //{"ago", "http://awesemantic-geo.link/ontology/"},
            //{"elastic", "http://www.ontotext.com/connectors/elasticsearch#"},
            //{"elastic-index", "http://www.ontotext.com/connectors/elasticsearch/instance#"},
            //{"iospress", "http://ld.iospress.nl/rdf/ontology/"}
        };

        public QuerySPARQL()
        {

        }

        public JToken SubmitQuery(string endpointKey, string query)
        {
            query = MakeSPARQLPrefix() + query;

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/sparql-results+json");

            var values = new Dictionary<string, string>
            {
                { "query", query }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync(defaultEndpoints[endpointKey], content);

            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            JObject json = JObject.Parse(responseString);

            return json["results"]["bindings"];
        }

        private string MakeSPARQLPrefix()
        {
            string queryPrefix = "";
            foreach (var prefix in _PREFIX)
            {
                queryPrefix += "PREFIX " + prefix.Key + ": <" + prefix.Value + "> ";
            }

            return queryPrefix;
        }

        public string IRIToPrefix(string iri)
        {
            string result = "";
            foreach (var prefix in _PREFIX)
            {
                if (iri.Contains(prefix.Value))
                {
                    result = iri.Replace(prefix.Value, prefix.Key + ":");
                }
            }

            return (result != "") ? result : iri;
        }
    }
}

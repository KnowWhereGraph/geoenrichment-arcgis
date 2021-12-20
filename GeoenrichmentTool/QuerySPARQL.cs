﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace KWG_Geoenrichment
{
    class QuerySPARQL
    {
        public Dictionary<string, string> defaultEndpoints = new Dictionary<string, string>() { 
            { "Knowledge Graph V2", "http://stko-roy.geog.ucsb.edu:7202/repositories/plume_soil_wildfire" }
        };
        protected Dictionary<string, string> _PREFIX = new Dictionary<string, string>() {
            //{"bd", "http://www.bigdata.com/rdf#"},
            //{"dbo", "http://dbpedia.org/ontology/"},
            //{"dbr", "http://dbpedia.org/resource/"},
            //{"ff", "http://factforge.net/"},
            {"geo", "http://www.opengis.net/ont/geosparql#"},
            //{"geof", "http://www.opengis.net/def/function/geosparql/"},
            //{"geo-pos", "http://www.w3.org/2003/01/geo/wgs84_pos#"},
            //{"kwgr", "http://stko-kwg.geog.ucsb.edu/lod/resource/"},
            {"kwg-ont", "http://stko-kwg.geog.ucsb.edu/lod/ontology/"},
            //{"om", "http://www.ontotext.com/owlim/"},
            //{"omgeo", "http://www.ontotext.com/owlim/geo#"},
            //{"owl", "http://www.w3.org/2002/07/owl#"},
            //{"p", "http://www.wikidata.org/prop/"},
            {"rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"},
            {"rdfs", "http://www.w3.org/2000/01/rdf-schema#"},
            //{"schema", "http://schema.org/"},
            //{"sosa", "http://www.w3.org/ns/sosa/"},
            //{"ssn", "http://www.w3.org/ns/ssn/"},
            //{"time", "http://www.w3.org/2006/time#"},
            //{"xsd", "http://www.w3.org/2001/XMLSchema#"},
            //{"wd", "http://www.wikidata.org/entity/"},
            //{"wdt", "http://www.wikidata.org/prop/direct/"},
            //{"wdtn", "http://www.wikidata.org/prop/direct-normalized/"},
            //{"wikibase", "http://wikiba.se/ontology#"}
        };

        public QuerySPARQL()
        {

        }

        public JToken SubmitQuery(string activeEndpoint, string query, bool doInference = false, string requestMethod = "post")
        {
            query = MakeSPARQLPrefix() + query;

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/sparql-results+json");

            var values = new Dictionary<string, string>
            {
                { "query", query },
                { "format", "json" },
                { "doInference", (doInference) ? "true" : "false" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync(activeEndpoint, content);

            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            JObject json = JObject.Parse(responseString);

            return json["results"]["bindings"];
        }

        public string MakeIRIPrefix(string iri)
        {
            string result = "";
            foreach (var prefix in _PREFIX)
            {
                if(iri.Contains(prefix.Value))
                {
                    result = iri.Replace(prefix.Value, prefix.Key + ":");
                }
            }

            return (result!="") ? result : iri;
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
    }
}

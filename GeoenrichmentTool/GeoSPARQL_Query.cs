using ArcGIS.Core.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoenrichmentTool
{
    public partial class GeoSPARQL_Query : Form
    {
        protected string defaultNameSpace = "http://stko-kwg.geog.ucsb.edu/"; //Change URLs below if changed
        protected string defaultEndpoint = "http://stko-roy.geog.ucsb.edu:7202/repositories/plume_soil_wildfire"; //http://stko-roy.geog.ucsb.edu:7200/repositories/kwg-seed-graph-v2
        protected Dictionary<string, string> _PREFIX = new Dictionary<string, string>() {
            {"bd", "http://www.bigdata.com/rdf#"},
            {"dbo", "http://dbpedia.org/ontology/"},
            {"dbr", "http://dbpedia.org/resource/"},
            {"ff", "http://factforge.net/"},
            {"geo", "http://www.opengis.net/ont/geosparql#"},
            {"geof", "http://www.opengis.net/def/function/geosparql/"},
            {"geo-pos", "http://www.w3.org/2003/01/geo/wgs84_pos#"},
            {"kwgr", "http://stko-kwg.geog.ucsb.edu/lod/resource/"}, //Change URL if defaultNameSpace changes
            {"kwg-ont", "http://stko-kwg.geog.ucsb.edu/lod/ontology/"}, //Change URL if defaultNameSpace changes
            {"om", "http://www.ontotext.com/owlim/"},
            {"omgeo", "http://www.ontotext.com/owlim/geo#"},
            {"owl", "http://www.w3.org/2002/07/owl#"},
            {"p", "http://www.wikidata.org/prop/"},
            {"rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"},
            {"rdfs", "http://www.w3.org/2000/01/rdf-schema#"},
            {"schema", "http://schema.org/"},
            {"sosa", "http://www.w3.org/ns/sosa/"},
            {"ssn", "http://www.w3.org/ns/ssn/"},
            {"time", "http://www.w3.org/2006/time#"},
            {"xsd", "http://www.w3.org/2001/XMLSchema#"},
            {"wd", "http://www.wikidata.org/entity/"},
            {"wdt", "http://www.wikidata.org/prop/direct/"},
            {"wdtn", "http://www.wikidata.org/prop/direct-normalized/"},
            {"wikibase", "http://wikiba.se/ontology#"}
        };

        protected string fileSavePath = "";
        protected Geometry geometry;

        public GeoSPARQL_Query(Geometry geo)
        {
            InitializeComponent();
            this.endPoint.Text = defaultEndpoint;
            this.geometry = geo;
        }

        private void submitGeoQueryForm(object sender, EventArgs e)
        {
            this.formError.Text = "";
            if (this.endPoint.Text == "" | this.className.Text == "")
            {
                this.formError.Text = "* Required fields missing!";
            }
            else
            {
                if(ChooseFolder())
                {
                    this.Close();

                    var gfEndPoint = this.endPoint.Text; //sparql_endpoint
                    var queryGeoExtent = ""; //TODO::Variable - queryGeoExtent
                    var gfPlaceType = this.placeType.Text; //inPlaceType
                    var gfSubclassReasoning = this.subclassReasoning.Checked; //isDirectInstance
                    var gfCalculator = this.calculator.SelectedItem; //inTopoCal
                    var gfFileSavePath = this.fileSavePath; //outLocation
                    var gfClassName = this.className.Text; //outFeatureClassName
                    var selectedURL = ""; //TODO::Variable - selectedURL

                    string[] geoFunc = new string[] { };
                    switch (gfCalculator)
                    {
                        case "Contain + Intersect":
                            geoFunc = new string[] { "geo:sfContains", "geo:sfIntersects" };
                            break;
                        case "Contain":
                            geoFunc = new string[] { "geo:sfContains" };
                            break;
                        case "Within":
                            geoFunc = new string[] { "geo:sfWithin" };
                            break;
                        case "Intersect":
                            geoFunc = new string[] { "geo:sfIntersects" };
                            break;
                        default:
                            //arcpy.AddError("The spatial relation is not supported!") //TODO::Reporting
                            break;
                    }

                    //query_geo_wkt = UTIL.get_geometrywkt_from_interactive_featureclass_by_idx(queryGeoExtent) //TODO::Function

                    //messages.addMessage("query_geo_wkt: {0}".format(query_geo_wkt)) //TODO::Reporting

                    //query_geo_wkt = UTIL.project_wkt_to_wgs84(queryGeoExtent, query_geo_wkt) //TODO::Function
                    var query_geo_wkt = "WKT_VARIABLE_GOES_HERE";

                    string out_path = "";
                    if(gfFileSavePath.Contains(".gdb"))
                    {
                        //if the outputLocation is a file geodatabase, cancatnate the outputlocation with gfClassName to create a feature class in current geodatabase
                        //out_path = os.path.join(gfFileSavePath, gfClassName) //TODO::Variable
                    }
                    else
                    {
                        //if the outputLocation is a folder, creats a shapefile in this folder
                        //out_path = os.path.join(gfFileSavePath,gfClassName) + ".shp" //TODO::Variable
                        //however, Relationship Class must be created in a geodatabase, so we forbid to create a shapfile
                        //messages.addErrorMessage("Please enter a file geodatabase as output location in order to create a relation class")
                        //raise arcpy.ExecuteError
                    }

                    //messages.addMessage("outpath: {0}".format(out_path)) //TODO::Reporting

                    var GeoQueryResult = TypeAndGeoSPARQLQuery(query_geo_wkt, selectedURL, gfSubclassReasoning, geoFunc, gfEndPoint); //TODO::Function

                    //Json2Field.createFeatureClassFromSPARQLResult(GeoQueryResult, out_path, gfPlaceType, selectedURL, gfSubclassReasoning) //TODO::Function
                }
            }
        }

        public bool ChooseFolder()
        {
            if (outputLocation.ShowDialog() == DialogResult.OK)
            {
                fileSavePath = outputLocation.SelectedPath;

                return true;
            }
            else
            {
                return false;
            }
        }





        /**
         * def get_geometrywkt_from_interactive_featureclass_by_idx(queryGeoExtent):
        '''
        Given a interactive feature class (the one we are drawing), we go to one of its row and get its geometry wkt
        Args:
            queryGeoExtent: an input feature class, "FeatureLayer"
            queryGeoExtent_idx: int, the row index of the geometry column we want to get, from [1, N]
        '''
        assert arcpy.Describe(queryGeoExtent).dataType == "FeatureLayer"
        query_geo_wkt = None
        cursor = arcpy.da.SearchCursor(queryGeoExtent, ["SHAPE@WKT"])
        for row in cursor:
            query_geo_wkt = row[0]
            break
        return query_geo_wkt
         **/





        /**
         * def project_wkt_to_wgs84(queryGeoExtent, query_geo_wkt):
        '''
        We need to project query_geo_wkt into WGS84 if necessary
        Args:
            queryGeoExtent: an input feature class, "FeatureLayer" or "FeatureClass"
            query_geo_wkt: the geometry WKT extracted from queryGeoExtent
        '''
        spa_ref = arcpy.Describe(queryGeoExtent).spatialReference
        if spa_ref.name != "GCS_WGS_1984":
            query_geometry = arcpy.FromWKT(query_geo_wkt,spa_ref)
            query_geometry_wgs84 = query_geometry.projectAs(arcpy.SpatialReference(4326))
            query_geo_wkt = query_geometry_wgs84.WKT
            arcpy.AddMessage("query_geo_wkt PROJECTED: {0}".format(query_geo_wkt))
        return query_geo_wkt
         **/





        /**
         * Format GeoSPARQL query by given query_geo_wkt and type
         * 
         * queryGeoWKT: the wkt literal //TODO::Variable - this is DEFINITELY probably not a string I think
         * selectedURL: the user spercified type IRI
         * isDirectInstance: True: use placeFlatType as the type of geo-entity, False: use selectedURL as the type of geo-entity
         * geoFunc: a list of geosparql functions
         * endPoint: URL for SPARQL endpoint
         **/
        private string TypeAndGeoSPARQLQuery(string queryGeoWKT, string selectedURL, bool isDirectInstance, string[] geoFunc, string endPoint)
        {
            string queryPrefix = MakeSPARQLPrefix();
            string query = queryPrefix + "select distinct ?place ?placeLabel ?placeFlatType ?wkt " +
                "where" +
                "{" +
                "?place geo:hasGeometry ?geometry . " +
                "?place rdfs:label ?placeLabel . " +
                "?geometry geo:asWKT ?wkt . " +
                "{ '''" +
                "<http://www.opengis.net/def/crs/OGC/1.3/CRS84>" +
                queryGeoWKT + "'''^^geo:wktLiteral " +
                geoFunc[0] + "  ?geometry .}";

            if(geoFunc.Length==2)
            {
                query += " union" +
                    "{ '''" +
                    "<http://www.opengis.net/def/crs/OGC/1.3/CRS84>" +
                    queryGeoWKT + "'''^^geo:wktLiteral  " +
                    geoFunc[1] + "   ?geometry . }";
            }

            if(selectedURL != "")
            {
                if(!isDirectInstance)
                {
                    query += "?place rdf:type ?placeFlatType ." +
                    "?placeFlatType rdfs:subClassOf* <" + selectedURL + ">.";
                } 
                else
                {
                    query += "?place rdf:type  <" + selectedURL + ">.";
                }
                
            }

            query += "}";

            var geoQueryResult = QuerySPARQL(query, endPoint, false); //TODO::function
            return geoQueryResult;
            //return GeoQueryResult["results"]["bindings"];
        }





        private string MakeSPARQLPrefix()
        {
            string queryPrefix = "";
            foreach (var prefix in _PREFIX)
            {
                queryPrefix += "PREFIX " + prefix.Key + ": <" + prefix.Value + ">\n";
            }

            return queryPrefix;
        }





        private string QuerySPARQL(string query, string endPoint, bool doInference=false, string requestMethod="post")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            string result = "";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "'application/sparql-results+json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"query\":\""+ query + "\", \"format\":\"json\", \"infer\":\"" + doInference + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
        /**
        iprint(sparqlRequest.url)
        arcpy.AddMessage("SPARQL URL: {0}".format(sparqlRequest.url))

        entityTypeJson = sparqlRequest.json() #["results"]["bindings"]
        return entityTypeJson
         **/





        /**
          * def createFeatureClassFromSPARQLResult(GeoQueryResult, out_path = "", inPlaceType = "", selectedURL = "", 
                 isDirectInstance = False, viz_res = True):
         '''
         GeoQueryResult: a sparql query result json obj serialized as a list of dict()
                     SPARQL query like this:
                     select distinct ?place ?placeLabel ?placeFlatType ?wkt
                     where
                     {...}
         out_path: the output path for the create geo feature class
         inPlaceType: the label of user spercified type IRI
         selectedURL: the user spercified type IRI
         isDirectInstance: True: use placeFlatType as the type of geo-entity
                           False: use selectedURL as the type of geo-entity
         '''
         # a set of unique WKT for each found places
         placeIRISet = set()
         placeList = []
         geom_type = None
         for idx, item in enumerate(GeoQueryResult):
             wkt_literal = item["wkt"]["value"]
             # for now, make sure all geom has the same geometry type 
             if idx == 0:
                 geom_type = UTIL.get_geometry_type_from_wkt(wkt_literal)
             else:
                 assert geom_type == UTIL.get_geometry_type_from_wkt(wkt_literal)

             if isDirectInstance == False:
                 placeType = item["placeFlatType"]["value"]
             else:
                 placeType = selectedURL
             print("{}\t{}\t{}".format(
                 item["place"]["value"], item["placeLabel"]["value"], placeType))
             if len(placeIRISet) == 0 or item["place"]["value"] not in placeIRISet:
                 placeIRISet.add(item["place"]["value"])
                 placeList.append(
                     [ item["place"]["value"], item["placeLabel"]["value"], placeType, wkt_literal ])
         
         if geom_type is None:
             raise Exception("geometry type not find")

         if len(placeList) == 0:
             arcpy.AddMessage("No {0} within the provided polygon can be finded!".format(inPlaceType))
         else:
             

             if out_path == None:
                 arcpy.AddMessage("No data will be added to the map document.")
                 # pythonaddins.MessageBox("No data will be added to the map document.", "Warning Message", 0)
             else:
                 geo_feature_class = arcpy.CreateFeatureclass_management(
                                     os.path.dirname(out_path), 
                                     os.path.basename(out_path), 
                                     geometry_type = geom_type,
                                     spatial_reference = arcpy.SpatialReference(4326) )

                 labelFieldLength = Json2Field.fieldLengthDecide(GeoQueryResult, "placeLabel")
                 arcpy.AddMessage("labelFieldLength: {0}".format(labelFieldLength))
                 urlFieldLength = Json2Field.fieldLengthDecide(GeoQueryResult, "place")
                 arcpy.AddMessage("urlFieldLength: {0}".format(urlFieldLength))
                 if isDirectInstance == False: 
                     classFieldLength = Json2Field.fieldLengthDecide(GeoQueryResult, "placeFlatType")
                 else:
                     classFieldLength = len(selectedURL) + 50
                 arcpy.AddMessage("classFieldLength: {0}".format(classFieldLength))
                 
                 # add field to this point feature class
                 arcpy.AddField_management(geo_feature_class, "Label", "TEXT", field_length=labelFieldLength)
                 arcpy.AddField_management(geo_feature_class, "URL", "TEXT", field_length=urlFieldLength)
                 arcpy.AddField_management(geo_feature_class, "Class", "TEXT", field_length=classFieldLength)


                 insertCursor = arcpy.da.InsertCursor(out_path, ['URL', 'Label', "Class", 'SHAPE@WKT',])
                 for item in placeList:
                     place_iri, label, type_iri, wkt_literal = item
                     wkt = wkt_literal.replace("<http://www.opengis.net/def/crs/OGC/1.3/CRS84>", "")
                     try:
                         insertCursor.insertRow( (place_iri, label, type_iri, wkt ) )
                     except Error:
                         arcpy.AddMessage("Error inserting geo data: {} {} {}".format(place_iri, label, type_iri))

                 del insertCursor

                 if viz_res:
                     ArcpyViz.visualize_current_layer(out_path)
         return
          **/
    }
}

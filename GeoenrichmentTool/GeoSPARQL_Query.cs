using ArcGIS.Core.Geometry;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace GeoenrichmentTool
{
    public partial class GeoSPARQL_Query : Form
    {
        

        protected string fileSavePath = "";
        protected Polygon geometry;
        protected Dictionary<string, string> ptArray;
        private readonly QuerySPARQL queryClass;

        public GeoSPARQL_Query(Polygon geo)
        {
            InitializeComponent();
            endPoint.Text = QuerySPARQL.GetDefaultEndPoint();
            geometry = geo;
            queryClass = new QuerySPARQL();
            populatePlaceTypes();
        }

        private void populatePlaceTypes()
        {
            var entityTypeQuery = "select distinct ?entityType ?entityTypeLabel where { ?entity rdf:type ?entityType . ?entity geo:hasGeometry ?aGeom . ?entityType rdfs:label ?entityTypeLabel }";

            JToken entityTypeJson = queryClass.SubmitQuery(entityTypeQuery);

            ptArray = new Dictionary<string, string>();
            List<object> termsList = new List<object>();

            foreach (var jsonResult in entityTypeJson)
            {
                string placeURI = jsonResult["entityType"]["value"].ToString();
                string placeType = jsonResult["entityTypeLabel"]["value"].ToString();
                string placeTypeFormatted = placeType + "(" + queryClass.MakeIRIPrefix(placeURI) + ")";
                ptArray[placeTypeFormatted] = placeURI;
                termsList.Add(placeTypeFormatted);
            }

            placeType.Items.AddRange(termsList.ToArray());
        }

        private void SubmitGeoQueryForm(object sender, EventArgs e)
        {
            formError.Text = "";
            if (endPoint.Text == "" | className.Text == "")
            {
                formError.Text = "* Required fields missing!";
            }
            else
            {
                if(ChooseFolder())
                {
                    Close();

                    var gfEndPoint = endPoint.Text; //sparql_endpoint
                    string gfPlaceType = placeType.Text; //inPlaceType
                    var gfSubclassReasoning = subclassReasoning.Checked; //isDirectInstance
                    var gfCalculator = calculator.SelectedItem; //inTopoCal
                    var gfFileSavePath = fileSavePath; //outLocation
                    var gfClassName = className.Text; //outFeatureClassName
                    var gfOutPath = outputLocation.FileName; //out_path
                    var gfPlaceURI = (gfPlaceType != "") ? ptArray[gfPlaceType] : "";

                    queryClass.UpdateActiveEndPoint(gfEndPoint);

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
                        default:
                            geoFunc = new string[] { "geo:sfIntersects" };
                            break;
                    }

                    //TODO:: Build proper WKT value
                    var coordinates = geometry.Copy2DCoordinatesToList();
                    foreach(var coor in coordinates)
                    {
                        var test = "test";
                    }
                    var geoWKT = geometry.SpatialReference.Wkt;

                    //TODO::Make a query that actual gives results
                    var geoQueryResult = TypeAndGeoSPARQLQuery(geoWKT, gfPlaceURI, gfSubclassReasoning, geoFunc, queryClass);

                    var tring = "";

                    //TODO::Build out this function
                    //Json2Field.createFeatureClassFromSPARQLResult(geoQueryResult, gfOutPath, gfPlaceType, gfPlaceURI, gfSubclassReasoning) //TODO::Function
                }
            }
        }
        
        public bool ChooseFolder()
        {
            if (outputLocation.ShowDialog() == DialogResult.OK)
            {
                //fileSavePath = outputLocation.SelectedPath;

                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Format GeoSPARQL query by given query_geo_wkt and type
         * 
         * queryGeoWKT: the wkt literal //TODO::Variable - this is DEFINITELY probably not a string I think
         * selectedURL: the user spercified type IRI
         * isDirectInstance: True: use placeFlatType as the type of geo-entity, False: use selectedURL as the type of geo-entity
         * geoFunc: a list of geosparql functions
         * endPoint: URL for SPARQL endpoint
         **/
        private JToken TypeAndGeoSPARQLQuery(string queryGeoWKT, string selectedURL, bool isDirectInstance, string[] geoFunc, QuerySPARQL queryClass)
        {
            string query = "select distinct ?place ?placeLabel ?placeFlatType ?wkt " +
                "where" +
                "{" +
                "?place geo:hasGeometry ?geometry . " +
                "?place rdfs:label ?placeLabel . " +
                "?geometry geo:asWKT ?wkt . " +
                "{ '''" +
                "<http://www.opengis.net/def/crs/OGC/1.3/CRS84>" +
                queryGeoWKT + "''s'^^geo:wktLiteral " +
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

            return queryClass.SubmitQuery(query);
        }

        

        





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

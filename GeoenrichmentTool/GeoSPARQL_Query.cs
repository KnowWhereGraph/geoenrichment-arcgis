using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;
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
                Close();

                var gfEndPoint = endPoint.Text; //sparql_endpoint
                string gfPlaceType = placeType.Text; //inPlaceType
                var gfSubclassReasoning = subclassReasoning.Checked; //isDirectInstance
                var gfCalculator = calculator.SelectedItem; //inTopoCal
                var gfClassName = className.Text; //outFeatureClassName
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
                    
                List<string> coorArray = new List<string>();
                foreach (Coordinate2D coor in coordinates)
                {
                    MapPoint geoCoor = coor.ToMapPoint();
                    coorArray.Add(coor.X.ToString() + " " + coor.Y.ToString());
                }
                string geoWKT = "'''<http://www.opengis.net/def/crs/OGC/1.3/CRS84> Polygon((-83.4 34.0, -83.1 34.0, -83.1 34.2, -83.4 34.2, -83.4 34.0)) '''";
                //string geoWKT = "'''<http://www.opengis.net/def/crs/OGC/1.3/CRS84> Polygon((" + String.Join(", ", coorArray) + ")) '''";

                //TODO::Make a query that actual gives results
                var geoQueryResult = TypeAndGeoSPARQLQuery(geoWKT, gfPlaceURI, gfSubclassReasoning, geoFunc, queryClass);

                //TODO::Build out this function
                CreateClassFromSPARQL(geoQueryResult, gfClassName, gfPlaceType, gfPlaceURI, gfSubclassReasoning);
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
                "{ " + queryGeoWKT + "^^geo:wktLiteral " +
                geoFunc[0] + "  ?geometry .}";

            if(geoFunc.Length==2)
            {
                query += " union" +
                    "{ " + queryGeoWKT + "^^geo:wktLiteral " +
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
         * Format GeoSPARQL query by given query_geo_wkt and type
         * 
         * GeoQueryResult: a sparql query result json obj serialized as a list of dict()
         *           SPARQL query like this:
         *           select distinct ?place ?placeLabel ?placeFlatType ?wkt
         *           where
         *           {...}
         * out_path: the output path for the create geo feature class
         * inPlaceType: the label of user spercified type IRI
         * selectedURL: the user spercified type IRI
         * isDirectInstance: True: use placeFlatType as the type of geo-entity, False: use selectedURL as the type of geo-entity
         **/
        private async void CreateClassFromSPARQL(JToken geoQueryResult, string className, string inPlaceType, string selectedURL, bool isDirectInstance=false, bool vizRes=true)
        {
            List<string> placeIRISet = new List<string>();
            List<string[]> placeList = new List<string[]>();
            string geomType = "";
            int index = 0;
            foreach (var item in geoQueryResult)
            {
                string wktLiteral = item["wkt"]["value"].ToString();
                if(index == 0)
                {
                    geomType = GetShapeFromWKT(wktLiteral);
                }
                else
                {
                    if(wktLiteral!=geomType)
                    {
                        continue;
                    }
                }

                string placeType = (item["placeFlatType"] != null) ? item["placeFlatType"]["value"].ToString() : "";
                if (isDirectInstance)
                {
                    placeType = selectedURL;
                }

                string place = item["place"]["value"].ToString();
                if(!placeIRISet.Contains(place))
                {
                    placeIRISet.Add(place);
                    placeList.Add(new string[] { place, item["placeLabel"]["value"].ToString(), placeType, wktLiteral });
                }

                index++;
            }

            if(placeList.Count != 0)
            {
                await FeatureClassHelper.CreatePolygonFeatureLayer(className);

                var fcLayer = MapView.Active.Map.GetLayersAsFlattenedList().Where((l) => l.Name == className).FirstOrDefault() as BasicFeatureLayer;

                if (fcLayer == null)
                {
                    MessageBox.Show($@"Unable to find {className} in the active map");
                }
                else
                {
                    await FeatureClassHelper.AddField(fcLayer, "Label", "TEXT");
                    await FeatureClassHelper.AddField(fcLayer, "URL", "TEXT");
                    await FeatureClassHelper.AddField(fcLayer, "Class", "TEXT");

                    InsertCursor cursor = fcLayer.GetTable().CreateInsertCursor(); //TODO::Queue this?

                    foreach (string[] item in placeList)
                    {
                        RowBuffer buff = fcLayer.GetTable().CreateRowBuffer();
                        buff["URL"] = item[0];
                        buff["Label"] = item[1];
                        buff["Class"] = item[2];
                        buff["SHAPE@WKT"] = item[3].Replace("<http://www.opengis.net/def/crs/OGC/1.3/CRS84>", "");
                        cursor.Insert(buff);
                    }

                    cursor.Dispose();

                    if (vizRes)
                    {
                        //TODO::ArcpyViz.visualize_current_layer(out_path)
                    }
                }
            }
        }

        private string GetShapeFromWKT(string WKT)
        {
            if(WKT.ToLower().Contains("point"))
            {
                return "POINT";
            } 
            else if (WKT.ToLower().Contains("multipoint"))
            {
                return "MULTIPOINT";
            }
            else if (WKT.ToLower().Contains("linestring"))
            {
                return "POLYLINE";
            }
            else if (WKT.ToLower().Contains("polygon"))
            {
                return "polygon";
            }

            return "";
        } 
    }
}

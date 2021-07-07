using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
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
        protected string polyString;
        protected Dictionary<string, string> ptArray;

        public GeoSPARQL_Query(string geo)
        {
            InitializeComponent();
            endPoint.Text = QuerySPARQL.GetDefaultEndPoint();
            ///TODO::DEV CODE///
            calculator.Text = "Contain";
            Random gen = new Random();
            className.Text = "layerName" + gen.Next(999999).ToString();
            ////////////////////
            polyString = geo;
            PopulatePlaceTypes();
        }

        private void RefreshPlaceList(object sender, EventArgs e)
        {
            placeType.Items.Clear();
            placeType.ResetText();
            GeoModule.Current.GetQueryClass().UpdateActiveEndPoint(endPoint.Text);

            try
            {
                PopulatePlaceTypes();
                MessageBox.Show($@"Place types updated!");
            }
            catch(Exception ex)
            {
                MessageBox.Show($@"Failed to connect to endpoint!");
            }
        }

        private void PopulatePlaceTypes()
        {
            var entityTypeQuery = "select distinct ?entityType ?entityTypeLabel where { ?entity rdf:type ?entityType . ?entity geo:hasGeometry ?aGeom . ?entityType rdfs:label ?entityTypeLabel }";
            QuerySPARQL queryClass = GeoModule.Current.GetQueryClass();

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
            if (endPoint.Text == "" | className.Text == "" | calculator.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                Close();

                string gfEndPoint = endPoint.Text;
                string gfPlaceType = placeType.Text;
                bool gfSubclassReasoning = subclassReasoning.Checked;
                string gfCalculator = calculator.Text;
                string gfClassName = className.Text.Replace(" ", "_");
                string gfPlaceURI = (gfPlaceType != "") ? ptArray[gfPlaceType] : "";

                QuerySPARQL queryClass = GeoModule.Current.GetQueryClass();
                queryClass.UpdateActiveEndPoint(gfEndPoint);

                string[] geoFunc = new string[] { };
                switch (gfCalculator)
                {
                    case "Contain or Intersect":
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

                //Build proper WKT value
                string geoWKT = "'''<http://www.opengis.net/def/crs/OGC/1.3/CRS84> " + polyString + " '''";

                var geoQueryResult = TypeAndGeoSPARQLQuery(geoWKT, gfPlaceURI, gfSubclassReasoning, geoFunc, queryClass);

                FeatureClassHelper.CreateClassFromSPARQL(geoQueryResult, gfClassName, gfPlaceType, gfPlaceURI, gfSubclassReasoning);

                //Enable the property enrichment tool since we have a layer for it to use
                FrameworkApplication.State.Activate("kwg_query_layer_added");
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
                "?place rdf:type ?placeFlatType ." +
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

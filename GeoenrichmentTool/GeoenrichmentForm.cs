using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KWG_Geoenrichment
{
    public partial class GeoenrichmentForm : Form
    {
        //Array that maps the formatted feature type label to the respective URI
        protected Dictionary<string, string> featureTypeArray;

        public GeoenrichmentForm()
        {
            InitializeComponent();

            KwgGeoModule.Current.activeGeoenrichmentForm = this;
            knowledgeGraph.Text = KwgGeoModule.Current.GetQueryClass().GetActiveEndPoint();
            PopulateFeatureTypes();
        }

        private void PopulateFeatureTypes()
        {
            var entityTypeQuery = "select distinct ?entityType ?entityTypeLabel where { ?entity rdf:type ?entityType . ?entity geo:hasGeometry ?aGeom . ?entityType rdfs:label ?entityTypeLabel }";
            QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();

            JToken entityTypeJson = queryClass.SubmitQuery(entityTypeQuery);

            featureTypeArray = new Dictionary<string, string>();
            List<object> termsList = new List<object>();

            foreach (var jsonResult in entityTypeJson)
            {
                string fURI = jsonResult["entityType"]["value"].ToString();
                string fType = jsonResult["entityTypeLabel"]["value"].ToString();
                string featureTypeFormatted = fType + " (" + queryClass.MakeIRIPrefix(fURI) + ")";
                featureTypeArray[featureTypeFormatted] = fURI;
                termsList.Add(featureTypeFormatted);
            }

            featureType.Items.AddRange(termsList.ToArray());
        }

        private void RefreshFeatureTypes(object sender, EventArgs e)
        {
            featureType.Items.Clear();
            featureType.ResetText();
            KwgGeoModule.Current.GetQueryClass().UpdateActiveEndPoint(knowledgeGraph.Text);

            try
            {
                PopulateFeatureTypes();
                MessageBox.Show($@"Feature types updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Failed to connect to endpoint!");
            }
        }

        private void getPropertiesForFeature(object sender, EventArgs e)
        {
            string selectedType = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(featureTypeArray[featureType.Text]);
            PropertyEnrichment propEnrichClass = new PropertyEnrichment(selectedType);

            List<string>[] commonProperties = propEnrichClass.CommonProperties;
            List<string>[] sosaObsProperties = propEnrichClass.SosaObsProperties;
            List<string>[] inverseProperties = propEnrichClass.InverseProperties;

            for (var i = 0; i < commonProperties[0].Count(); i++)
            {
                string url = commonProperties[0][i];
                string name = commonProperties[1][i];

                commonPropertiesBox.Rows.Add(false, name, null, url);
            }

            for (var i = 0; i < sosaObsProperties[0].Count(); i++)
            {
                string url = sosaObsProperties[0][i];
                string name = sosaObsProperties[1][i];

                //We want to add the value to main user selection list for common properties
                commonPropertiesBox.Rows.Add(false, name, null, url);
                //But we also want to keep track of the fact its a sosa observation value
                //soList.Add(value);
            }

            for (var i = 0; i < inverseProperties[0].Count(); i++)
            {
                string url = inverseProperties[0][i];
                string name = inverseProperties[1][i];

                inversePropertiesBox.Rows.Add(false, name, null, url);
            }
        }

        private void DrawAreaOfInterest(object sender, EventArgs e)
        {
            Close();
            FrameworkApplication.SetCurrentToolAsync("KWG_Geoenrichment_DrawPolygon");
        }

        public void SubmitGeoenrichmentForm(string polygonString)
        {
            if (knowledgeGraph.Text == "" | spatialRelation.Text == "" | saveLayerAs.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                //We need to update our global query module to point at the specified knowledge graph endpoint
                QuerySPARQL queryClass = KwgGeoModule.Current.GetQueryClass();
                queryClass.UpdateActiveEndPoint(knowledgeGraph.Text);

                string featureTypeURI = (featureType.Text != "") ? featureTypeArray[featureType.Text] : "";

                string[] geoFunctions = new string[] { };
                switch (spatialRelation.Text)
                {
                    case "Contain or Intersect":
                        geoFunctions = new string[] { "geo:sfContains", "geo:sfIntersects" };
                        break;
                    case "Contain":
                        geoFunctions = new string[] { "geo:sfContains" };
                        break;
                    case "Within":
                        geoFunctions = new string[] { "geo:sfWithin" };
                        break;
                    case "Intersect":
                    default:
                        geoFunctions = new string[] { "geo:sfIntersects" };
                        break;
                }

                //Build proper WKT value
                string polygonWKT = "'''<http://www.opengis.net/def/crs/OGC/1.3/CRS84> " + polygonString + " '''";

                var geoQueryResult = TypeAndGeoSPARQLQuery(polygonWKT, featureTypeURI, ignoreSubclasses.Checked, geoFunctions);

                FeatureClassHelper.CreateClassFromSPARQL(geoQueryResult, saveLayerAs.Text.Replace(" ", "_"), featureTypeURI, ignoreSubclasses.Checked);

                //TODO:: Integrate merge code

                //Enable the property enrichment tool since we have a layer for it to use
                FrameworkApplication.State.Activate("kwg_query_layer_added");
            }
        }

        /**
         * Format GeoSPARQL query
         * 
         * polygonWKT: the wkt literal for the user polygon
         * featureTypeUri: the user spercified feature type
         * ignoreSubclasses: ignore use of subclasses for feature type
         * geoFunctions: a list of geosparql functions
         **/
        private JToken TypeAndGeoSPARQLQuery(string polygonWKT, string featureTypeURI, bool ignoreSubclasses, string[] geoFunctions)
        {
            string query = "select distinct ?place ?placeLabel ?placeFlatType ?wkt " +
                "where" +
                "{" +
                "?place geo:hasGeometry ?geometry . " +
                "?place rdfs:label ?placeLabel . " +
                "?geometry geo:asWKT ?wkt . " +
                "?place rdf:type ?placeFlatType ." +
                "{ " + polygonWKT + "^^geo:wktLiteral " +
                geoFunctions[0] + "  ?geometry .}";

            if (geoFunctions.Length == 2)
            {
                query += " union" +
                    "{ " + polygonWKT + "^^geo:wktLiteral " +
                    geoFunctions[1] + "   ?geometry . }";
            }

            if (featureTypeURI != "")
            {
                if (!ignoreSubclasses)
                {
                    query += "?place rdf:type ?placeFlatType ." +
                    "?placeFlatType rdfs:subClassOf* <" + featureTypeURI + ">.";
                }
                else
                {
                    query += "?place rdf:type  <" + featureTypeURI + ">.";
                }

            }

            query += "}";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(query);
        }
    }
}

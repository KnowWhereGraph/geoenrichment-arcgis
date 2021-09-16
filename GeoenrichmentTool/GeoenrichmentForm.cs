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


            knowledgeGraph.Text = QuerySPARQL.GetDefaultEndPoint();
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

        private void DrawAreaOfInterest(object sender, EventArgs e)
        {
            DrawPolygon drawTool = new DrawPolygon();
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

                string value = name + " | " + url;

                commonPropertiesBox.Rows.Add(false, name, null, url);
            }

            for (var i = 0; i < sosaObsProperties[0].Count(); i++)
            {
                string url = sosaObsProperties[0][i];
                string name = sosaObsProperties[1][i];

                string value = name + " | " + url;

                //We want to add the value to main user selection list for common properties
                commonPropertiesBox.Rows.Add(false, name, null, url);
                //But we also want to keep track of the fact its a sosa observation value
                //soList.Add(value);
            }

            for (var i = 0; i < inverseProperties[0].Count(); i++)
            {
                string url = inverseProperties[0][i];
                string name = inverseProperties[1][i];

                string value = name + " | " + url;

                inversePropertiesBox.Rows.Add(false, name, null, url);
            }
        }
    }
}

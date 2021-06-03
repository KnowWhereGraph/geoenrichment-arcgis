using ArcGIS.Core.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoenrichmentTool
{
    public partial class GeoSPARQL_Query : Form
    {
        protected string defaultNameSpace = "http://stko-kwg.geog.ucsb.edu/";
        protected string defaultEndpoint = "http://stko-roy.geog.ucsb.edu:7202/repositories/plume_soil_wildfire";
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

                    string[] geosparql_func;
                    switch (gfCalculator)
                    {
                        case "Contain + Intersect":
                            geosparql_func = new string[] { "geo:sfContains", "geo:sfIntersects" };
                            break;
                        case "Contain":
                            geosparql_func = new string[] { "geo:sfContains" };
                            break;
                        case "Within":
                            geosparql_func = new string[] { "geo:sfWithin" };
                            break;
                        case "Intersect":
                            geosparql_func = new string[] { "geo:sfIntersects" };
                            break;
                        default:
                            //arcpy.AddError("The spatial relation is not supported!") //TODO::Reporting
                            break;
                    }

                    //query_geo_wkt = UTIL.get_geometrywkt_from_interactive_featureclass_by_idx(queryGeoExtent) //TODO::Function

                    //messages.addMessage("query_geo_wkt: {0}".format(query_geo_wkt)) //TODO::Reporting

                    //query_geo_wkt = UTIL.project_wkt_to_wgs84(queryGeoExtent, query_geo_wkt) //TODO::Function

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

                    //GeoQueryResult = SPARQLQuery.TypeAndGeoSPARQLQuery(query_geo_wkt, selectedURL, gfSubclassReasoning, geosparql_func, gfEndPoint); //TODO::Function

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
    }
}

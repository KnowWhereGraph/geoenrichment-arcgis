using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoenrichmentTool
{
    internal class CommonProperties : Button
    {
        protected async override void OnClick()
        {
            BasicFeatureLayer mainLayer = GeoModule.Current.GetLayers().First();
            List<string> uris = await FeatureClassHelper.GetURIs(mainLayer);

            if(uris.Count > 0)
            {
                /**
                # get the direct common property 
                commonPropertyJSONObj = SPARQLQuery.commonPropertyQuery(inplaceIRIList, 
                                                sparql_endpoint = sparql_endpoint,
                                                doSameAs = False)
                commonPropertyJSON = commonPropertyJSONObj["results"]["bindings"]

                if len(commonPropertyJSON) == 0:
                    arcpy.AddMessage("No property find.")
                    raise arcpy.ExecuteError
                else:
                    LinkedDataPropertyEnrichGeneral.propertyURLList = []
                    LinkedDataPropertyEnrichGeneral.propertyNameList = []
                    LinkedDataPropertyEnrichGeneral.propertyURLDict = dict()

                    LinkedDataPropertyEnrichGeneral.propertyURLDict = UTIL.extractCommonPropertyJSON(commonPropertyJSON, 
                                                p_url_list = LinkedDataPropertyEnrichGeneral.propertyURLList, 
                                                p_name_list = LinkedDataPropertyEnrichGeneral.propertyNameList, 
                                                url_dict = LinkedDataPropertyEnrichGeneral.propertyURLDict,
                                                p_var = "p", 
                                                plabel_var = "propLabel" if sparql_endpoint == SPARQLUtil._WIKIDATA_SPARQL_ENDPOINT else "pLabel", 
                                                numofsub_var = "NumofSub")
                            

                    in_com_property.filter.list = LinkedDataPropertyEnrichGeneral.propertyNameList
                            

                    # query for common sosa observabale property
                    LinkedDataPropertyEnrichGeneral.sosaObsPropURLList = []
                    LinkedDataPropertyEnrichGeneral.sosaObsPropNameList = []
                    LinkedDataPropertyEnrichGeneral.sosaObsPropURLDict = dict()

                    commonSosaObsPropJSONObj = SPARQLQuery.commonSosaObsPropertyQuery(inplaceIRIList, 
                                                sparql_endpoint = sparql_endpoint,
                                                doSameAs = False)
                    commonSosaObsPropJSON = commonSosaObsPropJSONObj["results"]["bindings"]
                    if len(commonSosaObsPropJSON) > 0:
                        LinkedDataPropertyEnrichGeneral.sosaObsPropURLDict = UTIL.extractCommonPropertyJSON(commonSosaObsPropJSON, 
                                                p_url_list = LinkedDataPropertyEnrichGeneral.sosaObsPropURLList, 
                                                p_name_list = LinkedDataPropertyEnrichGeneral.sosaObsPropNameList, 
                                                url_dict = LinkedDataPropertyEnrichGeneral.sosaObsPropURLDict,
                                                p_var = "p", 
                                                plabel_var = "pLabel", 
                                                numofsub_var = "NumofSub")

                        in_com_property.filter.list += LinkedDataPropertyEnrichGeneral.sosaObsPropNameList

                # get the inverse direct common property 
                if isInverse == True:
                    inverseCommonPropertyJSONObj = SPARQLQuery.inverseCommonPropertyQuery(inplaceIRIList, 
                                                                                            sparql_endpoint = sparql_endpoint, 
                                                                                            doSameAs = True)
                    inverseCommonPropertyJSON = inverseCommonPropertyJSONObj["results"]["bindings"]

                    if len(inverseCommonPropertyJSON) == 0:
                        arcpy.AddMessage("No inverse property find.")
                        out_message.value = "No inverse property find."
                        # in_inverse_com_property.value = "No inverse property find."
                        # raise arcpy.ExecuteError
                    else:
                                
                        LinkedDataPropertyEnrichGeneral.inversePropertyURLList = []
                        LinkedDataPropertyEnrichGeneral.inversePropertyNameList = []
                        LinkedDataPropertyEnrichGeneral.inversePropertyURLDict = dict()

                        LinkedDataPropertyEnrichGeneral.inversePropertyURLDict = UTIL.extractCommonPropertyJSON(inverseCommonPropertyJSON, 
                                                p_url_list = LinkedDataPropertyEnrichGeneral.inversePropertyURLList, 
                                                p_name_list = LinkedDataPropertyEnrichGeneral.inversePropertyNameList, 
                                                url_dict = LinkedDataPropertyEnrichGeneral.inversePropertyURLDict,
                                                p_var = "p", 
                                                plabel_var = "pLabel", 
                                                numofsub_var = "NumofSub")
                                

                        in_inverse_com_property.filter.list = LinkedDataPropertyEnrichGeneral.inversePropertyNameList
                 **/
            } 
            else
            {
                MessageBox.Show($@"No data to enrich for layer {mainLayer.Name}");
            }
        }
    }
}

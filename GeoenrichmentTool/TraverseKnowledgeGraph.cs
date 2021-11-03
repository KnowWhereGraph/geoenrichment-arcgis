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
    public partial class TraverseKnowledgeGraph : Form
    {
        /*
        # selectPropertyURLList: the selected peoperty URL list
         */
        private List<List<string>> permutations;

        private BasicFeatureLayer mainLayer;
        private string outTableName;
        private string outFeatureClassName;

        protected int maxDegree = 1;

        private int propertySpacing = 75;
        private int helpSpacing = 400;
        private bool helpOpen = true;

        public TraverseKnowledgeGraph()
        {
            InitializeComponent();
            ToggleHelpMenu();

            mainLayer = KwgGeoModule.Current.GetLayers().First();
            string featureClassName = mainLayer.Name;

            //TODO::Output
            //Random gen = new Random();
            //string identifier = gen.Next(999999).ToString();
            //outTableName = featureClassName + "PathQueryTripleStore_" + identifier;
            //outFeatureClassName = featureClassName + "PathQueryGeographicEntity_" + identifier;

            PopulateClassBox(1);
        }

        private async void PopulateClassBox(int degree)
        {
            //Clear boxes that have a higher degree
            for (int i = degree + 1; i <= maxDegree; i++)
            {
                ComboBox otherBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                otherBox.Enabled = false;
                otherBox.Text = "";
                otherBox.Items.Clear();
            }

            List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
            /*
            elif in_single_ent.value and in_do_single_ent_start.value:
                inplaceIRIList = [in_single_ent.valueAsText]
                featureClassName = "entity"
            */

            JToken finderJSON = RelationshipFinderClassQuery(inplaceIRIList, degree);

            ComboBox propBox = (ComboBox)this.Controls.Find("class" + degree.ToString(), true).First();
            propBox.Enabled = true;
            propBox.Text = "";
            propBox.Items.Clear();
            foreach (var item in finderJSON)
            {
                string itemClass = item["classLabel"]["value"].ToString();
                string itemVal = item["p" + degree.ToString()]["value"].ToString();
                string itemLabel = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
                if(!propBox.Items.Contains(itemClass + " | " + itemLabel + " | " + itemVal))
                    propBox.Items.Add(itemClass + " | " + itemLabel + " | " + itemVal);
            }
        }

        //private async void PopulatePropertyBox(int degree)
        //{
        //    //Clear boxes that have a higher degree
        //    for(int i=degree+1; i<=maxDegree; i++)
        //    {
        //        ComboBox otherBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
        //        otherBox.Enabled = false;
        //        otherBox.Text = "";
        //        otherBox.Items.Clear();
        //    }

        //    List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
        //    /*
        //    elif in_single_ent.value and in_do_single_ent_start.value:
        //        inplaceIRIList = [in_single_ent.valueAsText]
        //        featureClassName = "entity"
        //    */

        //    JToken finderJSON = RelationshipFinderCommonPropertyQuery(inplaceIRIList, degree);

        //    ComboBox propBox = (ComboBox)this.Controls.Find("prop" + degree.ToString(), true).First();
        //    propBox.Enabled = true;
        //    propBox.Text = "";
        //    propBox.Items.Clear();
        //    foreach (var item in finderJSON)
        //    {
        //        string itemVal = item["p" + degree.ToString()]["value"].ToString();
        //        string itemLabel = KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(itemVal);
        //        propBox.Items.Add(itemLabel + " | " + itemVal);
        //    }
        //}

        private JToken RelationshipFinderClassQuery(List<string> inplaceIRIList, int relationDegree)
        {
            string selectParam = "?p" + relationDegree.ToString();
            char[] delimPipe = { '|' };

            string relationFinderQuery = "SELECT distinct ?classLabel " + selectParam + " WHERE { ";

            if (relationDegree == 1)
            {
                relationFinderQuery += "{ ?place ?p1 ?o1. ?place a ?class. ?class rdfs:label ?classLabel }";
                relationFinderQuery += " UNION ";
                relationFinderQuery += "{ ?o1 ?p1 ?place. ?o1 a ?oclass. ?oclass rdfs:label ?classLabel }";
            }
            else
            {
                for (int index = 0; index < relationDegree-1; index++)
                {
                    int currDegree = index + 1;
                    ComboBox currentBox = (ComboBox)this.Controls.Find("class" + currDegree.ToString(), true).First();
                    string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                    string oValHigh = "?o" + currDegree.ToString();

                    relationFinderQuery += "{ " + oValLow + " <" + currentBox.Text.Split(delimPipe).Last().Trim() + "> " + oValHigh + ". }";
                    relationFinderQuery += " UNION ";
                    relationFinderQuery += "{ " + oValHigh + " <" + currentBox.Text.Split(delimPipe).Last().Trim() + "> " + oValLow + ". }";
                }

                string oValLowCurr = "?o" + (relationDegree-1).ToString();
                string oValHighCurr = "?o" + relationDegree.ToString();
                string pVal = "?p" + relationDegree.ToString();

                relationFinderQuery += " { " + oValLowCurr + " " + pVal + " " + oValHighCurr + ". " + oValLowCurr + " a ?oclass. ?oclass rdfs:label ?classLabel }";
            }

            relationFinderQuery += " VALUES ?place { ";

            foreach (var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "} }";

            return KwgGeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);
        }

        /*
        # get the property URL list in the specific degree path from the inplaceIRIList
        # inplaceIRIList: the URL list of wikidata locations
        # relationDegree: the degree of the property on the path the current query wants to get
         */
        //private JToken RelationshipFinderCommonPropertyQuery(List<string> inplaceIRIList, int relationDegree)
        //{
        //    string selectParam = "?p" + relationDegree.ToString();
        //    char[] delimPipe = { '|' };

        //    string relationFinderQuery = "SELECT distinct " + selectParam + " WHERE { ";

        //    for (int index = 0; index < relationDegree; index++)
        //    {
        //        int currDegree = index + 1;
        //        ComboBox currentBox = (ComboBox)this.Controls.Find("prop" + currDegree.ToString(), true).First();
        //        string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
        //        string oValHigh = "?o" + currDegree.ToString();
        //        string pVal = (currDegree == relationDegree) ? "?p" + currDegree.ToString() : "<" + currentBox.Text.Split(delimPipe).Last().Trim() + ">";

        //        relationFinderQuery += "{" + oValLow + " " + pVal + " " + oValHigh + ".} UNION {" + oValHigh + " " + pVal + " " + oValLow + ".}\n";
        //    }

        //    relationFinderQuery += " VALUES ?place { ";

        //    foreach (var iri in inplaceIRIList)
        //    {
        //        relationFinderQuery += "<" + iri + "> \n";
        //    }
        //    relationFinderQuery += "} }";

        //    return KwgGeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);
        //}

        private void ClassChanged(object sender, EventArgs e)
        {
            ComboBox classBox = (ComboBox)sender;
            int degree = int.Parse(classBox.Name.Replace("class", ""));

            if (degree < maxDegree)
            {
                PopulateClassBox(degree + 1);
            }
        }

        //private void PropertyChanged(object sender, EventArgs e)
        //{
        //    ComboBox propBox = (ComboBox)sender;
        //    int degree = int.Parse(propBox.Name.Replace("prop", ""));

        //    if (degree < maxDegree)
        //    {
        //        PopulateClassBox(degree + 1);
        //    }
        //}

        private void AddNewProperty(object sender, MouseEventArgs e)
        {
            int newDegree = maxDegree + 1;

            //Expand the form and move down the button elements
            this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height + propertySpacing);
            this.addPropertyBtn.Location = new System.Drawing.Point(this.addPropertyBtn.Location.X, this.addPropertyBtn.Location.Y + propertySpacing);
            this.runTraverseBtn.Location = new System.Drawing.Point(this.runTraverseBtn.Location.X, this.runTraverseBtn.Location.Y + propertySpacing);
            this.helpButton.Location = new System.Drawing.Point(this.helpButton.Location.X, this.helpButton.Location.Y + propertySpacing);
            this.helpPanel.Size = new System.Drawing.Size(this.helpPanel.Size.Width, this.helpPanel.Size.Height + propertySpacing);

            //Create new property elements based on old ones
            Label propRequired = (Label)this.Controls.Find("prop" + maxDegree.ToString() + "Req", true).First();
            Label propRequired_copy = new Label();
            propRequired_copy.AutoSize = propRequired.AutoSize;
            propRequired_copy.BackColor = propRequired.BackColor;
            propRequired_copy.Font = propRequired.Font;
            propRequired_copy.ForeColor = propRequired.ForeColor;
            propRequired_copy.Location = new System.Drawing.Point(45, propRequired.Location.Y + propertySpacing);
            propRequired_copy.Name = "prop" + newDegree.ToString() + "Req";
            propRequired_copy.Size = propRequired.Size;
            propRequired_copy.Text = propRequired.Text;
            this.Controls.Add(propRequired_copy);

            Label propLabel = (Label)this.Controls.Find("prop" + maxDegree.ToString() + "Label", true).First();
            Label propLabel_copy = new Label();
            propLabel_copy.AutoSize = propLabel.AutoSize;
            propLabel_copy.BackColor = propLabel.BackColor;
            propLabel_copy.Font = propLabel.Font;
            propLabel_copy.ForeColor = propLabel.ForeColor;
            propLabel_copy.Location = new System.Drawing.Point(60, propLabel.Location.Y + propertySpacing);
            propLabel_copy.Margin = propLabel.Margin;
            propLabel_copy.Name = "prop" + newDegree.ToString() + "Label";
            propLabel_copy.Size = propLabel.Size;
            propLabel_copy.Text = "More Content";
            this.Controls.Add(propLabel_copy);

            //ComboBox propBox = (ComboBox)this.Controls.Find("prop" + maxDegree.ToString(), true).First();
            //ComboBox propBox_copy = new ComboBox();
            //propBox_copy.Enabled = false;
            //propBox_copy.Font = propBox.Font;
            //propBox_copy.FormattingEnabled = propBox.FormattingEnabled;
            //propBox_copy.Location = new System.Drawing.Point(50, propBox.Location.Y + propertySpacing);
            //propBox_copy.Name = "prop" + newDegree.ToString();
            //propBox_copy.Size = propBox.Size;
            //propBox_copy.SelectedValueChanged += new System.EventHandler(this.PropertyChanged);
            //this.Controls.Add(propBox_copy);

            ComboBox classBox = (ComboBox)this.Controls.Find("class" + maxDegree.ToString(), true).First();
            ComboBox classBox_copy = new ComboBox();
            classBox_copy.Enabled = false;
            classBox_copy.Font = classBox.Font;
            classBox_copy.FormattingEnabled = classBox.FormattingEnabled;
            classBox_copy.Location = new System.Drawing.Point(50, classBox.Location.Y + propertySpacing);
            classBox_copy.Name = "class" + newDegree.ToString();
            classBox_copy.Size = classBox.Size;
            classBox_copy.SelectedValueChanged += new System.EventHandler(this.ClassChanged);
            this.Controls.Add(classBox_copy);

            //Populate list options and enable if previous property is selected
            if (classBox.Text != "")
            {
                classBox_copy.Enabled = true;
                PopulateClassBox(newDegree);
            }

            maxDegree++;
        }

        private async void RunTraverseGraph(object sender, EventArgs e)
        {
            if (class1.Text == "")
            {
                MessageBox.Show($@"Required fields missing!");
            }
            else
            {
                Close();

                /*
                if not in_do_single_ent_start
                */
                List<string> inplaceIRIList = await FeatureClassHelper.GetURIs(mainLayer);
                /*
                else:
                    # we use single iri as the start node
                    inplaceIRIList = [in_single_ent.valueAsText]
                */

                //Build the direction list
                int degreesUsed = 1;
                for (int i = 2; i <= maxDegree; i++)
                {
                    ComboBox propBox = (ComboBox)this.Controls.Find("prop" + i.ToString(), true).First();
                    if (propBox.Text != "")
                    {
                        degreesUsed = i;
                    }
                    else
                    {
                        break;
                    }
                }
                List<List<string>> directionExpendedLists = ExpandBothDirectionLists(degreesUsed);

                List<Dictionary<string, string>> tripleStore = new List<Dictionary<string, string>>() { };

                foreach (var currentDirectionList in directionExpendedLists)
                {
                    //# get a list of triples for curent specified property path
                    List<Dictionary<string, string>> newTripleStore = RelFinderTripleQuery(inplaceIRIList, currentDirectionList, degreesUsed);
                    tripleStore = tripleStore.Union(newTripleStore).ToList();
                }

                List<string> triplePropertyURLList = new List<string>() { };
                List<string> triplePropertyLabelList = new List<string>() { };
                foreach (var triple in tripleStore)
                {
                    if (!triplePropertyURLList.Contains(triple["p"]))
                    {
                        triplePropertyURLList.Add(triple["p"]);
                        triplePropertyLabelList.Add(KwgGeoModule.Current.GetQueryClass().MakeIRIPrefix(triple["p"]));
                    }
                }

                FeatureClassHelper.CreateRelationshipFinderTable(tripleStore, triplePropertyURLList, triplePropertyLabelList, outTableName);

                List<string> entitySet = new List<string>() { };
                foreach (var triple in tripleStore)
                {
                    entitySet.Add(triple["s"]);
                    entitySet.Add(triple["o"]);
                }

                JToken placeJSON = EndPlaceInformationQuery(entitySet);

                FeatureClassHelper.CreateRelationshipFinderFeatureClass(placeJSON, outTableName, outFeatureClassName);
            }
        }

        //Basically for the amount of degrees used, generate every binary list permutation of the values Origin and Destination
        // i.e. for 2 degrees, makes Origin Origin, Origin Destination, Destination Origin, Destination Destination
        private List<List<string>> ExpandBothDirectionLists(int degreeSize)
        {
            List<List<string>> propertyDirectionExpandedLists = new List<List<string>>() { };

            permutations = new List<List<string>>() { };
            GeneratePermutations(degreeSize);

            foreach (var perm in permutations)
            {
                List<string> newDirList = new List<string>(perm);

                propertyDirectionExpandedLists.Add(newDirList);
            }

            return propertyDirectionExpandedLists;
        }

        private void GeneratePermutations(int cnt, int curr = 0, List<string> prevList = null)
        {
            List<string> listOne = new List<string>() { };
            List<string> listTwo = new List<string>() { };

            if (prevList != null)
            {
                listOne = new List<string>(prevList); ;
                listTwo = new List<string>(prevList); ;
            }

            listOne.Add("Origin");
            listTwo.Add("Destination");

            curr++;
            if (curr < cnt)
            {
                GeneratePermutations(cnt, curr, listOne);
                GeneratePermutations(cnt, curr, listTwo);
            }
            else
            {
                permutations.Add(listOne);
                permutations.Add(listTwo);
            }
        }

        //# get the triple set in the specific degree path from the inplaceIRIList
        //# inplaceIRIList: the URL list of wikidata locations
        //# propertyDirectionList: the list of property direction, it has at most 4 elements, the length is the path degree. The element value is from ["ORIGIN", "DESTINATION"]
        //# selectPropertyURLList: the selected peoperty URL list, it always has 4 elements, "" if no property has been selected
        private List<Dictionary<string, string>> RelFinderTripleQuery(List<string> inplaceIRIList, List<string> currentDirectionList, int degreeSize)
        {
            string selectParam = "?place ";

            for (int i = 0; i < degreeSize; i++)
            {
                int currDegree = i + 1;
                //if selectPropertyURLList[0] == "":
                //    selectParam += "?p"+ currDegree.ToString() +" "

                selectParam += "?o" + currDegree.ToString() + " ";
            }

            string relationFinderQuery = "SELECT distinct " + selectParam + " WHERE {";
            string queryFilter = "";
            char[] delimPipe = { '|' };

            for (int index = 0; index < degreeSize; index++)
            {
                string currDirection = currentDirectionList[index];
                int currDegree = index + 1;
                ComboBox currentBox = (ComboBox)this.Controls.Find("prop" + currDegree.ToString(), true).First();
                string oValLow = (index == 0) ? "?place" : "?o" + index.ToString();
                string oValHigh = "?o" + currDegree.ToString();
                string pVal = (currDegree == degreeSize) ? "?p" + currDegree.ToString() : "<" + currentBox.Text.Split(delimPipe).Last().Trim() + ">";
                queryFilter += ". FILTER(!isLiteral(" + oValHigh + "))";

                switch (currDirection)
                {
                    case "Both":
                        relationFinderQuery += "{" + oValLow + " " + pVal + " " + oValHigh + ".} UNION {" + oValHigh + " " + pVal + " " + oValLow + ".}\n";
                        break;
                    case "Origin":
                        relationFinderQuery += oValLow + " " + pVal + " " + oValHigh + ".\n";
                        break;
                    case "Destination":
                        relationFinderQuery += oValHigh + " " + pVal + " " + oValLow + ".\n";
                        break;
                    default:
                        break;
                }
            }

            relationFinderQuery += " VALUES ?place { ";

            foreach (var iri in inplaceIRIList)
            {
                relationFinderQuery += "<" + iri + "> \n";
            }
            relationFinderQuery += "}" + queryFilter + "  }";

            JToken resultsJSON = KwgGeoModule.Current.GetQueryClass().SubmitQuery(relationFinderQuery);

            List<Dictionary<string, string>> tripleStore = new List<Dictionary<string, string>>() { };
            foreach (var jsonItem in resultsJSON)
            {
                for (int index = 0; index < degreeSize; index++)
                {
                    string currDirection = currentDirectionList[index];
                    int currDegree = index + 1;
                    ComboBox currentBox = (ComboBox)this.Controls.Find("prop" + currDegree.ToString(), true).First();
                    string oValLow = (index == 0) ? "place" : "o" + index.ToString();
                    string oValHigh = "o" + currDegree.ToString();

                    Dictionary<string, string> currentTriple = new Dictionary<string, string>() { };
                    switch (currDirection)
                    {
                        case "Origin":
                            currentTriple = new Dictionary<string, string>() {
                                { "s", jsonItem[oValLow]["value"].ToString() }, { "p", currentBox.Text.Split(delimPipe).Last().Trim() }, { "o", jsonItem[oValHigh]["value"].ToString() } //TODO:: CONFIRM THESE
                            };
                            break;
                        case "Destination":
                            currentTriple = new Dictionary<string, string>() {
                                { "s", jsonItem[oValHigh]["value"].ToString() }, { "p", currentBox.Text.Split(delimPipe).Last().Trim() }, { "o", jsonItem[oValLow]["value"].ToString() } //TODO:: CONFIRM THESE
                            };
                            break;
                        default:
                            break;
                    }

                    tripleStore.Add(currentTriple);
                }
            }

            return tripleStore;
        }

        private JToken EndPlaceInformationQuery(List<string> endPlaceIRIList)
        {
            JToken results = null;

            string endPlaceQueryPrefix = "SELECT distinct ?place ?placeLabel ?placeFlatType ?wkt WHERE { " +
                "?place geo:hasGeometry ?geometry . ?place rdfs:label ?placeLabel . ?geometry geo:asWKT ?wkt. " +
                "VALUES ?place {";
            string endPlaceQuerySuffix = "} }";

            string endPlaceQueryIRI = "";
            for (var i = 0; i < endPlaceIRIList.Count(); i++)
            {
                endPlaceQueryIRI += "<" + endPlaceIRIList[i] + "> \n";

                if (i % 50 == 0)
                {
                    string endPlaceQuery = endPlaceQueryPrefix + endPlaceQueryIRI + endPlaceQuerySuffix;
                    JToken subresults = KwgGeoModule.Current.GetQueryClass().SubmitQuery(endPlaceQuery);

                    if (results == null)
                    {
                        results = subresults;
                    }
                    else
                    {
                        results.Append(subresults);
                    }

                    endPlaceQueryIRI = "";
                }
            }

            return results;
        }

        private void ClickToggleHelpMenu(object sender, EventArgs e)
        {
            ToggleHelpMenu();
        }

        private void ToggleHelpMenu()
        {
            if (helpOpen)
            {
                this.Size = new System.Drawing.Size(this.Size.Width - helpSpacing, this.Size.Height);
                helpOpen = false;
            }
            else
            {
                this.Size = new System.Drawing.Size(this.Size.Width + helpSpacing, this.Size.Height);
                helpOpen = true;
            }
        }
    }
}

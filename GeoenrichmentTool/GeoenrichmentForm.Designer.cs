
namespace KWG_Geoenrichment
{
    partial class GeoenrichmentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.geoFormName = new System.Windows.Forms.Label();
            this.knowledgeGraph = new System.Windows.Forms.TextBox();
            this.requiredKnowledgeGraph = new System.Windows.Forms.Label();
            this.knowledgeGraphLabel = new System.Windows.Forms.Label();
            this.featureType = new System.Windows.Forms.ComboBox();
            this.featureTypeLabel = new System.Windows.Forms.Label();
            this.refreshPlaceTypeBtn = new System.Windows.Forms.Button();
            this.spatialRelationLabel = new System.Windows.Forms.Label();
            this.requiredSpatialRelation = new System.Windows.Forms.Label();
            this.spatialRelation = new System.Windows.Forms.ComboBox();
            this.requiredSaveLayerAs = new System.Windows.Forms.Label();
            this.saveLayerAsLabel = new System.Windows.Forms.Label();
            this.saveLayerAs = new System.Windows.Forms.TextBox();
            this.selectAreaBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.commonPropertiesBoxLabel = new System.Windows.Forms.Label();
            this.inverseCheckBoxLabel = new System.Windows.Forms.Label();
            this.commonPropertiesBox = new System.Windows.Forms.DataGridView();
            this.Use = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MergeRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.URI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.helpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.commonPropertiesBox)).BeginInit();
            this.SuspendLayout();
            // 
            // geoFormName
            // 
            this.geoFormName.AutoSize = true;
            this.geoFormName.BackColor = System.Drawing.Color.Transparent;
            this.geoFormName.Font = new System.Drawing.Font("Arial", 34F, System.Drawing.FontStyle.Bold);
            this.geoFormName.ForeColor = System.Drawing.Color.White;
            this.geoFormName.Location = new System.Drawing.Point(41, 51);
            this.geoFormName.Name = "geoFormName";
            this.geoFormName.Size = new System.Drawing.Size(774, 54);
            this.geoFormName.TabIndex = 3;
            this.geoFormName.Text = "KnowWhereGraph Geoenrichment";
            // 
            // knowledgeGraph
            // 
            this.knowledgeGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.knowledgeGraph.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.knowledgeGraph.Location = new System.Drawing.Point(50, 198);
            this.knowledgeGraph.Name = "knowledgeGraph";
            this.knowledgeGraph.Size = new System.Drawing.Size(765, 26);
            this.knowledgeGraph.TabIndex = 4;
            // 
            // requiredKnowledgeGraph
            // 
            this.requiredKnowledgeGraph.AutoSize = true;
            this.requiredKnowledgeGraph.BackColor = System.Drawing.Color.Transparent;
            this.requiredKnowledgeGraph.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredKnowledgeGraph.ForeColor = System.Drawing.Color.Red;
            this.requiredKnowledgeGraph.Location = new System.Drawing.Point(45, 166);
            this.requiredKnowledgeGraph.Name = "requiredKnowledgeGraph";
            this.requiredKnowledgeGraph.Size = new System.Drawing.Size(22, 29);
            this.requiredKnowledgeGraph.TabIndex = 15;
            this.requiredKnowledgeGraph.Text = "*";
            // 
            // knowledgeGraphLabel
            // 
            this.knowledgeGraphLabel.AutoSize = true;
            this.knowledgeGraphLabel.BackColor = System.Drawing.Color.Transparent;
            this.knowledgeGraphLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.knowledgeGraphLabel.ForeColor = System.Drawing.Color.White;
            this.knowledgeGraphLabel.Location = new System.Drawing.Point(60, 166);
            this.knowledgeGraphLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.knowledgeGraphLabel.Name = "knowledgeGraphLabel";
            this.knowledgeGraphLabel.Size = new System.Drawing.Size(315, 29);
            this.knowledgeGraphLabel.TabIndex = 16;
            this.knowledgeGraphLabel.Text = "Choose Knowledge Graph";
            // 
            // featureType
            // 
            this.featureType.Font = new System.Drawing.Font("Arial", 12F);
            this.featureType.FormattingEnabled = true;
            this.featureType.Location = new System.Drawing.Point(50, 304);
            this.featureType.Name = "featureType";
            this.featureType.Size = new System.Drawing.Size(661, 26);
            this.featureType.TabIndex = 17;
            this.featureType.SelectedIndexChanged += new System.EventHandler(this.getPropertiesForFeature);
            // 
            // featureTypeLabel
            // 
            this.featureTypeLabel.AutoSize = true;
            this.featureTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.featureTypeLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.featureTypeLabel.ForeColor = System.Drawing.Color.White;
            this.featureTypeLabel.Location = new System.Drawing.Point(50, 266);
            this.featureTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.featureTypeLabel.Name = "featureTypeLabel";
            this.featureTypeLabel.Size = new System.Drawing.Size(177, 29);
            this.featureTypeLabel.TabIndex = 19;
            this.featureTypeLabel.Text = "Select content";
            // 
            // refreshPlaceTypeBtn
            // 
            this.refreshPlaceTypeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.refreshPlaceTypeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.refreshPlaceTypeBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.refreshPlaceTypeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshPlaceTypeBtn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.refreshPlaceTypeBtn.ForeColor = System.Drawing.Color.White;
            this.refreshPlaceTypeBtn.Location = new System.Drawing.Point(717, 304);
            this.refreshPlaceTypeBtn.Name = "refreshPlaceTypeBtn";
            this.refreshPlaceTypeBtn.Size = new System.Drawing.Size(98, 26);
            this.refreshPlaceTypeBtn.TabIndex = 20;
            this.refreshPlaceTypeBtn.Text = "REFRESH";
            this.refreshPlaceTypeBtn.UseVisualStyleBackColor = false;
            this.refreshPlaceTypeBtn.Click += new System.EventHandler(this.RefreshFeatureTypes);
            // 
            // spatialRelationLabel
            // 
            this.spatialRelationLabel.AutoSize = true;
            this.spatialRelationLabel.BackColor = System.Drawing.Color.Transparent;
            this.spatialRelationLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.spatialRelationLabel.ForeColor = System.Drawing.Color.White;
            this.spatialRelationLabel.Location = new System.Drawing.Point(60, 580);
            this.spatialRelationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.spatialRelationLabel.Name = "spatialRelationLabel";
            this.spatialRelationLabel.Size = new System.Drawing.Size(260, 29);
            this.spatialRelationLabel.TabIndex = 22;
            this.spatialRelationLabel.Text = "Spatial Relation Filter";
            // 
            // requiredSpatialRelation
            // 
            this.requiredSpatialRelation.AutoSize = true;
            this.requiredSpatialRelation.BackColor = System.Drawing.Color.Transparent;
            this.requiredSpatialRelation.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredSpatialRelation.ForeColor = System.Drawing.Color.Red;
            this.requiredSpatialRelation.Location = new System.Drawing.Point(45, 580);
            this.requiredSpatialRelation.Name = "requiredSpatialRelation";
            this.requiredSpatialRelation.Size = new System.Drawing.Size(22, 29);
            this.requiredSpatialRelation.TabIndex = 23;
            this.requiredSpatialRelation.Text = "*";
            // 
            // spatialRelation
            // 
            this.spatialRelation.Font = new System.Drawing.Font("Arial", 12F);
            this.spatialRelation.FormattingEnabled = true;
            this.spatialRelation.Items.AddRange(new object[] {
            "Contain or Intersect",
            "Contain",
            "Within",
            "Intersect"});
            this.spatialRelation.Location = new System.Drawing.Point(50, 618);
            this.spatialRelation.Name = "spatialRelation";
            this.spatialRelation.Size = new System.Drawing.Size(765, 26);
            this.spatialRelation.TabIndex = 24;
            // 
            // requiredSaveLayerAs
            // 
            this.requiredSaveLayerAs.AutoSize = true;
            this.requiredSaveLayerAs.BackColor = System.Drawing.Color.Transparent;
            this.requiredSaveLayerAs.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredSaveLayerAs.ForeColor = System.Drawing.Color.Red;
            this.requiredSaveLayerAs.Location = new System.Drawing.Point(45, 680);
            this.requiredSaveLayerAs.Name = "requiredSaveLayerAs";
            this.requiredSaveLayerAs.Size = new System.Drawing.Size(22, 29);
            this.requiredSaveLayerAs.TabIndex = 25;
            this.requiredSaveLayerAs.Text = "*";
            // 
            // saveLayerAsLabel
            // 
            this.saveLayerAsLabel.AutoSize = true;
            this.saveLayerAsLabel.BackColor = System.Drawing.Color.Transparent;
            this.saveLayerAsLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.saveLayerAsLabel.ForeColor = System.Drawing.Color.White;
            this.saveLayerAsLabel.Location = new System.Drawing.Point(60, 680);
            this.saveLayerAsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.saveLayerAsLabel.Name = "saveLayerAsLabel";
            this.saveLayerAsLabel.Size = new System.Drawing.Size(267, 29);
            this.saveLayerAsLabel.TabIndex = 26;
            this.saveLayerAsLabel.Text = "Save Feature Layer As";
            // 
            // saveLayerAs
            // 
            this.saveLayerAs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.saveLayerAs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLayerAs.Location = new System.Drawing.Point(50, 718);
            this.saveLayerAs.Name = "saveLayerAs";
            this.saveLayerAs.Size = new System.Drawing.Size(765, 26);
            this.saveLayerAs.TabIndex = 27;
            // 
            // selectAreaBtn
            // 
            this.selectAreaBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.selectAreaBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectAreaBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.selectAreaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectAreaBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.selectAreaBtn.ForeColor = System.Drawing.Color.Black;
            this.selectAreaBtn.Location = new System.Drawing.Point(591, 803);
            this.selectAreaBtn.Name = "selectAreaBtn";
            this.selectAreaBtn.Size = new System.Drawing.Size(224, 63);
            this.selectAreaBtn.TabIndex = 28;
            this.selectAreaBtn.Text = "SELECT AREA";
            this.selectAreaBtn.UseVisualStyleBackColor = false;
            this.selectAreaBtn.Click += new System.EventHandler(this.DrawAreaOfInterest);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(350, 803);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 63);
            this.button1.TabIndex = 29;
            this.button1.Text = "OPEN GDB FILE";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // commonPropertiesBoxLabel
            // 
            this.commonPropertiesBoxLabel.AutoSize = true;
            this.commonPropertiesBoxLabel.BackColor = System.Drawing.Color.Transparent;
            this.commonPropertiesBoxLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.commonPropertiesBoxLabel.ForeColor = System.Drawing.Color.White;
            this.commonPropertiesBoxLabel.Location = new System.Drawing.Point(84, 346);
            this.commonPropertiesBoxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.commonPropertiesBoxLabel.Name = "commonPropertiesBoxLabel";
            this.commonPropertiesBoxLabel.Size = new System.Drawing.Size(146, 29);
            this.commonPropertiesBoxLabel.TabIndex = 31;
            this.commonPropertiesBoxLabel.Text = "Enrich Data";
            // 
            // inverseCheckBoxLabel
            // 
            this.inverseCheckBoxLabel.AutoSize = true;
            this.inverseCheckBoxLabel.BackColor = System.Drawing.Color.Transparent;
            this.inverseCheckBoxLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.inverseCheckBoxLabel.ForeColor = System.Drawing.Color.White;
            this.inverseCheckBoxLabel.Location = new System.Drawing.Point(82, 486);
            this.inverseCheckBoxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.inverseCheckBoxLabel.Name = "inverseCheckBoxLabel";
            this.inverseCheckBoxLabel.Size = new System.Drawing.Size(332, 29);
            this.inverseCheckBoxLabel.TabIndex = 33;
            this.inverseCheckBoxLabel.Text = "Inverse Common Properties";
            // 
            // commonPropertiesBox
            // 
            this.commonPropertiesBox.AllowUserToAddRows = false;
            this.commonPropertiesBox.AllowUserToDeleteRows = false;
            this.commonPropertiesBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.commonPropertiesBox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Use,
            this.Property,
            this.MergeRule,
            this.URI});
            this.commonPropertiesBox.Location = new System.Drawing.Point(89, 378);
            this.commonPropertiesBox.Name = "commonPropertiesBox";
            this.commonPropertiesBox.Size = new System.Drawing.Size(726, 150);
            this.commonPropertiesBox.TabIndex = 35;
            // 
            // Use
            // 
            this.Use.HeaderText = "Use";
            this.Use.Name = "Use";
            this.Use.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Use.Width = 40;
            // 
            // Property
            // 
            this.Property.HeaderText = "Property";
            this.Property.Name = "Property";
            this.Property.ReadOnly = true;
            this.Property.Width = 200;
            // 
            // MergeRule
            // 
            this.MergeRule.HeaderText = "Merge Rule";
            this.MergeRule.Items.AddRange(new object[] {
            "SUM",
            "MIN",
            "MAX",
            "STDEV",
            "MEAN",
            "COUNT",
            "FIRST",
            "LAST",
            "CONCATENATE"});
            this.MergeRule.Name = "MergeRule";
            this.MergeRule.Width = 150;
            // 
            // URI
            // 
            this.URI.HeaderText = "URI";
            this.URI.Name = "URI";
            this.URI.ReadOnly = true;
            this.URI.Width = 293;
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.Transparent;
            this.helpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.helpButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.helpButton.FlatAppearance.BorderSize = 0;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Image = global::KWG_Geoenrichment.Properties.Resources.help_circle;
            this.helpButton.Location = new System.Drawing.Point(12, 813);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(74, 70);
            this.helpButton.TabIndex = 36;
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.ToggleHelpMenu);
            // 
            // GeoenrichmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KWG_Geoenrichment.Properties.Resources.background_landing__2_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(894, 895);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.commonPropertiesBox);
            this.Controls.Add(this.inverseCheckBoxLabel);
            this.Controls.Add(this.commonPropertiesBoxLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.selectAreaBtn);
            this.Controls.Add(this.saveLayerAs);
            this.Controls.Add(this.saveLayerAsLabel);
            this.Controls.Add(this.requiredSaveLayerAs);
            this.Controls.Add(this.spatialRelation);
            this.Controls.Add(this.requiredSpatialRelation);
            this.Controls.Add(this.spatialRelationLabel);
            this.Controls.Add(this.refreshPlaceTypeBtn);
            this.Controls.Add(this.featureTypeLabel);
            this.Controls.Add(this.featureType);
            this.Controls.Add(this.knowledgeGraphLabel);
            this.Controls.Add(this.requiredKnowledgeGraph);
            this.Controls.Add(this.knowledgeGraph);
            this.Controls.Add(this.geoFormName);
            this.DoubleBuffered = true;
            this.HelpButton = true;
            this.Name = "GeoenrichmentForm";
            this.Text = "KnowWhereGraph Geoenrichment";
            ((System.ComponentModel.ISupportInitialize)(this.commonPropertiesBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label geoFormName;
        private System.Windows.Forms.TextBox knowledgeGraph;
        private System.Windows.Forms.Label requiredKnowledgeGraph;
        private System.Windows.Forms.Label knowledgeGraphLabel;
        private System.Windows.Forms.ComboBox featureType;
        private System.Windows.Forms.Label featureTypeLabel;
        private System.Windows.Forms.Button refreshPlaceTypeBtn;
        private System.Windows.Forms.Label spatialRelationLabel;
        private System.Windows.Forms.Label requiredSpatialRelation;
        private System.Windows.Forms.ComboBox spatialRelation;
        private System.Windows.Forms.Label requiredSaveLayerAs;
        private System.Windows.Forms.Label saveLayerAsLabel;
        private System.Windows.Forms.TextBox saveLayerAs;
        private System.Windows.Forms.Button selectAreaBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label commonPropertiesBoxLabel;
        private System.Windows.Forms.Label inverseCheckBoxLabel;
        private System.Windows.Forms.DataGridView commonPropertiesBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Use;
        private System.Windows.Forms.DataGridViewTextBoxColumn Property;
        private System.Windows.Forms.DataGridViewComboBoxColumn MergeRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn URI;
        private System.Windows.Forms.Button helpButton;
    }
}
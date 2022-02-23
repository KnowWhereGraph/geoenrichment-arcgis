
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoenrichmentForm));
            this.geoFormName = new System.Windows.Forms.Label();
            this.requiredKnowledgeGraph = new System.Windows.Forms.Label();
            this.knowledgeGraphLabel = new System.Windows.Forms.Label();
            this.selectContentBtn = new System.Windows.Forms.Button();
            this.spatialRelationLabel = new System.Windows.Forms.Label();
            this.requiredSpatialRelation = new System.Windows.Forms.Label();
            this.spatialRelation = new System.Windows.Forms.ComboBox();
            this.requiredSaveLayerAs = new System.Windows.Forms.Label();
            this.saveLayerAsLabel = new System.Windows.Forms.Label();
            this.saveLayerAs = new System.Windows.Forms.TextBox();
            this.selectAreaBtn = new System.Windows.Forms.Button();
            this.openGDBBtn = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.helpPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.helpPanelLogo = new System.Windows.Forms.PictureBox();
            this.knowledgeGraph = new System.Windows.Forms.ComboBox();
            this.runBtn = new System.Windows.Forms.Button();
            this.gdbFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).BeginInit();
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
            // requiredKnowledgeGraph
            // 
            this.requiredKnowledgeGraph.AutoSize = true;
            this.requiredKnowledgeGraph.BackColor = System.Drawing.Color.Transparent;
            this.requiredKnowledgeGraph.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredKnowledgeGraph.ForeColor = System.Drawing.Color.Red;
            this.requiredKnowledgeGraph.Location = new System.Drawing.Point(46, 166);
            this.requiredKnowledgeGraph.Name = "requiredKnowledgeGraph";
            this.requiredKnowledgeGraph.Size = new System.Drawing.Size(22, 29);
            this.requiredKnowledgeGraph.TabIndex = 15;
            this.requiredKnowledgeGraph.Text = "*";
            // 
            // knowledgeGraphLabel
            // 
            this.knowledgeGraphLabel.AutoSize = true;
            this.knowledgeGraphLabel.BackColor = System.Drawing.Color.Transparent;
            this.knowledgeGraphLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.knowledgeGraphLabel.ForeColor = System.Drawing.Color.White;
            this.knowledgeGraphLabel.Location = new System.Drawing.Point(61, 166);
            this.knowledgeGraphLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.knowledgeGraphLabel.Name = "knowledgeGraphLabel";
            this.knowledgeGraphLabel.Size = new System.Drawing.Size(211, 19);
            this.knowledgeGraphLabel.TabIndex = 16;
            this.knowledgeGraphLabel.Text = "Choose Knowledge Graph";
            // 
            // selectContentBtn
            // 
            this.selectContentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.selectContentBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectContentBtn.Enabled = false;
            this.selectContentBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.selectContentBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectContentBtn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.selectContentBtn.ForeColor = System.Drawing.Color.White;
            this.selectContentBtn.Location = new System.Drawing.Point(48, 232);
            this.selectContentBtn.Name = "selectContentBtn";
            this.selectContentBtn.Size = new System.Drawing.Size(157, 48);
            this.selectContentBtn.TabIndex = 20;
            this.selectContentBtn.Text = "SELECT CONTENT";
            this.selectContentBtn.UseVisualStyleBackColor = false;
            this.selectContentBtn.Click += new System.EventHandler(this.SelectContent);
            // 
            // spatialRelationLabel
            // 
            this.spatialRelationLabel.AutoSize = true;
            this.spatialRelationLabel.BackColor = System.Drawing.Color.Transparent;
            this.spatialRelationLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.spatialRelationLabel.ForeColor = System.Drawing.Color.White;
            this.spatialRelationLabel.Location = new System.Drawing.Point(293, 166);
            this.spatialRelationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.spatialRelationLabel.Name = "spatialRelationLabel";
            this.spatialRelationLabel.Size = new System.Drawing.Size(170, 19);
            this.spatialRelationLabel.TabIndex = 22;
            this.spatialRelationLabel.Text = "Spatial Relation Filter";
            // 
            // requiredSpatialRelation
            // 
            this.requiredSpatialRelation.AutoSize = true;
            this.requiredSpatialRelation.BackColor = System.Drawing.Color.Transparent;
            this.requiredSpatialRelation.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredSpatialRelation.ForeColor = System.Drawing.Color.Red;
            this.requiredSpatialRelation.Location = new System.Drawing.Point(279, 166);
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
            this.spatialRelation.Location = new System.Drawing.Point(278, 188);
            this.spatialRelation.Name = "spatialRelation";
            this.spatialRelation.Size = new System.Drawing.Size(224, 26);
            this.spatialRelation.TabIndex = 24;
            this.spatialRelation.SelectedIndexChanged += new System.EventHandler(this.OnChangeSpatialFilter);
            // 
            // requiredSaveLayerAs
            // 
            this.requiredSaveLayerAs.AutoSize = true;
            this.requiredSaveLayerAs.BackColor = System.Drawing.Color.Transparent;
            this.requiredSaveLayerAs.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredSaveLayerAs.ForeColor = System.Drawing.Color.Red;
            this.requiredSaveLayerAs.Location = new System.Drawing.Point(45, 322);
            this.requiredSaveLayerAs.Name = "requiredSaveLayerAs";
            this.requiredSaveLayerAs.Size = new System.Drawing.Size(22, 29);
            this.requiredSaveLayerAs.TabIndex = 25;
            this.requiredSaveLayerAs.Text = "*";
            // 
            // saveLayerAsLabel
            // 
            this.saveLayerAsLabel.AutoSize = true;
            this.saveLayerAsLabel.BackColor = System.Drawing.Color.Transparent;
            this.saveLayerAsLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.saveLayerAsLabel.ForeColor = System.Drawing.Color.White;
            this.saveLayerAsLabel.Location = new System.Drawing.Point(60, 322);
            this.saveLayerAsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.saveLayerAsLabel.Name = "saveLayerAsLabel";
            this.saveLayerAsLabel.Size = new System.Drawing.Size(179, 19);
            this.saveLayerAsLabel.TabIndex = 26;
            this.saveLayerAsLabel.Text = "Save Feature Layer As";
            // 
            // saveLayerAs
            // 
            this.saveLayerAs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.saveLayerAs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLayerAs.Location = new System.Drawing.Point(50, 344);
            this.saveLayerAs.Name = "saveLayerAs";
            this.saveLayerAs.Size = new System.Drawing.Size(780, 26);
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
            this.selectAreaBtn.Location = new System.Drawing.Point(671, 188);
            this.selectAreaBtn.Name = "selectAreaBtn";
            this.selectAreaBtn.Size = new System.Drawing.Size(157, 26);
            this.selectAreaBtn.TabIndex = 28;
            this.selectAreaBtn.Text = "SELECT AREA";
            this.selectAreaBtn.UseVisualStyleBackColor = false;
            this.selectAreaBtn.Click += new System.EventHandler(this.DrawAreaOfInterest);
            // 
            // openGDBBtn
            // 
            this.openGDBBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.openGDBBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.openGDBBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.openGDBBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openGDBBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.openGDBBtn.ForeColor = System.Drawing.Color.Black;
            this.openGDBBtn.Location = new System.Drawing.Point(508, 188);
            this.openGDBBtn.Name = "openGDBBtn";
            this.openGDBBtn.Size = new System.Drawing.Size(157, 26);
            this.openGDBBtn.TabIndex = 29;
            this.openGDBBtn.Text = "OPEN GDB FILE";
            this.openGDBBtn.UseVisualStyleBackColor = false;
            this.openGDBBtn.Click += new System.EventHandler(this.UploadGDBFile);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.Transparent;
            this.helpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.helpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.helpButton.FlatAppearance.BorderSize = 0;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Image = global::KWG_Geoenrichment.Properties.Resources.help_circle;
            this.helpButton.Location = new System.Drawing.Point(12, 506);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(74, 70);
            this.helpButton.TabIndex = 36;
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.ClickToggleHelpMenu);
            // 
            // helpPanel
            // 
            this.helpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(56)))), ((int)(((byte)(91)))));
            this.helpPanel.Controls.Add(this.label2);
            this.helpPanel.Controls.Add(this.label1);
            this.helpPanel.Controls.Add(this.helpPanelLogo);
            this.helpPanel.Location = new System.Drawing.Point(885, 51);
            this.helpPanel.Name = "helpPanel";
            this.helpPanel.Size = new System.Drawing.Size(377, 525);
            this.helpPanel.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 149);
            this.label2.MaximumSize = new System.Drawing.Size(325, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(319, 133);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(17, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose Knowledge Graph";
            // 
            // helpPanelLogo
            // 
            this.helpPanelLogo.Image = global::KWG_Geoenrichment.Properties.Resources.help_circle;
            this.helpPanelLogo.Location = new System.Drawing.Point(17, 3);
            this.helpPanelLogo.Name = "helpPanelLogo";
            this.helpPanelLogo.Size = new System.Drawing.Size(66, 64);
            this.helpPanelLogo.TabIndex = 0;
            this.helpPanelLogo.TabStop = false;
            // 
            // knowledgeGraph
            // 
            this.knowledgeGraph.Font = new System.Drawing.Font("Arial", 12F);
            this.knowledgeGraph.FormattingEnabled = true;
            this.knowledgeGraph.Location = new System.Drawing.Point(48, 188);
            this.knowledgeGraph.Name = "knowledgeGraph";
            this.knowledgeGraph.Size = new System.Drawing.Size(224, 26);
            this.knowledgeGraph.TabIndex = 38;
            this.knowledgeGraph.SelectedIndexChanged += new System.EventHandler(this.OnChangeGraph);
            // 
            // runBtn
            // 
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.runBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.runBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.runBtn.ForeColor = System.Drawing.Color.Black;
            this.runBtn.Location = new System.Drawing.Point(673, 506);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(157, 48);
            this.runBtn.TabIndex = 39;
            this.runBtn.Text = "RUN";
            this.runBtn.UseVisualStyleBackColor = false;
            // 
            // GeoenrichmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KWG_Geoenrichment.Properties.Resources.background_landing__2_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1284, 588);
            this.Controls.Add(this.spatialRelationLabel);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.knowledgeGraph);
            this.Controls.Add(this.helpPanel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.selectContentBtn);
            this.Controls.Add(this.selectAreaBtn);
            this.Controls.Add(this.openGDBBtn);
            this.Controls.Add(this.saveLayerAs);
            this.Controls.Add(this.saveLayerAsLabel);
            this.Controls.Add(this.requiredSaveLayerAs);
            this.Controls.Add(this.spatialRelation);
            this.Controls.Add(this.knowledgeGraphLabel);
            this.Controls.Add(this.requiredKnowledgeGraph);
            this.Controls.Add(this.geoFormName);
            this.Controls.Add(this.requiredSpatialRelation);
            this.DoubleBuffered = true;
            this.HelpButton = true;
            this.Name = "GeoenrichmentForm";
            this.Text = "KnowWhereGraph Geoenrichment";
            this.helpPanel.ResumeLayout(false);
            this.helpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label geoFormName;
        private System.Windows.Forms.Label requiredKnowledgeGraph;
        private System.Windows.Forms.Label knowledgeGraphLabel;
        private System.Windows.Forms.Button selectContentBtn;
        private System.Windows.Forms.Label spatialRelationLabel;
        private System.Windows.Forms.Label requiredSpatialRelation;
        private System.Windows.Forms.ComboBox spatialRelation;
        private System.Windows.Forms.Label requiredSaveLayerAs;
        private System.Windows.Forms.Label saveLayerAsLabel;
        private System.Windows.Forms.TextBox saveLayerAs;
        private System.Windows.Forms.Button selectAreaBtn;
        private System.Windows.Forms.Button openGDBBtn;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Panel helpPanel;
        private System.Windows.Forms.PictureBox helpPanelLogo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox knowledgeGraph;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.OpenFileDialog gdbFileDialog;
    }
}
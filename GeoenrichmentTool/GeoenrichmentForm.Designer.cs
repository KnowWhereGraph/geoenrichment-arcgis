
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
            this.requiredSaveLayerAs = new System.Windows.Forms.Label();
            this.saveLayerAsLabel = new System.Windows.Forms.Label();
            this.saveLayerAs = new System.Windows.Forms.TextBox();
            this.addLayerBtn = new System.Windows.Forms.Button();
            this.openLayerBtn = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.helpPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.helpPanelLogo = new System.Windows.Forms.PictureBox();
            this.knowledgeGraph = new System.Windows.Forms.ComboBox();
            this.runBtn = new System.Windows.Forms.Button();
            this.gdbFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contentLoading = new System.Windows.Forms.PictureBox();
            this.layerLoading = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.selectedLayer = new System.Windows.Forms.ComboBox();
            this.refreshLayersBtn = new System.Windows.Forms.Button();
            this.closeForm = new System.Windows.Forms.Button();
            this.helpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerLoading)).BeginInit();
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
            this.selectContentBtn.Location = new System.Drawing.Point(48, 293);
            this.selectContentBtn.Name = "selectContentBtn";
            this.selectContentBtn.Size = new System.Drawing.Size(157, 48);
            this.selectContentBtn.TabIndex = 20;
            this.selectContentBtn.Text = "SELECT CONTENT";
            this.selectContentBtn.UseVisualStyleBackColor = false;
            this.selectContentBtn.Click += new System.EventHandler(this.SelectContent);
            // 
            // requiredSaveLayerAs
            // 
            this.requiredSaveLayerAs.AutoSize = true;
            this.requiredSaveLayerAs.BackColor = System.Drawing.Color.Transparent;
            this.requiredSaveLayerAs.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.requiredSaveLayerAs.ForeColor = System.Drawing.Color.Red;
            this.requiredSaveLayerAs.Location = new System.Drawing.Point(46, 374);
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
            this.saveLayerAsLabel.Location = new System.Drawing.Point(61, 374);
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
            this.saveLayerAs.Location = new System.Drawing.Point(48, 399);
            this.saveLayerAs.Name = "saveLayerAs";
            this.saveLayerAs.Size = new System.Drawing.Size(780, 26);
            this.saveLayerAs.TabIndex = 27;
            this.saveLayerAs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnFeatureNameChage);
            // 
            // addLayerBtn
            // 
            this.addLayerBtn.BackColor = System.Drawing.Color.Transparent;
            this.addLayerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addLayerBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.addLayerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addLayerBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.addLayerBtn.ForeColor = System.Drawing.Color.Black;
            this.addLayerBtn.Image = global::KWG_Geoenrichment.Properties.Resources.plus;
            this.addLayerBtn.Location = new System.Drawing.Point(749, 241);
            this.addLayerBtn.Name = "addLayerBtn";
            this.addLayerBtn.Size = new System.Drawing.Size(36, 36);
            this.addLayerBtn.TabIndex = 28;
            this.addLayerBtn.UseVisualStyleBackColor = false;
            this.addLayerBtn.Click += new System.EventHandler(this.DrawAreaOfInterest);
            // 
            // openLayerBtn
            // 
            this.openLayerBtn.BackColor = System.Drawing.Color.Transparent;
            this.openLayerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.openLayerBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.openLayerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openLayerBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.openLayerBtn.ForeColor = System.Drawing.Color.Black;
            this.openLayerBtn.Image = global::KWG_Geoenrichment.Properties.Resources.file;
            this.openLayerBtn.Location = new System.Drawing.Point(791, 241);
            this.openLayerBtn.Name = "openLayerBtn";
            this.openLayerBtn.Size = new System.Drawing.Size(36, 36);
            this.openLayerBtn.TabIndex = 29;
            this.openLayerBtn.UseVisualStyleBackColor = false;
            this.openLayerBtn.Click += new System.EventHandler(this.UploadLayer);
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
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.MaximumSize = new System.Drawing.Size(325, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 380);
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
            this.knowledgeGraph.Size = new System.Drawing.Size(780, 26);
            this.knowledgeGraph.TabIndex = 38;
            this.knowledgeGraph.SelectedIndexChanged += new System.EventHandler(this.OnChangeGraph);
            // 
            // runBtn
            // 
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.runBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.runBtn.Enabled = false;
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
            this.runBtn.Click += new System.EventHandler(this.RunGeoenrichment);
            // 
            // contentLoading
            // 
            this.contentLoading.BackColor = System.Drawing.Color.Transparent;
            this.contentLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contentLoading.Image = global::KWG_Geoenrichment.Properties.Resources.loading;
            this.contentLoading.Location = new System.Drawing.Point(211, 293);
            this.contentLoading.Name = "contentLoading";
            this.contentLoading.Size = new System.Drawing.Size(50, 50);
            this.contentLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.contentLoading.TabIndex = 40;
            this.contentLoading.TabStop = false;
            this.contentLoading.Visible = false;
            // 
            // layerLoading
            // 
            this.layerLoading.BackColor = System.Drawing.Color.Transparent;
            this.layerLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.layerLoading.Image = global::KWG_Geoenrichment.Properties.Resources.loading;
            this.layerLoading.Location = new System.Drawing.Point(617, 504);
            this.layerLoading.Name = "layerLoading";
            this.layerLoading.Size = new System.Drawing.Size(50, 50);
            this.layerLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.layerLoading.TabIndex = 41;
            this.layerLoading.TabStop = false;
            this.layerLoading.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(61, 224);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 19);
            this.label5.TabIndex = 43;
            this.label5.Text = "Select Feature Layer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(46, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 29);
            this.label6.TabIndex = 42;
            this.label6.Text = "*";
            // 
            // selectedLayer
            // 
            this.selectedLayer.Font = new System.Drawing.Font("Arial", 12F);
            this.selectedLayer.FormattingEnabled = true;
            this.selectedLayer.Location = new System.Drawing.Point(51, 247);
            this.selectedLayer.Name = "selectedLayer";
            this.selectedLayer.Size = new System.Drawing.Size(650, 26);
            this.selectedLayer.TabIndex = 44;
            this.selectedLayer.SelectedIndexChanged += new System.EventHandler(this.OnChangeLayer);
            // 
            // refreshLayersBtn
            // 
            this.refreshLayersBtn.BackColor = System.Drawing.Color.Transparent;
            this.refreshLayersBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.refreshLayersBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.refreshLayersBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshLayersBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.refreshLayersBtn.ForeColor = System.Drawing.Color.Black;
            this.refreshLayersBtn.Image = global::KWG_Geoenrichment.Properties.Resources.refresh;
            this.refreshLayersBtn.Location = new System.Drawing.Point(707, 241);
            this.refreshLayersBtn.Name = "refreshLayersBtn";
            this.refreshLayersBtn.Size = new System.Drawing.Size(36, 36);
            this.refreshLayersBtn.TabIndex = 45;
            this.refreshLayersBtn.UseVisualStyleBackColor = false;
            this.refreshLayersBtn.Click += new System.EventHandler(this.RefreshLayerList);
            // 
            // closeForm
            // 
            this.closeForm.BackColor = System.Drawing.Color.Transparent;
            this.closeForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.closeForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.closeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeForm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.closeForm.ForeColor = System.Drawing.Color.Black;
            this.closeForm.Image = global::KWG_Geoenrichment.Properties.Resources.x;
            this.closeForm.Location = new System.Drawing.Point(12, 12);
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(36, 36);
            this.closeForm.TabIndex = 46;
            this.closeForm.UseVisualStyleBackColor = false;
            this.closeForm.Click += new System.EventHandler(this.CloseWindow);
            // 
            // GeoenrichmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KWG_Geoenrichment.Properties.Resources.background_landing__2_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1284, 588);
            this.ControlBox = false;
            this.Controls.Add(this.closeForm);
            this.Controls.Add(this.refreshLayersBtn);
            this.Controls.Add(this.selectedLayer);
            this.Controls.Add(this.openLayerBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.layerLoading);
            this.Controls.Add(this.contentLoading);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.knowledgeGraph);
            this.Controls.Add(this.helpPanel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.selectContentBtn);
            this.Controls.Add(this.addLayerBtn);
            this.Controls.Add(this.saveLayerAs);
            this.Controls.Add(this.saveLayerAsLabel);
            this.Controls.Add(this.requiredSaveLayerAs);
            this.Controls.Add(this.knowledgeGraphLabel);
            this.Controls.Add(this.requiredKnowledgeGraph);
            this.Controls.Add(this.geoFormName);
            this.DoubleBuffered = true;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeoenrichmentForm";
            this.Text = "KnowWhereGraph Geoenrichment";
            this.helpPanel.ResumeLayout(false);
            this.helpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label geoFormName;
        private System.Windows.Forms.Label requiredKnowledgeGraph;
        private System.Windows.Forms.Label knowledgeGraphLabel;
        private System.Windows.Forms.Button selectContentBtn;
        private System.Windows.Forms.Label requiredSaveLayerAs;
        private System.Windows.Forms.Label saveLayerAsLabel;
        private System.Windows.Forms.TextBox saveLayerAs;
        private System.Windows.Forms.Button addLayerBtn;
        private System.Windows.Forms.Button openLayerBtn;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Panel helpPanel;
        private System.Windows.Forms.PictureBox helpPanelLogo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox knowledgeGraph;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.OpenFileDialog gdbFileDialog;
        private System.Windows.Forms.PictureBox contentLoading;
        private System.Windows.Forms.PictureBox layerLoading;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox selectedLayer;
        private System.Windows.Forms.Button refreshLayersBtn;
        private System.Windows.Forms.Button closeForm;
    }
}

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
            geoFormName = new System.Windows.Forms.Label();
            requiredKnowledgeGraph = new System.Windows.Forms.Label();
            knowledgeGraphLabel = new System.Windows.Forms.Label();
            requiredSaveLayerAs = new System.Windows.Forms.Label();
            saveLayerAsLabel = new System.Windows.Forms.Label();
            saveLayerAs = new System.Windows.Forms.TextBox();
            addLayerBtn = new System.Windows.Forms.Button();
            openLayerBtn = new System.Windows.Forms.Button();
            helpButton = new System.Windows.Forms.Button();
            knowledgeGraph = new System.Windows.Forms.ComboBox();
            runBtn = new System.Windows.Forms.Button();
            gdbFileDialog = new System.Windows.Forms.OpenFileDialog();
            contentLoading = new System.Windows.Forms.PictureBox();
            layerLoading = new System.Windows.Forms.PictureBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            selectedLayer = new System.Windows.Forms.ComboBox();
            refreshLayersBtn = new System.Windows.Forms.Button();
            closeForm = new System.Windows.Forms.Button();
            helpProvider1 = new System.Windows.Forms.HelpProvider();
            featuresOfInterest = new System.Windows.Forms.ComboBox();
            featuresOfInterestLabel = new System.Windows.Forms.Label();
            requiredFeaturesOfInterest = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)contentLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layerLoading).BeginInit();
            SuspendLayout();
            // 
            // geoFormName
            // 
            geoFormName.AutoSize = true;
            geoFormName.BackColor = System.Drawing.Color.Transparent;
            geoFormName.Font = new System.Drawing.Font("Arial", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            geoFormName.ForeColor = System.Drawing.Color.White;
            geoFormName.Location = new System.Drawing.Point(50, 60);
            geoFormName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            geoFormName.Name = "geoFormName";
            geoFormName.Size = new System.Drawing.Size(774, 54);
            geoFormName.TabIndex = 3;
            geoFormName.Text = "KnowWhereGraph Geoenrichment";
            // 
            // requiredKnowledgeGraph
            // 
            requiredKnowledgeGraph.AutoSize = true;
            requiredKnowledgeGraph.BackColor = System.Drawing.Color.Transparent;
            requiredKnowledgeGraph.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            requiredKnowledgeGraph.ForeColor = System.Drawing.Color.Red;
            requiredKnowledgeGraph.Location = new System.Drawing.Point(50, 150);
            requiredKnowledgeGraph.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            requiredKnowledgeGraph.Name = "requiredKnowledgeGraph";
            requiredKnowledgeGraph.Size = new System.Drawing.Size(22, 29);
            requiredKnowledgeGraph.TabIndex = 15;
            requiredKnowledgeGraph.Text = "*";
            // 
            // knowledgeGraphLabel
            // 
            knowledgeGraphLabel.AutoSize = true;
            knowledgeGraphLabel.BackColor = System.Drawing.Color.Transparent;
            knowledgeGraphLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            knowledgeGraphLabel.ForeColor = System.Drawing.Color.White;
            knowledgeGraphLabel.Location = new System.Drawing.Point(70, 150);
            knowledgeGraphLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            knowledgeGraphLabel.Name = "knowledgeGraphLabel";
            knowledgeGraphLabel.Size = new System.Drawing.Size(211, 19);
            knowledgeGraphLabel.TabIndex = 16;
            knowledgeGraphLabel.Text = "Choose Knowledge Graph";
            // 
            // requiredSaveLayerAs
            // 
            requiredSaveLayerAs.AutoSize = true;
            requiredSaveLayerAs.BackColor = System.Drawing.Color.Transparent;
            requiredSaveLayerAs.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            requiredSaveLayerAs.ForeColor = System.Drawing.Color.Red;
            requiredSaveLayerAs.Location = new System.Drawing.Point(50, 430);
            requiredSaveLayerAs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            requiredSaveLayerAs.Name = "requiredSaveLayerAs";
            requiredSaveLayerAs.Size = new System.Drawing.Size(22, 29);
            requiredSaveLayerAs.TabIndex = 25;
            requiredSaveLayerAs.Text = "*";
            // 
            // saveLayerAsLabel
            // 
            saveLayerAsLabel.AutoSize = true;
            saveLayerAsLabel.BackColor = System.Drawing.Color.Transparent;
            saveLayerAsLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            saveLayerAsLabel.ForeColor = System.Drawing.Color.White;
            saveLayerAsLabel.Location = new System.Drawing.Point(70, 430);
            saveLayerAsLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            saveLayerAsLabel.Name = "saveLayerAsLabel";
            saveLayerAsLabel.Size = new System.Drawing.Size(179, 19);
            saveLayerAsLabel.TabIndex = 26;
            saveLayerAsLabel.Text = "Save Feature Layer As";
            // 
            // saveLayerAs
            // 
            saveLayerAs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            saveLayerAs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            saveLayerAs.Location = new System.Drawing.Point(50, 460);
            saveLayerAs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            saveLayerAs.Name = "saveLayerAs";
            saveLayerAs.Size = new System.Drawing.Size(900, 26);
            saveLayerAs.TabIndex = 27;
            saveLayerAs.KeyUp += OnFeatureNameChage;
            // 
            // addLayerBtn
            // 
            addLayerBtn.BackColor = System.Drawing.Color.Transparent;
            addLayerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            addLayerBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            addLayerBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            addLayerBtn.FlatAppearance.BorderSize = 0;
            addLayerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            addLayerBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            addLayerBtn.ForeColor = System.Drawing.Color.Black;
            addLayerBtn.Image = Properties.Resources.plus;
            addLayerBtn.Location = new System.Drawing.Point(859, 241);
            addLayerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            addLayerBtn.Name = "addLayerBtn";
            addLayerBtn.Size = new System.Drawing.Size(42, 42);
            addLayerBtn.TabIndex = 28;
            addLayerBtn.UseVisualStyleBackColor = false;
            addLayerBtn.Click += DrawAreaOfInterest;
            // 
            // openLayerBtn
            // 
            openLayerBtn.BackColor = System.Drawing.Color.Transparent;
            openLayerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            openLayerBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            openLayerBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            openLayerBtn.FlatAppearance.BorderSize = 0;
            openLayerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            openLayerBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            openLayerBtn.ForeColor = System.Drawing.Color.Black;
            openLayerBtn.Image = Properties.Resources.file;
            openLayerBtn.Location = new System.Drawing.Point(908, 241);
            openLayerBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            openLayerBtn.Name = "openLayerBtn";
            openLayerBtn.Size = new System.Drawing.Size(42, 42);
            openLayerBtn.TabIndex = 29;
            openLayerBtn.UseVisualStyleBackColor = false;
            openLayerBtn.Click += UploadLayer;
            // 
            // helpButton
            // 
            helpButton.BackColor = System.Drawing.Color.Transparent;
            helpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            helpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            helpButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            helpButton.FlatAppearance.BorderSize = 0;
            helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            helpButton.Image = Properties.Resources.help_circle;
            helpButton.Location = new System.Drawing.Point(50, 528);
            helpButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            helpButton.Name = "helpButton";
            helpButton.Size = new System.Drawing.Size(86, 81);
            helpButton.TabIndex = 36;
            helpButton.UseVisualStyleBackColor = false;
            helpButton.Click += ClickToggleHelpMenu;
            // 
            // knowledgeGraph
            // 
            knowledgeGraph.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            knowledgeGraph.FormattingEnabled = true;
            knowledgeGraph.Location = new System.Drawing.Point(50, 180);
            knowledgeGraph.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            knowledgeGraph.Name = "knowledgeGraph";
            knowledgeGraph.Size = new System.Drawing.Size(900, 26);
            knowledgeGraph.TabIndex = 38;
            knowledgeGraph.SelectedIndexChanged += OnChangeGraph;
            // 
            // runBtn
            // 
            runBtn.BackColor = System.Drawing.Color.FromArgb(66, 214, 237);
            runBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            runBtn.Enabled = false;
            runBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            runBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            runBtn.ForeColor = System.Drawing.Color.Black;
            runBtn.Location = new System.Drawing.Point(767, 550);
            runBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            runBtn.Name = "runBtn";
            runBtn.Size = new System.Drawing.Size(183, 55);
            runBtn.TabIndex = 39;
            runBtn.Text = "RUN";
            runBtn.UseVisualStyleBackColor = false;
            runBtn.Click += RunGeoenrichment;
            // 
            // contentLoading
            // 
            contentLoading.BackColor = System.Drawing.Color.Transparent;
            contentLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            contentLoading.Image = Properties.Resources.loading;
            contentLoading.Location = new System.Drawing.Point(296, 284);
            contentLoading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            contentLoading.Name = "contentLoading";
            contentLoading.Size = new System.Drawing.Size(30, 30);
            contentLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            contentLoading.TabIndex = 40;
            contentLoading.TabStop = false;
            contentLoading.Visible = false;
            // 
            // layerLoading
            // 
            layerLoading.BackColor = System.Drawing.Color.Transparent;
            layerLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            layerLoading.Image = Properties.Resources.loading;
            layerLoading.Location = new System.Drawing.Point(699, 549);
            layerLoading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            layerLoading.Name = "layerLoading";
            layerLoading.Size = new System.Drawing.Size(60, 60);
            layerLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            layerLoading.TabIndex = 41;
            layerLoading.TabStop = false;
            layerLoading.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = System.Drawing.Color.Transparent;
            label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(70, 220);
            label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(233, 19);
            label5.TabIndex = 43;
            label5.Text = "Select Polygon Feature Layer";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = System.Drawing.Color.Transparent;
            label6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label6.ForeColor = System.Drawing.Color.Red;
            label6.Location = new System.Drawing.Point(50, 220);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(22, 29);
            label6.TabIndex = 42;
            label6.Text = "*";
            // 
            // selectedLayer
            // 
            selectedLayer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            selectedLayer.FormattingEnabled = true;
            selectedLayer.Location = new System.Drawing.Point(50, 250);
            selectedLayer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            selectedLayer.Name = "selectedLayer";
            selectedLayer.Size = new System.Drawing.Size(752, 26);
            selectedLayer.TabIndex = 44;
            selectedLayer.SelectedIndexChanged += OnChangeLayer;
            // 
            // refreshLayersBtn
            // 
            refreshLayersBtn.BackColor = System.Drawing.Color.Transparent;
            refreshLayersBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            refreshLayersBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            refreshLayersBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            refreshLayersBtn.FlatAppearance.BorderSize = 0;
            refreshLayersBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            refreshLayersBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            refreshLayersBtn.ForeColor = System.Drawing.Color.Black;
            refreshLayersBtn.Image = Properties.Resources.refresh;
            refreshLayersBtn.Location = new System.Drawing.Point(810, 241);
            refreshLayersBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            refreshLayersBtn.Name = "refreshLayersBtn";
            refreshLayersBtn.Size = new System.Drawing.Size(42, 42);
            refreshLayersBtn.TabIndex = 45;
            refreshLayersBtn.UseVisualStyleBackColor = false;
            refreshLayersBtn.Click += RefreshLayerList;
            // 
            // closeForm
            // 
            closeForm.BackColor = System.Drawing.Color.Transparent;
            closeForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            closeForm.Cursor = System.Windows.Forms.Cursors.Hand;
            closeForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            closeForm.FlatAppearance.BorderSize = 0;
            closeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            closeForm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            closeForm.ForeColor = System.Drawing.Color.Black;
            closeForm.Image = Properties.Resources.x;
            closeForm.Location = new System.Drawing.Point(15, 15);
            closeForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            closeForm.Name = "closeForm";
            closeForm.Size = new System.Drawing.Size(40, 40);
            closeForm.TabIndex = 46;
            closeForm.UseVisualStyleBackColor = false;
            closeForm.Click += CloseWindow;
            // 
            // featuresOfInterest
            // 
            featuresOfInterest.DisplayMember = "Value";
            featuresOfInterest.Enabled = false;
            featuresOfInterest.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            featuresOfInterest.FormattingEnabled = true;
            featuresOfInterest.Location = new System.Drawing.Point(50, 320);
            featuresOfInterest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            featuresOfInterest.Name = "featuresOfInterest";
            featuresOfInterest.Size = new System.Drawing.Size(900, 26);
            featuresOfInterest.TabIndex = 49;
            featuresOfInterest.ValueMember = "Key";
            featuresOfInterest.SelectedIndexChanged += OnSelectFeature;
            // 
            // featuresOfInterestLabel
            // 
            featuresOfInterestLabel.AutoSize = true;
            featuresOfInterestLabel.BackColor = System.Drawing.Color.Transparent;
            featuresOfInterestLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            featuresOfInterestLabel.ForeColor = System.Drawing.Color.White;
            featuresOfInterestLabel.Location = new System.Drawing.Point(70, 290);
            featuresOfInterestLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            featuresOfInterestLabel.Name = "featuresOfInterestLabel";
            featuresOfInterestLabel.Size = new System.Drawing.Size(217, 19);
            featuresOfInterestLabel.TabIndex = 48;
            featuresOfInterestLabel.Text = "Select Feature(s) of Interest";
            // 
            // requiredFeaturesOfInterest
            // 
            requiredFeaturesOfInterest.AutoSize = true;
            requiredFeaturesOfInterest.BackColor = System.Drawing.Color.Transparent;
            requiredFeaturesOfInterest.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            requiredFeaturesOfInterest.ForeColor = System.Drawing.Color.Red;
            requiredFeaturesOfInterest.Location = new System.Drawing.Point(50, 290);
            requiredFeaturesOfInterest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            requiredFeaturesOfInterest.Name = "requiredFeaturesOfInterest";
            requiredFeaturesOfInterest.Size = new System.Drawing.Size(22, 29);
            requiredFeaturesOfInterest.TabIndex = 47;
            requiredFeaturesOfInterest.Text = "*";
            // 
            // GeoenrichmentForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackgroundImage = Properties.Resources.background_landing__2_1;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(984, 621);
            ControlBox = false;
            Controls.Add(featuresOfInterest);
            Controls.Add(featuresOfInterestLabel);
            Controls.Add(requiredFeaturesOfInterest);
            Controls.Add(closeForm);
            Controls.Add(refreshLayersBtn);
            Controls.Add(selectedLayer);
            Controls.Add(openLayerBtn);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(layerLoading);
            Controls.Add(contentLoading);
            Controls.Add(runBtn);
            Controls.Add(knowledgeGraph);
            Controls.Add(helpButton);
            Controls.Add(addLayerBtn);
            Controls.Add(saveLayerAs);
            Controls.Add(saveLayerAsLabel);
            Controls.Add(requiredSaveLayerAs);
            Controls.Add(knowledgeGraphLabel);
            Controls.Add(requiredKnowledgeGraph);
            Controls.Add(geoFormName);
            DoubleBuffered = true;
            HelpButton = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GeoenrichmentForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "KnowWhereGraph Geoenrichment";
            ((System.ComponentModel.ISupportInitialize)contentLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)layerLoading).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label geoFormName;
        private System.Windows.Forms.Label requiredKnowledgeGraph;
        private System.Windows.Forms.Label knowledgeGraphLabel;
        private System.Windows.Forms.Label requiredSaveLayerAs;
        private System.Windows.Forms.Label saveLayerAsLabel;
        private System.Windows.Forms.TextBox saveLayerAs;
        private System.Windows.Forms.Button addLayerBtn;
        private System.Windows.Forms.Button openLayerBtn;
        private System.Windows.Forms.Button helpButton;
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
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.ComboBox featuresOfInterest;
        private System.Windows.Forms.Label featuresOfInterestLabel;
        private System.Windows.Forms.Label requiredFeaturesOfInterest;
    }
}
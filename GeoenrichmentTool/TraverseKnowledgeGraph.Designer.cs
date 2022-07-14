
namespace KWG_Geoenrichment
{
    partial class TraverseKnowledgeGraph
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
            this.traverseGraph = new System.Windows.Forms.Label();
            this.prop1Req = new System.Windows.Forms.Label();
            this.object1 = new System.Windows.Forms.ComboBox();
            this.prop1Label = new System.Windows.Forms.Label();
            this.runTraverseBtn = new System.Windows.Forms.Button();
            this.addPropertyBtn = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.subject1 = new System.Windows.Forms.ComboBox();
            this.predicate1 = new System.Windows.Forms.ComboBox();
            this.edgeLoading = new System.Windows.Forms.PictureBox();
            this.closeForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.edgeLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // traverseGraph
            // 
            this.traverseGraph.AutoSize = true;
            this.traverseGraph.BackColor = System.Drawing.Color.Transparent;
            this.traverseGraph.Font = new System.Drawing.Font("Arial", 34F, System.Drawing.FontStyle.Bold);
            this.traverseGraph.ForeColor = System.Drawing.Color.White;
            this.traverseGraph.Location = new System.Drawing.Point(41, 51);
            this.traverseGraph.Name = "traverseGraph";
            this.traverseGraph.Size = new System.Drawing.Size(598, 54);
            this.traverseGraph.TabIndex = 5;
            this.traverseGraph.Text = "Explore Knowledge Graph";
            // 
            // prop1Req
            // 
            this.prop1Req.AutoSize = true;
            this.prop1Req.BackColor = System.Drawing.Color.Transparent;
            this.prop1Req.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.prop1Req.ForeColor = System.Drawing.Color.Red;
            this.prop1Req.Location = new System.Drawing.Point(45, 166);
            this.prop1Req.Name = "prop1Req";
            this.prop1Req.Size = new System.Drawing.Size(22, 29);
            this.prop1Req.TabIndex = 31;
            this.prop1Req.Text = "*";
            // 
            // object1
            // 
            this.object1.DisplayMember = "Value";
            this.object1.Enabled = false;
            this.object1.Font = new System.Drawing.Font("Arial", 12F);
            this.object1.FormattingEnabled = true;
            this.object1.Location = new System.Drawing.Point(574, 197);
            this.object1.Name = "object1";
            this.object1.Size = new System.Drawing.Size(256, 26);
            this.object1.TabIndex = 30;
            this.object1.ValueMember = "Key";
            this.object1.SelectedIndexChanged += new System.EventHandler(this.OnValueBoxChange);
            // 
            // prop1Label
            // 
            this.prop1Label.AutoSize = true;
            this.prop1Label.BackColor = System.Drawing.Color.Transparent;
            this.prop1Label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.prop1Label.ForeColor = System.Drawing.Color.White;
            this.prop1Label.Location = new System.Drawing.Point(60, 166);
            this.prop1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.prop1Label.Name = "prop1Label";
            this.prop1Label.Size = new System.Drawing.Size(198, 19);
            this.prop1Label.TabIndex = 29;
            this.prop1Label.Text = "Select Feature of Interest";
            // 
            // runTraverseBtn
            // 
            this.runTraverseBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.runTraverseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.runTraverseBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.runTraverseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runTraverseBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.runTraverseBtn.ForeColor = System.Drawing.Color.Black;
            this.runTraverseBtn.Location = new System.Drawing.Point(669, 396);
            this.runTraverseBtn.Name = "runTraverseBtn";
            this.runTraverseBtn.Size = new System.Drawing.Size(161, 63);
            this.runTraverseBtn.TabIndex = 32;
            this.runTraverseBtn.Text = "ADD CONTENT";
            this.runTraverseBtn.UseVisualStyleBackColor = false;
            this.runTraverseBtn.Click += new System.EventHandler(this.RunTraverseGraph);
            // 
            // addPropertyBtn
            // 
            this.addPropertyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.addPropertyBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addPropertyBtn.Enabled = false;
            this.addPropertyBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.addPropertyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addPropertyBtn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.addPropertyBtn.ForeColor = System.Drawing.Color.White;
            this.addPropertyBtn.Location = new System.Drawing.Point(50, 229);
            this.addPropertyBtn.Name = "addPropertyBtn";
            this.addPropertyBtn.Size = new System.Drawing.Size(195, 26);
            this.addPropertyBtn.TabIndex = 33;
            this.addPropertyBtn.Text = "EXPLORE FURTHER";
            this.addPropertyBtn.UseVisualStyleBackColor = false;
            this.addPropertyBtn.Click += new System.EventHandler(this.LearnMore);
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
            this.helpButton.Location = new System.Drawing.Point(12, 409);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(74, 70);
            this.helpButton.TabIndex = 37;
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.ClickToggleHelpMenu);
            // 
            // subject1
            // 
            this.subject1.DisplayMember = "Value";
            this.subject1.Enabled = false;
            this.subject1.Font = new System.Drawing.Font("Arial", 12F);
            this.subject1.FormattingEnabled = true;
            this.subject1.Location = new System.Drawing.Point(50, 197);
            this.subject1.Name = "subject1";
            this.subject1.Size = new System.Drawing.Size(256, 26);
            this.subject1.TabIndex = 39;
            this.subject1.ValueMember = "Key";
            this.subject1.SelectedIndexChanged += new System.EventHandler(this.OnClassBoxChange);
            // 
            // predicate1
            // 
            this.predicate1.DisplayMember = "Value";
            this.predicate1.Enabled = false;
            this.predicate1.Font = new System.Drawing.Font("Arial", 12F);
            this.predicate1.FormattingEnabled = true;
            this.predicate1.Location = new System.Drawing.Point(312, 197);
            this.predicate1.Name = "predicate1";
            this.predicate1.Size = new System.Drawing.Size(256, 26);
            this.predicate1.TabIndex = 40;
            this.predicate1.ValueMember = "Key";
            this.predicate1.SelectedIndexChanged += new System.EventHandler(this.OnPropBoxChange);
            // 
            // edgeLoading
            // 
            this.edgeLoading.BackColor = System.Drawing.Color.Transparent;
            this.edgeLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.edgeLoading.Image = global::KWG_Geoenrichment.Properties.Resources.loading;
            this.edgeLoading.Location = new System.Drawing.Point(265, 141);
            this.edgeLoading.Name = "edgeLoading";
            this.edgeLoading.Size = new System.Drawing.Size(50, 50);
            this.edgeLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.edgeLoading.TabIndex = 41;
            this.edgeLoading.TabStop = false;
            this.edgeLoading.Visible = false;
            // 
            // closeForm
            // 
            this.closeForm.BackColor = System.Drawing.Color.Transparent;
            this.closeForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.closeForm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.closeForm.FlatAppearance.BorderSize = 0;
            this.closeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeForm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.closeForm.ForeColor = System.Drawing.Color.Black;
            this.closeForm.Image = global::KWG_Geoenrichment.Properties.Resources.x;
            this.closeForm.Location = new System.Drawing.Point(12, 12);
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(36, 36);
            this.closeForm.TabIndex = 47;
            this.closeForm.UseVisualStyleBackColor = false;
            this.closeForm.Click += new System.EventHandler(this.CloseWindow);
            // 
            // TraverseKnowledgeGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KWG_Geoenrichment.Properties.Resources.background_landing__2_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(892, 491);
            this.ControlBox = false;
            this.Controls.Add(this.closeForm);
            this.Controls.Add(this.edgeLoading);
            this.Controls.Add(this.predicate1);
            this.Controls.Add(this.subject1);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.addPropertyBtn);
            this.Controls.Add(this.runTraverseBtn);
            this.Controls.Add(this.prop1Req);
            this.Controls.Add(this.object1);
            this.Controls.Add(this.prop1Label);
            this.Controls.Add(this.traverseGraph);
            this.DoubleBuffered = true;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TraverseKnowledgeGraph";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Explore Knowledge Graph";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TraverseKnowledgeGraph_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.edgeLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label traverseGraph;
        private System.Windows.Forms.Label prop1Req;
        private System.Windows.Forms.ComboBox object1;
        private System.Windows.Forms.Label prop1Label;
        private System.Windows.Forms.Button runTraverseBtn;
        private System.Windows.Forms.Button addPropertyBtn;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.ComboBox subject1;
        private System.Windows.Forms.ComboBox predicate1;
        private System.Windows.Forms.PictureBox edgeLoading;
        private System.Windows.Forms.Button closeForm;
    }
}
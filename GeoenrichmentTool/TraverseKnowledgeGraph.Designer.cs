
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraverseKnowledgeGraph));
            this.traverseGraph = new System.Windows.Forms.Label();
            this.prop1Req = new System.Windows.Forms.Label();
            this.object1 = new System.Windows.Forms.ComboBox();
            this.prop1Label = new System.Windows.Forms.Label();
            this.runTraverseBtn = new System.Windows.Forms.Button();
            this.addPropertyBtn = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.helpPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.helpPanelLogo = new System.Windows.Forms.PictureBox();
            this.subject1 = new System.Windows.Forms.ComboBox();
            this.predicate1 = new System.Windows.Forms.ComboBox();
            this.helpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).BeginInit();
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
            this.object1.Enabled = false;
            this.object1.Font = new System.Drawing.Font("Arial", 12F);
            this.object1.FormattingEnabled = true;
            this.object1.Location = new System.Drawing.Point(574, 197);
            this.object1.Name = "object1";
            this.object1.Size = new System.Drawing.Size(256, 26);
            this.object1.TabIndex = 30;
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
            this.prop1Label.Size = new System.Drawing.Size(121, 19);
            this.prop1Label.TabIndex = 29;
            this.prop1Label.Text = "Select Content";
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
            this.runTraverseBtn.Text = "RUN";
            this.runTraverseBtn.UseVisualStyleBackColor = false;
            this.runTraverseBtn.Click += new System.EventHandler(this.RunTraverseGraph);
            // 
            // addPropertyBtn
            // 
            this.addPropertyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.addPropertyBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addPropertyBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.addPropertyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addPropertyBtn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.addPropertyBtn.ForeColor = System.Drawing.Color.White;
            this.addPropertyBtn.Location = new System.Drawing.Point(50, 229);
            this.addPropertyBtn.Name = "addPropertyBtn";
            this.addPropertyBtn.Size = new System.Drawing.Size(195, 26);
            this.addPropertyBtn.TabIndex = 33;
            this.addPropertyBtn.Text = "LEARN MORE";
            this.addPropertyBtn.UseVisualStyleBackColor = false;
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
            // helpPanel
            // 
            this.helpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(56)))), ((int)(((byte)(91)))));
            this.helpPanel.Controls.Add(this.label2);
            this.helpPanel.Controls.Add(this.label1);
            this.helpPanel.Controls.Add(this.helpPanelLogo);
            this.helpPanel.Location = new System.Drawing.Point(885, 51);
            this.helpPanel.Name = "helpPanel";
            this.helpPanel.Size = new System.Drawing.Size(377, 408);
            this.helpPanel.TabIndex = 38;
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
            this.label1.Location = new System.Drawing.Point(12, 115);
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
            // subject1
            // 
            this.subject1.Enabled = false;
            this.subject1.Font = new System.Drawing.Font("Arial", 12F);
            this.subject1.FormattingEnabled = true;
            this.subject1.Location = new System.Drawing.Point(50, 197);
            this.subject1.Name = "subject1";
            this.subject1.Size = new System.Drawing.Size(256, 26);
            this.subject1.TabIndex = 39;
            // 
            // predicate1
            // 
            this.predicate1.Enabled = false;
            this.predicate1.Font = new System.Drawing.Font("Arial", 12F);
            this.predicate1.FormattingEnabled = true;
            this.predicate1.Location = new System.Drawing.Point(312, 197);
            this.predicate1.Name = "predicate1";
            this.predicate1.Size = new System.Drawing.Size(256, 26);
            this.predicate1.TabIndex = 40;
            // 
            // TraverseKnowledgeGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KWG_Geoenrichment.Properties.Resources.background_landing__2_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1284, 491);
            this.Controls.Add(this.predicate1);
            this.Controls.Add(this.subject1);
            this.Controls.Add(this.helpPanel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.addPropertyBtn);
            this.Controls.Add(this.runTraverseBtn);
            this.Controls.Add(this.prop1Req);
            this.Controls.Add(this.object1);
            this.Controls.Add(this.prop1Label);
            this.Controls.Add(this.traverseGraph);
            this.DoubleBuffered = true;
            this.Name = "TraverseKnowledgeGraph";
            this.Text = "Traverse Knowledge Graph";
            this.helpPanel.ResumeLayout(false);
            this.helpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).EndInit();
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
        private System.Windows.Forms.Panel helpPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox helpPanelLogo;
        private System.Windows.Forms.ComboBox subject1;
        private System.Windows.Forms.ComboBox predicate1;
    }
}
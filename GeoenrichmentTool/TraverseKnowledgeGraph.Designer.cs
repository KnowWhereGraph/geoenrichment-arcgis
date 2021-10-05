
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
            this.prop1 = new System.Windows.Forms.ComboBox();
            this.prop1Label = new System.Windows.Forms.Label();
            this.runTraverseBtn = new System.Windows.Forms.Button();
            this.addPropertyBtn = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
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
            this.traverseGraph.Size = new System.Drawing.Size(619, 54);
            this.traverseGraph.TabIndex = 5;
            this.traverseGraph.Text = "Traverse Knowledge Graph";
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
            // prop1
            // 
            this.prop1.Enabled = false;
            this.prop1.Font = new System.Drawing.Font("Arial", 12F);
            this.prop1.FormattingEnabled = true;
            this.prop1.Location = new System.Drawing.Point(50, 198);
            this.prop1.Name = "prop1";
            this.prop1.Size = new System.Drawing.Size(765, 26);
            this.prop1.TabIndex = 30;
            this.prop1.SelectedValueChanged += new System.EventHandler(this.PropertyChanged);
            // 
            // prop1Label
            // 
            this.prop1Label.AutoSize = true;
            this.prop1Label.BackColor = System.Drawing.Color.Transparent;
            this.prop1Label.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.prop1Label.ForeColor = System.Drawing.Color.White;
            this.prop1Label.Location = new System.Drawing.Point(60, 166);
            this.prop1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.prop1Label.Name = "prop1Label";
            this.prop1Label.Size = new System.Drawing.Size(288, 29);
            this.prop1Label.TabIndex = 29;
            this.prop1Label.Text = "Keep Exploring Content";
            // 
            // runTraverseBtn
            // 
            this.runTraverseBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.runTraverseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.runTraverseBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.runTraverseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runTraverseBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.runTraverseBtn.ForeColor = System.Drawing.Color.Black;
            this.runTraverseBtn.Location = new System.Drawing.Point(654, 396);
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
            this.addPropertyBtn.Location = new System.Drawing.Point(50, 268);
            this.addPropertyBtn.Name = "addPropertyBtn";
            this.addPropertyBtn.Size = new System.Drawing.Size(195, 55);
            this.addPropertyBtn.TabIndex = 33;
            this.addPropertyBtn.Text = "LEARN MORE";
            this.addPropertyBtn.UseVisualStyleBackColor = false;
            this.addPropertyBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddNewProperty);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.Transparent;
            this.helpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.helpButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.helpButton.FlatAppearance.BorderSize = 0;
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Image = global::KWG_Geoenrichment.Properties.Resources.help_circle;
            this.helpButton.Location = new System.Drawing.Point(12, 409);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(74, 70);
            this.helpButton.TabIndex = 37;
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler(this.ToggleHelpMenu);
            // 
            // TraverseKnowledgeGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KWG_Geoenrichment.Properties.Resources.background_landing__2_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(894, 491);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.addPropertyBtn);
            this.Controls.Add(this.runTraverseBtn);
            this.Controls.Add(this.prop1Req);
            this.Controls.Add(this.prop1);
            this.Controls.Add(this.prop1Label);
            this.Controls.Add(this.traverseGraph);
            this.DoubleBuffered = true;
            this.Name = "TraverseKnowledgeGraph";
            this.Text = "Traverse Knowledge Graph";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label traverseGraph;
        private System.Windows.Forms.Label prop1Req;
        private System.Windows.Forms.ComboBox prop1;
        private System.Windows.Forms.Label prop1Label;
        private System.Windows.Forms.Button runTraverseBtn;
        private System.Windows.Forms.Button addPropertyBtn;
        private System.Windows.Forms.Button helpButton;
    }
}
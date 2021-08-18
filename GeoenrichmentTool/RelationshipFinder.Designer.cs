
namespace GeoenrichmentTool
{
    partial class RelationshipFinder
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
            this.linkedDataFinder = new System.Windows.Forms.Label();
            this.firstPropDirectionReq = new System.Windows.Forms.Label();
            this.firstPropDirection = new System.Windows.Forms.ComboBox();
            this.firstPropDirectionLabel = new System.Windows.Forms.Label();
            this.submitFormBtn = new System.Windows.Forms.Button();
            this.firstPropReq = new System.Windows.Forms.Label();
            this.firstProp = new System.Windows.Forms.ComboBox();
            this.firstPropLabel = new System.Windows.Forms.Label();
            this.secondProp = new System.Windows.Forms.ComboBox();
            this.secondPropLabel = new System.Windows.Forms.Label();
            this.secondPropDirection = new System.Windows.Forms.ComboBox();
            this.secondPropDirectionLabel = new System.Windows.Forms.Label();
            this.thirdProp = new System.Windows.Forms.ComboBox();
            this.thirdPropLabel = new System.Windows.Forms.Label();
            this.thirdPropDirection = new System.Windows.Forms.ComboBox();
            this.thirdPropDirectionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkedDataFinder
            // 
            this.linkedDataFinder.AutoSize = true;
            this.linkedDataFinder.BackColor = System.Drawing.Color.Transparent;
            this.linkedDataFinder.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkedDataFinder.ForeColor = System.Drawing.Color.White;
            this.linkedDataFinder.Location = new System.Drawing.Point(12, 9);
            this.linkedDataFinder.Name = "linkedDataFinder";
            this.linkedDataFinder.Size = new System.Drawing.Size(510, 37);
            this.linkedDataFinder.TabIndex = 4;
            this.linkedDataFinder.Text = "Linked Data Relationship Finder";
            // 
            // firstPropDirectionReq
            // 
            this.firstPropDirectionReq.AutoSize = true;
            this.firstPropDirectionReq.BackColor = System.Drawing.Color.Transparent;
            this.firstPropDirectionReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstPropDirectionReq.ForeColor = System.Drawing.Color.Red;
            this.firstPropDirectionReq.Location = new System.Drawing.Point(15, 95);
            this.firstPropDirectionReq.Name = "firstPropDirectionReq";
            this.firstPropDirectionReq.Size = new System.Drawing.Size(15, 20);
            this.firstPropDirectionReq.TabIndex = 24;
            this.firstPropDirectionReq.Text = "*";
            // 
            // firstPropDirection
            // 
            this.firstPropDirection.Font = new System.Drawing.Font("Arial", 12F);
            this.firstPropDirection.FormattingEnabled = true;
            this.firstPropDirection.Items.AddRange(new object[] {
            "Both",
            "Origin",
            "Destination"});
            this.firstPropDirection.Location = new System.Drawing.Point(19, 118);
            this.firstPropDirection.Name = "firstPropDirection";
            this.firstPropDirection.Size = new System.Drawing.Size(740, 26);
            this.firstPropDirection.TabIndex = 23;
            this.firstPropDirection.SelectedValueChanged += new System.EventHandler(this.firstPropDirectionChanged);
            // 
            // firstPropDirectionLabel
            // 
            this.firstPropDirectionLabel.AutoSize = true;
            this.firstPropDirectionLabel.BackColor = System.Drawing.Color.Transparent;
            this.firstPropDirectionLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.firstPropDirectionLabel.ForeColor = System.Drawing.Color.White;
            this.firstPropDirectionLabel.Location = new System.Drawing.Point(25, 95);
            this.firstPropDirectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.firstPropDirectionLabel.Name = "firstPropDirectionLabel";
            this.firstPropDirectionLabel.Size = new System.Drawing.Size(245, 19);
            this.firstPropDirectionLabel.TabIndex = 22;
            this.firstPropDirectionLabel.Text = "First Degree Property Direction";
            // 
            // submitFormBtn
            // 
            this.submitFormBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.submitFormBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.submitFormBtn.FlatAppearance.BorderSize = 0;
            this.submitFormBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.submitFormBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.submitFormBtn.Location = new System.Drawing.Point(640, 416);
            this.submitFormBtn.Name = "submitFormBtn";
            this.submitFormBtn.Size = new System.Drawing.Size(119, 44);
            this.submitFormBtn.TabIndex = 25;
            this.submitFormBtn.Text = "Run";
            this.submitFormBtn.UseVisualStyleBackColor = false;
            this.submitFormBtn.Click += new System.EventHandler(this.FindRelatedLinkedData);
            // 
            // firstPropReq
            // 
            this.firstPropReq.AutoSize = true;
            this.firstPropReq.BackColor = System.Drawing.Color.Transparent;
            this.firstPropReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstPropReq.ForeColor = System.Drawing.Color.Red;
            this.firstPropReq.Location = new System.Drawing.Point(15, 147);
            this.firstPropReq.Name = "firstPropReq";
            this.firstPropReq.Size = new System.Drawing.Size(15, 20);
            this.firstPropReq.TabIndex = 28;
            this.firstPropReq.Text = "*";
            // 
            // firstProp
            // 
            this.firstProp.Enabled = false;
            this.firstProp.Font = new System.Drawing.Font("Arial", 12F);
            this.firstProp.FormattingEnabled = true;
            this.firstProp.Location = new System.Drawing.Point(19, 170);
            this.firstProp.Name = "firstProp";
            this.firstProp.Size = new System.Drawing.Size(740, 26);
            this.firstProp.TabIndex = 27;
            this.firstProp.SelectedValueChanged += new System.EventHandler(this.firstPropChanged);
            // 
            // firstPropLabel
            // 
            this.firstPropLabel.AutoSize = true;
            this.firstPropLabel.BackColor = System.Drawing.Color.Transparent;
            this.firstPropLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.firstPropLabel.ForeColor = System.Drawing.Color.White;
            this.firstPropLabel.Location = new System.Drawing.Point(25, 147);
            this.firstPropLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.firstPropLabel.Name = "firstPropLabel";
            this.firstPropLabel.Size = new System.Drawing.Size(172, 19);
            this.firstPropLabel.TabIndex = 26;
            this.firstPropLabel.Text = "First Degree Property";
            // 
            // secondProp
            // 
            this.secondProp.Enabled = false;
            this.secondProp.Font = new System.Drawing.Font("Arial", 12F);
            this.secondProp.FormattingEnabled = true;
            this.secondProp.Location = new System.Drawing.Point(19, 274);
            this.secondProp.Name = "secondProp";
            this.secondProp.Size = new System.Drawing.Size(740, 26);
            this.secondProp.TabIndex = 33;
            this.secondProp.SelectedValueChanged += new System.EventHandler(this.secondPropChanged);
            // 
            // secondPropLabel
            // 
            this.secondPropLabel.AutoSize = true;
            this.secondPropLabel.BackColor = System.Drawing.Color.Transparent;
            this.secondPropLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.secondPropLabel.ForeColor = System.Drawing.Color.White;
            this.secondPropLabel.Location = new System.Drawing.Point(15, 251);
            this.secondPropLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.secondPropLabel.Name = "secondPropLabel";
            this.secondPropLabel.Size = new System.Drawing.Size(197, 19);
            this.secondPropLabel.TabIndex = 32;
            this.secondPropLabel.Text = "Second Degree Property";
            // 
            // secondPropDirection
            // 
            this.secondPropDirection.Enabled = false;
            this.secondPropDirection.Font = new System.Drawing.Font("Arial", 12F);
            this.secondPropDirection.FormattingEnabled = true;
            this.secondPropDirection.Items.AddRange(new object[] {
            "Both",
            "Origin",
            "Destination"});
            this.secondPropDirection.Location = new System.Drawing.Point(19, 222);
            this.secondPropDirection.Name = "secondPropDirection";
            this.secondPropDirection.Size = new System.Drawing.Size(740, 26);
            this.secondPropDirection.TabIndex = 30;
            this.secondPropDirection.SelectedValueChanged += new System.EventHandler(this.secondPropDirectionChanged);
            // 
            // secondPropDirectionLabel
            // 
            this.secondPropDirectionLabel.AutoSize = true;
            this.secondPropDirectionLabel.BackColor = System.Drawing.Color.Transparent;
            this.secondPropDirectionLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.secondPropDirectionLabel.ForeColor = System.Drawing.Color.White;
            this.secondPropDirectionLabel.Location = new System.Drawing.Point(15, 200);
            this.secondPropDirectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.secondPropDirectionLabel.Name = "secondPropDirectionLabel";
            this.secondPropDirectionLabel.Size = new System.Drawing.Size(270, 19);
            this.secondPropDirectionLabel.TabIndex = 29;
            this.secondPropDirectionLabel.Text = "Second Degree Property Direction";
            // 
            // thirdProp
            // 
            this.thirdProp.Enabled = false;
            this.thirdProp.Font = new System.Drawing.Font("Arial", 12F);
            this.thirdProp.FormattingEnabled = true;
            this.thirdProp.Location = new System.Drawing.Point(19, 380);
            this.thirdProp.Name = "thirdProp";
            this.thirdProp.Size = new System.Drawing.Size(740, 26);
            this.thirdProp.TabIndex = 39;
            this.thirdProp.SelectedValueChanged += new System.EventHandler(this.thirdPropChanged);
            // 
            // thirdPropLabel
            // 
            this.thirdPropLabel.AutoSize = true;
            this.thirdPropLabel.BackColor = System.Drawing.Color.Transparent;
            this.thirdPropLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.thirdPropLabel.ForeColor = System.Drawing.Color.White;
            this.thirdPropLabel.Location = new System.Drawing.Point(13, 357);
            this.thirdPropLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thirdPropLabel.Name = "thirdPropLabel";
            this.thirdPropLabel.Size = new System.Drawing.Size(178, 19);
            this.thirdPropLabel.TabIndex = 38;
            this.thirdPropLabel.Text = "Third Degree Property";
            // 
            // thirdPropDirection
            // 
            this.thirdPropDirection.Enabled = false;
            this.thirdPropDirection.Font = new System.Drawing.Font("Arial", 12F);
            this.thirdPropDirection.FormattingEnabled = true;
            this.thirdPropDirection.Items.AddRange(new object[] {
            "Both",
            "Origin",
            "Destination"});
            this.thirdPropDirection.Location = new System.Drawing.Point(19, 328);
            this.thirdPropDirection.Name = "thirdPropDirection";
            this.thirdPropDirection.Size = new System.Drawing.Size(740, 26);
            this.thirdPropDirection.TabIndex = 36;
            this.thirdPropDirection.SelectedValueChanged += new System.EventHandler(this.thirdPropDirectionChanged);
            // 
            // thirdPropDirectionLabel
            // 
            this.thirdPropDirectionLabel.AutoSize = true;
            this.thirdPropDirectionLabel.BackColor = System.Drawing.Color.Transparent;
            this.thirdPropDirectionLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.thirdPropDirectionLabel.ForeColor = System.Drawing.Color.White;
            this.thirdPropDirectionLabel.Location = new System.Drawing.Point(13, 306);
            this.thirdPropDirectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thirdPropDirectionLabel.Name = "thirdPropDirectionLabel";
            this.thirdPropDirectionLabel.Size = new System.Drawing.Size(251, 19);
            this.thirdPropDirectionLabel.TabIndex = 35;
            this.thirdPropDirectionLabel.Text = "Third Degree Property Direction";
            // 
            // RelationshipFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::GeoenrichmentTool.Properties.Resources.background_landing__2_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(778, 471);
            this.Controls.Add(this.thirdProp);
            this.Controls.Add(this.thirdPropLabel);
            this.Controls.Add(this.thirdPropDirection);
            this.Controls.Add(this.thirdPropDirectionLabel);
            this.Controls.Add(this.secondProp);
            this.Controls.Add(this.secondPropLabel);
            this.Controls.Add(this.secondPropDirection);
            this.Controls.Add(this.secondPropDirectionLabel);
            this.Controls.Add(this.firstPropReq);
            this.Controls.Add(this.firstProp);
            this.Controls.Add(this.firstPropLabel);
            this.Controls.Add(this.submitFormBtn);
            this.Controls.Add(this.firstPropDirectionReq);
            this.Controls.Add(this.firstPropDirection);
            this.Controls.Add(this.firstPropDirectionLabel);
            this.Controls.Add(this.linkedDataFinder);
            this.Name = "RelationshipFinder";
            this.Text = "Relationship Finder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label linkedDataFinder;
        private System.Windows.Forms.Label firstPropDirectionReq;
        private System.Windows.Forms.ComboBox firstPropDirection;
        private System.Windows.Forms.Label firstPropDirectionLabel;
        private System.Windows.Forms.Button submitFormBtn;
        private System.Windows.Forms.Label firstPropReq;
        private System.Windows.Forms.ComboBox firstProp;
        private System.Windows.Forms.Label firstPropLabel;
        private System.Windows.Forms.ComboBox secondProp;
        private System.Windows.Forms.Label secondPropLabel;
        private System.Windows.Forms.ComboBox secondPropDirection;
        private System.Windows.Forms.Label secondPropDirectionLabel;
        private System.Windows.Forms.ComboBox thirdProp;
        private System.Windows.Forms.Label thirdPropLabel;
        private System.Windows.Forms.ComboBox thirdPropDirection;
        private System.Windows.Forms.Label thirdPropDirectionLabel;
    }
}
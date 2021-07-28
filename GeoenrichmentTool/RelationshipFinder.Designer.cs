
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
            this.requiredRelatedTables = new System.Windows.Forms.Label();
            this.relationshipDegree = new System.Windows.Forms.ComboBox();
            this.relationshipDegreeLabel = new System.Windows.Forms.Label();
            this.submitFormBtn = new System.Windows.Forms.Button();
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
            // requiredRelatedTables
            // 
            this.requiredRelatedTables.AutoSize = true;
            this.requiredRelatedTables.BackColor = System.Drawing.Color.Transparent;
            this.requiredRelatedTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiredRelatedTables.ForeColor = System.Drawing.Color.Red;
            this.requiredRelatedTables.Location = new System.Drawing.Point(15, 95);
            this.requiredRelatedTables.Name = "requiredRelatedTables";
            this.requiredRelatedTables.Size = new System.Drawing.Size(15, 20);
            this.requiredRelatedTables.TabIndex = 24;
            this.requiredRelatedTables.Text = "*";
            // 
            // relationshipDegree
            // 
            this.relationshipDegree.Font = new System.Drawing.Font("Arial", 12F);
            this.relationshipDegree.FormattingEnabled = true;
            this.relationshipDegree.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.relationshipDegree.Location = new System.Drawing.Point(19, 118);
            this.relationshipDegree.Name = "relationshipDegree";
            this.relationshipDegree.Size = new System.Drawing.Size(740, 26);
            this.relationshipDegree.TabIndex = 23;
            // 
            // relationshipDegreeLabel
            // 
            this.relationshipDegreeLabel.AutoSize = true;
            this.relationshipDegreeLabel.BackColor = System.Drawing.Color.Transparent;
            this.relationshipDegreeLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.relationshipDegreeLabel.ForeColor = System.Drawing.Color.White;
            this.relationshipDegreeLabel.Location = new System.Drawing.Point(25, 95);
            this.relationshipDegreeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.relationshipDegreeLabel.Name = "relationshipDegreeLabel";
            this.relationshipDegreeLabel.Size = new System.Drawing.Size(170, 19);
            this.relationshipDegreeLabel.TabIndex = 22;
            this.relationshipDegreeLabel.Text = "Relationship Degree:";
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
            // RelationshipFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::GeoenrichmentTool.Properties.Resources.background_landing__2_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(778, 471);
            this.Controls.Add(this.submitFormBtn);
            this.Controls.Add(this.requiredRelatedTables);
            this.Controls.Add(this.relationshipDegree);
            this.Controls.Add(this.relationshipDegreeLabel);
            this.Controls.Add(this.linkedDataFinder);
            this.Name = "RelationshipFinder";
            this.Text = "Relationship Finder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label linkedDataFinder;
        private System.Windows.Forms.Label requiredRelatedTables;
        private System.Windows.Forms.ComboBox relationshipDegree;
        private System.Windows.Forms.Label relationshipDegreeLabel;
        private System.Windows.Forms.Button submitFormBtn;
    }
}
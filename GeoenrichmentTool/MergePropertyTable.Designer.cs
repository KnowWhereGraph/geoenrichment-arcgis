
namespace GeoenrichmentTool
{
    partial class MergePropertyTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergePropertyTable));
            this.mergePropTable = new System.Windows.Forms.Label();
            this.submitFormBtn = new System.Windows.Forms.Button();
            this.requiredRelatedTables = new System.Windows.Forms.Label();
            this.relatedTables = new System.Windows.Forms.ComboBox();
            this.relatedTablesLabel = new System.Windows.Forms.Label();
            this.requiredMergeRules = new System.Windows.Forms.Label();
            this.mergeRulesLabel = new System.Windows.Forms.Label();
            this.mergeRules = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mergePropTable
            // 
            this.mergePropTable.AutoSize = true;
            this.mergePropTable.BackColor = System.Drawing.Color.Transparent;
            this.mergePropTable.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mergePropTable.ForeColor = System.Drawing.Color.White;
            this.mergePropTable.Location = new System.Drawing.Point(12, 9);
            this.mergePropTable.Name = "mergePropTable";
            this.mergePropTable.Size = new System.Drawing.Size(343, 37);
            this.mergePropTable.TabIndex = 3;
            this.mergePropTable.Text = "Merge Property Table";
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
            this.submitFormBtn.TabIndex = 9;
            this.submitFormBtn.Text = "Run";
            this.submitFormBtn.UseVisualStyleBackColor = false;
            this.submitFormBtn.Click += new System.EventHandler(this.MergeTables);
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
            this.requiredRelatedTables.TabIndex = 21;
            this.requiredRelatedTables.Text = "*";
            // 
            // relatedTables
            // 
            this.relatedTables.Font = new System.Drawing.Font("Arial", 12F);
            this.relatedTables.FormattingEnabled = true;
            this.relatedTables.Location = new System.Drawing.Point(19, 118);
            this.relatedTables.Name = "relatedTables";
            this.relatedTables.Size = new System.Drawing.Size(740, 26);
            this.relatedTables.TabIndex = 20;
            this.relatedTables.SelectedValueChanged += new System.EventHandler(this.UpdateMergeRules);
            // 
            // relatedTablesLabel
            // 
            this.relatedTablesLabel.AutoSize = true;
            this.relatedTablesLabel.BackColor = System.Drawing.Color.Transparent;
            this.relatedTablesLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.relatedTablesLabel.ForeColor = System.Drawing.Color.White;
            this.relatedTablesLabel.Location = new System.Drawing.Point(25, 95);
            this.relatedTablesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.relatedTablesLabel.Name = "relatedTablesLabel";
            this.relatedTablesLabel.Size = new System.Drawing.Size(127, 19);
            this.relatedTablesLabel.TabIndex = 19;
            this.relatedTablesLabel.Text = "Related Tables:";
            // 
            // requiredMergeRules
            // 
            this.requiredMergeRules.AutoSize = true;
            this.requiredMergeRules.BackColor = System.Drawing.Color.Transparent;
            this.requiredMergeRules.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiredMergeRules.ForeColor = System.Drawing.Color.Red;
            this.requiredMergeRules.Location = new System.Drawing.Point(15, 172);
            this.requiredMergeRules.Name = "requiredMergeRules";
            this.requiredMergeRules.Size = new System.Drawing.Size(15, 20);
            this.requiredMergeRules.TabIndex = 23;
            this.requiredMergeRules.Text = "*";
            // 
            // mergeRulesLabel
            // 
            this.mergeRulesLabel.AutoSize = true;
            this.mergeRulesLabel.BackColor = System.Drawing.Color.Transparent;
            this.mergeRulesLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.mergeRulesLabel.ForeColor = System.Drawing.Color.White;
            this.mergeRulesLabel.Location = new System.Drawing.Point(25, 172);
            this.mergeRulesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mergeRulesLabel.Name = "mergeRulesLabel";
            this.mergeRulesLabel.Size = new System.Drawing.Size(110, 19);
            this.mergeRulesLabel.TabIndex = 22;
            this.mergeRulesLabel.Text = "Merge Rules:";
            // 
            // mergeRules
            // 
            this.mergeRules.Font = new System.Drawing.Font("Arial", 12F);
            this.mergeRules.FormattingEnabled = true;
            this.mergeRules.Location = new System.Drawing.Point(19, 195);
            this.mergeRules.Name = "mergeRules";
            this.mergeRules.Size = new System.Drawing.Size(740, 26);
            this.mergeRules.TabIndex = 24;
            // 
            // MergePropertyTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(778, 471);
            this.Controls.Add(this.mergeRules);
            this.Controls.Add(this.requiredMergeRules);
            this.Controls.Add(this.mergeRulesLabel);
            this.Controls.Add(this.requiredRelatedTables);
            this.Controls.Add(this.relatedTables);
            this.Controls.Add(this.relatedTablesLabel);
            this.Controls.Add(this.submitFormBtn);
            this.Controls.Add(this.mergePropTable);
            this.Name = "MergePropertyTable";
            this.Text = "Merge Property Table";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mergePropTable;
        private System.Windows.Forms.Button submitFormBtn;
        private System.Windows.Forms.Label requiredRelatedTables;
        private System.Windows.Forms.ComboBox relatedTables;
        private System.Windows.Forms.Label relatedTablesLabel;
        private System.Windows.Forms.Label requiredMergeRules;
        private System.Windows.Forms.Label mergeRulesLabel;
        private System.Windows.Forms.ComboBox mergeRules;
    }
}
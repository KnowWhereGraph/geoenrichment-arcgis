
namespace GeoenrichmentTool
{
    partial class PropertyEnrichment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyEnrichment));
            this.propFormLabel = new System.Windows.Forms.Label();
            this.commonCheckBox = new System.Windows.Forms.CheckedListBox();
            this.commonLabel = new System.Windows.Forms.Label();
            this.inverseLabel = new System.Windows.Forms.Label();
            this.inverseCheckBox = new System.Windows.Forms.CheckedListBox();
            this.submitFormBtn = new System.Windows.Forms.Button();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.inverseSelectAllButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propFormLabel
            // 
            this.propFormLabel.AutoSize = true;
            this.propFormLabel.BackColor = System.Drawing.Color.Transparent;
            this.propFormLabel.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propFormLabel.ForeColor = System.Drawing.Color.White;
            this.propFormLabel.Location = new System.Drawing.Point(12, 9);
            this.propFormLabel.Name = "propFormLabel";
            this.propFormLabel.Size = new System.Drawing.Size(332, 37);
            this.propFormLabel.TabIndex = 3;
            this.propFormLabel.Text = "Property Enrichment";
            // 
            // commonCheckBox
            // 
            this.commonCheckBox.BackColor = System.Drawing.Color.White;
            this.commonCheckBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commonCheckBox.Font = new System.Drawing.Font("Arial", 10F);
            this.commonCheckBox.FormattingEnabled = true;
            this.commonCheckBox.Location = new System.Drawing.Point(19, 117);
            this.commonCheckBox.Name = "commonCheckBox";
            this.commonCheckBox.Size = new System.Drawing.Size(740, 90);
            this.commonCheckBox.TabIndex = 4;
            // 
            // commonLabel
            // 
            this.commonLabel.AutoSize = true;
            this.commonLabel.BackColor = System.Drawing.Color.Transparent;
            this.commonLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.commonLabel.ForeColor = System.Drawing.Color.White;
            this.commonLabel.Location = new System.Drawing.Point(15, 95);
            this.commonLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.commonLabel.Name = "commonLabel";
            this.commonLabel.Size = new System.Drawing.Size(168, 19);
            this.commonLabel.TabIndex = 5;
            this.commonLabel.Text = "Common Properties:";
            // 
            // inverseLabel
            // 
            this.inverseLabel.AutoSize = true;
            this.inverseLabel.BackColor = System.Drawing.Color.Transparent;
            this.inverseLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.inverseLabel.ForeColor = System.Drawing.Color.White;
            this.inverseLabel.Location = new System.Drawing.Point(15, 231);
            this.inverseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.inverseLabel.Name = "inverseLabel";
            this.inverseLabel.Size = new System.Drawing.Size(228, 19);
            this.inverseLabel.TabIndex = 6;
            this.inverseLabel.Text = "Inverse Common Properties:";
            // 
            // inverseCheckBox
            // 
            this.inverseCheckBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inverseCheckBox.Font = new System.Drawing.Font("Arial", 10F);
            this.inverseCheckBox.FormattingEnabled = true;
            this.inverseCheckBox.Location = new System.Drawing.Point(19, 253);
            this.inverseCheckBox.Name = "inverseCheckBox";
            this.inverseCheckBox.Size = new System.Drawing.Size(740, 90);
            this.inverseCheckBox.TabIndex = 7;
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
            this.submitFormBtn.Click += new System.EventHandler(this.EnrichData);
            // 
            // selectAllButton
            // 
            this.selectAllButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.selectAllButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectAllButton.FlatAppearance.BorderSize = 0;
            this.selectAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectAllButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.selectAllButton.Location = new System.Drawing.Point(640, 84);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(119, 27);
            this.selectAllButton.TabIndex = 10;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.UseVisualStyleBackColor = false;
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllProperties);
            // 
            // inverseSelectAllButton
            // 
            this.inverseSelectAllButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(214)))), ((int)(((byte)(237)))));
            this.inverseSelectAllButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.inverseSelectAllButton.FlatAppearance.BorderSize = 0;
            this.inverseSelectAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inverseSelectAllButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.inverseSelectAllButton.Location = new System.Drawing.Point(640, 220);
            this.inverseSelectAllButton.Name = "inverseSelectAllButton";
            this.inverseSelectAllButton.Size = new System.Drawing.Size(119, 27);
            this.inverseSelectAllButton.TabIndex = 11;
            this.inverseSelectAllButton.Text = "Select All";
            this.inverseSelectAllButton.UseVisualStyleBackColor = false;
            this.inverseSelectAllButton.Click += new System.EventHandler(this.InverseSelectAllProperties);
            // 
            // PropertyEnrichment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(778, 471);
            this.Controls.Add(this.inverseSelectAllButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.submitFormBtn);
            this.Controls.Add(this.inverseCheckBox);
            this.Controls.Add(this.inverseLabel);
            this.Controls.Add(this.commonLabel);
            this.Controls.Add(this.commonCheckBox);
            this.Controls.Add(this.propFormLabel);
            this.Name = "PropertyEnrichment";
            this.Text = "Property Enrichment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label propFormLabel;
        private System.Windows.Forms.CheckedListBox commonCheckBox;
        private System.Windows.Forms.Label commonLabel;
        private System.Windows.Forms.Label inverseLabel;
        private System.Windows.Forms.CheckedListBox inverseCheckBox;
        private System.Windows.Forms.Button submitFormBtn;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.Button inverseSelectAllButton;
    }
}
namespace KWG_Geoenrichment
{
    partial class KWGHelp
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
            this.helpText = new System.Windows.Forms.Label();
            this.helpPanelLogo = new System.Windows.Forms.PictureBox();
            this.closeForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // helpText
            // 
            this.helpText.AutoSize = true;
            this.helpText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.helpText.ForeColor = System.Drawing.Color.White;
            this.helpText.Location = new System.Drawing.Point(8, 79);
            this.helpText.MaximumSize = new System.Drawing.Size(325, 0);
            this.helpText.Name = "helpText";
            this.helpText.Size = new System.Drawing.Size(121, 19);
            this.helpText.TabIndex = 4;
            this.helpText.Text = "Text goes here";
            // 
            // helpPanelLogo
            // 
            this.helpPanelLogo.Image = global::KWG_Geoenrichment.Properties.Resources.help_circle;
            this.helpPanelLogo.Location = new System.Drawing.Point(12, 12);
            this.helpPanelLogo.Name = "helpPanelLogo";
            this.helpPanelLogo.Size = new System.Drawing.Size(66, 64);
            this.helpPanelLogo.TabIndex = 3;
            this.helpPanelLogo.TabStop = false;
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
            this.closeForm.Location = new System.Drawing.Point(289, 12);
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(36, 36);
            this.closeForm.TabIndex = 47;
            this.closeForm.UseVisualStyleBackColor = false;
            this.closeForm.Click += new System.EventHandler(this.CloseWindow);
            // 
            // KWGHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(56)))), ((int)(((byte)(91)))));
            this.ClientSize = new System.Drawing.Size(337, 479);
            this.ControlBox = false;
            this.Controls.Add(this.closeForm);
            this.Controls.Add(this.helpText);
            this.Controls.Add(this.helpPanelLogo);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KWGHelp";
            this.Text = "KWG Help";
            ((System.ComponentModel.ISupportInitialize)(this.helpPanelLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label helpText;
        private System.Windows.Forms.PictureBox helpPanelLogo;
        private System.Windows.Forms.Button closeForm;
    }
}
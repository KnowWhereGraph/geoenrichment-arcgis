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
            helpText = new System.Windows.Forms.Label();
            helpPanelLogo = new System.Windows.Forms.PictureBox();
            closeForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)helpPanelLogo).BeginInit();
            SuspendLayout();
            // 
            // helpText
            // 
            helpText.AutoSize = true;
            helpText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            helpText.ForeColor = System.Drawing.Color.White;
            helpText.Location = new System.Drawing.Point(9, 91);
            helpText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            helpText.MaximumSize = new System.Drawing.Size(379, 0);
            helpText.Name = "helpText";
            helpText.Size = new System.Drawing.Size(121, 19);
            helpText.TabIndex = 4;
            helpText.Text = "Text goes here";
            // 
            // helpPanelLogo
            // 
            helpPanelLogo.Image = Properties.Resources.help_circle;
            helpPanelLogo.Location = new System.Drawing.Point(14, 14);
            helpPanelLogo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            helpPanelLogo.Name = "helpPanelLogo";
            helpPanelLogo.Size = new System.Drawing.Size(77, 74);
            helpPanelLogo.TabIndex = 3;
            helpPanelLogo.TabStop = false;
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
            closeForm.Location = new System.Drawing.Point(337, 14);
            closeForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            closeForm.Name = "closeForm";
            closeForm.Size = new System.Drawing.Size(42, 42);
            closeForm.TabIndex = 47;
            closeForm.UseVisualStyleBackColor = false;
            closeForm.Click += CloseWindow;
            // 
            // KWGHelp
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.FromArgb(54, 56, 91);
            ClientSize = new System.Drawing.Size(393, 553);
            ControlBox = false;
            Controls.Add(closeForm);
            Controls.Add(helpText);
            Controls.Add(helpPanelLogo);
            HelpButton = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "KWGHelp";
            Text = "KWG Help";
            ((System.ComponentModel.ISupportInitialize)helpPanelLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label helpText;
        private System.Windows.Forms.PictureBox helpPanelLogo;
        private System.Windows.Forms.Button closeForm;
    }
}
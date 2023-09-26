
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
            exploreProperties = new System.Windows.Forms.Label();
            propReq = new System.Windows.Forms.Label();
            value1 = new System.Windows.Forms.ComboBox();
            propLabel = new System.Windows.Forms.Label();
            runTraverseBtn = new System.Windows.Forms.Button();
            exploreFurtherBtn = new System.Windows.Forms.Button();
            helpButton = new System.Windows.Forms.Button();
            prop1 = new System.Windows.Forms.ComboBox();
            propLoading = new System.Windows.Forms.PictureBox();
            closeForm = new System.Windows.Forms.Button();
            valueLoading = new System.Windows.Forms.PictureBox();
            valueReq = new System.Windows.Forms.Label();
            valueLabel = new System.Windows.Forms.Label();
            addValueBtn = new System.Windows.Forms.Button();
            propertyValueLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)propLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)valueLoading).BeginInit();
            SuspendLayout();
            // 
            // exploreProperties
            // 
            exploreProperties.AutoSize = true;
            exploreProperties.BackColor = System.Drawing.Color.Transparent;
            exploreProperties.Font = new System.Drawing.Font("Arial", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            exploreProperties.ForeColor = System.Drawing.Color.White;
            exploreProperties.Location = new System.Drawing.Point(50, 60);
            exploreProperties.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            exploreProperties.Name = "exploreProperties";
            exploreProperties.Size = new System.Drawing.Size(432, 54);
            exploreProperties.TabIndex = 5;
            exploreProperties.Text = "Explore Properties";
            // 
            // propReq
            // 
            propReq.AutoSize = true;
            propReq.BackColor = System.Drawing.Color.Transparent;
            propReq.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            propReq.ForeColor = System.Drawing.Color.Red;
            propReq.Location = new System.Drawing.Point(50, 150);
            propReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            propReq.Name = "propReq";
            propReq.Size = new System.Drawing.Size(22, 29);
            propReq.TabIndex = 31;
            propReq.Text = "*";
            // 
            // value1
            // 
            value1.DisplayMember = "Value";
            value1.Enabled = false;
            value1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            value1.FormattingEnabled = true;
            value1.Location = new System.Drawing.Point(510, 180);
            value1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            value1.Name = "value1";
            value1.Size = new System.Drawing.Size(440, 26);
            value1.TabIndex = 30;
            value1.ValueMember = "Key";
            value1.SelectedIndexChanged += OnValueBoxChange;
            // 
            // propLabel
            // 
            propLabel.AutoSize = true;
            propLabel.BackColor = System.Drawing.Color.Transparent;
            propLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            propLabel.ForeColor = System.Drawing.Color.White;
            propLabel.Location = new System.Drawing.Point(70, 150);
            propLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            propLabel.Name = "propLabel";
            propLabel.Size = new System.Drawing.Size(126, 19);
            propLabel.TabIndex = 29;
            propLabel.Text = "Select Property";
            // 
            // runTraverseBtn
            // 
            runTraverseBtn.BackColor = System.Drawing.Color.FromArgb(66, 214, 237);
            runTraverseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            runTraverseBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            runTraverseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            runTraverseBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            runTraverseBtn.ForeColor = System.Drawing.Color.Black;
            runTraverseBtn.Location = new System.Drawing.Point(767, 390);
            runTraverseBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            runTraverseBtn.Name = "runTraverseBtn";
            runTraverseBtn.Size = new System.Drawing.Size(183, 55);
            runTraverseBtn.TabIndex = 32;
            runTraverseBtn.Text = "ADD PROPERTIES";
            runTraverseBtn.UseVisualStyleBackColor = false;
            runTraverseBtn.Click += RunTraverseGraph;
            // 
            // exploreFurtherBtn
            // 
            exploreFurtherBtn.BackColor = System.Drawing.Color.FromArgb(33, 111, 179);
            exploreFurtherBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            exploreFurtherBtn.Enabled = false;
            exploreFurtherBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            exploreFurtherBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            exploreFurtherBtn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            exploreFurtherBtn.ForeColor = System.Drawing.Color.White;
            exploreFurtherBtn.Location = new System.Drawing.Point(50, 220);
            exploreFurtherBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            exploreFurtherBtn.Name = "exploreFurtherBtn";
            exploreFurtherBtn.Size = new System.Drawing.Size(146, 30);
            exploreFurtherBtn.TabIndex = 33;
            exploreFurtherBtn.Text = "Explore Further";
            exploreFurtherBtn.UseVisualStyleBackColor = false;
            exploreFurtherBtn.Click += LearnMore;
            // 
            // helpButton
            // 
            helpButton.BackColor = System.Drawing.Color.Transparent;
            helpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            helpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            helpButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            helpButton.FlatAppearance.BorderSize = 0;
            helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            helpButton.Image = Properties.Resources.help_circle;
            helpButton.Location = new System.Drawing.Point(50, 364);
            helpButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            helpButton.Name = "helpButton";
            helpButton.Size = new System.Drawing.Size(86, 81);
            helpButton.TabIndex = 37;
            helpButton.UseVisualStyleBackColor = false;
            helpButton.Click += ClickToggleHelpMenu;
            // 
            // prop1
            // 
            prop1.DisplayMember = "Value";
            prop1.Enabled = false;
            prop1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            prop1.FormattingEnabled = true;
            prop1.Location = new System.Drawing.Point(50, 180);
            prop1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            prop1.Name = "prop1";
            prop1.Size = new System.Drawing.Size(440, 26);
            prop1.TabIndex = 40;
            prop1.ValueMember = "Key";
            prop1.SelectedIndexChanged += OnPropBoxChange;
            // 
            // propLoading
            // 
            propLoading.BackColor = System.Drawing.Color.Transparent;
            propLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            propLoading.Image = Properties.Resources.loading;
            propLoading.Location = new System.Drawing.Point(205, 144);
            propLoading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            propLoading.Name = "propLoading";
            propLoading.Size = new System.Drawing.Size(30, 30);
            propLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            propLoading.TabIndex = 41;
            propLoading.TabStop = false;
            propLoading.Visible = false;
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
            closeForm.TabIndex = 47;
            closeForm.UseVisualStyleBackColor = false;
            closeForm.Click += CloseWindow;
            // 
            // valueLoading
            // 
            valueLoading.BackColor = System.Drawing.Color.Transparent;
            valueLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            valueLoading.Image = Properties.Resources.loading;
            valueLoading.Location = new System.Drawing.Point(641, 144);
            valueLoading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            valueLoading.Name = "valueLoading";
            valueLoading.Size = new System.Drawing.Size(30, 30);
            valueLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            valueLoading.TabIndex = 50;
            valueLoading.TabStop = false;
            valueLoading.Visible = false;
            // 
            // valueReq
            // 
            valueReq.AutoSize = true;
            valueReq.BackColor = System.Drawing.Color.Transparent;
            valueReq.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            valueReq.ForeColor = System.Drawing.Color.Red;
            valueReq.Location = new System.Drawing.Point(510, 150);
            valueReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            valueReq.Name = "valueReq";
            valueReq.Size = new System.Drawing.Size(22, 29);
            valueReq.TabIndex = 49;
            valueReq.Text = "*";
            // 
            // valueLabel
            // 
            valueLabel.AutoSize = true;
            valueLabel.BackColor = System.Drawing.Color.Transparent;
            valueLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            valueLabel.ForeColor = System.Drawing.Color.White;
            valueLabel.Location = new System.Drawing.Point(530, 150);
            valueLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            valueLabel.Name = "valueLabel";
            valueLabel.Size = new System.Drawing.Size(102, 19);
            valueLabel.TabIndex = 48;
            valueLabel.Text = "Select Value";
            // 
            // addValueBtn
            // 
            addValueBtn.BackColor = System.Drawing.Color.FromArgb(66, 214, 237);
            addValueBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            addValueBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(33, 111, 179);
            addValueBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            addValueBtn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            addValueBtn.ForeColor = System.Drawing.Color.Black;
            addValueBtn.Location = new System.Drawing.Point(804, 220);
            addValueBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            addValueBtn.Name = "addValueBtn";
            addValueBtn.Size = new System.Drawing.Size(146, 30);
            addValueBtn.TabIndex = 51;
            addValueBtn.Text = "Add Value";
            addValueBtn.UseVisualStyleBackColor = false;
            // 
            // propertyValueLabel
            // 
            propertyValueLabel.AutoSize = true;
            propertyValueLabel.BackColor = System.Drawing.Color.Transparent;
            propertyValueLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            propertyValueLabel.ForeColor = System.Drawing.Color.White;
            propertyValueLabel.Location = new System.Drawing.Point(50, 300);
            propertyValueLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            propertyValueLabel.Name = "propertyValueLabel";
            propertyValueLabel.Size = new System.Drawing.Size(299, 29);
            propertyValueLabel.TabIndex = 52;
            propertyValueLabel.Text = "Selected Property Values";
            // 
            // TraverseKnowledgeGraph
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackgroundImage = Properties.Resources.background_landing__2_;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(984, 461);
            ControlBox = false;
            Controls.Add(propertyValueLabel);
            Controls.Add(addValueBtn);
            Controls.Add(valueLoading);
            Controls.Add(valueReq);
            Controls.Add(valueLabel);
            Controls.Add(closeForm);
            Controls.Add(propLoading);
            Controls.Add(prop1);
            Controls.Add(helpButton);
            Controls.Add(exploreFurtherBtn);
            Controls.Add(runTraverseBtn);
            Controls.Add(propReq);
            Controls.Add(value1);
            Controls.Add(propLabel);
            Controls.Add(exploreProperties);
            DoubleBuffered = true;
            HelpButton = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TraverseKnowledgeGraph";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Explore Knowledge Graph";
            FormClosing += TraverseKnowledgeGraph_FormClosing;
            ((System.ComponentModel.ISupportInitialize)propLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)valueLoading).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label exploreProperties;
        private System.Windows.Forms.Label propReq;
        private System.Windows.Forms.ComboBox value1;
        private System.Windows.Forms.Label propLabel;
        private System.Windows.Forms.Button runTraverseBtn;
        private System.Windows.Forms.Button exploreFurtherBtn;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.ComboBox prop1;
        private System.Windows.Forms.PictureBox propLoading;
        private System.Windows.Forms.Button closeForm;
        private System.Windows.Forms.PictureBox valueLoading;
        private System.Windows.Forms.Label valueReq;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.Button addValueBtn;
        private System.Windows.Forms.Label propertyValueLabel;
    }
}
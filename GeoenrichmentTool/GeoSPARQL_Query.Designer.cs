
namespace GeoenrichmentTool
{
    partial class GeoSPARQL_Query
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoSPARQL_Query));
            this.endPointLabel = new System.Windows.Forms.Label();
            this.endPoint = new System.Windows.Forms.TextBox();
            this.geoFormName = new System.Windows.Forms.Label();
            this.placeTypeLabel = new System.Windows.Forms.Label();
            this.placeType = new System.Windows.Forms.TextBox();
            this.subclassReasoning = new System.Windows.Forms.CheckBox();
            this.calculatorLabel = new System.Windows.Forms.Label();
            this.calculator = new System.Windows.Forms.ComboBox();
            this.submitFormBtn = new System.Windows.Forms.Button();
            this.classNameLabel = new System.Windows.Forms.Label();
            this.className = new System.Windows.Forms.TextBox();
            this.requiredEndpoint = new System.Windows.Forms.Label();
            this.requiredClassName = new System.Windows.Forms.Label();
            this.formError = new System.Windows.Forms.Label();
            this.outputLocation = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // endPointLabel
            // 
            this.endPointLabel.AutoSize = true;
            this.endPointLabel.BackColor = System.Drawing.Color.Transparent;
            this.endPointLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endPointLabel.ForeColor = System.Drawing.Color.White;
            this.endPointLabel.Location = new System.Drawing.Point(25, 95);
            this.endPointLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.endPointLabel.Name = "endPointLabel";
            this.endPointLabel.Size = new System.Drawing.Size(186, 19);
            this.endPointLabel.TabIndex = 0;
            this.endPointLabel.Text = "GeoSPARQL Endpoint:";
            // 
            // endPoint
            // 
            this.endPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.endPoint.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endPoint.Location = new System.Drawing.Point(19, 118);
            this.endPoint.Name = "endPoint";
            this.endPoint.Size = new System.Drawing.Size(740, 26);
            this.endPoint.TabIndex = 1;
            // 
            // geoFormName
            // 
            this.geoFormName.AutoSize = true;
            this.geoFormName.BackColor = System.Drawing.Color.Transparent;
            this.geoFormName.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.geoFormName.ForeColor = System.Drawing.Color.White;
            this.geoFormName.Location = new System.Drawing.Point(12, 9);
            this.geoFormName.Name = "geoFormName";
            this.geoFormName.Size = new System.Drawing.Size(306, 37);
            this.geoFormName.TabIndex = 2;
            this.geoFormName.Text = "GeoSPARQL Query";
            // 
            // placeTypeLabel
            // 
            this.placeTypeLabel.AutoSize = true;
            this.placeTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.placeTypeLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.placeTypeLabel.ForeColor = System.Drawing.Color.White;
            this.placeTypeLabel.Location = new System.Drawing.Point(15, 160);
            this.placeTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.placeTypeLabel.Name = "placeTypeLabel";
            this.placeTypeLabel.Size = new System.Drawing.Size(98, 19);
            this.placeTypeLabel.TabIndex = 3;
            this.placeTypeLabel.Text = "Place Type:";
            // 
            // placeType
            // 
            this.placeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.placeType.Font = new System.Drawing.Font("Arial", 12F);
            this.placeType.Location = new System.Drawing.Point(19, 183);
            this.placeType.Name = "placeType";
            this.placeType.Size = new System.Drawing.Size(740, 26);
            this.placeType.TabIndex = 4;
            // 
            // subclassReasoning
            // 
            this.subclassReasoning.AutoSize = true;
            this.subclassReasoning.BackColor = System.Drawing.Color.Transparent;
            this.subclassReasoning.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.subclassReasoning.ForeColor = System.Drawing.Color.White;
            this.subclassReasoning.Location = new System.Drawing.Point(19, 232);
            this.subclassReasoning.Name = "subclassReasoning";
            this.subclassReasoning.Size = new System.Drawing.Size(334, 23);
            this.subclassReasoning.TabIndex = 5;
            this.subclassReasoning.Text = "Disable Transistive Subclass Reasoning";
            this.subclassReasoning.UseVisualStyleBackColor = false;
            // 
            // calculatorLabel
            // 
            this.calculatorLabel.AutoSize = true;
            this.calculatorLabel.BackColor = System.Drawing.Color.Transparent;
            this.calculatorLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.calculatorLabel.ForeColor = System.Drawing.Color.White;
            this.calculatorLabel.Location = new System.Drawing.Point(15, 275);
            this.calculatorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.calculatorLabel.Name = "calculatorLabel";
            this.calculatorLabel.Size = new System.Drawing.Size(216, 19);
            this.calculatorLabel.TabIndex = 6;
            this.calculatorLabel.Text = "Spatial Relation Calculator:";
            // 
            // calculator
            // 
            this.calculator.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.calculator.FormattingEnabled = true;
            this.calculator.Items.AddRange(new object[] {
            "Contain + Intersect",
            "Contain",
            "Within",
            "Intersect"});
            this.calculator.Location = new System.Drawing.Point(19, 299);
            this.calculator.Name = "calculator";
            this.calculator.Size = new System.Drawing.Size(740, 27);
            this.calculator.TabIndex = 7;
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
            this.submitFormBtn.TabIndex = 8;
            this.submitFormBtn.Text = "Run";
            this.submitFormBtn.UseVisualStyleBackColor = false;
            this.submitFormBtn.Click += new System.EventHandler(this.submitGeoQueryForm);
            // 
            // classNameLabel
            // 
            this.classNameLabel.AutoSize = true;
            this.classNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.classNameLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.classNameLabel.ForeColor = System.Drawing.Color.White;
            this.classNameLabel.Location = new System.Drawing.Point(25, 343);
            this.classNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.classNameLabel.Name = "classNameLabel";
            this.classNameLabel.Size = new System.Drawing.Size(162, 19);
            this.classNameLabel.TabIndex = 10;
            this.classNameLabel.Text = "Feature Class Name";
            // 
            // className
            // 
            this.className.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.className.Font = new System.Drawing.Font("Arial", 12F);
            this.className.Location = new System.Drawing.Point(19, 366);
            this.className.Name = "className";
            this.className.Size = new System.Drawing.Size(740, 26);
            this.className.TabIndex = 11;
            // 
            // requiredEndpoint
            // 
            this.requiredEndpoint.AutoSize = true;
            this.requiredEndpoint.BackColor = System.Drawing.Color.Transparent;
            this.requiredEndpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiredEndpoint.ForeColor = System.Drawing.Color.Red;
            this.requiredEndpoint.Location = new System.Drawing.Point(15, 95);
            this.requiredEndpoint.Name = "requiredEndpoint";
            this.requiredEndpoint.Size = new System.Drawing.Size(15, 20);
            this.requiredEndpoint.TabIndex = 14;
            this.requiredEndpoint.Text = "*";
            // 
            // requiredClassName
            // 
            this.requiredClassName.AutoSize = true;
            this.requiredClassName.BackColor = System.Drawing.Color.Transparent;
            this.requiredClassName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiredClassName.ForeColor = System.Drawing.Color.Red;
            this.requiredClassName.Location = new System.Drawing.Point(15, 343);
            this.requiredClassName.Name = "requiredClassName";
            this.requiredClassName.Size = new System.Drawing.Size(15, 20);
            this.requiredClassName.TabIndex = 15;
            this.requiredClassName.Text = "*";
            // 
            // formError
            // 
            this.formError.AutoSize = true;
            this.formError.ForeColor = System.Drawing.Color.Red;
            this.formError.Location = new System.Drawing.Point(473, 428);
            this.formError.Name = "formError";
            this.formError.Size = new System.Drawing.Size(0, 20);
            this.formError.TabIndex = 16;
            // 
            // outputLocation
            // 
            this.outputLocation.Filter = "All|*.*|File Geodatabases|*.gdb";
            this.outputLocation.Title = "Select a GDB file to save to...";
            // 
            // GeoSPARQL_Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(778, 471);
            this.Controls.Add(this.formError);
            this.Controls.Add(this.requiredClassName);
            this.Controls.Add(this.requiredEndpoint);
            this.Controls.Add(this.className);
            this.Controls.Add(this.classNameLabel);
            this.Controls.Add(this.submitFormBtn);
            this.Controls.Add(this.calculator);
            this.Controls.Add(this.calculatorLabel);
            this.Controls.Add(this.subclassReasoning);
            this.Controls.Add(this.placeType);
            this.Controls.Add(this.placeTypeLabel);
            this.Controls.Add(this.geoFormName);
            this.Controls.Add(this.endPoint);
            this.Controls.Add(this.endPointLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GeoSPARQL_Query";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label endPointLabel;
        private System.Windows.Forms.TextBox endPoint;
        private System.Windows.Forms.Label geoFormName;
        private System.Windows.Forms.Label placeTypeLabel;
        private System.Windows.Forms.TextBox placeType;
        private System.Windows.Forms.CheckBox subclassReasoning;
        private System.Windows.Forms.Label calculatorLabel;
        private System.Windows.Forms.ComboBox calculator;
        private System.Windows.Forms.Button submitFormBtn;
        private System.Windows.Forms.Label classNameLabel;
        private System.Windows.Forms.TextBox className;
        private System.Windows.Forms.Label requiredEndpoint;
        private System.Windows.Forms.Label requiredClassName;
        private System.Windows.Forms.Label formError;
        private System.Windows.Forms.OpenFileDialog outputLocation;
    }
}
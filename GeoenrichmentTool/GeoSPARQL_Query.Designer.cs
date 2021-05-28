
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
            this.endPointLabel = new System.Windows.Forms.Label();
            this.endPoint = new System.Windows.Forms.TextBox();
            this.geoFormName = new System.Windows.Forms.Label();
            this.placeTypeLabel = new System.Windows.Forms.Label();
            this.placeType = new System.Windows.Forms.TextBox();
            this.subclassReasoning = new System.Windows.Forms.CheckBox();
            this.calculatorLabel = new System.Windows.Forms.Label();
            this.calculator = new System.Windows.Forms.ComboBox();
            this.submitFormBtn = new System.Windows.Forms.Button();
            this.resultFolderLabel = new System.Windows.Forms.Label();
            this.classNameLabel = new System.Windows.Forms.Label();
            this.className = new System.Windows.Forms.TextBox();
            this.requiredEndpoint = new System.Windows.Forms.Label();
            this.requiredClassName = new System.Windows.Forms.Label();
            this.formError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // endPointLabel
            // 
            this.endPointLabel.AutoSize = true;
            this.endPointLabel.Location = new System.Drawing.Point(25, 95);
            this.endPointLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.endPointLabel.Name = "endPointLabel";
            this.endPointLabel.Size = new System.Drawing.Size(196, 20);
            this.endPointLabel.TabIndex = 0;
            this.endPointLabel.Text = "GeoSPARQL Endpoint:";
            // 
            // endPoint
            // 
            this.endPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endPoint.Location = new System.Drawing.Point(19, 118);
            this.endPoint.Name = "endPoint";
            this.endPoint.Size = new System.Drawing.Size(740, 26);
            this.endPoint.TabIndex = 1;
            this.endPoint.Text = "http://stko-roy.geog.ucsb.edu:7202/repositories/plume_soil_wildfire";
            // 
            // geoFormName
            // 
            this.geoFormName.AutoSize = true;
            this.geoFormName.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.geoFormName.Location = new System.Drawing.Point(12, 9);
            this.geoFormName.Name = "geoFormName";
            this.geoFormName.Size = new System.Drawing.Size(316, 37);
            this.geoFormName.TabIndex = 2;
            this.geoFormName.Text = "GeoSPARQL Query";
            // 
            // placeTypeLabel
            // 
            this.placeTypeLabel.AutoSize = true;
            this.placeTypeLabel.Location = new System.Drawing.Point(15, 147);
            this.placeTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.placeTypeLabel.Name = "placeTypeLabel";
            this.placeTypeLabel.Size = new System.Drawing.Size(101, 20);
            this.placeTypeLabel.TabIndex = 3;
            this.placeTypeLabel.Text = "Place Type:";
            // 
            // placeType
            // 
            this.placeType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.placeType.Location = new System.Drawing.Point(19, 170);
            this.placeType.Name = "placeType";
            this.placeType.Size = new System.Drawing.Size(740, 26);
            this.placeType.TabIndex = 4;
            // 
            // subclassReasoning
            // 
            this.subclassReasoning.AutoSize = true;
            this.subclassReasoning.Location = new System.Drawing.Point(19, 203);
            this.subclassReasoning.Name = "subclassReasoning";
            this.subclassReasoning.Size = new System.Drawing.Size(348, 24);
            this.subclassReasoning.TabIndex = 5;
            this.subclassReasoning.Text = "Disable Transistive Subclass Reasoning";
            this.subclassReasoning.UseVisualStyleBackColor = true;
            // 
            // calculatorLabel
            // 
            this.calculatorLabel.AutoSize = true;
            this.calculatorLabel.Location = new System.Drawing.Point(15, 230);
            this.calculatorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.calculatorLabel.Name = "calculatorLabel";
            this.calculatorLabel.Size = new System.Drawing.Size(228, 20);
            this.calculatorLabel.TabIndex = 6;
            this.calculatorLabel.Text = "Spatial Relation Calculator:";
            // 
            // calculator
            // 
            this.calculator.FormattingEnabled = true;
            this.calculator.Items.AddRange(new object[] {
            "Contain + Intersect",
            "Contain",
            "Within",
            "Intersect"});
            this.calculator.Location = new System.Drawing.Point(19, 254);
            this.calculator.Name = "calculator";
            this.calculator.Size = new System.Drawing.Size(740, 28);
            this.calculator.TabIndex = 7;
            // 
            // submitFormBtn
            // 
            this.submitFormBtn.Location = new System.Drawing.Point(691, 424);
            this.submitFormBtn.Name = "submitFormBtn";
            this.submitFormBtn.Size = new System.Drawing.Size(75, 29);
            this.submitFormBtn.TabIndex = 8;
            this.submitFormBtn.Text = "Run";
            this.submitFormBtn.UseVisualStyleBackColor = true;
            this.submitFormBtn.Click += new System.EventHandler(this.submitGeoQueryForm);
            // 
            // resultFolderLabel
            // 
            this.resultFolderLabel.AutoSize = true;
            this.resultFolderLabel.Location = new System.Drawing.Point(15, 285);
            this.resultFolderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.resultFolderLabel.Name = "resultFolderLabel";
            this.resultFolderLabel.Size = new System.Drawing.Size(169, 20);
            this.resultFolderLabel.TabIndex = 9;
            this.resultFolderLabel.Text = "Query Result Folder";
            // 
            // classNameLabel
            // 
            this.classNameLabel.AutoSize = true;
            this.classNameLabel.Location = new System.Drawing.Point(25, 305);
            this.classNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.classNameLabel.Name = "classNameLabel";
            this.classNameLabel.Size = new System.Drawing.Size(172, 20);
            this.classNameLabel.TabIndex = 10;
            this.classNameLabel.Text = "Feature Class Name";
            // 
            // className
            // 
            this.className.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.className.Location = new System.Drawing.Point(19, 328);
            this.className.Name = "className";
            this.className.Size = new System.Drawing.Size(740, 26);
            this.className.TabIndex = 11;
            // 
            // requiredEndpoint
            // 
            this.requiredEndpoint.AutoSize = true;
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
            this.requiredClassName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiredClassName.ForeColor = System.Drawing.Color.Red;
            this.requiredClassName.Location = new System.Drawing.Point(15, 305);
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
            // GeoSPARQL_Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(778, 465);
            this.Controls.Add(this.formError);
            this.Controls.Add(this.requiredClassName);
            this.Controls.Add(this.requiredEndpoint);
            this.Controls.Add(this.className);
            this.Controls.Add(this.classNameLabel);
            this.Controls.Add(this.resultFolderLabel);
            this.Controls.Add(this.submitFormBtn);
            this.Controls.Add(this.calculator);
            this.Controls.Add(this.calculatorLabel);
            this.Controls.Add(this.subclassReasoning);
            this.Controls.Add(this.placeType);
            this.Controls.Add(this.placeTypeLabel);
            this.Controls.Add(this.geoFormName);
            this.Controls.Add(this.endPoint);
            this.Controls.Add(this.endPointLabel);
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
        private System.Windows.Forms.Label resultFolderLabel;
        private System.Windows.Forms.Label classNameLabel;
        private System.Windows.Forms.TextBox className;
        private System.Windows.Forms.Label requiredEndpoint;
        private System.Windows.Forms.Label requiredClassName;
        private System.Windows.Forms.Label formError;
    }
}
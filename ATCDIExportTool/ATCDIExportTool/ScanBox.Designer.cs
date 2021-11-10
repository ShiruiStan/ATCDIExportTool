namespace ATCDIExportTool
{
    partial class ScanBox
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
            this.ScanButton = new System.Windows.Forms.Button();
            this.ScanText = new System.Windows.Forms.TextBox();
            this.ExportCenter = new System.Windows.Forms.Button();
            this.ExportGltf = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(96, 12);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(110, 47);
            this.ScanButton.TabIndex = 0;
            this.ScanButton.Text = "扫描元素";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // ScanText
            // 
            this.ScanText.Location = new System.Drawing.Point(22, 65);
            this.ScanText.Multiline = true;
            this.ScanText.Name = "ScanText";
            this.ScanText.ReadOnly = true;
            this.ScanText.Size = new System.Drawing.Size(256, 171);
            this.ScanText.TabIndex = 1;
            // 
            // ExportCenter
            // 
            this.ExportCenter.Location = new System.Drawing.Point(22, 263);
            this.ExportCenter.Name = "ExportCenter";
            this.ExportCenter.Size = new System.Drawing.Size(107, 47);
            this.ExportCenter.TabIndex = 0;
            this.ExportCenter.Text = "导出元素中心";
            this.ExportCenter.UseVisualStyleBackColor = true;
            this.ExportCenter.Click += new System.EventHandler(this.ExportCenter_Click);
            // 
            // ExportGltf
            // 
            this.ExportGltf.Location = new System.Drawing.Point(170, 263);
            this.ExportGltf.Name = "ExportGltf";
            this.ExportGltf.Size = new System.Drawing.Size(108, 47);
            this.ExportGltf.TabIndex = 0;
            this.ExportGltf.Text = "导出  GLTF";
            this.ExportGltf.UseVisualStyleBackColor = true;
            this.ExportGltf.Click += new System.EventHandler(this.ExportGltf_Click);
            // 
            // ScanBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 334);
            this.Controls.Add(this.ScanText);
            this.Controls.Add(this.ExportGltf);
            this.Controls.Add(this.ExportCenter);
            this.Controls.Add(this.ScanButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ScanBox";
            this.Text = "ScanBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.TextBox ScanText;
        private System.Windows.Forms.Button ExportGltf;
        private System.Windows.Forms.Button ExportCenter;
    }
}
namespace ATCDIExportTool
{
    partial class ExportGltfBox
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
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ExportButton = new System.Windows.Forms.Button();
            this.PositionGroup = new System.Windows.Forms.GroupBox();
            this.BaseX = new System.Windows.Forms.TextBox();
            this.BaseY = new System.Windows.Forms.TextBox();
            this.BaseZ = new System.Windows.Forms.TextBox();
            this.LabelX = new System.Windows.Forms.Label();
            this.LabelY = new System.Windows.Forms.Label();
            this.LabelZ = new System.Windows.Forms.Label();
            this.SelectButton = new System.Windows.Forms.Button();
            this.PositionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(24, 326);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(413, 23);
            this.ProgressBar.TabIndex = 0;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(457, 313);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(75, 23);
            this.ExportButton.TabIndex = 1;
            this.ExportButton.Text = "导出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // PositionGroup
            // 
            this.PositionGroup.Controls.Add(this.SelectButton);
            this.PositionGroup.Controls.Add(this.LabelZ);
            this.PositionGroup.Controls.Add(this.LabelY);
            this.PositionGroup.Controls.Add(this.LabelX);
            this.PositionGroup.Controls.Add(this.BaseZ);
            this.PositionGroup.Controls.Add(this.BaseY);
            this.PositionGroup.Controls.Add(this.BaseX);
            this.PositionGroup.Location = new System.Drawing.Point(24, 12);
            this.PositionGroup.Name = "PositionGroup";
            this.PositionGroup.Size = new System.Drawing.Size(281, 225);
            this.PositionGroup.TabIndex = 2;
            this.PositionGroup.TabStop = false;
            this.PositionGroup.Text = "坐标变换";
            // 
            // BaseX
            // 
            this.BaseX.Location = new System.Drawing.Point(64, 42);
            this.BaseX.Name = "BaseX";
            this.BaseX.ReadOnly = true;
            this.BaseX.Size = new System.Drawing.Size(100, 21);
            this.BaseX.TabIndex = 0;
            // 
            // BaseY
            // 
            this.BaseY.Location = new System.Drawing.Point(64, 90);
            this.BaseY.Name = "BaseY";
            this.BaseY.ReadOnly = true;
            this.BaseY.Size = new System.Drawing.Size(100, 21);
            this.BaseY.TabIndex = 0;
            // 
            // BaseZ
            // 
            this.BaseZ.Location = new System.Drawing.Point(64, 138);
            this.BaseZ.Name = "BaseZ";
            this.BaseZ.ReadOnly = true;
            this.BaseZ.Size = new System.Drawing.Size(100, 21);
            this.BaseZ.TabIndex = 0;
            // 
            // LabelX
            // 
            this.LabelX.AutoSize = true;
            this.LabelX.Location = new System.Drawing.Point(11, 45);
            this.LabelX.Name = "LabelX";
            this.LabelX.Size = new System.Drawing.Size(47, 12);
            this.LabelX.TabIndex = 1;
            this.LabelX.Text = "参考点X";
            // 
            // LabelY
            // 
            this.LabelY.AutoSize = true;
            this.LabelY.Location = new System.Drawing.Point(11, 93);
            this.LabelY.Name = "LabelY";
            this.LabelY.Size = new System.Drawing.Size(47, 12);
            this.LabelY.TabIndex = 1;
            this.LabelY.Text = "参考点Y";
            // 
            // LabelZ
            // 
            this.LabelZ.AutoSize = true;
            this.LabelZ.Location = new System.Drawing.Point(11, 141);
            this.LabelZ.Name = "LabelZ";
            this.LabelZ.Size = new System.Drawing.Size(47, 12);
            this.LabelZ.TabIndex = 1;
            this.LabelZ.Text = "参考点Z";
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(58, 184);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(75, 23);
            this.SelectButton.TabIndex = 2;
            this.SelectButton.Text = "拾取参考点";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // ExportGltfBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 370);
            this.Controls.Add(this.PositionGroup);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.ProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ExportGltfBox";
            this.Text = "导出设置";
            this.PositionGroup.ResumeLayout(false);
            this.PositionGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.GroupBox PositionGroup;
        private System.Windows.Forms.Label LabelX;
        private System.Windows.Forms.Label LabelZ;
        private System.Windows.Forms.Label LabelY;
        private System.Windows.Forms.Button SelectButton;
        public System.Windows.Forms.TextBox BaseZ;
        public System.Windows.Forms.TextBox BaseY;
        public System.Windows.Forms.TextBox BaseX;
    }
}
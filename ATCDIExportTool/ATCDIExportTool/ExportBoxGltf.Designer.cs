namespace ATCDIExportTool
{
    partial class ExportBoxGltf
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
            this.ExportButton = new System.Windows.Forms.Button();
            this.FileGroup = new System.Windows.Forms.GroupBox();
            this.Gltf = new System.Windows.Forms.RadioButton();
            this.Glb = new System.Windows.Forms.RadioButton();
            this.MeshParas = new System.Windows.Forms.GroupBox();
            this.MaxAngleText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.MaxLengthText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ChordTexT = new System.Windows.Forms.TextBox();
            this.ChordLabel = new System.Windows.Forms.Label();
            this.ExportProgress = new System.Windows.Forms.ProgressBar();
            this.AixsGroup = new System.Windows.Forms.GroupBox();
            this.Zup = new System.Windows.Forms.RadioButton();
            this.Yup = new System.Windows.Forms.RadioButton();
            this.GltfEmbed = new System.Windows.Forms.RadioButton();
            this.FileGroup.SuspendLayout();
            this.MeshParas.SuspendLayout();
            this.AixsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(54, 300);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(100, 34);
            this.ExportButton.TabIndex = 1;
            this.ExportButton.Text = "导出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // FileGroup
            // 
            this.FileGroup.Controls.Add(this.GltfEmbed);
            this.FileGroup.Controls.Add(this.Gltf);
            this.FileGroup.Controls.Add(this.Glb);
            this.FileGroup.Location = new System.Drawing.Point(12, 231);
            this.FileGroup.Name = "FileGroup";
            this.FileGroup.Size = new System.Drawing.Size(185, 63);
            this.FileGroup.TabIndex = 3;
            this.FileGroup.TabStop = false;
            this.FileGroup.Text = "文件格式";
            // 
            // Gltf
            // 
            this.Gltf.AutoSize = true;
            this.Gltf.Location = new System.Drawing.Point(53, 30);
            this.Gltf.Name = "Gltf";
            this.Gltf.Size = new System.Drawing.Size(47, 16);
            this.Gltf.TabIndex = 1;
            this.Gltf.TabStop = true;
            this.Gltf.Text = "gltf";
            this.Gltf.UseVisualStyleBackColor = true;
            // 
            // Glb
            // 
            this.Glb.AutoSize = true;
            this.Glb.Checked = true;
            this.Glb.Location = new System.Drawing.Point(6, 30);
            this.Glb.Name = "Glb";
            this.Glb.Size = new System.Drawing.Size(41, 16);
            this.Glb.TabIndex = 0;
            this.Glb.TabStop = true;
            this.Glb.Text = "glb";
            this.Glb.UseVisualStyleBackColor = true;
            // 
            // MeshParas
            // 
            this.MeshParas.Controls.Add(this.MaxAngleText);
            this.MeshParas.Controls.Add(this.label6);
            this.MeshParas.Controls.Add(this.MaxLengthText);
            this.MeshParas.Controls.Add(this.label5);
            this.MeshParas.Controls.Add(this.ChordTexT);
            this.MeshParas.Controls.Add(this.ChordLabel);
            this.MeshParas.Location = new System.Drawing.Point(12, 81);
            this.MeshParas.Name = "MeshParas";
            this.MeshParas.Size = new System.Drawing.Size(185, 144);
            this.MeshParas.TabIndex = 4;
            this.MeshParas.TabStop = false;
            this.MeshParas.Text = "模型精度控制";
            // 
            // MaxAngleText
            // 
            this.MaxAngleText.Location = new System.Drawing.Point(69, 104);
            this.MaxAngleText.Name = "MaxAngleText";
            this.MaxAngleText.Size = new System.Drawing.Size(100, 21);
            this.MaxAngleText.TabIndex = 0;
            this.MaxAngleText.Text = "10";
            this.MaxAngleText.Leave += new System.EventHandler(this.Num_Validator);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "最大转角";
            // 
            // MaxLengthText
            // 
            this.MaxLengthText.Location = new System.Drawing.Point(69, 63);
            this.MaxLengthText.Name = "MaxLengthText";
            this.MaxLengthText.Size = new System.Drawing.Size(100, 21);
            this.MaxLengthText.TabIndex = 0;
            this.MaxLengthText.Text = "5";
            this.MaxLengthText.Leave += new System.EventHandler(this.Num_Validator);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "最大边长";
            // 
            // ChordTexT
            // 
            this.ChordTexT.Location = new System.Drawing.Point(69, 26);
            this.ChordTexT.Name = "ChordTexT";
            this.ChordTexT.Size = new System.Drawing.Size(100, 21);
            this.ChordTexT.TabIndex = 0;
            this.ChordTexT.Text = "1";
            this.ChordTexT.Leave += new System.EventHandler(this.Num_Validator);
            // 
            // ChordLabel
            // 
            this.ChordLabel.AutoSize = true;
            this.ChordLabel.Location = new System.Drawing.Point(22, 29);
            this.ChordLabel.Name = "ChordLabel";
            this.ChordLabel.Size = new System.Drawing.Size(41, 12);
            this.ChordLabel.TabIndex = 1;
            this.ChordLabel.Text = "弦弧差";
            // 
            // ExportProgress
            // 
            this.ExportProgress.Location = new System.Drawing.Point(12, 340);
            this.ExportProgress.Name = "ExportProgress";
            this.ExportProgress.Size = new System.Drawing.Size(185, 23);
            this.ExportProgress.TabIndex = 5;
            // 
            // AixsGroup
            // 
            this.AixsGroup.Controls.Add(this.Zup);
            this.AixsGroup.Controls.Add(this.Yup);
            this.AixsGroup.Location = new System.Drawing.Point(12, 12);
            this.AixsGroup.Name = "AixsGroup";
            this.AixsGroup.Size = new System.Drawing.Size(185, 63);
            this.AixsGroup.TabIndex = 3;
            this.AixsGroup.TabStop = false;
            this.AixsGroup.Text = "文件格式";
            // 
            // Zup
            // 
            this.Zup.AutoSize = true;
            this.Zup.Location = new System.Drawing.Point(95, 30);
            this.Zup.Name = "Zup";
            this.Zup.Size = new System.Drawing.Size(65, 16);
            this.Zup.TabIndex = 1;
            this.Zup.TabStop = true;
            this.Zup.Text = "Z轴向上";
            this.Zup.UseVisualStyleBackColor = true;
            // 
            // Yup
            // 
            this.Yup.AutoSize = true;
            this.Yup.Checked = true;
            this.Yup.Location = new System.Drawing.Point(24, 30);
            this.Yup.Name = "Yup";
            this.Yup.Size = new System.Drawing.Size(65, 16);
            this.Yup.TabIndex = 0;
            this.Yup.TabStop = true;
            this.Yup.Text = "Y轴向上";
            this.Yup.UseVisualStyleBackColor = true;
            // 
            // GltfEmbed
            // 
            this.GltfEmbed.AutoSize = true;
            this.GltfEmbed.Location = new System.Drawing.Point(106, 30);
            this.GltfEmbed.Name = "GltfEmbed";
            this.GltfEmbed.Size = new System.Drawing.Size(83, 16);
            this.GltfEmbed.TabIndex = 1;
            this.GltfEmbed.TabStop = true;
            this.GltfEmbed.Text = "gltf-embed";
            this.GltfEmbed.UseVisualStyleBackColor = true;
            // 
            // ExportBoxGltf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 375);
            this.Controls.Add(this.ExportProgress);
            this.Controls.Add(this.MeshParas);
            this.Controls.Add(this.AixsGroup);
            this.Controls.Add(this.FileGroup);
            this.Controls.Add(this.ExportButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ExportBoxGltf";
            this.Text = "导出设置";
            this.FileGroup.ResumeLayout(false);
            this.FileGroup.PerformLayout();
            this.MeshParas.ResumeLayout(false);
            this.MeshParas.PerformLayout();
            this.AixsGroup.ResumeLayout(false);
            this.AixsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.GroupBox FileGroup;
        private System.Windows.Forms.RadioButton Gltf;
        private System.Windows.Forms.RadioButton Glb;
        private System.Windows.Forms.GroupBox MeshParas;
        private System.Windows.Forms.TextBox MaxAngleText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox MaxLengthText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ChordTexT;
        private System.Windows.Forms.Label ChordLabel;
        private System.Windows.Forms.GroupBox AixsGroup;
        private System.Windows.Forms.RadioButton Zup;
        private System.Windows.Forms.RadioButton Yup;
        public System.Windows.Forms.ProgressBar ExportProgress;
        private System.Windows.Forms.RadioButton GltfEmbed;
    }
}
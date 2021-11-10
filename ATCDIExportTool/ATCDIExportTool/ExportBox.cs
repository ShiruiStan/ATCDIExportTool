using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Bentley.DgnPlatformNET;

namespace ATCDIExportTool
{
    public partial class ExportBox : Form
    {
        private ExportFile export;
        private string directory;
        private MeshTool meshTool;

        public ExportBox(ExportFile export)
        {
            InitializeComponent();
            this.export = export;
            this.TopMost = true;
            this.ExportProgress.Value = 0;
            this.ExportProgress.Step = 1;
            this.ExportProgress.Maximum = export.elements.Count;
            this.Show();

        }

        private void ExportButton_Click(object sender, EventArgs e)
        {

            this.Enabled = false;
            this.meshTool = new MeshTool(Convert.ToDouble(this.ChordTexT.Text), Convert.ToDouble(this.MaxLengthText.Text), Convert.ToDouble(this.MaxAngleText.Text));
            FolderBrowserDialog dialog = new FolderBrowserDialog() {
                Description = "选择导出路径",
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.directory = dialog.SelectedPath + "\\";
                StartExport();
            }
            this.Enabled = true;
        }


        private void Num_Validator(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(((TextBox)sender).Text, @"^[+-]?\d*[.]?\d*$"))
            {
                MessageBox.Show("请正确填写数字");
            }
        }

        private void StartExport()
        {
            foreach ( ExportElement el in this.export.elements)
            {
                ElementGraphicsOutput.Process(el.element, this.meshTool);
            }
        }
    }
}

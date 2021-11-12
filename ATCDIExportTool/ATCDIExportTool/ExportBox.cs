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
            TopMost = true;
            ExportProgress.Value = 0;
            ExportProgress.Step = 1;
            ExportProgress.Maximum = export.elements.Count;
            Show();

        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            ExportProgress.Value = 0;
            Enabled = false;
            meshTool = new MeshTool(Convert.ToDouble(ChordTexT.Text), Convert.ToDouble(MaxLengthText.Text), Convert.ToDouble(MaxAngleText.Text));
            FolderBrowserDialog dialog = new FolderBrowserDialog() {
                Description = "选择导出路径",
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                directory = dialog.SelectedPath + "\\";
                StartExport();
            }
            Enabled = true;
            MessageBox.Show("导出完成");
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
            foreach (ExportElement el in export.elements)
            {
                ElementGraphicsOutput.Process(el.element, meshTool);
                el.mesh = meshTool.output;
                ExportProgress.PerformStep();
                GltfTool gltf = new GltfTool(el, directory);
                gltf.StartExport(false);
            }
        }
    }
}

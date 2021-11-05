using System;
using System.Windows.Forms;

namespace ATCDIExportTool
{
    public partial class ExportGltfBox : Form
    {
        private ExportFile export;

        public ExportGltfBox(ExportFile export)
        {
            InitializeComponent();
            this.export = export;
            this.BaseX.Text = export.center.X.ToString();
            this.BaseY.Text = export.center.Y.ToString();
            this.BaseZ.Text = export.center.Z.ToString();
            this.TopMost = true;
            this.Show();

        }

        private void ExportButton_Click(object sender, EventArgs e)
        {

        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            SelectPointTool.InstallNewTool();
        }
    }
}

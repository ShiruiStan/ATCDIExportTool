using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATCDIExportTool
{
    public partial class ScanBox : Form
    {
        public ScanBox()
        {
            InitializeComponent();
            this.TopMost = true;
            this.Show();
        }

        public static void StartExportTool(string unparsed)
        {
            ScanBox mainBox = new ScanBox();
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            ScanTool tool = new ScanTool();
            tool.StartScan();
        }

        private void ExportCenter_Click(object sender, EventArgs e)
        {

        }

        private void ExportModel_Click(object sender, EventArgs e)
        {

        }
    }
}

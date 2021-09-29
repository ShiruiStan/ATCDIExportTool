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
            throw new Exception("测试错误");
        }
    }
}

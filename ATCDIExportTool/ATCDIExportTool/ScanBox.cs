using System;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace ATCDIExportTool
{
    public partial class ScanBox : Form
    {
        private ScanTool tool;
        public ScanBox()
        {
            InitializeComponent();
            this.TopMost = true;
            this.Show();
            this.tool = new ScanTool();
        }

        public static void StartExportTool(string unparsed)
        {
            ScanBox mainBox = new ScanBox();
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            this.ScanText.Text = tool.StartScan();
        }

        private void ExportCenter_Click(object sender, EventArgs e)
        {
            if (this.tool.export.elements.Count == 0)
            {
                MessageBox.Show("请重新执行扫描");
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Title = "选择保存文件位置",
                    Filter = "Excel|*.xlsx",
                    FileName = "构件中心点"
                };
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    XLWorkbook xls = new XLWorkbook();
                    IXLWorksheet sheet = xls.AddWorksheet();
                    sheet.Cell(1, 1).InsertData(new List<string>() { "guid", "elementId", "x", "y", "z" }, true);
                    foreach (ExportElement el in this.tool.export.elements)
                    {
                        sheet.LastRowUsed().RowBelow().Cell(1).InsertData(
                            new List<object>(){ el.props.ATCDI_guid, el.elementId, el.center.X, el.center.Y, el.center.Z}, true);
                    }
                    xls.SaveAs(dialog.FileName);
                    MessageBox.Show("导出完成");
                }
            }
        }


        private void ExportGltf_Click(object sender, EventArgs e)
        {
            if ( tool.export.elements.Count == 0)
            {
                MessageBox.Show("请重新执行扫描");
            }
            else
            {
                Close();
                new ExportBoxGltf(tool.export);
            }
        }

        private void Export3DTiles_Click(object sender, EventArgs e)
        {
            if (this.tool.export.elements.Count == 0)
            {
                MessageBox.Show("请重新执行扫描");
            }
            else
            {
                MessageBox.Show("功能开发中");
                // ExportBoxGltf box = new ExportBoxGltf(tool.export);
                // this.Close();
            }
        }
    }
}

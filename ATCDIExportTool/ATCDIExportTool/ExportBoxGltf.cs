using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;

namespace ATCDIExportTool
{
    public partial class ExportBoxGltf : Form
    {
        private ExportFile export;
        private string directory;
        double chord = 1;
        double maxLength = 10;
        double maxAngle = 10;

        public ExportBoxGltf(ExportFile export)
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
            chord = Convert.ToDouble(ChordTexT.Text);
            maxLength = Convert.ToDouble(MaxLengthText.Text);
            maxAngle = Convert.ToDouble(MaxAngleText.Text);
            FolderBrowserDialog dialog = new FolderBrowserDialog() {
                Description = "选择导出路径",
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                directory = dialog.SelectedPath + "\\";
                try
                {
                    StartExport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    Enabled = true;
                }
            }
            else
            {
                Enabled = true;
            }

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
            GltfTool gltf = new GltfTool(directory, Yup.Checked);
            foreach (ExportElement el in export.elements)
            {
                el.meshes.Clear();
                RecursionConvertElmentToMeshes(el, el.element);
                if (el.meshes.Count > 0)
                {
                    gltf.element.Add(el);
                }
                ExportProgress.PerformStep();
            }
            
            if (Glb.Checked)
            {
                gltf.StartGlbExport();
            }
            else if (Gltf.Checked)
            {
                gltf.StartGltfExport();
            }
            else if(GltfEmbed.Checked)
            {
                gltf.StartGltfEmbedExport();
            }
            
            MessageBox.Show("导出完成，导出元素个数：" + gltf.element.Count.ToString());
            Close();
        }

        // 后续扩展元素处理
        private void RecursionConvertElmentToMeshes(ExportElement res, Element el)
        {
            // TODO 共享单元待处理, 已在过滤函数中剔除
            switch (el.ElementType.ToString())
            {
                case "CellHeader":
                    if (el.TypeName == "智能实体/曲面")
                    {
                        //TransformInfo info = new TransformInfo(DTransform3d.Scale(10000));
                        //el.ApplyTransform(info);
                        TransElementToMesh(res, el);
                    }
                    else
                    {
                        foreach (Element child in el.GetChildren())
                        {
                            RecursionConvertElmentToMeshes(res, child);
                        }
                    }
                    break;
                case "SharedCellInstance":
                    break;
                case "SharedCellDefinition":
                    break;
                case "MeshHeader":
                    // 本来做的三角化可能存在每个面定点数多于3，需要经过工具处理一遍
                    // res.meshes.Add(((MeshHeaderElement)el).GetMeshData());
                    TransElementToMesh(res, el);
                    break;
                default:
                    TransElementToMesh(res, el); 
                    break;
            }
        }


        private void TransElementToMesh(ExportElement res, Element el)
        {
            
            
            MeshTool tool = new MeshTool(chord, maxLength, maxAngle);
            ElementGraphicsOutput.Process(el, tool);
            PolyfaceHeader mesh = new PolyfaceHeader();
            mesh.CopyFrom(tool.output);

            //MeshHeaderElement meshHeaderEle = new MeshHeaderElement(Session.Instance.GetActiveDgnModel(), null, mesh);
            //meshHeaderEle.AddToModel();

            // 对于参考文件中的元素，可能还需要加上一个tranform
            res.color = tool.color;
            res.meshes.Add(mesh);
            
        }
    }


}

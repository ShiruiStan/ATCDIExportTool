using System;
using System.Collections.Generic;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.GeometryNET;
using Newtonsoft.Json;
using System.Linq;
using System.Windows.Forms;

namespace ATCDIExportTool
{
    class ScanTool
    {
        private readonly List<string> ElementTypeFilter = new List<string>()
        {
            "CellHeader",
            "Shape",
            "ComplexShape",
            "Ellipse",
            "Surfce",
            "Solid",
            "Cone",
            "BsplineSurface",
            "SharedCellDefinition",
            "SharedCellInstance",
            "MeshHeader",
            "106"
        };
        private Dictionary<IntPtr, DgnModel> models = new Dictionary<IntPtr, DgnModel>();
        public ExportFile export = new ExportFile();

        public string StartScan()
        {
            models.Clear();
            export.Reset();
            ConfirmModel(Session.Instance.GetActiveDgnModel());
            Dictionary<string, List<string>> ignore = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> confirm = new Dictionary<string, List<string>>();
            foreach ( DgnModel model in models.Values)
            {
                foreach (Element el in model.GetGraphicElements())
                {
                    string id = el.ElementId.ToString();
                    string type = el.TypeName.ToString();
                    if (Utils.IsElementSupport(el))
                    {
                        export.AddElement(el);
                        if (confirm.ContainsKey(type))
                        {
                            confirm[type].Add(id);
                        }
                        else
                        {
                            confirm.Add(type, new List<string>() { id });
                        }
                    }
                    else
                    {
                        if (ignore.ContainsKey(type))
                        {
                            ignore[type].Add(id);
                        }
                        else
                        {
                            ignore.Add(type, new List<string>() {id });
                        }
                    }
                }
            }

            MessageCenter.Instance.ShowInfoMessage("忽略元素详细 ", 
                JsonConvert.SerializeObject(ignore.Select(x=> x.Key + " : " + string.Join(", ", x.Value)), Formatting.Indented), false);
            MessageCenter.Instance.ShowInfoMessage("选中元素详细",
                JsonConvert.SerializeObject(confirm.Select(x => x.Key + " : " + string.Join(", ", x.Value)), Formatting.Indented), false);

            return "total : " + confirm.Select(x => x.Value.Count).Sum() + Environment.NewLine +
                JsonConvert.SerializeObject(confirm.Select(x => x.Key + " : " + x.Value.Count), Formatting.Indented);
        }

        // 递归查找所有参考的dgn文件，也加入元素扫描的范围
        private void ConfirmModel(DgnModel model)
        {
            if (!models.ContainsKey(model.GetNative())){
                models.Add(model.GetNative(), model);
                model.ReadAndLoadDgnAttachments(new DgnAttachmentLoadOptions(true, true, true));
                foreach (var attach in model.GetDgnAttachments())
                {
                    if (attach.IsDisplayed && !attach.IsMissingFile)
                    {
                        ConfirmModel(attach.GetDgnModel());
                    }
                }
            }

        }

    }

    public class ExportElement
    {
        public readonly Element element;
        public readonly string elementId;
        public readonly string elementType;
        public readonly ExportProps props;
        public readonly DPoint3d center;
        public readonly DRange3d range;
        //public readonly DMatrix3d rotation;
        //public readonly Material material;
        public uint color;
        public List<PolyfaceHeader> meshes;


        public ExportElement(Element el)
        {
            element = el;
            props = Utils.GetElementProps(el);
            elementId = el.ElementId.ToString();
            elementType = el.ElementType.ToString(); 
            meshes = new List<PolyfaceHeader>();
            DRange3d uorRange;
            ((DisplayableElement)element).CalcElementRange(out uorRange);
            range = DRange3d.FromPoints(Utils.ConvertUorToMeter(uorRange.Low), Utils.ConvertUorToMeter(uorRange.High));
            center = DPoint3d.FromXYZ((range.Low.X + range.High.X) / 2, (range.Low.Y + range.High.Y) / 2, (range.Low.Z + range.High.Z) / 2);
            //((DisplayableElement)el).GetOrientation(out rotation); // 好像不需要
            //material = ((DisplayableElement)el).GetElementDisplayParameters(true).Material;

        }
    }

    public class ExportFile
    {
        public List<ExportElement> elements = new List<ExportElement>();

        public ExportFile()
        {
            Reset();
        }

        public void AddElement(Element el)
        {
            elements.Add(new ExportElement(el));
        }

        public void Reset()
        {
            elements.Clear();
        }
    }
}

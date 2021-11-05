using System;
using System.Collections.Generic;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.GeometryNET;
using Newtonsoft.Json;
using System.Linq;

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
            "MeshHeader"
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
                    string type = el.ElementType.ToString();
                    if (el.IsValid && !el.IsInvisible && ElementTypeFilter.Contains(type))
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

        private void ConfirmModel(DgnModel model)
        {
            if (!this.models.ContainsKey(model.GetNative())){
                this.models.Add(model.GetNative(), model);
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
        public readonly DPoint3d center;
        public readonly string guid;
        public readonly string elementId;
        public ExportElement(Element el)
        {
            element = el;
            center = Utils.GetElementCenter(el);
            guid = Utils.GetElementGuid(el);
            elementId = el.ElementId.ToString();
        }

    }

    public class ExportFile
    {
        public List<ExportElement> elements = new List<ExportElement>();
        public DPoint3d center;
        public DRange3d range;

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
            center = DPoint3d.Zero;
            range = DRange3d.NullRange;
            Session.Instance.GetActiveDgnModel().GetRange(out range);
            range.High = Utils.ConvertUorToMeter(range.High);
            range.Low = Utils.ConvertUorToMeter(range.Low);
            center = DPoint3d.FromXYZ((range.High.X + range.Low.X) / 2, (range.High.Y + range.Low.Y) / 2, (range.High.Z + range.Low.Z) / 2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.GeometryNET;

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
        private ExportFile export = new ExportFile();

        public void StartScan()
        {
            this.models.Clear();
            this.export.Reset();
            ConfirmModel(Session.Instance.GetActiveDgnModel());
            foreach ( DgnModel model in models.Values)
            {
                foreach (Element el in model.GetGraphicElements())
                {
                    if (el.IsValid && !el.IsInvisible && this.ElementTypeFilter.Contains(el.ElementType.ToString()))
                    {
                        this.export.AddElement(el);
                    }
                    else
                    {
                        MessageCenter.Instance.ShowInfoMessage("忽略元素说明", "", false);
                    }
                }
            }
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

    class ExportElement
    {
        public readonly Element element;
        public readonly DPoint3d center;
        public readonly string guid;
        public ExportElement(Element el)
        {
            this.element = el;
            this.center = Utils.GetElementCenter(el);
            this.guid = Utils.GetElementGuid(el);
        }

    }

    class ExportFile
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
            this.elements.Clear();
            this.center = DPoint3d.Zero;
            this.range = DRange3d.NullRange;
            Session.Instance.GetActiveDgnModel().GetRange(out range);
            range.High = Utils.ConvertUorToMeter(range.High);
            range.Low = Utils.ConvertUorToMeter(range.Low);
            center = DPoint3d.FromXYZ((range.High.X + range.Low.X) / 2, (range.High.Y + range.Low.Y) / 2, (range.High.Z + range.Low.Z) / 2);
        }
    }
}

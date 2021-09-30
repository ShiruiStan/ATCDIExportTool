using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;

namespace ATCDIExportTool
{
    class ScanTool
    {
        private enum ElementTypeFilter
        {
            CellHeader = 2,
            Shape = 6,
            ComplexShape = 14,
            Ellipse = 15,
            Surfce = 18,
            Solid = 19,
            Cone = 23,
            BsplineSurface = 24,
            SharedCellDefinition = 34,
            SharedCellInstance = 35,
            MeshHeader = 105
        }

        public void StartScan()
        {
            var models = FindAllModels(Session.Instance.GetActiveDgnModel());
            MessageBox.Show(models.Count.ToString());
            
        }

        private HashSet<DgnModel> FindAllModels(DgnModel model)
        {
            // TODO 待处理嵌套参考文件
            var attachs = new HashSet<DgnModel>{ model };
            model.ReadAndLoadDgnAttachments(new DgnAttachmentLoadOptions(true, true, true));
            foreach (var attach in model.GetDgnAttachments())
            {
                if (attach.IsDisplayed && !attach.IsMissingFile)
                {
                    attachs.UnionWith(FindAllModels(attach.GetDgnModel()));
                }
            }
            return attachs;
        }
    }
}

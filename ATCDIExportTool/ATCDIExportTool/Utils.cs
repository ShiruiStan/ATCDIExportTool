using System.Windows.Forms;
using Bentley.GeometryNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.DgnPlatformNET;

namespace ATCDIExportTool
{
    public class Utils
    {
        public static DPoint3d GetElementCenter(Element el)
        {
            DRange3d range = new DRange3d();
            ((DisplayableElement)el).CalcElementRange(out range);
            DPoint3d center = new DPoint3d();
            center.X = ConvertUorToMeter((range.Low.X + range.High.X) / 2);
            center.Y = ConvertUorToMeter((range.Low.Y + range.High.Y) / 2);
            center.Z = ConvertUorToMeter((range.Low.Z + range.High.Z) / 2);
            return center;
        }


        public static double ConvertUorToMeter(double uor)
        {
            return uor / Session.Instance.GetActiveDgnModel().GetModelInfo().UorPerMeter;
        }

        public static DPoint3d ConvertUorToMeter(DPoint3d point)
        {
            double uor = Session.Instance.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            point.X /= uor;
            point.Y /= uor;
            point.Z /= uor;
            return point;
        }


        public static string GetElementGuid(Element element)
        {

            return null;
        }
    }

    public class SelectPointTool : DgnPrimitiveTool
    {
        private ExportGltfBox form;

        public SelectPointTool() : base(0, 0)
        {
            form = (ExportGltfBox)Form.ActiveForm;
        }

        public static void InstallNewTool()
        {
            SelectPointTool selector = new SelectPointTool();
            selector.InstallTool();
        }
        protected override void OnPostInstall()
        {
            AccuSnap.LocateEnabled = true;
            AccuSnap.SnapEnabled = true;
            base.OnPostInstall();
        }

        protected override bool OnDataButton(DgnButtonEvent ev)
        {
            DPoint3d point = Utils.ConvertUorToMeter(ev.Point);
            form.BaseX.Text = point.X.ToString();
            form.BaseY.Text = point.Y.ToString();
            form.BaseZ.Text = point.Z.ToString();
            return true;
        }

        protected override bool OnResetButton(DgnButtonEvent ev)
        {
            ExitTool();
            return false;
        }

        protected override void OnRestartTool()
        {
            InstallNewTool();
        }
    }
}

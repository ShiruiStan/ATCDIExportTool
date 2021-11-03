using System;
using Bentley.GeometryNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;

namespace ATCDIExportTool
{
    class Utils
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
}

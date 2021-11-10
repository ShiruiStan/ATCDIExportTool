using System.Windows.Forms;
using Bentley.GeometryNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.DgnPlatformNET;
using System.Collections.Generic;

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

        public static void StepProgressBar()
        {
            ((ExportBox)Form.ActiveForm).ExportProgress.PerformStep();
        }
    }

    public class MeshTool : ElementGraphicsProcessor
    {
        private readonly double chord;
        private readonly double len;
        private readonly double angle;

        public  PolyfaceHeader output;
        private DTransform3d trans;


        public MeshTool(double chord, double len, double angle) {
            this.chord = chord;
            this.len = len;
            this.angle = angle;
            output = new PolyfaceHeader();
            trans = DTransform3d.Identity;
        }

        public override FacetOptions GetFacetOptions()
        {
            double uor = Session.Instance.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            FacetOptions option = new FacetOptions
            {
                ChordTolerance = this.chord * uor,
                MaxEdgeLength = this.len * uor,
                AngleTolerance = this.angle,
                MaxPerFace = 3,
                EdgeHiding = false
            };
            return option;
        }

        public override bool ProcessAsBody(bool isCurved)
        {
            return false;
        }
        public override bool ProcessAsFacets(bool isPolyface)
        {
            return true;
        }
        public override void AnnounceTransform(DTransform3d trans)
        {
            this.trans = trans;
        }
        public override BentleyStatus ProcessFacets(PolyfaceHeader data, bool filled)
        {
            output.CopyFrom(data);
            output.Transform(ref trans, false);
            return BentleyStatus.Success;
        }
    }


    public class GltfTool
    {
        private readonly PolyfaceHeader mesh;
        public GltfTool(PolyfaceHeader mesh)
        {
            this.mesh = mesh;
        }

    }

}

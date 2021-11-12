using System.Windows.Forms;
using Bentley.GeometryNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.DgnPlatformNET;
using System.Linq;
using glTFLoader.Schema;
using Material = glTFLoader.Schema.Material;
using System.IO;
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
            return "";
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
        private readonly ExportElement element;
        private readonly string name;
        private Gltf gltf;
        private BinaryWriter writer;

        public GltfTool(ExportElement element, string directory)
        {
            this.element = element;
            DgnFile dgn = Session.Instance.GetActiveDgnFile();
            name = directory + Path.GetFileNameWithoutExtension(dgn.GetFileName()) + " -^- " + dgn.GetLevelCache().GetLevel(element.element.LevelId).DisplayName + " -^- " + element.elementId;
            gltf = new Gltf();
            InitGltf();

        }

        private void InitGltf()
        {
            gltf.Asset = new Asset
            {
                Version = "2.0",
                Generator = "ATCDI Export Tool",
                Copyright = "安徽省交通规划设计研究总院股份有限公司 All Rights Reserved"
            };
            gltf.Scenes = new Scene[1] {
                new Scene
                {
                    Name = element.guid,
                    Nodes = new int[1] { 0 }
                }
            };
            gltf.Nodes = new Node[1] {
                new Node
                {
                    Mesh = 0,
                    Name = element.guid
                }
            };
            gltf.Materials = new Material[1]{
                new Material
                {
                    Name = "temp",
                    DoubleSided = true,
                    PbrMetallicRoughness = new MaterialPbrMetallicRoughness
                    {
                        BaseColorFactor = new float[4] { 1f, 1f, 1f, 1f },
                        MetallicFactor = 0.5f
                    }
                }
            };
            gltf.Meshes = new Mesh[1]
            {
                new Mesh
                {
                    Primitives = new MeshPrimitive[2]
                }
            };
            gltf.Buffers = new Buffer[1] {
               new Buffer
               {
                   Uri = name + ".bin",
                   ByteLength = (int)element.mesh.FacetCount * (36 + 6)
               }
            };
            gltf.BufferViews = new BufferView[2] {
                new BufferView
                {
                    Name = "Positions",
                    Buffer = 0,
                    ByteLength = (int)element.mesh.FacetCount * 36
                },
                    new BufferView
                {
                    Name = "Indices",
                    Buffer = 0,
                    ByteOffset = (int)element.mesh.FacetCount,
                    ByteLength = (int)element.mesh.FacetCount * 6
                }
            };
            gltf.Accessors = new Accessor[2] {
                new Accessor
                {
                    Name = "Positions Accessor",
                    BufferView = 0,
                    ComponentType = Accessor.ComponentTypeEnum.FLOAT,
                    Count = (int)element.mesh.FacetCount * 3,
                    Type = Accessor.TypeEnum.VEC3,
                    Max = new float[3]
                    {
                        (float)element.mesh.PointRange().High.X,
                        (float)element.mesh.PointRange().High.Y,
                        (float)element.mesh.PointRange().High.Z
                    },
                    Min = new float[3]
                    {
                        (float)element.mesh.PointRange().Low.X,
                        (float)element.mesh.PointRange().Low.Y,
                        (float)element.mesh.PointRange().Low.Z
                    }
                },
                     new Accessor
                {
                    Name = "Indices Accessor",
                    BufferView = 1,
                    ComponentType = Accessor.ComponentTypeEnum.UNSIGNED_SHORT,
                    Count = (int)element.mesh.FacetCount * 3,
                    Type = Accessor.TypeEnum.SCALAR
                }
            };
            
        }

        public void StartExport(bool glb)
        {
            PackBin();
            if (glb)
            {
                PackGlb();
            }
            else
            {
                glTFLoader.Interface.SaveModel(gltf, name + ".gltf");
            }
        }

        public void PackBin()
        {
            writer = new BinaryWriter(new FileStream(name + ".bin", FileMode.Create));
            foreach (DPoint3d point in element.mesh.Point)
            {
                writer.Write((float)point.X);
                writer.Write((float)point.Y);
                writer.Write((float)point.Z);
            }
            foreach (int index in element.mesh.PointIndex)
            {
                writer.Write((ushort)index);
            }
            writer.Close();
        }

        private void PackGlb()
        {
            writer = new BinaryWriter(new FileStream(name + ".glb", FileMode.Create));

        }
    }

}

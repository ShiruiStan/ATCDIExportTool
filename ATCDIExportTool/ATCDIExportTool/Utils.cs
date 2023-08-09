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
using Bentley.DgnPlatformNET.DgnEC;
using Bentley.ECObjects.Instance;
using System;
using Buffer = glTFLoader.Schema.Buffer;
using System.Text;
using Newtonsoft.Json;

namespace ATCDIExportTool
{
    public class Utils
    {

        public static ExportProps GetElementProps(Element element)
        {
            ExportProps prop = new ExportProps();
            prop.elementId = element.ElementId.ToString();
            double uor = Session.Instance.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            foreach (IDgnECInstance i in DgnECManager.Manager.GetElementProperties(element, ECQueryProcessFlags.SearchAllClasses))
            {
                IECPropertyValue guid = i.GetPropertyValue("ATCDI_guid");
                if (guid != null)
                {
                    prop.ATCDI_guid = guid.StringValue;
                }
                if (element.ElementType.ToString() == "CellHeader" && element.TypeName == "智能实体/曲面")
                {
                    IECPropertyValue level = i.GetPropertyValue("Level");
                    if (level != null)
                    {
                        prop.layer = Session.Instance.GetActiveDgnFile().GetLevelCache().GetLevel(level.IntValue).DisplayName;
                    }
                }
                else
                {
                    prop.layer = "";
                }

                IECPropertyValue v = i.GetPropertyValue("Volume");
                if (v != null)
                {
                    prop.volume = v.DoubleValue / uor / uor / uor;
                }
                IECPropertyValue a = i.GetPropertyValue("SurfaceArea");
                if (a != null)
                {
                    prop.area = a.DoubleValue / uor / uor;
                }


            }
            
            return prop;
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


        
        public static void StepProgressBar()
        {
            ((ExportBoxGltf)Form.ActiveForm).ExportProgress.PerformStep();
        }

        public static bool IsElementSupport(Element el)
        {
            List<string> ElementTypeFilter = new List<string>()
        {
            "CellHeader",
            "Shape",
            "ComplexShape",
            "Ellipse",
            "Surfce",
            "Solid",
            "Cone",
            "BsplineSurface",
            //"SharedCellDefinition",
            //"SharedCellInstance",
            "MeshHeader",
            "106"
        };
            if (ElementTypeFilter.Contains(el.ElementType.ToString()))
            {
                if ( el.IsValid && !el.IsInvisible &&  el.ElementType.ToString() != "106")
                {
                    return true;
                }else if (el.ElementType.ToString() == "106" && el.TypeName.ToString() == "参数化实体/曲面")
                {
                    // 经不完全测试，参数化实体可直接网格化，在筛选时已过滤掉对应的不可见子元素，且遍历子元素时不会遍历到子元素
                    return true;
                }else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
    }

        public static uint[] TransUintColorToRGBA(uint color)
        {
            uint[] rgba = new uint[4];
            rgba[0] = (color) & 0xff;
            rgba[1] = (color >> 8) & 0xff;
            rgba[2] = (color >> 16) & 0xff;
            rgba[3] = (color >> 24) & 0xff;
            return rgba;
        }
    }

    public class MeshTool : ElementGraphicsProcessor
    {

        private FacetOptions options;

        public  PolyfaceHeader output;
        public DTransform3d trans;
        public uint color;


        public MeshTool(double chord, double len, double angle) {
            double uor = Session.Instance.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            options = new FacetOptions
            {
                ChordTolerance = chord * uor,
                MaxEdgeLength = len * uor,
                AngleTolerance = angle,
                MaxPerFace = 3,
                EdgeHiding = false,
                CombineFacets = true,
                ParamsRequired = true,
                // ParamMode = FacetParamMode.ZeroOneBothAxes,
                NormalsRequired = true
            };
            output = new PolyfaceHeader();
            trans = DTransform3d.Identity;
        }

        public override FacetOptions GetFacetOptions()
        {
            return options;
        }

        public override bool ProcessAsBody(bool isCurved)
        {
            return false;
        }

        public override void AnnounceElementMatSymbology(ElementMatSymbology matSymb)
        {
            color = matSymb.FillColor;
            // color = matSymb.LineColor;
        }

        public override void AnnounceElementDisplayParameters(ElementDisplayParameters displayParams)
        {
            // 此处displayParams再获取Material会报空指针异常，怀疑是C#的SDK导致的BUG，考虑采用C++方式调用
        }

        public override bool ProcessAsFacets(bool isPolyface)
        {
            return true;
        }
        public override BentleyStatus ProcessBody(SolidKernelEntity entity)
        {
            return BentleyStatus.Error;
        }
        public override void AnnounceTransform(DTransform3d trans)
        {
            this.trans = trans == null ? DTransform3d.Identity : trans;
        }
        public override BentleyStatus ProcessFacets(PolyfaceHeader data, bool filled)
        {
            MeshHeaderElement meshEl = new MeshHeaderElement(Session.Instance.GetActiveDgnModel(), null, data);
            
            TransformInfo transInfo = new TransformInfo(trans);
            meshEl.ApplyTransform(transInfo);
            output.CopyFrom(meshEl.GetMeshData());

            return BentleyStatus.Success;
        }
    }

    public class GltfTool
    {
        public List<ExportElement> element;
        private readonly string fileName;
        private readonly string filePath;
        private Gltf gltf;
        private BinaryWriter writer;
        private bool yUp;

        private List<int> scenesNodes = new List<int>();
        private List<Node> nodes = new List<Node>();
        private List<Mesh> meshes = new List<Mesh>();
        private List<Accessor> accessors = new List<Accessor>();
        private List<BufferView> bufferViews = new List<BufferView>();

        // TODO 材质导出需研究相关API
        private List<Material> materials = new List<Material>();
        private List<uint> colors = new List<uint>();
        private List<byte> binData = new List<byte>();
        

        

        public GltfTool(string directory, bool yUp = true)
        {
            element = new List<ExportElement>();
            DgnFile dgn = Session.Instance.GetActiveDgnFile();
            fileName = Path.GetFileNameWithoutExtension(dgn.GetFileName());
            filePath = directory;
            gltf = new Gltf();
            this.yUp = yUp;
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
            gltf.Scene = 0;

        }

        public void StartExport()
        {
            // TODO
            // gltf的对应关系应该是一个元素为一个node， 但是node可以有children套子元素
            // 然后一个node对应一个mesh， 一个mesh对应若干个accessor ， mesh可以直接含position、normal、index多种accessor
            // 如果有index的话，则单独用一个index的accessor去对应的pos normal中的accessor找点，减少pos和nornal的重复点以压缩大小
            // 一个accessor对应一个bufferview
            int nodeIndex = 0;
            int meshIndex = 0;
            int bufferIndex = 0;
            int offset = 0;
            foreach (ExportElement el in element)
            {
                int colorIndex = colors.Count;
                if (colors.Contains(el.color))
                {
                    colorIndex = colors.IndexOf(el.color);
                }
                else
                {
                    colors.Add(el.color);
                }
                foreach (PolyfaceHeader m in el.meshes)
                {
                    scenesNodes.Add(nodeIndex);
                    Node node = new Node()
                    {
                        Extras = el.props,
                        Name = el.props.ATCDI_guid,
                        Mesh = meshIndex
                    };
                    nodes.Add(node);
                    nodeIndex++;
                    Mesh mesh = new Mesh
                    {
                        Primitives = new MeshPrimitive[1] {
                            new MeshPrimitive {
                                Mode = MeshPrimitive.ModeEnum.TRIANGLES,
                                Attributes = new Dictionary<string, int>
                                {
                                    {"POSITION" , bufferIndex },
                                    {"NORMAL" , bufferIndex + 1 }
                                },
                                Material = colorIndex,
                            }
                        }
                    };
                    meshes.Add(mesh);
                    meshIndex++;
                    // POINT
                    DPoint3d[] points = m.Point.ToArray();
                    DRange3d pointRange = DRange3d.NullRange;
                    foreach (int pIndex in m.PointIndex)
                    {
                        // 其中每第四个index均为0，无效，其余的点需要求绝对值然后 -1
                        if (pIndex == 0) continue;
                        DPoint3d p = Utils.ConvertUorToMeter(points[Math.Abs(pIndex) - 1]);
                        if (yUp)
                        {
                            DTransform3d trans = new DTransform3d(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0);
                            p = trans.MultiplyPoint(p.X, p.Y, p.Z);
                        }
                        binData.AddRange(BitConverter.GetBytes((float)p.X));
                        binData.AddRange(BitConverter.GetBytes((float)p.Y));
                        binData.AddRange(BitConverter.GetBytes((float)p.Z));
                        pointRange.Extend(p);
                    }
                    accessors.Add(new Accessor { 
                        BufferView = bufferIndex,
                        ComponentType = Accessor.ComponentTypeEnum.FLOAT,
                        Count = (int)m.FacetCount * 3,
                        Type = Accessor.TypeEnum.VEC3,
                        Max = new float[3]
                        {
                            (float)pointRange.High.X,
                            (float)pointRange.High.Y,
                            (float)pointRange.High.Z
                        },
                            Min = new float[3]
                        {
                            (float)pointRange.Low.X,
                            (float)pointRange.Low.Y,
                            (float)pointRange.Low.Z
                        }
                    });
                    bufferViews.Add(new BufferView
                    {
                        Buffer = 0,
                        ByteOffset = offset,
                        ByteLength =  (int)m.FacetCount * 3 * 3 * 4,
                        Target = BufferView.TargetEnum.ARRAY_BUFFER,
                        ByteStride = 12,
                    });
                    offset += (int)m.FacetCount * 3 * 3 * 4;
                    bufferIndex++;

                    // NORMAL
                    DVector3d[] normals = m.Normal.ToArray();
                    DRange3d normalRange = DRange3d.NullRange;
                    foreach (int nIndex in m.NormalIndex)
                    {
                        // 其中每第四个index均为0，无效，其余的点需要-1
                        if (nIndex == 0) continue;
                        //DTransform3d t = new DTransform3d(el.rotation);
                        //DVector3d n = t.MultiplyTranspose(normals[Math.Abs(nIndex) - 1]);

                        DVector3d n = normals[Math.Abs(nIndex) - 1];
                        if (yUp)
                        {
                            DTransform3d trans = new DTransform3d(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0);
                            n = trans.MultiplyVector(n.X, n.Y, n.Z);
                        }
                        binData.AddRange(BitConverter.GetBytes((float)n.X));
                        binData.AddRange(BitConverter.GetBytes((float)n.Y));
                        binData.AddRange(BitConverter.GetBytes((float)n.Z));
                        normalRange.Extend(n);
                    }
                    accessors.Add(new Accessor
                    {
                        BufferView = bufferIndex,
                        ComponentType = Accessor.ComponentTypeEnum.FLOAT,
                        Count = (int)m.FacetCount * 3,
                        Type = Accessor.TypeEnum.VEC3,
                        Max = new float[3]
                        {
                            (float)normalRange.High.X,
                            (float)normalRange.High.Y,
                            (float)normalRange.High.Z
                        },
                        Min = new float[3]
                        {
                            (float)normalRange.Low.X,
                            (float)normalRange.Low.Y,
                            (float)normalRange.Low.Z
                        }
                    });
                    bufferViews.Add(new BufferView
                    {
                        Buffer = 0,
                        ByteOffset = offset,
                        ByteLength = (int)m.FacetCount * 3 * 3 * 4,
                        Target = BufferView.TargetEnum.ARRAY_BUFFER,
                        ByteStride = 12,
                    });
                    offset += (int)m.FacetCount * 3 * 3 * 4;
                    bufferIndex ++;
                }
            }
            
            foreach (uint c in colors)
            {
                uint[] rgba = Utils.TransUintColorToRGBA(c);
                materials.Add(new Material
                {
                    DoubleSided = true,
                    PbrMetallicRoughness = new MaterialPbrMetallicRoughness
                    {
                        MetallicFactor = 0.8f,
                        RoughnessFactor = 0.8f,
                        BaseColorFactor = new float[] { (float)Math.Pow((float)rgba[0] / 255, 2.2), (float)Math.Pow((float)rgba[1] / 255, 2.2), (float)Math.Pow((float)rgba[2] / 255, 2.2), 1 },
                    }
                });

            }
            
            gltf.Scenes = new Scene[1] {
                new Scene
                {
                    Name = fileName,
                    Nodes = scenesNodes.ToArray()
                }
            };
            gltf.Nodes = nodes.ToArray();
            gltf.Meshes = meshes.ToArray();
            gltf.Accessors = accessors.ToArray();
            gltf.BufferViews = bufferViews.ToArray();
            gltf.Materials = materials.ToArray();
            gltf.Buffers = new Buffer[1] {
                new Buffer
                {
                    ByteLength = offset,
                
                }
            };
        }

        public void StartGlbExport()
        {
            StartExport();

            JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            byte[] gltfBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(gltf, Formatting.None, jsonSetting));
            uint gltfLen = (uint)gltfBytes.Length;
            uint gltfAlign = 4 - gltfLen % 4;
            gltfAlign = gltfAlign == 4 ? 0 : gltfAlign;
            byte gltfAlignByte = 0x20;

            byte[] dataBytes = binData.ToArray();
            uint dataLen = (uint)dataBytes.Length;
            uint dataAlign = 4 - dataLen % 4;
            dataAlign = dataAlign == 4 ? 0 : dataAlign;
            byte dataAlignByte = 0x00;

            uint magic = 0x46546C67; // 代表 glTF
            uint version = 2;

            uint totalLen = 12 + 8 + gltfLen + gltfAlign + 8 + dataLen + dataAlign;

            writer = new BinaryWriter(new FileStream(filePath + fileName + ".glb", FileMode.Create));

            writer.Write(BitConverter.GetBytes(magic));
            writer.Write(BitConverter.GetBytes(version));
            writer.Write(BitConverter.GetBytes(totalLen));

            writer.Write(BitConverter.GetBytes(gltfLen + gltfAlign));
            writer.Write(BitConverter.GetBytes(0x4E4F534A)); //  代表 JSON
            writer.Write(gltfBytes);
            for (int i = 0; i < gltfAlign; i++)
            {
                writer.Write(gltfAlignByte); // JSON 部分chunk需要用4位对齐
            }

            
            writer.Write(BitConverter.GetBytes(dataLen + dataAlign));
            writer.Write(BitConverter.GetBytes(0x004E4942)); //  代表 BIN
            writer.Write(dataBytes);
            for (int i = 0; i < dataAlign; i++)
            {
                writer.Write(dataAlignByte); // JSON 部分chunk需要用4位对齐
            }
            
            writer.Close();
        }
        public void StartGltfExport()
        {
            StartExport();
            gltf.Buffers[0].Uri = "./" + fileName + ".bin";
            glTFLoader.Interface.SaveModel(gltf, filePath + fileName + ".gltf");
            writer = new BinaryWriter(new FileStream(filePath + fileName + ".bin", FileMode.Create));
            writer.Write(binData.ToArray());
            writer.Close();
        }
        public void StartGltfEmbedExport()
        {
            StartExport();
            string data = "data:application/octet-stream;base64," + Convert.ToBase64String(binData.ToArray());
            gltf.Buffers[0].Uri = data;
            glTFLoader.Interface.SaveModel(gltf, filePath + fileName + ".gltf");
        }

    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;

namespace ArcEngineTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddShpfile();
        }

        #region
        /// <summary>
        /// 添加SHP文件
        /// </summary>
        private void AddShpfile()    
        {
            //存储打开文件的全路径
            string fullFilePath;
            //设置OpenFileDialog的属性，使其能打开多种类型文件
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "shape文件(*.shp)|*.shp";

            openFile.Title = "打开文件";
            try
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    fullFilePath = openFile.FileName;
                    //获得文件路径
                    int index = fullFilePath.LastIndexOf("\\");
                    string filePath = fullFilePath.Substring(0, index);
                    //获得文件名称
                    string fileNam = fullFilePath.Substring(index + 1);
                    //加载shape文件
                    if (openFile.FilterIndex == 1)
                    {
                        //打开工作空间工厂
                        IWorkspaceFactory workspcFac = new ShapefileWorkspaceFactory();
                        IFeatureWorkspace featureWorkspc;
                        IFeatureLayer featureLay = new FeatureLayerClass();
                        //打开路径
                        featureWorkspc = workspcFac.OpenFromFile(filePath, 0) as IFeatureWorkspace;
                        //打开类要素
                        featureLay.FeatureClass = featureWorkspc.OpenFeatureClass(fileNam);
                        //清空图层
                        axMapControl.ClearLayers();
                        //添加图层
                        axMapControl.AddLayer(featureLay);
                        axMapControl.Refresh();

                    }
                }
            }


            catch (Exception e)
            {
                MessageBox.Show("添加图层是失败" + e.ToString());

            }
        }
        #endregion
       
        /// <summary>
        ///加载文件
        /// </summary>
        #region
        public static IMapDocument pMapDocument;//定义地图文档接口变量
        //定义加载各种地理数据的静态方法
        public static void openGeodata(AxMapControl axMapControl)
        {
            //打开文件路径
            string fullFilePath;
            //设置属性，打开文件类型
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog = new OpenFileDialog();
            
            openFileDialog.Title = "打开文件";
           // openFileDialog.Filter = "shape document(*.shp)|*.shp|栅格数据(*.jpg)|*.jpg|(*.bmp)|*.bmp|(*.tif)|*.tif|map document(*.mxd)|*.mxd";
            //openFileDialog.Filter += "|栅格数据(*.jpg)|*.jpg(*.bmp)|*.bmp(*.tif)|*.tif";//
            //openFileDialog.Filter += "|map document(*.mxd)|*.mxd";//设置打开地图文档的属性
            //string fullFilePath = openFileDialog.FileName;
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fullFilePath = openFileDialog.FileName;
                    //获得文件路径
                    int index = fullFilePath.LastIndexOf("\\");
                    string filePath = fullFilePath.Substring(0, index);
                    //获得文件名称
                    string filename = fullFilePath.Substring(index + 1);
                    
                    string strFExtenN = System.IO.Path.GetExtension(filename);
                    //添加SHP数据
                    switch (strFExtenN)
                    {
                        case ".shp":
                            {
                                //打开工作空间
                                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                                IFeatureWorkspace featureWorkspc;
                                IFeatureLayer featureLay = new FeatureLayerClass();
                                //打开路径
                                featureWorkspc = workspaceFactory.OpenFromFile(filePath, 0) as IFeatureWorkspace;
                                //打开要素
                                featureLay.FeatureClass = featureWorkspc.OpenFeatureClass(filename);
                                //清空图层
                                axMapControl.ClearLayers();
                                //添加图层
                                axMapControl.AddLayer(featureLay);
                                axMapControl.Refresh();
                            }
                            break;
                            //添加栅格数据
                        case ".jpg":
                        case ".bmp":
                        case ".tif":
                            {
                                string pathname = System.IO.Path.GetDirectoryName(fullFilePath);//栅格数据文件
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(fullFilePath);
                                IWorkspaceFactory pWSF;
                                pWSF = new RasterWorkspaceFactoryClass();
                                IWorkspace pWS;
                                pWS = pWSF.OpenFromFile(pathname, 0);//实例化工作空间变量
                                IRasterWorkspace pRWS;
                                pRWS = pWS as IRasterWorkspace;//利用接口跳转的方式实例化
                                IRasterDataset pRasterDataset;
                                pRasterDataset = pRWS.OpenRasterDataset(filename);
                                //影像金字塔判断与创建
                                IRasterPyramid pRasPyrmid;
                                pRasPyrmid = pRasterDataset as IRasterPyramid;
                                if (pRasPyrmid != null)
                                {
                                    if (!(pRasPyrmid.Present))
                                    {
                                        pRasPyrmid.Create();//在进度条中说明正在创建金字塔
                                    }
                                }
                                IRaster pRaster;
                                pRaster = pRasterDataset.CreateDefaultRaster();
                                IRasterLayer pRasterLayer;
                                pRasterLayer = new RasterLayerClass();
                                pRasterLayer.CreateFromRaster(pRaster);
                                ILayer pLayer = pRasterLayer as ILayer;
                                axMapControl.AddLayer(pLayer, 0);//向axMapcontrol中添加栅格图层
                            }
                            break;
                        case ".mxd":
                            {
                                //IMapDocument pMapDocument;//定义接口变量
                                //pMapDocument = new MapDocumentClass();//实例化地图文档对象
                                ////将数据加载入pMapDocument
                                //pMapDocument.Open(filename, "");
                                //axMapControl.ClearLayers();
                                //for (int i = 0; i < pMapDocument.MapCount - 1; i++)
                                //{
                                //    //遍历所有可能的Map对象
                                //    axMapControl.Map = pMapDocument.get_Map(i);
                                //}
                                axMapControl.LoadMxFile(fullFilePath);
                                //IActiveView activeView = axMapControl.Map as IActiveView;
                                //activeView.Extent = axMapControl.FullExtent;
                                //刷新地图
                                axMapControl.Refresh();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+e.ToString());
            }

        }
        #endregion

        #region
        /// <summary>
        /// 开始编辑
        /// </summary>
        private void StartEdit()
        {
            ILayer layer = null;
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IDataset dataset = (IDataset)featureClass;
            IWorkspace workspace = dataset.Workspace;
            //开始空间编辑
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();
            IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();
            IFeatureCursor featureCursor;
            //清除图层原有的实体对象
            featureCursor = featureClass.Search(null, true);
            IFeature feature;
            feature = featureCursor.NextFeature();
            //结束空间编辑
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
        }
        #endregion

        private void 开始编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartEdit();
        }

        private void openMapDoc_Click(object sender, EventArgs e)
        {
            openGeodata(axMapControl);
        }

       






        public string strFileName { get; set; }
    }
}      
    
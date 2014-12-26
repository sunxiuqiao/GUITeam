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


       
        /// <summary>
        /// 添加SHP文件
        /// </summary>

        #region //添加SHP文件
        

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
                      //  ILayer plyr = featureLay as ILayer;
                        featureLay.Name = fileNam;
                       // axMapControl.ClearLayers();
                        //添加图层
                        axMapControl.AddLayer(featureLay);
                       
                        axMapControl.Refresh();

                    }
                }
            }


            catch (Exception e)
            {
                MessageBox.Show("添加图层失败" + e.ToString());

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
                        case ".png":
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

    
        /// <summary>
        /// 开始编辑
        /// </summary>

 

        private void axTOCControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {

        }

        private void axMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {

            edittool(editbool, Type, e);
             
        }
        //#region 开始编辑


        //private void StartEdit()
        //{
        //    ILayer layer = axMapControl.get_Layer(0) as ILayer ;
        //    IFeatureLayer featurelayer=layer as IFeatureLayer ;
           
        //    IFeatureClass featureclass = featurelayer.FeatureClass;
        //    IDataset dataset = (IDataset)featureclass;
        //    IWorkspace workspace = dataset.Workspace;
        //    //开始空间编辑
        //    IWorkspaceEdit workspaceedit = (IWorkspaceEdit)workspace;
        //    workspaceedit.StartEditing(true);
        //    workspaceedit.StartEditOperation();
        //   IFeatureBuffer featurebuffer = featureclass.CreateFeatureBuffer();
        //   IFeatureCursor featurecursor;
        //   //清除图层原有的实体对象
        //   featurecursor = featureclass.Search(null, true);
        //   IFeature feature;
        //    feature = featurecursor.NextFeature();
        //    //结束空间编辑
        //    workspaceedit.StopEditOperation();
        //    workspaceedit.StopEditing(true);
        //   System.Runtime.InteropServices.Marshal.ReleaseComObject(featurecursor);
        //}
        //#endregion



        private void openMapDoc_Click(object sender, EventArgs e)
        {
            openGeodata(axMapControl);
        }

       



        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {


        }
        #region //画点
        private void editpoint(IMapControlEvents2_OnMouseDownEvent e)
        {
            IGraphicsContainer pGraphicsContainer;
            //获取当前视图
            axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            IActiveView pActiveView = this.axMapControl.ActiveView;
            //获取鼠标点
            IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            
            IPoint pt;
            pt = axMapControl.ToMapPoint(e.x, e.y);
            IMarkerElement pMarkerElement;
            pMarkerElement = new MarkerElementClass();
            IElement pElement;
            pElement = pMarkerElement as IElement;
            pElement.Geometry = pt;
            pGraphicsContainer = axMapControl.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement((IElement)pMarkerElement, 0);
            pActiveView.Refresh();



        }


        private IRgbColor getcolor(int r, int g, int b)
        {
            IRgbColor rgbcolor = new RgbColorClass();
            rgbcolor.Red = r;
            rgbcolor.Blue = b;
            rgbcolor.Green = g;

            return rgbcolor;

        }
        #endregion
        #region //画线
        private void editline(IMapControlEvents2_OnMouseDownEvent e)
        {
            //IFeatureLayer pFeatureLayer = axMapControl.get_Layer(0) as IFeatureLayer;
            //IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            ////IFeature pfeature = pFeatureClass.GetFeature(0);
            IGraphicsContainer pGraphicsContainer;
            //获取当前视图
            axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            IActiveView pActiveView = this.axMapControl.ActiveView;
            //获取鼠标点
            IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            ILineElement pLineEle = new LineElementClass();
            IElement pEle = (IElement)pLineEle;
            ILineSymbol pLsym = new SimpleLineSymbolClass();
            pLsym.Color = getcolor(0,0,255);
            pLsym.Width = 2;
            pLineEle.Symbol = pLsym;
            IGeometry polyline;
            polyline = axMapControl.TrackLine();
           
            ILineElement pLineElement= new LineElementClass();
            pLineElement.Symbol = pLsym;
            IElement pElement;
            pElement = pLineElement as IElement;
            pElement.Geometry = polyline;
            pGraphicsContainer = axMapControl.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement((IElement)pLineElement, 0);
            pActiveView.Refresh();
        }
        #endregion
        #region//画面
        private void editpoly(IMapControlEvents2_OnMouseDownEvent e)
        {
            IGraphicsContainer pGraphicsContainer;
            //获取当前视图
            axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            IActiveView pActiveView = this.axMapControl.ActiveView;
            //获取鼠标点
            IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
           
            IPolygon Polygon = axMapControl.TrackPolygon() as IPolygon;
            IPolygonElement PolygonElement= new PolygonElementClass();
           
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = getcolor(255, 255, 255);
            pFillSymbol.Outline.Color = getcolor(0, 0, 255);
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            IFillShapeElement pfillshpEle = new PolygonElementClass();
            pfillshpEle.Symbol = pFillSymbol;

            IElement pElement = pfillshpEle as IElement;
            pElement.Geometry = Polygon;
           
            pGraphicsContainer = axMapControl.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            pActiveView.Refresh();
        
        }
        #endregion

        #region  //编辑
        private  void edittool(bool startorend ,string type,IMapControlEvents2_OnMouseDownEvent e)
        {
          if (startorend)
          {
              switch(type)
              {
                  case "point": editpoint(e);  break;

                  case "line": editline(e);  break;

                  case "pology": editpoly(e); break;
                  default: editbool = false; break;
              }
       
          }

          else { MessageBox.Show("无法编辑!"); startorend = false; }
        }
         #endregion
        private bool editbool  ;
        private string Type;
       

        private void 点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.Enabled==true)
            {
               
                    Type = "point";


                


            }

        }

        private void 线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Enabled == true)
            {

                Type = "line";
            }
        }





        public string strFileName { get; set; }
    



        private void 面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Enabled == true)
            {

                Type = "pology";





            }
        }

        private void 开始编辑ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            editbool = true;
            
        }
 

    }
}      
    
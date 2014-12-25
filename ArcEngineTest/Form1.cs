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
 

        private void axTOCControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {

        }

        private void axMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {

            edittool(editbool, Type, e);
             
        }
         #region 开始编辑
        
        private void StartEdit()
        { }
        #endregion

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
        private void 开始编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            editbool = true;
        }

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

        private void 面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Enabled == true)
            {

                Type = "pology";





            }
        }
 
    }
}      
    
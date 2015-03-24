using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;
using Microsoft.VisualBasic;
using Microsoft.Office.Interop.Word;

namespace GUI.Model
{
   public class EditData
    {
       private static string pserverStyleGalleryPath = System.Windows.Forms.Application.StartupPath + @"\配置文件\ESRI.ServerStyle";
       private static IRubberBand pRubberBand;
       /// <summary>
       /// 绘制面要素
       /// </summary>
       /// <param name="axMapControl"></param>
       /// <param name="pLyrName"></param>
       public static void DrawPolygon(AxMapControl axMapControl, string pLyrName)
       {        
           //获取当前视图
           axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
           IActiveView pActiveView = axMapControl.Map as IActiveView;

           ILayer pLayer = GetLayerByName(pLyrName, axMapControl);
           IPolygon pPolygon;
           pRubberBand = new RubberPolygonClass();//在屏幕上画个多边形
           pPolygon = pRubberBand.TrackNew(axMapControl.ActiveView.ScreenDisplay, null) as IPolygon;
           IGeometry geo = pPolygon as IGeometry;
           //添加到GDB
           addFeatureTOGDB(axMapControl, geo, pLayer);
           axMapControl.Refresh();
       }
       /// <summary>
       /// 绘制线要素
       /// </summary>
       /// <param name="axMapControl"></param>
       /// <param name="pLyrName"></param>
       public static void DrawPolyLine(AxMapControl axMapControl, string pLyrName)
       {
           ILayer pLayer = GetLayerByName(pLyrName, axMapControl);
           //获取当前视图
           axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
           IActiveView pActiveView = pLayer as IActiveView;
           IPolyline polyline = axMapControl.TrackLine() as IPolyline;
           IGeometry geo = polyline as IGeometry;
           //添加到GDB
           addFeatureTOGDB(axMapControl, geo, pLayer);
           axMapControl.Refresh();
       }
       /// <summary>
       /// 绘制点要素
       /// </summary>
       /// <param name="axMapControl"></param>
       /// <param name="pLyrName"></param>
       public static void DrawPoint(AxMapControl axMapControl, string pLyrName, IMapControlEvents2_OnMouseDownEvent e)
       {
           ILayer pLayer = GetLayerByName(pLyrName, axMapControl);         
           //获取当前视图
           axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
           IActiveView pActiveView = axMapControl.ActiveView;
           IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
           IPoint pt = new PointClass();//点
           pt.PutCoords(pPoint.X, pPoint.Y);
           IGeometry geo = pt as IGeometry;
           addFeatureTOGDB(axMapControl, geo, pLayer); //添加到GDB
           axMapControl.Refresh();
       }
      
       /// <summary>
       /// 批量生成界址点
       /// </summary>
       /// <param name="axMapControl"></param>
       public static void BatchGenerateJZD(AxMapControl axMapControl)
       {
           IFeatureLayer pFlayer = QueryAndAnalysis.GetLayerByName("界址点", axMapControl) as IFeatureLayer;//得到界址点图层
           IActiveView pactiv = pFlayer as IActiveView;
           IFeatureClass pfeaturecls = pFlayer.FeatureClass;//创建要素类
           IFeature pJZDFeature = null;
           IGraphicsContainer pGraphicsContainer;
           pGraphicsContainer = axMapControl.Map as IGraphicsContainer;

           //遍历每个地块标注界址点
           ILayer player = QueryAndAnalysis.GetLayerByName("地块", axMapControl);
           IFeatureLayer pDKlayer = player as IFeatureLayer;
           IFeatureClass pFeatureCls = pDKlayer.FeatureClass;
           int pFieldDKBMIndex = pFeatureCls.FindField("DKBM");
           IQueryFilter pQueryFilter = new QueryFilter();//实例化一个查询条件对象 
           pQueryFilter.WhereClause = null;//将查询条件赋值 
           IFeatureCursor pDKFeatureCursor = pDKlayer.Search(pQueryFilter, false);//进行查询 
           IFeature pDKFeatuer = pDKFeatureCursor.NextFeature();
           while (pDKFeatuer != null)
           {
               string DKBM = pDKFeatuer.get_Value(pFieldDKBMIndex).ToString();//得到地块编码
               IPointCollection pPointCol = GetFeaturePointColl(pDKFeatuer);//得到地块的界址点点集
               //标注界址点
               for (int i = 0; i < pPointCol.PointCount; i++)
               {
                   pJZDFeature = pfeaturecls.CreateFeature();
                   IPoint _point = pPointCol.get_Point(i);
                   IGeometry pPointGeo = _point as IGeometry;
                   pJZDFeature.Shape = pPointGeo;
                   //创建界止点标注符号
                   ISimpleMarkerSymbol simpleMark = new SimpleMarkerSymbol();
                   simpleMark.Size = 0.5;
                   simpleMark.Color = QueryAndAnalysis.getRGB(255, 0, 0);
                   simpleMark.Style = esriSimpleMarkerStyle.esriSMSDiamond;
                   IMarkerElement markElement = new ESRI.ArcGIS.Carto.MarkerElement() as IMarkerElement;
                   markElement.Symbol = simpleMark;
                   IElement element = markElement as IElement;
                   element.Geometry = pPointGeo;
                   pGraphicsContainer.AddElement(element, 0);
                   string j = Convert.ToString(i + 1);
                   QueryAndAnalysis.WtriteFieldValue(pFlayer, pJZDFeature, "JZDH", j);
                   QueryAndAnalysis.WtriteFieldValue(pFlayer, pJZDFeature, "YSDM", "211021");
                   QueryAndAnalysis.WtriteFieldValue(pFlayer, pJZDFeature, "DKBM", DKBM);
                   pJZDFeature.Store();
               }
               pDKFeatuer = pDKFeatureCursor.NextFeature();
           }
           GC.Collect();
           axMapControl.Refresh();
       }
       /// <summary>
       /// 批量生成界址线
       /// </summary>
       /// <param name="axMapControl"></param>
       public static void BatchGenerateJZX(AxMapControl axMapControl)
       {
           ILayer pLayer = QueryAndAnalysis.GetLayerByName("界址线", axMapControl) as IFeatureLayer;//得到界址线图层
           IFeatureLayer pFeatureLyr = pLayer as IFeatureLayer;
           IFeatureClass pFeatCls = pFeatureLyr.FeatureClass;
           //遍历每个地块标注界址线
           ILayer player = QueryAndAnalysis.GetLayerByName("地块", axMapControl);
           IFeatureLayer pDKlayer = player as IFeatureLayer;
           IFeatureClass pFeatureCls = pDKlayer.FeatureClass;
           int pFieldDKBMIndex = pFeatureCls.FindField("DKBM");
           IQueryFilter pQueryFilter = new QueryFilter();//实例化一个查询条件对象 
           pQueryFilter.WhereClause = null;//将查询条件赋值 
           IFeatureCursor pDKFeatureCursor = pDKlayer.Search(pQueryFilter, false);//进行查询 
           IFeature pDKFeatuer = pDKFeatureCursor.NextFeature();
           while (pDKFeatuer != null)
           {
               string DKBM = pDKFeatuer.get_Value(pFieldDKBMIndex).ToString();//得到地块编码
               IPointCollection pJZDXHPointCollection = GetFeaturePointColl(pDKFeatuer);//得到地块的界址点点集
               List<IFeature> pJZXList = GetFeaturelineList(pJZDXHPointCollection, DKBM,pLayer);//得到界址线集合
               //List<IFeature> pToucheFeatures=GetTouchesFeature(pDKlayer,pDKFeatuer);              
               QueryLDKQLR(pDKlayer, pJZXList, pFeatureLyr, DKBM);
               pDKFeatuer = pDKFeatureCursor.NextFeature();
           }
           axMapControl.Refresh();
       }
       /// <summary>
       /// 删除单个要素
       /// </summary>
       /// <param name="axMapControl"></param>
       /// <param name="pLyrName"></param>
       public static void DeletFeture(AxMapControl axMapControl, string pLyrName, IMapControlEvents2_OnMouseDownEvent e)
       {
           ILayer pLayer = GetLayerByName(pLyrName, axMapControl);
           List<IFeature> pFeatureList = SelectFturLisByPoint(axMapControl, e, pLayer);
           for (int i = 0; i < pFeatureList.Count; i++)
           {
               IFeature pFeature = pFeatureList[i];
               pFeature.Delete();
           }
           GC.Collect();
           axMapControl.Refresh();
       }
       /// <summary>
       /// 删除框选的要素
       /// </summary>
       /// <param name="axMapControl"></param>
       public static void DeletSomeFeatureByEnv(AxMapControl axMapControl)
       {
           IEnumFeature pEnumFture = SeletFturByEnv(axMapControl);
           IFeature pFeature = pEnumFture.Next();
           while (pFeature != null)
           {
               pFeature.Delete();
               pFeature = pEnumFture.Next();
           }
           axMapControl.Refresh();
       }
       /// <summary>
       /// 删除某个图层的所有要素
       /// </summary>
       /// <param name="pLayerName"></param>
       /// <param name="pAxMapControl"></param>
       public static void DeleteAllFeatureByLayer(string pLayerName, AxMapControl pAxMapControl)
       {
           ILayer player = QueryAndAnalysis.GetLayerByName(pLayerName, pAxMapControl);
           IFeatureLayer pJZDLayer = player as IFeatureLayer;
           IFeatureClass pJZDFeatureClass = pJZDLayer.FeatureClass;
           IQueryFilter pQueryFilter = new QueryFilter();//实例化一个查询条件对象 
           pQueryFilter.WhereClause = null;//将查询条件赋值 
           IFeatureCursor pJZDFeatureCursor = pJZDFeatureClass.Update(null, false);
           IFeature pJZDFeatuer = pJZDFeatureCursor.NextFeature();
           while (pJZDFeatuer != null)
           {
               pJZDFeatureCursor.DeleteFeature();
               pJZDFeatuer = pJZDFeatureCursor.NextFeature();
           }
           GC.Collect();
           pAxMapControl.Refresh();
       }
       /// <summary>
       /// 图属挂接
       /// </summary>
       /// <param name="axMapControl"></param>
       public static void GeoUNIONAtrribute(AxMapControl axMapControl,string  openfilePath)
       {
           try
           {              
               //获取字段值
               axMapControl.Map.ClearSelection();
               ILayer pLayer = QueryAndAnalysis.GetLayerByName("地块", axMapControl);
               IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
               IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;

               ITable pTable = pFeatureClass as ITable;
               ICursor pCurosor = pTable.Search(null, false);
               IRow pRow = pCurosor.NextRow();
               //使要素处于编辑状态
               IDataset dataset = (IDataset)pFeatureClass;
               IWorkspace workspace = dataset.Workspace;
               IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
               workspaceEdit.StartEditing(true);
               workspaceEdit.StartEditOperation();
               IFields pFields = pRow.Fields;
               int i0 = pFields.FindField("DKBM");
               int i1 = pFields.FindField("SYQXZ");
               int i2 = pFields.FindField("DKLB");
               int i3 = pFields.FindField("TDLYLX");
               int i4 = pFields.FindField("DLDJ");
               int i5 = pFields.FindField("TDYT");
               int i6 = pFields.FindField("SFJBNT");
               int i7 = pFields.FindField("SCMJ");
               int i8 = pFields.FindField("HTMJM");
               int i9 = pFields.FindField("CBFBM");
               int i10 = pFields.FindField("CBFXM");
               int i11 = pFields.FindField("DKMC");
               int i12 = pFields.FindField("DKDZ");
               int i13 = pFields.FindField("DKXZ");
               int i14 = pFields.FindField("DKNZ");
               int i15 = pFields.FindField("DKBZ");
               string _QSXZ = "", _QSXZDM = "", _JYQZBM = "", _SCMJ = "", _SFJBNT = "", _HTMJ = "", _TDLX = "", _TDYT = "", _TDYTDM = "", _DLDJDM = "", _DLDJ = "", _CBFMC = "", _CBFBM = "", _DKLB = "", _DKLBDM = "", _DKMC = "", _DKDZ = "", _DKXZ = "", _DKNZ = "", _DKBZ = "";
               while (pRow != null)
               {

                   object pDKXH = pRow.get_Value(i0);
                   string pDKBM = pDKXH.ToString().Substring(14, 5);
                   System.Data.DataTable dtDK = HandleExcel.ImportExcel(openfilePath, "地块信息", 0);//发包方信息读入DataTable
                   var queryDKBM = from row in dtDK.AsEnumerable() where row.Field<string>("宗地编码") == pDKBM select row;
                   foreach (var qresult in queryDKBM)
                   {
                       _QSXZ = qresult.Field<string>("权属性质");
                       _JYQZBM = qresult.Field<string>("经营权证编码");
                       _SCMJ = qresult.Field<string>("实测面积（亩）");
                       _SFJBNT = qresult.Field<string>("是否基本农田");
                       _HTMJ = qresult.Field<string>("合同面积(亩)");
                       _TDLX = qresult.Field<string>("土地类型");
                       _TDYT = qresult.Field<string>("土地用途");
                       _DLDJ = qresult.Field<string>("地力等级");
                       _CBFMC = qresult.Field<string>("承包方名称");
                       _CBFBM = qresult.Field<string>("承包方编码");
                       _DKLB = qresult.Field<string>("地块类别");
                       _DKMC = qresult.Field<string>("宗地名称");
                       _DKDZ = qresult.Field<string>("宗地东至");
                       _DKXZ = qresult.Field<string>("宗地西至");
                       _DKNZ = qresult.Field<string>("宗地南至");
                       _DKBZ = qresult.Field<string>("宗地北至");

                       _DLDJDM = DLDJDM(_DLDJ);
                       _DKLBDM = DKLBDM(_DKLB);
                       _TDYTDM = TDYTDM(_TDYT);
                       _QSXZDM = SYQXZDM(_QSXZ);
                       pRow.set_Value(i1, _QSXZDM);
                       pRow.set_Value(i2, _DKLBDM);
                       pRow.set_Value(i3, _TDLX);
                       pRow.set_Value(i4, _DLDJDM);
                       pRow.set_Value(i5, _TDYTDM);
                       pRow.set_Value(i6, _SFJBNT);
                       pRow.set_Value(i7, _SCMJ);
                       pRow.set_Value(i8, _HTMJ);
                       pRow.set_Value(i9, _CBFBM);
                       pRow.set_Value(i10, _CBFMC);
                       pRow.set_Value(i11, _DKMC);
                       pRow.set_Value(i12, _DKDZ);
                       pRow.set_Value(i13, _DKXZ);
                       pRow.set_Value(i14, _DKNZ);
                       pRow.set_Value(i15, _DKBZ);
                       pRow.Store();

                   }
                   pRow = pCurosor.NextRow();
               }
               //关闭要素编辑状态
               workspaceEdit.StopEditOperation();
               workspaceEdit.StopEditing(true);

           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }
       /// <summary>
       /// 地块自动注记
       /// </summary>
       /// <param name="pGeoFeatlayer"></param>
       /// <param name="annoField"></param>
       public static void Annotation(IGeoFeatureLayer pGeoFeatlayer, string annoField, AxMapControl paxMapControl)
       {
           IAnnotateLayerPropertiesCollection pAnnoProps = pGeoFeatlayer.AnnotationProperties;
           pAnnoProps.Clear();
           IAnnotateLayerProperties pAnnoLayerProps;
           IBasicOverposterLayerProperties pBasicpostLayerPro = new BasicOverposterLayerPropertiesClass();
           ILabelEngineLayerProperties pLabelEngine = new LabelEngineLayerPropertiesClass();
           pBasicpostLayerPro.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolygon;
           pLabelEngine.BasicOverposterLayerProperties = pBasicpostLayerPro;
           pLabelEngine.IsExpressionSimple = false;
           pLabelEngine.Expression = annoField;
           stdole.IFontDisp pFont = new stdole.StdFont() as stdole.IFontDisp;
           pFont.Name = "宋体";
           pFont.Bold = true;
           ITextSymbol pTextSymbol = new TextSymbolClass();
           pTextSymbol.Font = pFont;
           pTextSymbol.Size = 8;
           pLabelEngine.Symbol = pTextSymbol;
           pAnnoLayerProps = pLabelEngine as IAnnotateLayerProperties;
           pAnnoProps.Add(pAnnoLayerProps);
           pGeoFeatlayer.DisplayAnnotation = true;
           paxMapControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);
       }
       /// <summary>
       /// 界址点注记
       /// </summary>
       /// <param name="pGeoFeatlayer"></param>
       /// <param name="annoField"></param>
       /// <param name="paxMapControl"></param>
       public static void JZDAnnotation(IGeoFeatureLayer pGeoFeatlayer, string annoField, IRgbColor pRGB, int size, AxMapControl paxMapControl)
       {
           //得到图层的标注属性几何对象
           IAnnotateLayerPropertiesCollection pAnnoPropsCol = pGeoFeatlayer.AnnotationProperties;
           pAnnoPropsCol.Clear();
           //IAnnotateLayerProperties pAnnoLayerProps;
           //新建一个图层标注引擎
           ILabelEngineLayerProperties pLabelEngineLayerProperties = new LabelEngineLayerPropertiesClass();
           pLabelEngineLayerProperties.Expression = annoField;

           //设置标注样式
           stdole.IFontDisp pFont = new stdole.StdFont() as stdole.IFontDisp;
           pFont.Name = "宋体";
           ITextSymbol pTextSymbol = new TextSymbolClass();
           pTextSymbol.Color = pRGB;
           pTextSymbol.Font = pFont;
           pTextSymbol.Size = size;
           if (pRGB == null)
           {
               pRGB = new RgbColorClass();
               pRGB.Red = 0;
               pRGB.Green = 0;
               pRGB.Blue = 0;
           }
           pLabelEngineLayerProperties.Symbol = pTextSymbol;
           //设置文本注记的位置
           IBasicOverposterLayerProperties pBasicOverLayerProperties = new BasicOverposterLayerPropertiesClass();
           pBasicOverLayerProperties.FeatureType = esriBasicOverposterFeatureType.esriOverposterPoint;
           pBasicOverLayerProperties.FeatureWeight = esriBasicOverposterWeight.esriNoWeight;
           pBasicOverLayerProperties.BufferRatio = 0;
           pBasicOverLayerProperties.PointPlacementMethod = esriOverposterPointPlacementMethod.esriRotationField;

           pLabelEngineLayerProperties.BasicOverposterLayerProperties = pBasicOverLayerProperties;
           IAnnotateLayerProperties pAnnoLayerProp = (IAnnotateLayerProperties)pLabelEngineLayerProperties;
           pAnnoPropsCol.Add(pAnnoLayerProp);

           //pGeoFeatlayer.DisplayField = pLabelEngineLayerProperties.Expression;
           pGeoFeatlayer.DisplayAnnotation = true;
           paxMapControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);
       }
       /// <summary>
       /// 像坐标转换为地图单元
       /// </summary>
       /// <param name="pActiveView"></param>
       /// <param name="pixelUnits"></param>
       /// <returns></returns>
       private static double ConvertPixelsToMapUnits(IActiveView pActiveView, double pixelUnits)
       {
           // Uses the ratio of the size of the map in pixels to map units to do the conversion
           IPoint p1 = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperLeft;
           IPoint p2 = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperRight;
           int x1, x2, y1, y2;
           pActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p1, out x1, out y1);
           pActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p2, out x2, out y2);
           double pixelExtent = x2 - x1;
           double realWorldDisplayExtent = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
           double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
           return pixelUnits * sizeOfOnePixel;
       }
       /// <summary>
       /// 点选要素
       /// </summary>
       /// <param name="e"></param>
       private static List<IFeature> SelectFturLisByPoint(AxMapControl axMapControl, IMapControlEvents2_OnMouseDownEvent e, ILayer player)
       {

           IMap pMap = axMapControl.Map;
           IActiveView pActiveView = pMap as IActiveView;
           IFeatureLayer pFeatureLayer = player as IFeatureLayer;
           IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
           List<IFeature> pFeatureList = new List<IFeature>();

           //设置点击点的位置
           IPoint point = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
           ITopologicalOperator pTOpo = point as ITopologicalOperator;
           double length;
           length = ConvertPixelsToMapUnits(pActiveView, 4);
           IGeometry pBuffer = pTOpo.Buffer(length);
           IGeometry pGeomentry = pBuffer.Envelope;
           //空间滤过器
           ISpatialFilter pSpatialFilter = new SpatialFilter();
           pSpatialFilter.Geometry = pGeomentry;
           //根据被选择要素的不同，设置不同的空间滤过关系
           switch (pFeatureClass.ShapeType)
           {
               case esriGeometryType.esriGeometryPoint:
                   pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                   break;
               case esriGeometryType.esriGeometryPolyline:
                   pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                   break;
               case esriGeometryType.esriGeometryPolygon:
                   pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                   break;

           }
           if (pFeatureLayer.Visible)
           {
               IFeatureSelection pFSelection = pFeatureLayer as IFeatureSelection;
               pFSelection.Clear();
               pFSelection.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
               ISelectionSet pSelectionset = pFSelection.SelectionSet;
               ICursor pCursor;
               pSelectionset.Search(null, true, out pCursor);
               IFeatureCursor pFeatCursor = pCursor as IFeatureCursor;
               IFeature pFeature = pFeatCursor.NextFeature();
               while (pFeature != null)
               {
                   pMap.SelectFeature(pFeatureLayer, pFeature);
                   pFeatureList.Add(pFeature);
                   pFeature = pFeatCursor.NextFeature();
               }
               pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, null);
           }
           return pFeatureList;
       }
       /// <summary>
       /// 框选要素
       /// </summary>
       /// <param name="axMapControl"></param>
       private static IEnumFeature SeletFturByEnv(AxMapControl axMapControl)
       {
            IMap pMap = axMapControl.Map;
            IActiveView pActiveView = pMap as IActiveView;
            IEnvelope pEnv = axMapControl.TrackRectangle();
            pMap.SelectByShape(pEnv, null, false);
            IEnumFeature pEnumFture = pMap.FeatureSelection as IEnumFeature;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null, null);
            return pEnumFture;
       }
       /// <summary>
       /// 添加要素到GDB
       /// </summary>
       /// <param name="axMapControl"></param>
       /// <param name="featureGeo"></param>
       /// <param name="featureLyer"></param>
       private static void addFeatureTOGDB(AxMapControl axMapControl,IGeometry featureGeo,ILayer featureLyer)
       {
           IFeatureLayer featurelayer = featureLyer as IFeatureLayer;
           IFeatureClass featureclass = featurelayer.FeatureClass;
           IDataset dataset = featureclass as IDataset;
           IWorkspace workspace = dataset.Workspace;
           IWorkspaceEdit workspaceedit = workspace as IWorkspaceEdit;
           workspaceedit.StartEditing(true);
           workspaceedit.StartEditOperation();
           //IFeatureBuffer featurebuffer = featureclass.CreateFeatureBuffer();
           //IFeatureCursor featurecursor = featureclass.Insert(true);
           //featurebuffer.Shape = featureGeo;
           //object featureOID = featurecursor.InsertFeature(featurebuffer);
           IFeature pFeatue = featureclass.CreateFeature();
           pFeatue.Shape = featureGeo;
           if (featureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
           {
               WtriteFieldValue(featurelayer, pFeatue, "YSDM", "211011");
           }
           else if (featureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
           {
               WtriteFieldValue(featurelayer, pFeatue, "YSDM", "211031");
           }
           else if (featureclass.ShapeType == esriGeometryType.esriGeometryPoint)
           {
               WtriteFieldValue(featurelayer, pFeatue, "YSDM", "211021");
           }
           //featurecursor.Flush();
           pFeatue.Store();
           workspaceedit.StartEditOperation();
           workspaceedit.StopEditing(true);
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatue);
       }
       /// <summary>
       /// 在MapControl中查找图层
       /// </summary>
       /// <param name="strLayerName"></param>
       /// <returns></returns>
       private static ILayer GetLayerByName(string strLayerName, AxMapControl paxMapControl)
       {
           ILayer pLayer = null;
           for (int i = 0; i < paxMapControl.LayerCount; i++)
           {
               if (strLayerName == paxMapControl.get_Layer(i).Name)
               {
                   pLayer = paxMapControl.get_Layer(i);
                   break;
               }
           }
           return pLayer;
       }
       /// <summary>
        /// 得到多边形的节点集
        /// </summary>
        /// <returns></returns>
        private static IPointCollection GetFeaturePointColl(IFeature pFeatuer)
        {
            IGeometry pDKGeometry = pFeatuer as IGeometry;
            IPointCollection pDKPointCollection;
          
            IPolygon pDKpolygon=pFeatuer.Shape as IPolygon;
            pDKPointCollection = pDKpolygon as IPointCollection;
            pDKPointCollection.RemovePoints(pDKPointCollection.PointCount-1, 1);

            for (int i = 0; i < pDKPointCollection.PointCount; i++)
            {
                IPoint pt1 = pDKPointCollection.get_Point(i);
                IPoint pt2;
                if (i != pDKPointCollection.PointCount - 1)
                    pt2 = pDKPointCollection.get_Point(i + 1);
                else
                    pt2 = pDKPointCollection.get_Point(0);

                double plegth = (pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y);
                if (plegth == 0)
                {
                    if (i < pDKPointCollection.PointCount - 1)
                       pDKPointCollection.RemovePoints(i + 1, 1);
                    else
                        pDKPointCollection.RemovePoints(i, 1);
                }
            }
            //界址点排序
            IPointCollection pJZDXHPointCollection = new PolygonClass();
            int index = GetIndexofJDPoint(pDKPointCollection, 1);
            for (int i = index; i < pDKPointCollection.PointCount; i++)
            {
                IPoint pt = pDKPointCollection.get_Point(i);
                pJZDXHPointCollection.AddPoint(pt);
            }
            for (int i = 0; i < index; i++)
            {
                IPoint pt = pDKPointCollection.get_Point(i);
                pJZDXHPointCollection.AddPoint(pt);
            }
            return pJZDXHPointCollection;
        }
        /// <summary>
        /// 找到四个角点的点索引
        /// </summary>
        /// <param name="pPointCollection"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        private static int  GetIndexofJDPoint(IPointCollection pPointCollection, int corner)
        {
            IEnvelope pEnvelop = (pPointCollection as IPolygon).Envelope;
            IPoint orinalPoint = new PointClass();
            switch (corner)
            {
                case 1:
                    orinalPoint.PutCoords(pEnvelop.XMin, pEnvelop.YMax);
                    break;
                case 2:
                    orinalPoint.PutCoords(pEnvelop.XMax, pEnvelop.YMax);
                    break;
                case 3:
                    orinalPoint.PutCoords(pEnvelop.XMax, pEnvelop.YMin);
                    break;
                case 4:
                    orinalPoint.PutCoords(pEnvelop.XMin, pEnvelop.YMin);
                    break;
            }
            IPoint tempPoint = pPointCollection.get_Point(0);
            double minDistance = (tempPoint.X - orinalPoint.X) * (tempPoint.X - orinalPoint.X) + (tempPoint.Y - orinalPoint.Y) * (tempPoint.Y - orinalPoint.Y);
            int index = 0;
            for (int i = 1; i < pPointCollection.PointCount; i++)
            {
                IPoint tPoint = pPointCollection.get_Point(i);
                double distance = (tPoint.X - orinalPoint.X) * (tPoint.X - orinalPoint.X) + (tPoint.Y - orinalPoint.Y) * (tPoint.Y - orinalPoint.Y);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }
            return index;
        }
        /// <summary>
        /// 得到多边形的界址线
        /// </summary>
        /// <param name="pPointColl"></param>
        /// <returns></returns>
        private static List<IFeature> GetFeaturelineList(IPointCollection pJZDXHPointCollection, string DKBM,ILayer pLayer)
        {
            IFeatureLayer pFeatureLyr = pLayer as IFeatureLayer;
            IFeatureClass pFeatCls = pFeatureLyr.FeatureClass;

            IFeature pJZXFeature;
            List<IFeature> pJZXList = new List<IFeature>();
            for (int i = 0; i < pJZDXHPointCollection.PointCount; i++)
            {
                //获取两个界止点的序号
                IPoint temppoint1 = pJZDXHPointCollection.get_Point(i);
                IPoint temppoint2;

                if (i != pJZDXHPointCollection.PointCount - 1)
                {
                    temppoint2 = pJZDXHPointCollection.get_Point(i + 1);
                }
                else
                {
                    temppoint2 = pJZDXHPointCollection.get_Point(0);
                }
                IPointCollection pPtCollLine = new PolylineClass();
                pPtCollLine.AddPoint(temppoint1);
                pPtCollLine.AddPoint(temppoint2);

                IPolyline pPolyline = pPtCollLine as IPolyline;
                pJZXFeature = pFeatCls.CreateFeature();
                IGeometry pJZXline = pPolyline as IGeometry;
                pJZXList.Add(pJZXFeature);
                WtriteFieldValue(pFeatureLyr, pJZXFeature, "YSDM", "211031");
                WtriteFieldValue(pFeatureLyr, pJZXFeature, "DKBM", DKBM);
                pJZXFeature.Store();
            }
            return pJZXList;
        }
        /// <summary>
        /// 得到对应界址线的邻地块权利人
        /// </summary>
        /// <param name="pFeatureLineList"></param>
        /// <param name="pToucheFeatures"></param>
        private static void QueryLDKQLR(IFeatureLayer pFeatureLayer, List<IFeature> pFeatureLineList, IFeatureLayer player, string DKBM)
        {
            IFeatureClass pFeatureCls = player.FeatureClass;
            List<IFeature> pPLDWQLRJZXlist = new List<IFeature>();
            try
            {
                foreach (IFeature pobject in pFeatureLineList)
                {
                    IPolyline ppolyLine = pobject.Shape as PolylineClass;

                    IGeometry pGeometry = ppolyLine as IGeometry;
                    ITopologicalOperator pTopo = pGeometry as ITopologicalOperator;
                    //IGeometry pBuffer = pTopo.Buffer(0.05);//做缓冲

                    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    pSpatialFilter.Geometry = pGeometry;
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pSpatialFilter, false);
                    IFeature pToucheFeature = pFeatureCursor.NextFeature();
                    while (pToucheFeature != null)
                    {
                        IGeometry pTouGeo = pToucheFeature.Shape as IGeometry;
                        pTopo = pGeometry as ITopologicalOperator;
                        IPolyline plineResult = pTopo.Intersect(pTouGeo, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                        double presultLen = plineResult.Length;
                        if (presultLen > ppolyLine.Length / 2)
                        {
                            //获取相邻地块权利人名称                    
                            string pLZDDKBM = QueryAndAnalysis.GetFieldvalue(pFeatureLayer, pToucheFeature, "DKBM").ToString();
                            if (pLZDDKBM != DKBM)
                            {
                                string pLZDCBFMC = QueryAndAnalysis.GetFieldvalue(pFeatureLayer, pToucheFeature, "CBFXM").ToString();
                                QueryAndAnalysis.WtriteFieldValue(player, pobject, "PLDWQLR", pLZDCBFMC);
                                pobject.Store();
                            }
                        }
                        pToucheFeature = pFeatureCursor.NextFeature();
                    }
                    pPLDWQLRJZXlist.Add(pobject);
                }
                foreach (IFeature pfeature in pPLDWQLRJZXlist)
                {
                    IFields pFields = pFeatureCls.Fields;
                    int PLDWQLRFieldIndex = pFields.FindField("PLDWQLR");
                    string PLDWQLR = pfeature.get_Value(PLDWQLRFieldIndex).ToString();
                    if (PLDWQLR == "")
                    {
                        pfeature.set_Value(PLDWQLRFieldIndex, "道路");
                    }
                    pfeature.Store();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }
        /// <summary>
        /// 写入要素字段值
        /// </summary>
        /// <param name="player"></param>
        /// <param name="pFeature"></param>
        /// <param name="pField"></param>
        /// <param name="pValue"></param>
        private static void WtriteFieldValue(IFeatureLayer player, IFeature pFeature, string pField, object pValue)
        {
            IFeatureClass pFeatureCls = player.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(pField);
            pFeature.set_Value(pFieldIndex, pValue);
        }
        /// <summary>
        /// 读取并返回要素字段值
        /// </summary>
        /// <param name="player"></param>
        /// <param name="pFeature"></param>
        /// <param name="pField"></param>
        /// <returns></returns>
        private static object GetFieldvalue(IFeatureLayer player, IFeature pFeature, string pField)
        {
            IFeatureClass pFeatureCls = player.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(pField);
            object pFieldValue = pFeature.get_Value(pFieldIndex);
            return pFieldValue;
        }
        private static string SYQXZDM(string SYQXZ)
        {
            string _SYQXZDM = "";
            switch (SYQXZ)
            {
                case "国有土地所有权":
                    _SYQXZDM = "10";
                    break;
                case "集体土地所有权":
                    _SYQXZDM = "30";
                    break;
                case "村民小组":
                    _SYQXZDM = "31";
                    break;
                case "村级集体经济组织":
                    _SYQXZDM = "32";
                    break;
                case "乡级集体经济组织":
                    _SYQXZDM = "33";
                    break;
                case "其它农民集体经济组织":
                    _SYQXZDM = "34";
                    break;
                default:
                    break;
            }
            return _SYQXZDM;
        }
        private static string DKLBDM(string DKLB)
        {
            string _DKLBDM = "";
            switch (DKLB)
            {
                case "承包地块":
                    _DKLBDM = "10";
                    break;
                case "自留地":
                    _DKLBDM = "21";
                    break;
                case "机动地":
                    _DKLBDM = "22";
                    break;
                case "开荒地":
                    _DKLBDM = "23";
                    break;
                case "其它集体土地":
                    _DKLBDM = "99";
                    break;
                default:
                    break;
            }
            return _DKLBDM;
        }
        private static string DLDJDM(string DLDJ)
        {
            string _DLDJDM = "";
            switch (DLDJ)
            {
                case "一等地":
                    _DLDJDM = "01";
                    break;
                case "二等地":
                    _DLDJDM = "02";
                    break;
                case "三等地":
                    _DLDJDM = "03";
                    break;
                case "四等地":
                    _DLDJDM = "04";
                    break;
                case "五等地":
                    _DLDJDM = "05";
                    break;
                case "六等地":
                    _DLDJDM = "06";
                    break;
                case "七等地":
                    _DLDJDM = "07";
                    break;
                case "八等地":
                    _DLDJDM = "08";
                    break;
                case "九等地":
                    _DLDJDM = "09";
                    break;
                case "十等地":
                    _DLDJDM = "10";
                    break;
                default:
                    break;
            }
            return _DLDJDM;
        }
        private static string TDYTDM(string TDYT)
        {
            string _TDYTDM = "";
            switch (TDYT)
            {
                case "种植地":
                    _TDYTDM = "1";
                    break;
                case "林地":
                    _TDYTDM = "2";
                    break;
                case "畜牧地":
                    _TDYTDM = "3";
                    break;
                case "渔业":
                    _TDYTDM = "4";
                    break;
                case "非农业用途":
                    _TDYTDM = "5";
                    break;
                default:
                    break;
            }
            return _TDYTDM;
        }
    }
}

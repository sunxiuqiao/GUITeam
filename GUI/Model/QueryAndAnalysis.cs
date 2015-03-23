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
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using Microsoft.VisualBasic;
using Microsoft.Office.Interop.Word;
using ESRI.ArcGIS.DataSourcesGDB;
namespace GUI.Model
{
    public class QueryAndAnalysis
    {
        /// <summary>
        /// 在MapControl中查找图层
        /// </summary>
        /// <param name="strLayerName"></param>
        /// <returns></returns>
        public static ILayer GetLayerByName(string strLayerName,AxMapControl paxMapControl)
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
        /// MapControl加载图层
        /// </summary>
        /// <param name="strLayerName"></param>
        /// <returns></returns>
        public static void CreateLayer(AxMapControl paxMapControl, string filePath)
        {
            IMap pMap = paxMapControl.Map;
            List<ILayer> layerList = ReadLayerFromGDB(filePath);
            for (int i = 0; i < layerList.Count;i++)
            {
                ILayer player = layerList[i];
                pMap.AddLayer(player);
            }
            paxMapControl.ActiveView.Refresh();
        }
        /// <summary>
        /// 从GDB中读取图层
        /// </summary>
        /// <param name="filePathList"></param>
        /// <returns></returns>
        public static List<ILayer> ReadLayerFromGDB(string filePath)
        {
            List<ILayer> layerList = new List<ILayer>();
            if (filePath==null) return null;
            else
            {
                
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(filePath, 0); 
                IFeatureWorkspace pFeatureWorkspace;
                IFeatureDataset pFeatureDataset;
                IFeatureLayer pFeatureLayer;
                ILayer pLayer;
                IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTAny) as IEnumDataset;
                pEnumDataset.Reset();
                IDataset pDataset = pEnumDataset.Next();
                while (pDataset != null)
                {
                    if (pDataset is IFeatureDataset)
                    {
                        pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                        pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);
                        IEnumDataset pEnumDataset0 = pFeatureDataset.Subsets;
                        pEnumDataset0.Reset();
                        IDataset pDataset0 = pEnumDataset0.Next();
                        while (pDataset0 != null)
                        {
                            if (pDataset0 is IFeatureClass)
                            {
                                pFeatureLayer = new FeatureLayerClass();
                                pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset0.Name);
                                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                                pLayer = pFeatureLayer as ILayer;
                                layerList.Add(pFeatureLayer as ILayer);
                            }
                            pDataset0 = pEnumDataset0.Next();
                        }
                    }
                    else
                    {
                        pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                        pFeatureLayer = new FeatureLayerClass();
                        pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);
                        pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                        pLayer = pFeatureLayer as ILayer;
                        layerList.Add(pFeatureLayer as ILayer);
                    }
                    pDataset = pEnumDataset.Next();
                }
                return layerList;
            }
        }  
        /// <summary>
        /// 查找要素返回查找到的要素
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pLayer"></param>
        public static IFeature QueryFeature(string sql, ILayer pLayer)
        {
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            IQueryFilter pQueryFilter = new QueryFilter();//实例化一个查询条件对象 
            pQueryFilter.WhereClause = sql;//将查询条件赋值 
            IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);//进行查询 
            IFeature pFeature;
            pFeature = pFeatureCursor.NextFeature();//此步是将游标中的第一个交给pFeature           
            return pFeature;
        }
        /// <summary>
        /// 查找要素返回查找到的要素集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pLayer"></param>
        public static List<IFeature> QueryFeatureCol(string sql, ILayer pLayer)
        {
            List<IFeature> pFeatureList = new List<IFeature>();
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            IQueryFilter pQueryFilter = new QueryFilter();//实例化一个查询条件对象 
            pQueryFilter.WhereClause = sql;//将查询条件赋值 
            IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);//进行查询 
            IFeature pFeature;
            pFeature = pFeatureCursor.NextFeature();//此步是将游标中的第一个交给pFeature  
            while (pFeature != null)
            {
                pFeatureList.Add(pFeature);
                pFeature = pFeatureCursor.NextFeature();
            }
            return pFeatureList;
        }
        /// <summary>
        /// 设置图层的透明度
        /// </summary>
        /// <param name="LayerName"></param>
        /// <param name="pLayer"></param>
        public static void SettingLayerColour(string LayerName, ILayer pLayer)
        {
            switch (LayerName)
            {
                case "CBQ_DK":
                    ILayerEffects pLayerEffects = pLayer as ILayerEffects;
                    pLayerEffects.Transparency = 10;
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 图层的着色
        /// </summary>
        /// <param name="LayerName"></param>
        /// <param name="pLayer"></param>
        public static void SettingFeatureColour(string LayerName, ILayer pLayer)
        {
            IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            ISimpleRenderer pSimpleRender = new SimpleRendererClass();
            ITransparencyRenderer pTransREnder;
            pTransREnder = pSimpleRender as ITransparencyRenderer;

            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            switch (LayerName)
            {
                case "地块":                    
                    //新建填充符号
                    IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                    pFillSymbol.Color =getRGB(255,255,255);
                    ILineSymbol plineSym=new SimpleLineSymbolClass();
                    plineSym.Color=QueryAndAnalysis.getRGB(0,0,0);
                    plineSym.Width=0.3;
                    pFillSymbol.Outline = plineSym;
                    pSimpleRender.Symbol =(ISymbol)pFillSymbol;
                    pGeoFeatureLayer.Renderer = pTransREnder as IFeatureRenderer;                    
                    break;
                case "界址点":
                    ISimpleMarkerSymbol simpleMark = new SimpleMarkerSymbol();
                    simpleMark.Size =5;
                    simpleMark.Color = QueryAndAnalysis.getRGB(0, 0, 0);
                    simpleMark.OutlineColor = getRGB(255, 255, 255);
                    simpleMark.Outline = true;
                    simpleMark.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSimpleRender.Symbol = (ISymbol)simpleMark;
                    pGeoFeatureLayer.Renderer = pTransREnder as IFeatureRenderer;
                    break;
                case "界址线":
                    ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Color = QueryAndAnalysis.getRGB(0, 0, 0);
                    pSimpleLineSymbol.Width = 3;
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleRender.Symbol = (ISymbol)pSimpleLineSymbol;
                    pGeoFeatureLayer.Renderer = pTransREnder as IFeatureRenderer;
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 符号颜色赋值
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static IRgbColor getRGB(int red, int green, int blue)
        {
            IRgbColor pRGBcolor = new RgbColorClass();
            pRGBcolor.Red = red;
            pRGBcolor.Green = green;
            pRGBcolor.Blue = blue;
            return pRGBcolor;
        }
        /// <summary>
        /// 获得符号
        /// </summary>
        /// <param name="cla"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ISymbol GetPointstyle(string cla, string name) //获得符号
        {
            ISymbol pSymbol=null;
            IStyleGallery pStyleGallery = new ServerStyleGalleryClass();
            IStyleGalleryStorage pStyleGalleryStorage = pStyleGallery as IStyleGalleryStorage;
            IEnumStyleGalleryItem pEnumStyleGalleryItem = null;
            IStyleGalleryItem pStyleGalleryItem = null;
            IStyleGalleryClass pStyleGalleryClass = null;
            pStyleGalleryStorage.AddFile(@"C:\Program Files (x86)\ArcGIS\Server10.0\Styles\ESRI.ServerStyle");
            for (int i = 0; i < pStyleGallery.ClassCount; i++)
            {
                pStyleGalleryClass = pStyleGallery.get_Class(i);
            }
            pEnumStyleGalleryItem = pStyleGallery.get_Items(cla, @"C:\Program Files (x86)\ArcGIS\Server10.0\Styles\ESRI.ServerStyle", "");
            pEnumStyleGalleryItem.Reset();
            pStyleGalleryItem = pEnumStyleGalleryItem.Next();
            if (pStyleGalleryItem == null)
                MessageBox.Show("错了");
            while (pStyleGalleryItem != null)
            {
                if (pStyleGalleryItem.Name == name)
                {
                    pSymbol = (ISymbol)pStyleGalleryItem.Item;
                }
                else
                    pStyleGalleryItem = pEnumStyleGalleryItem.Next();
            }
            return pSymbol;
        }
       
        ///// <summary>
        ///// 界址线注记
        ///// </summary>
        ///// <param name="pGeoFeatlayer"></param>
        ///// <param name="annoField"></param>
        ///// <param name="pRGB"></param>
        ///// <param name="size"></param>
        ///// <param name="paxMapControl"></param>
        //public static void JZXAnnotation(IGeoFeatureLayer pGeoFeatlayer, string annoField, IRgbColor pRGB, int size, double JZXangel,AxMapControl paxMapControl)
        //{
        //    //得到图层的标注属性几何对象
        //    IAnnotateLayerPropertiesCollection pAnnoPropsCol = pGeoFeatlayer.AnnotationProperties;
        //    pAnnoPropsCol.Clear();
        //    //IAnnotateLayerProperties pAnnoLayerProps;
        //    //新建一个图层标注引擎
        //    ILabelEngineLayerProperties pLabelEngineLayerProperties = new LabelEngineLayerPropertiesClass();
        //    pLabelEngineLayerProperties.Expression = annoField;

        //    //设置标注样式
        //    stdole.IFontDisp pFont = new stdole.StdFont() as stdole.IFontDisp;
        //    pFont.Name = "宋体";
        //    ITextSymbol pTextSymbol = new TextSymbolClass();
        //    pTextSymbol.Color = pRGB;
        //    pTextSymbol.Font = pFont;
        //    pTextSymbol.Size = size;
        //    if (pRGB == null)
        //    {
        //        pRGB = new RgbColorClass();
        //        pRGB.Red = 0;
        //        pRGB.Green = 0;
        //        pRGB.Blue = 0;
        //    }
        //    pLabelEngineLayerProperties.Symbol = pTextSymbol;
        //    //设置文本注记的位置
        //    IBasicOverposterLayerProperties pBasicOverposterlayerProps4 = new BasicOverposterLayerPropertiesClass();
        //    switch (pGeoFeatlayer.FeatureClass.ShapeType)//判断图层类型  
        //    {
        //        case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
        //            pBasicOverposterlayerProps4.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolygon;
        //            break;
        //        case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
        //            pBasicOverposterlayerProps4.FeatureType = esriBasicOverposterFeatureType.esriOverposterPoint;
        //            break;
        //        case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
        //            pBasicOverposterlayerProps4.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolyline;
        //            break;
        //    }
        //    pBasicOverposterlayerProps4.PointPlacementMethod = esriOverposterPointPlacementMethod.esriRotationField;
            
        //}
        /// <summary>
        /// 将MapView中的数据同步到PagelayerView
        /// </summary>
        public static void CopyAndOverwriteMap(AxPageLayoutControl paxPageLayeroutControl, object toCopyMap, AxMapControl paxMapControl)
        {
            IObjectCopy objectCopy = new ObjectCopyClass();
            object copiedMap = objectCopy.Copy(toCopyMap);
            object toOverwriteMap = paxPageLayeroutControl.ActiveView.FocusMap;
            objectCopy.Overwrite(copiedMap, ref toOverwriteMap);

        }
        /// <summary>
        /// 写入要素字段值
        /// </summary>
        /// <param name="player"></param>
        /// <param name="pFeature"></param>
        /// <param name="pField"></param>
        /// <param name="pValue"></param>
        public static void WtriteFieldValue(IFeatureLayer player, IFeature pFeature, string pField, object pValue)
        {
            IFeatureClass pFeatureCls=player.FeatureClass;
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
        public static object GetFieldvalue(IFeatureLayer player, IFeature pFeature, string pField)
        {
            IFeatureClass pFeatureCls = player.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(pField);
            object pFieldValue = pFeature.get_Value(pFieldIndex);
            return pFieldValue;
        }
        /// <summary>
        /// 删除某个图层的要素
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
            pAxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
    }
}

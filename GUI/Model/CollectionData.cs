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
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using Microsoft.VisualBasic;
using Microsoft.Office.Interop.Word;
using GUI.Model;

namespace CBJYQ
{
   public class CollectionData
    {
       private static string pserverStyleGalleryPath = System.Windows.Forms.Application.StartupPath + @"\配置文件\ESRI.ServerStyle";//符号库
       private static string modefilePathGDB = System.Windows.Forms.Application.StartupPath + @"\配置文件\KJK.gdb";//GDB库
         /// <summary>
         /// 新建工程
        /// </summary>
        /// <param name="axMapControl"></param>
       public static void CreateNewProgram(AxMapControl axMapControl)
       {
           string savefilePath0 = "";
           string savefilePath = "";
           string fileName = "";
           SaveFileDialog saveFileDig = new SaveFileDialog();
           saveFileDig.Title = "创建文件";
           saveFileDig.Filter = "gdb文件（*.gdb;*.mdb)|*.gdb;*mdb|所有文件（*.*)|*.*";
           saveFileDig.RestoreDirectory = true;
           if (saveFileDig.ShowDialog() == DialogResult.OK)
           {
               savefilePath0 = saveFileDig.FileName;
               savefilePath = System.IO.Path.GetDirectoryName(savefilePath0);
               fileName = System.IO.Path.GetFileName(savefilePath0);
               int index = fileName.IndexOf(".");
               fileName = fileName.Substring(0, index);
           }
           if (savefilePath == "") return;
           IWorkspaceFactory pWSF = new FileGDBWorkspaceFactoryClass();
           IWorkspaceName targetWorkspaceName = pWSF.Create(savefilePath, fileName, null, 0);
           targetWorkspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.FileGDBWorkspaceFactory";

           IWorkspaceName pworkspaceName = new WorkspaceNameClass();
           pworkspaceName.PathName = modefilePathGDB;
           pworkspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.FileGDBWorkspaceFactory";
           Transfer(pworkspaceName, targetWorkspaceName, "CBQ", "CBJYQ");
           CreateLayer(axMapControl, savefilePath0);
       }
      /// <summary>
      /// 加载shp文件并转换到GDB中
      /// </summary>
      /// <param name="axMapControl"></param>
       public static void LoadShpFile(AxMapControl axMapControl)
       {
           //要转换的shp
           OpenFileDialog openFileDialog1 = new OpenFileDialog();
           openFileDialog1.Filter = "shaperfile(*.shp)|*.shp";
           //openFileDialog1.InitialDirectory = @"C:\Users\WMX\Desktop\农村承包经营权建库系统开发文档\测试数据";
           openFileDialog1.Multiselect = false;
           DialogResult pDialogResult = openFileDialog1.ShowDialog();
           if (pDialogResult != DialogResult.OK)
               return;
           string pPath = openFileDialog1.FileName;
           string pFolder = System.IO.Path.GetDirectoryName(pPath);
           string pFileName = System.IO.Path.GetFileName(pPath);
           CollectionData.shpToGDB(pFolder, pFileName, axMapControl);
       }
       /// <summary>
       /// 图层的标准化
       /// </summary>
       /// <param name="axMapControl"></param>
       public static void StandardLayer(AxMapControl axMapControl)
       {
           IMap pMap = axMapControl.Map;
           IEnumLayer pEnumlayer = pMap.get_Layers(null, true);
           pEnumlayer.Reset();
           ILayer player = pEnumlayer.Next();
           while (player != null)
           {
               string playerName = player.Name;
               CollectionData.standardFeature(playerName, player);
               player = pEnumlayer.Next();
           }
           axMapControl.Refresh();
       }
       /// <summary>
       /// 图层的标准化
       /// </summary>
       /// <param name="LayerName"></param>
       /// <param name="pLayer"></param>
       private static void standardFeature(string LayerName, ILayer pLayer)
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
                   pFillSymbol.Color = getRGB(255, 255, 255);
                   ILineSymbol plineSym = new SimpleLineSymbolClass();
                   plineSym.Color = QueryAndAnalysis.getRGB(0, 0, 0);
                   plineSym.Width = 1.5;
                   pFillSymbol.Outline = plineSym;
                   pSimpleRender.Symbol = (ISymbol)pFillSymbol;
                   pGeoFeatureLayer.Renderer = pTransREnder as IFeatureRenderer;
                   break;
               case "界址点":
                   pSimpleRender.Symbol = GetSymbolstyle(pserverStyleGalleryPath, "Marker Symbols", "Circle 5");
                   pGeoFeatureLayer.Renderer = pTransREnder as IFeatureRenderer;
                   break;
               case "界址线":
                   ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                   pSimpleLineSymbol.Color = QueryAndAnalysis.getRGB(0, 0, 0);
                   pSimpleLineSymbol.Width = 1.5;
                   pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                   pSimpleRender.Symbol = (ISymbol)pSimpleLineSymbol;
                   pGeoFeatureLayer.Renderer = pTransREnder as IFeatureRenderer;
                   break;
               default:
                   break;
           }

       }
       /// <summary>
       /// shp数据转换到标准GDB中
       /// </summary>
       /// <param name="fileFolder"></param>
       /// <param name="fileName"></param>
       /// <param name="axMapControl"></param>
        private static void shpToGDB(string fileFolder, string fileName, AxMapControl axMapControl)
        {
            try
            {
                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory(); // 1
                IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(fileFolder, 0); // 2
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                IFeatureClass pFC = pFeatureWorkspace.OpenFeatureClass(fileName); //3
                IFeatureLayer pFLayer = new FeatureLayerClass(); // 4
                pFLayer.FeatureClass = pFC;
                pFLayer.Name = pFC.AliasName; // 5

                IFeatureLayer pTargetFlayer = QueryAndAnalysis.GetLayerByName("地块", axMapControl) as IFeatureLayer;//得到GDB中地块图层
                IActiveView pactiv = pTargetFlayer as IActiveView;
                IFeatureClass pTargetfeaturecls = pTargetFlayer.FeatureClass;//创建要素类
                //遍历原shp文件
                IQueryFilter pQueryFilter = new QueryFilter();//实例化一个查询条件对象 
                pQueryFilter.WhereClause = null;//将查询条件赋值 
                IFeatureCursor pDKFeatureCursor = pFLayer.Search(pQueryFilter, false);//进行查询 
                IFeature pDKFeatuer = pDKFeatureCursor.NextFeature();
                IFeature pTargetFeature = null;//定义GDB中DK要素
                while (pDKFeatuer != null)
                {
                    //几何属性转换
                    pTargetFeature = pTargetfeaturecls.CreateFeature();
                    pTargetFeature.Shape = pDKFeatuer.ShapeCopy;
                    IGeometry pGeometry = pTargetFeature.Shape as IGeometry;
                    //属性要素转换                                    
                    int pFieldDKBMIndex = pFC.FindField("DKBM");
                    int pFieldDKBZIndex = pFC.FindField("DKBZ");
                    int pFieldDKDZIndex = pFC.FindField("DKDZ");
                    int pFieldDKXZIndex = pFC.FindField("DKXZ");
                    int pFieldDKNZIndex = pFC.FindField("DKNZ");
                    int pFieldDKMCIndex = pFC.FindField("DKMC");
                    int pFieldFBFBMIndex = pFC.FindField("FBFBM");
                    int pFieldYSDMIndex = pFC.FindField("YSDM");
                    string DKBM = pDKFeatuer.get_Value(pFieldDKBMIndex).ToString();
                    string DKBZ = pDKFeatuer.get_Value(pFieldDKBZIndex).ToString();
                    string DKDZ = pDKFeatuer.get_Value(pFieldDKDZIndex).ToString();
                    string DKXZ = pDKFeatuer.get_Value(pFieldDKXZIndex).ToString();
                    string DKNZ = pDKFeatuer.get_Value(pFieldDKNZIndex).ToString();
                    string DKMC = pDKFeatuer.get_Value(pFieldDKMCIndex).ToString();
                    string FBFBM = pDKFeatuer.get_Value(pFieldFBFBMIndex).ToString();
                    string YSDM = pDKFeatuer.get_Value(pFieldYSDMIndex).ToString();
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "YSDM", YSDM);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "DKBM", DKBM);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "DKMC", DKMC);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "DKDZ", DKDZ);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "DKXZ", DKXZ);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "DKNZ", DKNZ);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "DKBZ", DKBZ);
                    QueryAndAnalysis.WtriteFieldValue(pTargetFlayer, pTargetFeature, "FBFBM", FBFBM);
                    pTargetFeature.Store();
                    IArea pArea = pDKFeatuer.Shape as IArea;
                    IPoint ppoint = pArea.Centroid;
                    axMapControl.CenterAt(ppoint);
                    pDKFeatuer = pDKFeatureCursor.NextFeature();
                }
                ILayer pFlayer = pTargetFlayer as ILayer;
                axMapControl.MapScale = 10000;
                axMapControl.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }   
       /// <summary>
        /// 从ServerStyle符号库获取指定的符号
       /// </summary>
       /// <param name="serverStyleGalleryPath"></param>
       /// <param name="galleryClassName"></param>
       /// <param name="symbolName"></param>
       /// <returns></returns>
        private static ISymbol GetSymbolstyle(string serverStyleGalleryPath, string galleryClassName, string symbolName) //获得符号
        {
           
            ISymbol pSymbol;
            IStyleGallery pStyleGallery = new ServerStyleGalleryClass();
            IStyleGalleryStorage pStyleGalleryStorage = pStyleGallery as IStyleGalleryStorage;
            IEnumStyleGalleryItem pEnumStyleGalleryItem = null;
            IStyleGalleryItem pStyleGalleryItem = null;
            IStyleGalleryClass pStyleGalleryClass = null;
            pStyleGalleryStorage.AddFile(serverStyleGalleryPath);
            for (int i = 0; i < pStyleGallery.ClassCount; i++)
            {
                pStyleGalleryClass = pStyleGallery.get_Class(i);
                if (pStyleGalleryClass.Name != galleryClassName)
                    continue;
                //获取EnumStyleGalleryItem对象
                pEnumStyleGalleryItem = pStyleGallery.get_Items(galleryClassName, serverStyleGalleryPath, "");
                pEnumStyleGalleryItem.Reset();
                pStyleGalleryItem = pEnumStyleGalleryItem.Next();
                if (pStyleGalleryItem == null)
                    MessageBox.Show("错了");
                while (pStyleGalleryItem != null)
                {
                    if (pStyleGalleryItem.Name == symbolName)
                    {
                        pSymbol = (ISymbol)pStyleGalleryItem.Item;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pStyleGalleryItem);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pStyleGalleryClass);
                        return pSymbol;
                    }
                    else 
                        pStyleGalleryItem = pEnumStyleGalleryItem.Next();
                }
            }
            return null;        
        }
        /// <summary>
        /// 符号颜色赋值
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        private static IRgbColor getRGB(int red, int green, int blue)
        {
            IRgbColor pRGBcolor = new RgbColorClass();
            pRGBcolor.Red = red;
            pRGBcolor.Green = green;
            pRGBcolor.Blue = blue;
            return pRGBcolor;
        }
       /// <summary>
       /// 数据集复制
       /// </summary>
       /// <param name="sourceWorkspaceName"></param>
       /// <param name="targetWorkspaceName"></param>
       /// <param name="sourcedataset"></param>
       /// <param name="targetdataset"></param>
        private static void Transfer(IWorkspaceName sourceWorkspaceName, IWorkspaceName targetWorkspaceName, string sourcedataset, string targetdataset)
        {
            try
            {
                IFeatureDatasetName sourcefeatureDatasetName = new FeatureDatasetNameClass();
                IDatasetName sourcedatasetName = sourcefeatureDatasetName as IDatasetName;
                sourcedatasetName.WorkspaceName = sourceWorkspaceName;
                sourcedatasetName.Name = sourcedataset;
                IName sourceName = sourcedatasetName as IName;

                IFeatureDatasetName targetfeatureDatasetName = new FeatureDatasetNameClass();
                IDatasetName targetdatasetName = targetfeatureDatasetName as IDatasetName;
                targetdatasetName.WorkspaceName = targetWorkspaceName;
                targetdatasetName.Name = targetdataset;
                IName targetName = targetdatasetName as IName;

                IEnumName sourceEnumName = new NamesEnumeratorClass();
                IEnumNameEdit sourceEnumNameEdit = (IEnumNameEdit)sourceEnumName;
                IEnumDatasetName enumsourceDatasetName = sourcedatasetName.SubsetNames;
                enumsourceDatasetName.Reset();
                IDatasetName sourcefeatureClassName = enumsourceDatasetName.Next();
                while (sourcefeatureClassName != null)
                {
                    sourceEnumNameEdit.Add(sourcefeatureClassName as IName);
                    sourcefeatureClassName = enumsourceDatasetName.Next();
                }

                IGeoDBDataTransfer geoDBDataTransfer = new GeoDBDataTransferClass();
                IEnumNameMapping enumNameMapping = null;
                Boolean conflictsFound = geoDBDataTransfer.GenerateNameMapping(sourceEnumName, targetWorkspaceName as IName, out enumNameMapping);
                enumNameMapping.Reset();

                if (conflictsFound)
                {
                    INameMapping nameMapping = null;
                    while ((nameMapping = enumNameMapping.Next()) != null)
                    {
                        if (nameMapping.NameConflicts)
                        {
                            nameMapping.TargetName = nameMapping.GetSuggestedName(targetName);
                        }
                    }

                }

                geoDBDataTransfer.Transfer(enumNameMapping, targetName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// MapControl加载图层
        /// </summary>
        /// <param name="strLayerName"></param>
        /// <returns></returns>
        private static void CreateLayer(AxMapControl paxMapControl, string filePath)
        {
            IMap pMap = paxMapControl.Map;
            List<ILayer> layerList = ReadLayerFromGDB(filePath);
            for (int i = 0; i < layerList.Count; i++)
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
        private static List<ILayer> ReadLayerFromGDB(string filePath)
        {
            List<ILayer> layerList = new List<ILayer>();
            if (filePath == null) return null;
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
   }
}

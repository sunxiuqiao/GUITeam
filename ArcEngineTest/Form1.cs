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

        }
    }
}      
    
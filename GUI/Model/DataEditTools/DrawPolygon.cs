using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using CreateDatabase;
using ESRI.ArcGIS.Geodatabase;

namespace GUI.Model.DataEditTools
{
    [Guid("d30936d2-3d2f-40f4-b919-0253bb5604e9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("WindowsFormsApplication1.Tool1")]
    public sealed class DrawPolygon : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private INewPolygonFeedback m_polygonFeedback = null;

        private bool m_isMouseDown;//鼠标是否按下

        public DrawPolygon()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_cursor = new System.Windows.Forms.Cursor("../../Config/Cursor/Sketch.cur");
            base.m_category = "DataEditTools/DrawPolygon"; //localizable text 
            base.m_caption = "绘制面要素";  //localizable text 
            base.m_message = "绘制面要素";  //localizable text
            base.m_toolTip = "绘制面要素";  //localizable text
            base.m_name = "绘制面要素";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }

                base.m_enabled = true;
                base.m_checked = false;
                m_isMouseDown = false;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add Tool1.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.TOCControl_OnMouseDown implementation
            m_isMouseDown = true;
            IActiveView activeView = m_hookHelper.FocusMap as IActiveView;
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            if(m_polygonFeedback == null)
            {
                m_polygonFeedback = new NewPolygonFeedbackClass();

                //ISimpleLineSymbol lineSymbol = m_polygonFeedback.Symbol as ISimpleLineSymbol;
                //IRgbColor rgb = new RgbColorClass();
                //rgb.Red = 140;
                //rgb.Green = 140;
                //rgb.Blue = 255;

                //lineSymbol.Color = rgb;
                //lineSymbol.Style = esriSimpleLineStyle.esriSLSDot;


                m_polygonFeedback.Display = activeView.ScreenDisplay;
                m_polygonFeedback.Start(point);

            }
            else
            {
                m_polygonFeedback.AddPoint(point);
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseMove implementation
            if (m_isMouseDown == false)
                return;

            if(m_polygonFeedback!= null)
            {
                IActiveView activeView = m_hookHelper.FocusMap as IActiveView;
                IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                m_polygonFeedback.MoveTo(point);
            }

        }

        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="keyCode">键编码</param>
        /// <param name="shift">shift键</param>
        public void OnKeyDown(int keyCode, int shift)
        {
            if (m_isMouseDown)
            {
                if (keyCode == 27)
                {
                    m_isMouseDown = false;
                    m_polygonFeedback = null;
                    m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                }
            }
        }

        /// <summary>
        /// 键盘放开事件
        /// </summary>
        /// <param name="keyCode">键编码</param>
        /// <param name="shift">shift键</param>
        public void OnKeyUp(int keyCode, int shift)
        {
            if (m_isMouseDown)
            {
                if (keyCode == 27)
                {
                    m_isMouseDown = false;
                    m_polygonFeedback = null;
                    m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                }
            }
        }


        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseUp implementation
        }
        #endregion

        public override void OnDblClick()
        {
            IGeometry geometry = null;
            if (m_polygonFeedback != null)
                geometry = m_polygonFeedback.Stop() as IGeometry;

            if (geometry != null)
            {
                if (geometry.GeometryType.Equals(esriGeometryType.esriGeometryPolygon))
                {
                    Draw(geometry);
                }
                    
            }
            m_polygonFeedback = null;
        }

        private void Draw(IGeometry geometry)
        {
            if ((geometry == null))
                return;
            IMap map = m_hookHelper.FocusMap;
            int layerIndex = GetLayerByName(map, "地块");
            if(layerIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("图层错误");
                return;
            }
               
            ILayer layer = map.get_Layer(layerIndex);
            if (layer is IFeatureLayer)
            {
                IFeatureLayer featureLyr = layer as IFeatureLayer;
                IFeatureClass featureClass = featureLyr.FeatureClass;
                IDataset dataSet = featureClass as IDataset;
                IWorkspace workSpace = dataSet.Workspace;
                IFeature feature = featureClass.CreateFeature();
                feature.Shape = geometry;
                SetFieldValue(featureLyr, feature, "YSDM", "211011");
                //feature.Store();
            }
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }

        private int GetLayerByName(IMap Map, string LyrName)
        {
            try
            {
                int index = -1;
                for (int i = 0; i < Map.LayerCount; ++i)
                {
                    if (Map.get_Layer(i).Name == LyrName)
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return -1;
            }
        }

        private static void SetFieldValue(IFeatureLayer Layer, IFeature Feature, string Field, object Value)
        {
            IFeatureClass pFeatureCls = Layer.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(Field);
            Feature.set_Value(pFieldIndex, Value);
        }

        
    }
}

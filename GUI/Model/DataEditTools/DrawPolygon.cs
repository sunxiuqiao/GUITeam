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
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Editor;

namespace GUI.Model.DataEditTools
{
    [Guid("d30936d2-3d2f-40f4-b919-0253bb5604e9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("WindowsFormsApplication1.Tool1")]
    public class DrawPolygon : BaseTool
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

        protected IHookHelper m_hookHelper = null;
        protected INewPolygonFeedback m_polygonFeedback = null;
        protected IGeometry m_geometry = null;
        private ISnappingEnvironment m_snapEnv = new SnappingClass();
        private ISnappingFeedback m_snapFeedback = new SnappingFeedbackClass();
        private IPoint m_currentPoint = null;
        
        //IPointSnapper pointSnapper = new
            
        

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
            base.m_name = "DataEditTools_DrawPolygon";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            m_snapEnv.SnappingType = esriSnappingType.esriSnappingTypeEdge;
            
            m_snapEnv.Tolerance = 15;

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
                m_snapFeedback.Initialize(m_hookHelper.Hook, m_snapEnv, true);
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

            IHookHelper2 m_hookHelper2 = (IHookHelper2)m_hookHelper;
            IExtensionManager extensionManager = m_hookHelper2.ExtensionManager;
            if (extensionManager != null)
            {
                UID guid = new UIDClass();
                guid.Value = "{E07B4C52-C894-4558-B8D4-D4050018D1DA}"; //Snapping extension.
                IExtension extension = extensionManager.FindExtension(guid);
                m_snapEnv = extension as ISnappingEnvironment;
            }
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.TOCControl_OnMouseDown implementation
            
            IActiveView activeView = m_hookHelper.FocusMap as IActiveView;
            //

            if(!m_isMouseDown)
            {
                m_isMouseDown = true;
                m_polygonFeedback.Start(m_currentPoint);

            }
            else
            {
                //m_isMouseDown = true;
                m_polygonFeedback.AddPoint(m_currentPoint);
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseMove implementation
            m_currentPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            IPointSnapper pointSnapper = m_snapEnv.PointSnapper;
            ISnappingResult result = pointSnapper.Snap(m_currentPoint);
            if (m_polygonFeedback == null)
            {
                m_polygonFeedback = new NewPolygonFeedbackClass();
                m_polygonFeedback.Display = m_hookHelper.ActiveView.ScreenDisplay;
            }
            if (result != null)
            {
                m_snapFeedback.Update(result, 0);
                m_currentPoint = result.Location;
                m_polygonFeedback.MoveTo(m_currentPoint);
            }
            else
            {
                if (m_isMouseDown == false)
                    return;
                m_polygonFeedback.MoveTo(m_currentPoint);
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


        #endregion

        public override void OnDblClick()
        {
            IGeometry geo = null;
            if (m_polygonFeedback != null)
                geo = m_polygonFeedback.Stop() as IGeometry;

            if (geo != null)
            {
                m_geometry =geo;
            }
            m_isMouseDown = false;
            m_polygonFeedback = null;
        }

        public override void Refresh(int hDC)
        {
            base.Refresh(hDC);
            m_snapFeedback.Refresh(hDC);
        }

    }
}

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
    public class DrawPolyline : BaseTool
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
        protected INewLineFeedback m_lineFeedbback = null;
        protected IGeometry geometry = null;

        private bool m_isMouseDown;//鼠标是否按下

        public DrawPolyline()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_cursor = new System.Windows.Forms.Cursor("../../Config/Cursor/Sketch.cur");
            base.m_category = "DataEditTools/DrawPolygon"; //localizable text 
            base.m_caption = "绘制线要素";  //localizable text 
            base.m_message = "绘制线要素";  //localizable text
            base.m_toolTip = "绘制线要素";  //localizable text
            base.m_name = "DataEditTools_DrawPolygon";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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
                m_enabled = true;
                m_checked = false;
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
            IActiveView activeView = m_hookHelper.ActiveView;
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X,Y);

            if(m_lineFeedbback == null)
            {
                m_lineFeedbback = new NewLineFeedbackClass();
                m_lineFeedbback.Display = activeView.ScreenDisplay;
                m_lineFeedbback.Start(point);
            }
            else
            {
                m_lineFeedbback.AddPoint(point);
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseMove implementation
            if (m_isMouseDown == false)
                return;
            if(m_lineFeedbback != null)
            {
                IPoint point = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                m_lineFeedbback.MoveTo(point);
            }
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseUp implementation
        }

        /// <summary>
        /// OnKeyDown
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="Shift"></param>
        public override void OnKeyDown(int keyCode, int Shift)
        {
            if(keyCode == 27)
            {
                m_lineFeedbback = null;
                m_isMouseDown = false;
                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            }
        }

        /// <summary>
        /// OnKeyUp
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="Shift"></param>
        public override void OnKeyUp(int keyCode, int Shift)
        {
            if (keyCode == 27)
            {
                m_lineFeedbback = null;
                m_isMouseDown = false;
                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            }
        }

        public override void OnDblClick()
        {
            IGeometry geo = null;
            if (m_lineFeedbback != null)
                geo = m_lineFeedbback.Stop() as IGeometry;

            if (geo != null)
            {
                geometry = geo;
            }
            m_lineFeedbback = null;
        }
        #endregion
    }
}

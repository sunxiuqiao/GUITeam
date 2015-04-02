using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;


namespace GUI.Model.DataEditTools
{
    /// <summary>
    /// Summary description for Tool1.
    /// </summary>
    [Guid("d30936d2-3d2f-40f4-b919-0253bb5604e9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("WindowsFormsApplication1.Tool1")]
    public class SelectFeaturesTool : BaseTool
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
        private bool m_isMouseDown = false;
        private IGeometry m_envelopeGeometry = null;
        private IMap m_map;
        private DataEditor dataEdit;

        public SelectFeaturesTool(DataEditor edit)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_cursor = new System.Windows.Forms.Cursor("../../Config/Cursor/Edit.cur");
            //base.m_bitmap = new Bitmap("../../Config/SelectFeature.png");
            base.m_category = "DataEditTools/SelectFeaturesTool"; //localizable text 
            base.m_caption = "选择要素";  //localizable text 
            base.m_message = "拉框或单击选择要素";  //localizable text
            base.m_toolTip = "选择要素";  //localizable text
            base.m_name = "SelectFeaturesTool";   //unique id, non-localizable (e.g. "MyCategory_MyTool")

            dataEdit = edit;

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

        public override bool Enabled
        {
            get
            {
                if (m_hookHelper == null) return false;
                return (m_hookHelper.FocusMap.LayerCount > 0);
            }
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
            m_isMouseDown = true;
            
            IPoint clickedPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            //If ActiveView is a PageLayout 
            if(m_hookHelper.ActiveView is IPageLayout)
            {
                //See whether the mouse has been clicked over a Map in the PageLayout 
                m_map = m_hookHelper.ActiveView.HitTestMap(clickedPoint);

                //If mouse click isn't over a Map exit
                if (m_map == null)
                    return;

                //Ensure the Map is the FocusMap
                if((!object.ReferenceEquals(m_hookHelper.ActiveView.FocusMap,m_map)))
                {
                    m_hookHelper.ActiveView.FocusMap = m_map;
                    m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }

                //Still need to convert the clickedPoint into map units using the map's IActiveView 
                clickedPoint = ((IActiveView)m_map).ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            }
            else
            {
                m_map = m_hookHelper.ActiveView.FocusMap;
            }

            

            IActiveView activeView = m_map as IActiveView;
            IRubberBand envelopeRubber = new RubberEnvelopeClass();

            if (Shift != 2 || m_envelopeGeometry ==null)
            {
                m_envelopeGeometry = envelopeRubber.TrackNew(activeView.ScreenDisplay, null);
                IArea area = (IArea)m_envelopeGeometry;

                if((m_envelopeGeometry.IsEmpty==true) || (area == null))
                {
                    //create a new envelope 
                    IEnvelope tempEnv = new EnvelopeClass();

                    //create a small rectangle 
                    ESRI.ArcGIS.esriSystem.tagRECT RECT = new tagRECT();
                    RECT.bottom = 0;
                    RECT.left = 0;
                    RECT.right = 5;
                    RECT.top = 5;

                    //transform rectangle into map units and apply to the tempEnv envelope 
                    IDisplayTransformation dispTrans = activeView.ScreenDisplay.DisplayTransformation;
                    dispTrans.TransformRect(tempEnv, ref RECT, 4); //4 = esriDisplayTransformationEnum.esriTransformToMap)
                    tempEnv.CenterAt(clickedPoint);
                    m_envelopeGeometry = (IGeometry)tempEnv;
                }
            }
            else
            {
                IGeometryCollection geometryBag = new GeometryBagClass();
                geometryBag.AddGeometry(m_envelopeGeometry);
                IGeometry geom = envelopeRubber.TrackNew(activeView.ScreenDisplay, null);
                IArea area = (IArea)geom;
                if ((geom.IsEmpty == true) || (area == null))
                {
                    //create a new envelope 
                    IEnvelope tempEnv = new EnvelopeClass();

                    //create a small rectangle 
                    ESRI.ArcGIS.esriSystem.tagRECT RECT = new tagRECT();
                    RECT.bottom = 0;
                    RECT.left = 0;
                    RECT.right = 5;
                    RECT.top = 5;

                    //transform rectangle into map units and apply to the tempEnv envelope 
                    IDisplayTransformation dispTrans = activeView.ScreenDisplay.DisplayTransformation;
                    dispTrans.TransformRect(tempEnv, ref RECT, 4); //4 = esriDisplayTransformationEnum.esriTransformToMap)
                    tempEnv.CenterAt(clickedPoint);
                    geom = (IGeometry)tempEnv;
                }
                geometryBag.AddGeometry(geom);
                ITopologicalOperator topo = new PolygonClass();
                topo.ConstructUnion(geometryBag as IEnumGeometry);
                m_envelopeGeometry = topo as IGeometry;
                

            }
            dataEdit.WKSEditor.StartEditOperation();
            ISpatialReference spatialReference = m_map.SpatialReference;
            m_envelopeGeometry.SpatialReference = spatialReference;
            m_map.SelectByShape(m_envelopeGeometry, null, false);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent); 
        }

        
        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseMove implementation
            //if (!m_isMouseDown)
            //    return;
            //IPoint point = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            //IEnumFeature features = m_hookHelper.FocusMap.FeatureSelection as IEnumFeature;
            //features.Reset();
            //IFeature feature = features.Next();
            //while (feature != null)
            //{
            //    ITransform2D transform = feature.Shape as ITransform2D;
            //    transform.Move(point.X, point.Y);
            //    feature.Store();
            //}
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseUp implementation
            m_isMouseDown = false;
            dataEdit.WKSEditor.StopEditOperation();
            
        }
        public override void OnDblClick()
        {
            base.OnDblClick();
        }
        #endregion
    }


    public sealed class ClearFeatureSelection: BaseCommand
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion
        private IHookHelper m_HookHelper = new HookHelperClass();

        public ClearFeatureSelection()
        {
            //Create an IHookHelper object
            m_HookHelper = new HookHelperClass();

            //Set the tool properties
            base.m_caption = "清除选择的要素";
            base.m_category = "DataEditTools/ClearFeatureSelection";
            base.m_name = "Sample_Select(C#)_Clear Feature Selection";
            base.m_message = "清除选择的要素";
            base.m_toolTip = "清除选择的要素";
        }

        public override void OnCreate(object hook)
        {
            m_HookHelper.Hook = hook;
        }

        public override bool Enabled
        {
            get
            {
                if (m_HookHelper.FocusMap == null) return false;
                return (m_HookHelper.FocusMap.SelectionCount > 0);
            }
        }

        public override void OnClick()
        {
            //Clear selection
            m_HookHelper.FocusMap.ClearSelection();

            //Get the IActiveView of the FocusMap
            IActiveView activeView = (IActiveView)m_HookHelper.FocusMap;

            //Get the visible extent of the display
            IEnvelope bounds = activeView.ScreenDisplay.DisplayTransformation.FittedBounds;

            //Refresh the visible extent of the display
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, bounds);
        }

    }
}

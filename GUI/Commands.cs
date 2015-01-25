using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GUI
{
    /// <summary>
    /// Summary description for Command1.
    /// </summary>
    //[Guid("9a67ee8b-5d77-4c10-9fc8-319b63332746")]
    //[ClassInterface(ClassInterfaceType.None)]
    //[ProgId("WindowsFormsApplication2.Command1")]
    namespace Commands
    {
        public sealed class RemoveCommand : BaseCommand,ICommandSubType
        {
            private IMapControl3 _MapControl;
            private long _SubType;

            public RemoveCommand()
            {
                //
                // TODO: Define values for the public properties
                //
                base.m_category = ""; //localizable text
                base.m_caption = "移除";  //localizable text
                base.m_message = "移除";  //localizable text 
                base.m_toolTip = "";  //localizable text 
                base.m_name = "RemoveLayerCommand";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

                try
                {
                    //
                    // TODO: change bitmap name if necessary
                    //
                    string bitmapResourceName = GetType().Name + ".bmp";
                    base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
                }
            }

            #region Overridden Class Methods

            /// <summary>
            /// Occurs when this command is created
            /// </summary>
            /// <param name="hook">Instance of the application</param>
            public override void OnCreate(object hook)
            {
                _MapControl = (IMapControl3)(((AxMapControl)hook).Object);

            }

            /// <summary>
            /// Occurs when this command is clicked
            /// </summary>
            public override void OnClick()
            {
                // TODO: Add Command1.OnClick implementation
                if(_SubType == 1)
                {
                    _MapControl.Map.DeleteLayer((ILayer)_MapControl.CustomProperty);
                }
                else
                {
                    IEnumLayer layers = _MapControl.Map.Layers;
                    layers.Reset();
                    ILayer layer = layers.Next();
                    while (layer != null)
                    {
                        _MapControl.Map.DeleteLayer(layer);
                        layer = layers.Next();
                    }
                }
                
            }

            public int GetCount()
            {
                return 2;
            }
            public void SetSubType(int SubType)
            {
                _SubType = SubType;
            }
            #endregion
        }

        public sealed class ScaleThresholdCommand : BaseCommand,ICommandSubType
        {
            private IMapControl3 _MapControl = null;
            private long _subType;

            public int GetCount()
            {
                return 3;
            }

            public void SetSubType(int SubType)
            {
                _subType = SubType;
            }

            public override string Caption
            {
                get
                {
                    if (_subType == 1)
                        return "设置为最大比例";
                    else if (_subType == 2)
                        return "设展位最小比例";
                    else if (_subType == 3)
                        return "清除比例范围";
                    
                    return base.Caption;
                }
            }

            public override bool Enabled
            {
                get
                {
                    bool enable = true;

                    if(_subType == 3)
                    {
                        ILayer layer = (ILayer)_MapControl.CustomProperty;
                        if(layer.MaximumScale == 0 && layer.MinimumScale ==0)
                        {
                            enable = false;
                        }
                    }
                    return enable;
                }
            }

            public override void OnCreate(object hook)
            {
                _MapControl = (IMapControl3)(((AxMapControl)hook).Object);
            }

            public override void OnClick()
            {
                ILayer layer = (ILayer)_MapControl.CustomProperty;
                if(_subType == 1)
                {
                    layer.MaximumScale = _MapControl.MapScale;
                }
                else if (_subType == 2)
                {
                    layer.MinimumScale = _MapControl.MapScale;
                }
                else if (_subType == 3)
                {
                    layer.MinimumScale = 0;
                    layer.MaximumScale = 0;
                }
            }
        }

        public sealed class ZoomToLayerCommand : BaseCommand
        {
            IMapControl3 _MapControl;

            public override void OnCreate(object hook)
            {
                if (hook == null)
                    return;
                _MapControl = (IMapControl3)(((AxMapControl)hook).Object);
            }

            public override void OnClick()
            {
                ILayer layer = (ILayer)_MapControl.CustomProperty;
                _MapControl.Extent = layer.AreaOfInterest;

            }

            public override string Caption
            {
                get
                {
                    return "缩放到图层";
                }
            }
        }
       
        //TODO finish ZoomInCommand
        public sealed class ConstFactorZoomOutCommand : BaseCommand
        {
            private HookHelper m_HookHelper = new HookHelperClass();

            public override void OnCreate(object hook)
            {
                if (hook == null)
                    return;
                m_HookHelper.Hook = ((AxMapControl)hook).Object;
            }

            public override void OnClick()
            {
                IActiveView activeView = m_HookHelper.ActiveView;
                IEnvelope envelope = activeView.Extent;

                double zoomFactor = 1.5;
                envelope.Expand((zoomFactor / System.Convert.ToDouble(1.0)), (zoomFactor / System.Convert.ToDouble(1.0)), true);
                activeView.Extent = envelope;
                activeView.Refresh();
            }
            public override string Caption
            {
                get
                {
                    return "ZoomIn";
                }
            }
        }

        public sealed class ConstFactorZoomInCommand : BaseCommand
        {
            private HookHelper m_HookHelper = new HookHelperClass();

            public override void OnCreate(object hook)
            {
                if (hook == null)
                    return;
                m_HookHelper.Hook = ((AxMapControl)hook).Object;
            }

            public override void OnClick()
            {
                IActiveView activeView = m_HookHelper.ActiveView;
                IEnvelope envelope = activeView.Extent;

                double zoomFactor = 1.5;
                envelope.Expand((System.Convert.ToDouble(1.0) / zoomFactor), (System.Convert.ToDouble(1.0) / zoomFactor), true);
                activeView.Extent = envelope;
                activeView.Refresh();
            }
            public override string Caption
            {
                get
                {
                    return "ZoomOut";
                }
            }
        }

        public sealed class OverViewCommand : BaseCommand
        {
            private IMapControl3 _MapControl = null;

            public override void OnCreate(object hook)
            {
                if (hook == null)
                    return;
                _MapControl = (IMapControl3)(((AxMapControl)hook).Object);
            }

            public override void OnClick()
            {
                _MapControl.Extent = _MapControl.FullExtent;
            }

            public override string Caption
            {
                get
                {
                    return "全图";
                }
            }
        }

    }
    
}

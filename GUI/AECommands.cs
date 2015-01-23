using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;

namespace GUI
{
    /// <summary>
    /// Summary description for Command1.
    /// </summary>
    //[Guid("9a67ee8b-5d77-4c10-9fc8-319b63332746")]
    //[ClassInterface(ClassInterfaceType.None)]
    //[ProgId("WindowsFormsApplication2.Command1")]
    namespace AECommand
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
                base.m_caption = "�Ƴ�";  //localizable text
                base.m_message = "�Ƴ�";  //localizable text 
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
            private HookHelper m_HookHelper = new HookHelper();
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
                        return "����Ϊ������";
                    else if (_subType == 2)
                        return "��չλ��С����";
                    else if (_subType == 3)
                        return "���������Χ";
                    
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
                        IMapControl3 map = (IMapControl3)(((AxMapControl)(m_HookHelper.Hook)).Object);
                        ILayer layer = (ILayer)map.CustomProperty;
                        if (layer.MaximumScale == 0 && layer.MinimumScale == 0)
                        {
                            enable = false;
                        }
                    }
                    return enable;
                }
            }

            public override void OnCreate(object hook)
            {
                m_HookHelper.Hook = ((AxMapControl)hook).Object;
            }

            public override void OnClick()
            {
                IMapControl3 map = (IMapControl3)(m_HookHelper.ActiveView.FocusMap);
                if(_subType == 1)
                {
                    ((ILayer)map.CustomProperty).MaximumScale = m_HookHelper.ActiveView.FocusMap.MapScale;
                }
                else if (_subType == 2)
                {
                    ((ILayer)map.CustomProperty).MinimumScale = m_HookHelper.ActiveView.FocusMap.MapScale;
                }
                else if (_subType == 3)
                {
                    ((ILayer)map.CustomProperty).MinimumScale = 0;
                    ((ILayer)map.CustomProperty).MaximumScale = 0;
                }
            }
        }
    }
    
}
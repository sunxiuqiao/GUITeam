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
            private AxTOCControl _TOCControl;
            private long _SubType;

            public RemoveCommand()
            {
                //
                // TODO: Define values for the public properties
                //
                base.m_category = ""; //localizable text
                base.m_caption = "ÒÆ³ý";  //localizable text
                base.m_message = "ÒÆ³ý";  //localizable text 
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
                if (hook == null)
                    return;
                if (_TOCControl == null)
                    _TOCControl = new AxTOCControl();
                _TOCControl = (AxTOCControl)hook;

            }

            /// <summary>
            /// Occurs when this command is clicked
            /// </summary>
            public override void OnClick()
            {
                // TODO: Add Command1.OnClick implementation
                if(_SubType == 1)
                {
                    IMapControl3 map = (IMapControl3)_TOCControl.Buddy;
                    
                    map.Map.DeleteLayer((ILayer)map.CustomProperty);
                }
                else
                {
                    IMapControl3 map = (IMapControl3)_TOCControl.Buddy;
                    map.ClearLayers();
                    _TOCControl.Update();
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
    }
    
}

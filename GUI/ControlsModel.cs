using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.Windows;
using ESRI.ArcGIS.SystemUI;

namespace GUI
{
    class ControlsModel
    {
        private AxMapControl _MapControl = new AxMapControl();
        private readonly AxTOCControl _TOCControl = new AxTOCControl();
        private esriControlsDragDropEffect __MapControlEffect = esriControlsDragDropEffect.esriDragDropNone;

        public ControlsModel()
        {
            MapControl.OnOleDrop += MapControl_OnOleDrop;
            //TOCControl.OnMouseDown += OnMouseDown;
        }

        #region properties
        public AxMapControl MapControl
        {
            get
            {
                return _MapControl;
            }
        }

        public AxTOCControl TOCControl
        {
            get
            {
                return _TOCControl;
            }
        }
        #endregion

        #region Event Handler

        private void OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            MessageBox.Show("Mouse down");
        }

        private void MapControl_OnOleDrop(object sender, IMapControlEvents2_OnOleDropEvent e)
        {
            IDataObjectHelper dataObjectHelper = (IDataObjectHelper)e.dataObjectHelper;
            esriControlsDropAction action = e.dropAction;
            e.effect = (int)esriControlsDragDropEffect.esriDragDropNone;

            if (action == esriControlsDropAction.esriDropEnter)
            {
                if (dataObjectHelper.CanGetFiles() || dataObjectHelper.CanGetNames())
                {
                    __MapControlEffect = esriControlsDragDropEffect.esriDragDropCopy;
                }
            }
            else if (action == esriControlsDropAction.esriDropOver)
            {
                e.effect = (int)__MapControlEffect;
            }
            else if (action == esriControlsDropAction.esriDropped)
            {
                if (dataObjectHelper.CanGetFiles() == true)
                {
                    System.Array filePaths = System.Array.CreateInstance(typeof(string), 0, 0);
                    filePaths = (System.Array)dataObjectHelper.GetFiles();

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        if (MapControl.CheckMxFile(filePaths.GetValue(i).ToString()) == true)
                        {
                            try
                            {
                                MapControl.LoadMxFile(filePaths.GetValue(i).ToString(), Type.Missing, "");
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            IFileName fileName = new FileNameClass();
                            fileName.Path = filePaths.GetValue(i).ToString();
                            CreateLayer((IName)fileName);
                        }
                    }

                }
                else if (dataObjectHelper.CanGetNames() == true)
                {
                    //Get the IEnumName interface through the IDataObjectHelper.
                    IEnumName enumName = dataObjectHelper.GetNames();
                    enumName.Reset();
                    //Get the IName interface.
                    IName name = enumName.Next();
                    //Loop through the names.
                    while (name != null)
                    {
                        //Create a map layer.
                        CreateLayer(name);
                        name = enumName.Next();
                    }
                }
            }

        }

        private void CreateLayer(IName name)
        {
            _MapControl.MousePointer = esriControlsMousePointer.esriPointerHourglass;

            ILayerFactoryHelper layerFactoryHelper = new LayerFactoryHelperClass();

            try
            {
                IEnumLayer layers = layerFactoryHelper.CreateLayersFromName(name);
                layers.Reset();
                ILayer layer = layers.Next();

                while (layer != null)
                {
                    _MapControl.AddLayer(layer, 0);
                    layer = layers.Next();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return;
            }
            _MapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }
        #endregion
    }
        
}

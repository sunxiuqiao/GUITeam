using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using System.Windows;
using ESRI.ArcGIS.esriSystem;


namespace GUI
{
    class ControlsModel
    {
        private readonly AxMapControl _MapControl = new AxMapControl();
        private readonly AxTOCControl _TOCControl = new AxTOCControl();

        private esriControlsDragDropEffect _MapControlEffect = esriControlsDragDropEffect.esriDragDropNone;
        private IToolbarMenu _MapMenu = null;
        private IToolbarMenu _LayerMenu = null;

        public ControlsModel()
        {
            MapControl.OnOleDrop += MapControl_OnOleDrop;
            MapControl.OnMouseDown += MapControl_OnMouseDown;
            TOCControl.OnMouseDown += TOCControl_OnMouseDown;

            InitTOCControlContextMenu();
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

        private void TOCControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 1) return;

            else if (e.button == 2)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map =null;
                ILayer layer = null;
                object other = null;
                object index = null;
                _TOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

                if(item == esriTOCControlItem.esriTOCControlItemMap)
                {
                    _TOCControl.SelectItem(map, null);
                    _MapControl.CustomProperty = layer;
                    if(_MapMenu != null)
                        _MapMenu.PopupMenu(e.x, e.y, _TOCControl.hWnd);
                }
                else if(item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    _TOCControl.SelectItem(layer, null);
                    _MapControl.CustomProperty = layer;
                    if(_LayerMenu != null)
                        _LayerMenu.PopupMenu(e.x, e.y, _TOCControl.hWnd);
                }
            }
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
                    _MapControlEffect = esriControlsDragDropEffect.esriDragDropCopy;
                }
            }
            else if (action == esriControlsDropAction.esriDropOver)
            {
                e.effect = (int)_MapControlEffect;
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

        private void MapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1)
                return;
            if (e.button == 2)
            {
                ESRI.ArcGIS.Controls.ControlsMapViewMenu tool = new ControlsMapViewMenuClass();
                ICommand cmd = (ICommand)tool;
                cmd.OnCreate(_MapControl.Object);
                _MapControl.CurrentTool = cmd as ITool;
            }
        }
        
        #endregion

        private void InitTOCControlContextMenu()
        {
            //init MapMenu
             
            _MapMenu = new ToolbarMenuClass();
            _MapMenu.AddItem(new Commands.OverViewCommand(), -1, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _MapMenu.AddItem(new Commands.RemoveCommand(), 2, -1, true, esriCommandStyles.esriCommandStyleTextOnly);
            //init LayerMenu
            _LayerMenu = new ToolbarMenuClass();
            _LayerMenu.AddItem(new Commands.RemoveCommand(), 1, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new Commands.ScaleThresholdCommand(), 1, -1, true, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new Commands.ScaleThresholdCommand(), 2, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new Commands.ScaleThresholdCommand(), 3, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new Commands.ZoomToLayerCommand(), -1, -1, true, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.SetHook(_MapControl);
            _MapMenu.SetHook(_MapControl);
        }
    }
        
}

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
using GUI.Model;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;


namespace GUI.ViewModel
{
    class ControlsVM
    {
        private static readonly AxMapControl mapControl = new AxMapControl();
        private static readonly AxTOCControl tocControl = new AxTOCControl();

        private esriControlsDragDropEffect _MapControlEffect = esriControlsDragDropEffect.esriDragDropNone;
        private IToolbarMenu _MapMenu = null;
        private IToolbarMenu _LayerMenu = null;

        public ControlsVM()
        {
            MapControl().OnOleDrop += MapControl_OnOleDrop;
            MapControl().OnMouseDown += MapControl_OnMouseDown;
            TOCControl().OnMouseDown += TOCControl_OnMouseDown;
            MapControl().HandleDestroyed += MapControl_HandelDestroyed;

            InitTOCControlContextMenu();
        }

        #region static functions
        public static AxMapControl MapControl()
        {
            return mapControl;
        }
        public static AxTOCControl TOCControl()
        {
            return tocControl;
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
                tocControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

                if(item == esriTOCControlItem.esriTOCControlItemMap)
                {
                    tocControl.SelectItem(map, null);
                    mapControl.CustomProperty = layer;
                    if(_MapMenu != null)
                        _MapMenu.PopupMenu(e.x, e.y, tocControl.hWnd);
                }
                else if(item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    tocControl.SelectItem(layer, null);
                    mapControl.CustomProperty = layer;
                    if(_LayerMenu != null)
                        _LayerMenu.PopupMenu(e.x, e.y, tocControl.hWnd);
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
                        if (MapControl().CheckMxFile(filePaths.GetValue(i).ToString()) == true)
                        {
                            try
                            {
                                MapControl().LoadMxFile(filePaths.GetValue(i).ToString(), Type.Missing, "");
                            }
                            catch (System.Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message);
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
            mapControl.MousePointer = esriControlsMousePointer.esriPointerHourglass;

            ILayerFactoryHelper layerFactoryHelper = new LayerFactoryHelperClass();

            try
            {
                IEnumLayer layers = layerFactoryHelper.CreateLayersFromName(name);
                layers.Reset();
                ILayer layer = layers.Next();

                while (layer != null)
                {
                    mapControl.AddLayer(layer, 0);
                    layer = layers.Next();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + e.Message);
                return;
            }
            mapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }

        private void MapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1)
                return;
            if (e.button == 2)
            {
                //ESRI.ArcGIS.Controls.ControlsMapViewMenu tool = new ControlsMapViewMenuClass();

                //ICommand cmd = tool as ICommand;
                //cmd.OnCreate(mapControl.Object);
                //mapControl.CurrentTool = cmd as ITool;
            }
        }
        //TODO
        private void MapControl_HandelDestroyed(System.Object sender, EventArgs e)
        {
            //IMap map = MapControl().Map;

            //for(int index =0;index<map.LayerCount;++index)
            //{
            //    ILayer lyr = map.get_Layer(index);
            //    IFeatureLayer featurelyr = lyr as IFeatureLayer;
            //    IFeatureClass featureClas = featurelyr.FeatureClass;
            //    IDataset dataset = featureClas as IDataset;
            //    IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
            //    if (workspaceEdit == null)
            //        return;
            //    else
            //    {
            //        if(workspaceEdit.IsBeingEdited())
            //        {
            //            if (System.Windows.Forms.MessageBox.Show("是否保存编辑？", "Save Prompt?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //            {
            //                workspaceEdit.StopEditOperation();
            //                workspaceEdit.StopEditing(true);
            //            }
            //            else
            //            {
            //                workspaceEdit.StopEditOperation();
            //                workspaceEdit.StopEditing(false);
            //            }
            //        }
            //        else
            //        {
            //            workspaceEdit.StopEditOperation();
            //            workspaceEdit.StopEditing(false);
            //        }
            //    }

            //}
        }
        
        #endregion

        private void InitTOCControlContextMenu()
        {
            //init MapMenu
             
            _MapMenu = new ToolbarMenuClass();
            _MapMenu.AddItem(new GUI.Model.Commands.OverViewCommand(), -1, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _MapMenu.AddItem(new GUI.Model.Commands.RemoveCommand(), 2, -1, true, esriCommandStyles.esriCommandStyleTextOnly);
            //init LayerMenu
            _LayerMenu = new ToolbarMenuClass();
            _LayerMenu.AddItem(new GUI.Model.Commands.RemoveCommand(), 1, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new GUI.Model.Commands.ScaleThresholdCommand(), 1, -1, true, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new GUI.Model.Commands.ScaleThresholdCommand(), 2, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new GUI.Model.Commands.ScaleThresholdCommand(), 3, -1, false, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.AddItem(new GUI.Model.Commands.ZoomToLayerCommand(), -1, -1, true, esriCommandStyles.esriCommandStyleTextOnly);
            _LayerMenu.SetHook(mapControl);
            _MapMenu.SetHook(mapControl);
        }
    }
        
}

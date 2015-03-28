using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using GUI.Model;
using GUI.MVVMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.ViewModel
{
    public class DataEditVM : MVVMBase.ObservableObject
    {
        #region member
        bool isDKDraw = false;
        bool isJZDDraw = false;
        bool isJZXDraw = false;
        bool isAttributeEdit = false;
        bool isAnnotationEdit = false;
        bool isStop = false;
        IWorkspaceEdit2 wksEditor;
        //IEngineEditor engineEditor = new EngineEditorClass();
        IOperationStack operationStack = new ControlsOperationStackClass();
        #endregion

        #region Properties
        public bool IsDKDraw
        {
            get { return isDKDraw; }
            set
            {
                isDKDraw = value;
                RaisePropertyChanged("IsDKDraw");
            }
        }

        public bool IsJZXDraw
        {
            get { return isJZXDraw; }
            set
            {
                isJZXDraw = value;
                RaisePropertyChanged("IsJZXDraw");
            }
        }

        public bool IsJZDDraw
        {
            get { return isJZDDraw; }
            set
            {
                isJZDDraw = value;
                RaisePropertyChanged("IsJZDDraw");
            }
        }

        public bool IsAttributeEdit
        {
            get { return isAttributeEdit; }
            set
            {
                isAttributeEdit = value;
                RaisePropertyChanged("IsAttributeEdit");
            }
        }

        public bool IsAnnotationEdit
        {
            get { return isAnnotationEdit; }
            set
            {
                isAnnotationEdit = value;
                RaisePropertyChanged("IsAnnotationEdit");
            }
        }

        /// <summary>
        /// 被绑定在 localProject TabItem
        /// </summary>
        public bool IsStop
        {
            get { return isStop; }
            set
            {
                isStop = value;
                RaisePropertyChanged("IsStop");
            }
        }

        public IWorkspaceEdit2 WKSEditor
        {
            get { return wksEditor; }
            set
            {
                wksEditor = value;
            }
        }

        //public IEngineEditor EngineEditor
        //{
        //    get { return engineEditor; }
        //    set { engineEditor = value; }
        //}
        #endregion

        #region functions
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataEditVM()
        {
        }

        /// <summary>
        /// 指定图层开始编辑
        /// </summary>
        /// <param name="MapControl"></param>
        /// <param name="LyrName"></param>
        private bool StartEdit(IMapControl2 MapControl, string LyrName)
        {
            bool isOk = false;
            try
            {
                IMap map = MapControl.Map;
                int layerIndex = GetLayerByName(map, LyrName);
                if (layerIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("图层错误");
                }
                ILayer currentLayer = map.get_Layer(layerIndex);
                if (currentLayer is IFeatureLayer)
                {
                    IFeatureLayer featureLyr = currentLayer as IFeatureLayer;
                    IDataset dataSet = featureLyr.FeatureClass as IDataset;
                    WKSEditor = dataSet.Workspace as IWorkspaceEdit2;
                    if (!WKSEditor.IsBeingEdited())
                        WKSEditor.StartEditing(true);
                    if (!WKSEditor.IsInEditOperation)
                        WKSEditor.StartEditOperation();
                    isOk = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("图层错误");
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                // If an exception was raised, make sure the edit operation and
                // edit session are discarded.
                try
                {
                    if (WKSEditor.IsInEditOperation)
                    {
                        WKSEditor.AbortEditOperation();
                    }
                    if (WKSEditor.IsBeingEdited())
                    {
                        WKSEditor.StopEditing(false);
                    }
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show(exc.Message);
                }

            }
            return isOk;
        }

        /// <summary>
        /// 停止编辑
        /// </summary>
        private void StopEdit()
        {
            //if (EngineEditor.HasEdits() == false)
            //    EngineEditor.StopEditing(false);
            //else
            //{
            //    if (MessageBox.Show("Save Edits?", "Save Prompt", MessageBoxButtons.YesNo)
            //        == DialogResult.Yes)
            //        EngineEditor.StopEditing(true);
            //    else
            //        EngineEditor.StopEditing(false);
            //}

            try
            {
                if (WKSEditor == null)
                    return;
                else
                {
                    WKSEditor.StopEditOperation();
                    WKSEditor.StopEditing(true);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                // If an exception was raised, make sure the edit operation and
                // edit session are discarded.
                try
                {
                    if (WKSEditor.IsInEditOperation)
                    {
                        WKSEditor.AbortEditOperation();
                    }
                    if (WKSEditor.IsBeingEdited())
                    {
                        WKSEditor.StopEditing(false);
                    }
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show(exc.Message);
                }
            }


        }

        /// <summary>
        /// 通过图层名称获取图层
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="LyrName"></param>
        /// <returns></returns>
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
        #endregion

        #region StartDrawDKCommand
        private void StartDrawDK_Executed()
        {
            if (StartEdit((IMapControl2)ControlsVM.MapControl().Object, "地块"))
            {
                IsStop = false;
                IsDKDraw = true;
            }
        }

        private bool StartDrawDK_CanExecute()
        {
            if (IsJZDDraw || IsAnnotationEdit || IsAttributeEdit || IsJZXDraw)
            {
                return false;
            }
            if (!LocalProjectVM.IsProjectOpened)
            {
                return false;
            }
            return true;
        }
        public System.Windows.Input.ICommand StartDrawDKCommand { get { return new RelayCommand(StartDrawDK_Executed, StartDrawDK_CanExecute); } }
        #endregion

        #region StartDrawJZXCommand
        private void StartDrawJZX_Executed()
        {
            if (StartEdit((IMapControl2)ControlsVM.MapControl().Object, "界址线"))
            {
                IsStop = false;
                IsJZXDraw = true;
            }
        }

        private bool StartDrawJZX_CanExecute()
        {
            if (IsDKDraw || IsAnnotationEdit || IsAttributeEdit || isJZDDraw)
                return false;
            if (!LocalProjectVM.IsProjectOpened)
            {
                return false;
            }
            return true;
        }
        public System.Windows.Input.ICommand StartDrawJZXCommand { get { return new RelayCommand(StartDrawJZX_Executed, StartDrawJZX_CanExecute); } }
        #endregion

        #region StartDrawJZDCommand
        private void StartDrawJZD_Executed()
        {
            if (StartEdit((IMapControl2)ControlsVM.MapControl().Object, "界址点"))
            {
                IsJZDDraw = true;
                IsStop = false;
            }
        }

        private bool StartDrawJZD_CanExecute()
        {
            if (IsDKDraw || IsAnnotationEdit || IsAttributeEdit || IsJZXDraw)
                return false;
            if (!LocalProjectVM.IsProjectOpened)
            {
                return false;
            }
            return true;
        }
        public System.Windows.Input.ICommand StartDrawJZDCommand { get { return new RelayCommand(StartDrawJZD_Executed, StartDrawJZD_CanExecute); } }
        #endregion

        #region StartEditAttributeCommand
        private void StartEditAttribute_Executed()
        {
            IsAttributeEdit = true;
        }

        private bool StartEditAttribute_CanExecute()
        {
            if (IsJZDDraw || IsDKDraw || IsAnnotationEdit || IsJZXDraw)
                return false;
            if (!LocalProjectVM.IsProjectOpened)
            {
                return false;
            }
            return true;
        }
        public System.Windows.Input.ICommand StartEditAttributeCommand { get { return new RelayCommand(StartEditAttribute_Executed, StartEditAttribute_CanExecute); } }
        #endregion

        #region StartEditAnnotationCommand
        private void StartEditAnnotation_Executed()
        {
            IsAnnotationEdit = true;
        }

        private bool StartEditAnnotation_CanExecute()
        {
            if (IsJZDDraw || IsAnnotationEdit || IsDKDraw || IsJZXDraw)
                return false;
            if (!LocalProjectVM.IsProjectOpened)
            {
                return false;
            }
            return true;
        }
        public System.Windows.Input.ICommand StartEditAnnotationCommand { get { return new RelayCommand(StartEditAnnotation_Executed, StartEditAnnotation_CanExecute); } }
        #endregion

        #region StopDrawDKCommand
        private void StopDrawDK_Executed()
        {
            StopEdit();
            IsDKDraw = false;
            IsStop = true;
        }

        private bool StopDrawDK_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopDrawDKCommand { get { return new RelayCommand(StopDrawDK_Executed, StopDrawDK_CanExecute); } }

        #endregion

        #region StopDrawJZXCommand
        private void StopDrawJZX_Executed()
        {
            StopEdit();
            IsStop = true;
            IsJZXDraw = false;
        }

        private bool StopDrawJZX_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopDrawJZXCommand { get { return new RelayCommand(StopDrawJZX_Executed, StopDrawJZX_CanExecute); } }

        #endregion

        #region StopDrawJZDCommand
        private void StopDrawJZD_Executed()
        {
            StopEdit();
            IsJZDDraw = false;
            IsStop = true;
        }

        private bool StopDrawJZD_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopDrawJZDCommand { get { return new RelayCommand(StopDrawJZD_Executed, StopDrawJZD_CanExecute); } }

        #endregion

        #region StopEditAttributeCommand
        private void StopEditAttribute_Executed()
        {
            IsAttributeEdit = false;
        }

        private bool StopEditAttribute_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopEditAttributeCommand { get { return new RelayCommand(StopEditAttribute_Executed, StopEditAttribute_CanExecute); } }

        #endregion

        #region StopEditAnnotationCommand
        private void StopEditAnnotation_Executed()
        {
            IsAnnotationEdit = false;
        }

        private bool StopEditAnnotation_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopEditAnnotationCommand { get { return new RelayCommand(StopEditAnnotation_Executed, StopEditAnnotation_CanExecute); } }

        #endregion

        #region SketchCommand
        private void SketchCommand_Executed()
        {
            ESRI.ArcGIS.Controls.ControlsEditingSketchTool tool = new ESRI.ArcGIS.Controls.ControlsEditingSketchToolClass();
            ESRI.ArcGIS.SystemUI.ICommand cmd = tool as ESRI.ArcGIS.SystemUI.ICommand;
            cmd.OnCreate(ControlsVM.MapControl().Object);
            ControlsVM.MapControl().CurrentTool = cmd as ESRI.ArcGIS.SystemUI.ITool;
        }
        private bool SketchCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand SketchCommand { get { return new RelayCommand(SketchCommand_Executed, SketchCommand_CanExecute); } }

        #endregion

        #region EditCommand
        private void EditCommand_Executed()
        {
            ITool tool = new Model.DataEditTools.SelectFeaturesTool();
            ESRI.ArcGIS.SystemUI.ICommand cmd = tool as ESRI.ArcGIS.SystemUI.ICommand;
            cmd.OnCreate(ControlsVM.MapControl().Object);
            ControlsVM.MapControl().CurrentTool = cmd as ESRI.ArcGIS.SystemUI.ITool;
        }
        private bool EditCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand EditCommand { get { return new RelayCommand(EditCommand_Executed, EditCommand_CanExecute); } }

        #endregion

        #region UndoCommand
        private void UndoCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new ESRI.ArcGIS.Controls.ControlsUndoCommandClass();
            cmd.OnCreate(ControlsVM.MapControl().Object);
            cmd.OnClick();
        }
        private bool UndoCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand UndoCommand { get { return new RelayCommand(UndoCommand_Executed, UndoCommand_CanExecute); } }

        #endregion

        #region RedoCommand
        private void RedoCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new ESRI.ArcGIS.Controls.ControlsRedoCommandClass();
            cmd.OnCreate(ControlsVM.MapControl().Object);
            cmd.OnClick();
        }
        private bool RedoCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand RedoCommand { get { return new RelayCommand(RedoCommand_Executed, RedoCommand_CanExecute); } }

        #endregion

        #region CutCommand
        private void CutCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new ESRI.ArcGIS.Controls.ControlsEditingCutCommandClass();
            cmd.OnCreate(ControlsVM.MapControl().Object);
            cmd.OnClick();
        }
        private bool CutCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand CutCommand { get { return new RelayCommand(CutCommand_Executed, CutCommand_CanExecute); } }

        #endregion

        #region CopyCommand
        private void CopyCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new ESRI.ArcGIS.Controls.ControlsEditingCopyCommandClass();
            cmd.OnCreate(ControlsVM.MapControl().Object);
            cmd.OnClick();
        }
        private bool CopyCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand CopyCommand { get { return new RelayCommand(CopyCommand_Executed, CopyCommand_CanExecute); } }

        #endregion

        #region PasteCommand
        private void PasteCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new ESRI.ArcGIS.Controls.ControlsEditingPasteCommandClass();
            cmd.OnCreate(ControlsVM.MapControl().Object);
            cmd.OnClick();
        }
        private bool PasteCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand PasteCommand { get { return new RelayCommand(PasteCommand_Executed, PasteCommand_CanExecute); } }

        #endregion

        #region ClearCommand
        private void ClearCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new ESRI.ArcGIS.Controls.ControlsEditingClearCommandClass();
            cmd.OnCreate(ControlsVM.MapControl().Object);
            cmd.OnClick();
        }
        private bool ClearCommand_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand ClearCommand { get { return new RelayCommand(ClearCommand_Executed, ClearCommand_CanExecute); } }

        #endregion

    }
}

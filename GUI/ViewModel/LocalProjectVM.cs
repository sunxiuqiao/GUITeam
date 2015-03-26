using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using GUI.MVVMBase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.ViewModel
{
    public class LocalProjectVM : MVVMBase.ObservableObject
    {
        private static string projectPath = "";
        private static bool isProjectOpened = false;

        #region Properties
        public static string ProjectPath
        {
            get { return projectPath; }
            set { projectPath = value; }
        }

        public static bool IsProjectOpened
        {
            get { return isProjectOpened; }
            set 
            {
                isProjectOpened = value;
            }
        }
        #endregion

        #region AddDataCommand
        private bool AddDataCommand_CanExecute()
        {
            return IsProjectOpened;
        }
        private void AddDataCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd;
            cmd = new ESRI.ArcGIS.Controls.ControlsAddDataCommandClass();
            cmd.OnCreate(ControlsViewModel.MapControl().Object);
            cmd.OnClick();

        }

        //private IRasterLayer CreateRasterLayerFromFile(System.IO.FileInfo fileInfo)
        //{
        //    try
        //    {
        //        IWorkspaceFactory pWSF;
        //        pWSF = new RasterWorkspaceFactoryClass();

        //IWorkspace pWS;
        //        pWS = pWSF.OpenFromFile(fileInfo.DirectoryName, 0);

        //        IRasterWorkspace pRWS;
        //        pRWS = pWS as IRasterWorkspace;


        //        IRasterDataset pRasterDataset;
        //        pRasterDataset = pRWS.OpenRasterDataset(fileInfo.Name);

        //        //影像金字塔的判断与创建
        //        IRasterPyramid pRasPyrmid;
        //        pRasPyrmid = pRasterDataset as IRasterPyramid;

        //        if (pRasPyrmid != null)
        //        {
        //            if (!(pRasPyrmid.Present))
        //            {
        //                pRasPyrmid.Create();
        //            }
        //        }

        //        IRaster pRaster;
        //        pRaster = pRasterDataset.CreateDefaultRaster();

        //        IRasterLayer pRasterLayer = new RasterLayerClass();
        //        pRasterLayer.CreateFromRaster(pRaster);
        //        return pRasterLayer;
        //    }
        //    catch(Exception ex)
        //    {
        //        System.Windows.MessageBox.Show("Error: " + ex.Message);
        //        return null;
        //    }
        //}


        public System.Windows.Input.ICommand AddDataCommand { get { return new RelayCommand(AddDataCommand_Executed, AddDataCommand_CanExecute); } }


        #endregion

        #region OpenFileCommand
        private bool OpenProjectCommand_CanExecute()
        {
            return true;
        }
        private void OpenProjectCommand_Executed()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                //string DefaultfilePath = "";
                dialog.Description = "请选择地理数据库路径";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ProjectPath = dialog.SelectedPath;
                    LoadGDBFile(dialog.SelectedPath);
                    IsProjectOpened = true;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            

        }
        public System.Windows.Input.ICommand OpenProjectCommand { get { return new RelayCommand(OpenProjectCommand_Executed, OpenProjectCommand_CanExecute); } }

        #endregion

        #region NewProjectCommand
        private bool NewProjectCommand_CanExecute()
        {
            return true;
        }
        private void NewProjectCommand_Executed()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                //string DefaultfilePath = "";
                dialog.Description = "请选择创建地理数据库路径";
                dialog.SelectedPath = ProjectPath;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //记录选中的目录
                    //DefaultfilePath = dialog.SelectedPath;
                    string StyleTargetPath = dialog.SelectedPath;
                    string GDBTargetPath = dialog.SelectedPath + "\\KJK.gdb";

                    //copy style file
                    string StyleFileSourceFile = System.IO.Path.Combine(@"../../Config", "ESRI.ServerStyle");
                    string StyleFileDestFile = System.IO.Path.Combine(StyleTargetPath, "ESRI.ServerStyle");
                    System.IO.File.Copy(StyleFileSourceFile, StyleFileDestFile, true);
                    //copy gdb
                    if (!System.IO.Directory.Exists(GDBTargetPath))
                    {
                        System.IO.Directory.CreateDirectory(GDBTargetPath);
                    }
                    if (System.IO.Directory.Exists(GDBTargetPath))
                    {
                        string[] Files = System.IO.Directory.GetFiles(@"../../Config/KJK.gdb");
                        foreach (string File in Files)
                        {
                            string FileName = System.IO.Path.GetFileName(File);
                            string GDBFileDestFile = System.IO.Path.Combine(GDBTargetPath, FileName);
                            string GDBFileSourceFile = System.IO.Path.Combine(@"../../Config/KJK.gdb", FileName);
                            System.IO.File.Copy(GDBFileSourceFile, GDBFileDestFile, true);
                        }
                    }
                    ProjectPath = GDBTargetPath;
                    LoadGDBFile(GDBTargetPath);
                    IsProjectOpened = true;
                }
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            
        }
        public System.Windows.Input.ICommand NewProjectCommand { get { return new RelayCommand(NewProjectCommand_Executed, NewProjectCommand_CanExecute); } }

        private void LoadGDBFile(string filePath)
        {
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(filePath, 0);
            IFeatureDataset featureDataset = featureWorkspace.OpenFeatureDataset("CBQ");
            IFeatureClassContainer featureClassContainer = featureDataset as IFeatureClassContainer;
            for (int i = 0; i < featureClassContainer.ClassCount; i++)
            {
                IFeatureClass featureclass = featureClassContainer.get_Class(i);
                IFeatureLayer layer = new FeatureLayerClass();
                layer.FeatureClass = featureclass;
                layer.Name = featureclass.AliasName;
                ControlsViewModel.MapControl().AddLayer(layer as ILayer);
            }
        }
        #endregion

        #region ZoomIn ZoomOut
        private bool ZoomInCommand_CanExecute()
        {
            return IsProjectOpened;
        }

        private void ZoomInCommand_Executed()
        {
            ESRI.ArcGIS.Controls.ControlsMapZoomInTool tool = new ESRI.ArcGIS.Controls.ControlsMapZoomInToolClass();
            ICommand cmd = tool as ICommand;
            cmd.OnCreate(ControlsViewModel.MapControl().Object);
            ControlsViewModel.MapControl().CurrentTool = cmd as ITool;
        }

        private bool ZoomOutCommand_CanExecute()
        {
            return IsProjectOpened;
        }

        private void ZoomOutCommand_Executed()
        {
            ESRI.ArcGIS.Controls.ControlsMapZoomOutTool tool = new ESRI.ArcGIS.Controls.ControlsMapZoomOutToolClass();
            ICommand cmd = tool as ICommand;
            cmd.OnCreate(ControlsViewModel.MapControl().Object);
            ControlsViewModel.MapControl().CurrentTool = cmd as ITool;
        }

        private bool ConstFactorZoomInCommand_CanExecute()
        {
            return IsProjectOpened;
        }

        private void ConstFactorZoomInCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new GUI.Model.Commands.ConstFactorZoomInCommand();
            cmd.OnCreate(ControlsViewModel.MapControl());
            cmd.OnClick();
        }

        private bool ConstFactorZoomOutCommand_CanExecute()
        {
            return IsProjectOpened;
        }

        private void ConstFactorZoomOutCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new GUI.Model.Commands.ConstFactorZoomOutCommand();
            cmd.OnCreate(ControlsViewModel.MapControl());
            cmd.OnClick();
        }

        public System.Windows.Input.ICommand ZoomInCommand { get { return new RelayCommand(ZoomInCommand_Executed, ZoomInCommand_CanExecute); } }
        public System.Windows.Input.ICommand ConstFactorZoomOutCommand { get { return new RelayCommand(ConstFactorZoomOutCommand_Executed, ConstFactorZoomOutCommand_CanExecute); } }
        public System.Windows.Input.ICommand ConstFactorZoomInCommand { get { return new RelayCommand(ConstFactorZoomInCommand_Executed, ConstFactorZoomInCommand_CanExecute); } }
        public System.Windows.Input.ICommand ZoomOutCommand { get { return new RelayCommand(ZoomOutCommand_Executed, ZoomOutCommand_CanExecute); } }


        #endregion

        #region MapPanCommand
        private bool MapPanCommand_CanExecute()
        {
            return IsProjectOpened;
        }

        private void MapPanCommand_Executed()
        {
            ESRI.ArcGIS.Controls.ControlsMapPanTool tool = new ESRI.ArcGIS.Controls.ControlsMapPanToolClass();
            ICommand cmd = tool as ICommand;
            cmd.OnCreate(ControlsViewModel.MapControl().Object);
            ControlsViewModel.MapControl().CurrentTool = cmd as ITool;
        }
        public System.Windows.Input.ICommand MapPanCommand { get { return new RelayCommand(MapPanCommand_Executed, MapPanCommand_CanExecute); } }

        #endregion

        #region OverView
        private bool OverViewCommand_CanExecute()
        {
            return IsProjectOpened;
        }

        private void OverViewCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd = new GUI.Model.Commands.OverViewCommand();
            cmd.OnCreate(ControlsViewModel.MapControl());
            cmd.OnClick();
        }
        public System.Windows.Input.ICommand OverViewCommand { get { return new RelayCommand(OverViewCommand_Executed, OverViewCommand_CanExecute); } }
        #endregion

        #region LoadDataFromShp
        private void LoadDataFromShpCommand_Executed()
        {
            Model.CollectionData.LoadShpFile(ControlsViewModel.MapControl());
        }

        private bool LoadDataFromShpCommand_CanExecute()
        {
            return IsProjectOpened;
        }
        public System.Windows.Input.ICommand LoadDataFromShpCommand { get { return new RelayCommand(LoadDataFromShpCommand_Executed, LoadDataFromShpCommand_CanExecute); } }
        #endregion

        #region StandardLayer
        private bool StandardLayerCommand_CanExecute()
        {
            return IsProjectOpened;
        }
        private void StandardLayerCommand_Executed()
        {
            Model.CollectionData.StandardLayer(ControlsViewModel.MapControl());
        }
        public System.Windows.Input.ICommand StandardLayerCommand { get { return new RelayCommand(StandardLayerCommand_Executed, StandardLayerCommand_CanExecute); } }
        #endregion

        #region SaveAs
        private void SaveAsCommand_Executed()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "选择新建项目路径：";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (ProjectPath == "")
                    {
                        System.Windows.Forms.MessageBox.Show("原项目路径无效！");
                        return;
                    }
                    System.IO.DirectoryInfo dictionaryInfo = System.IO.Directory.GetParent(ProjectPath);
                    string StyleSourcePath = dictionaryInfo.FullName;
                    string GDBSourcePath = ProjectPath;
                    string StyleTargetPath = dialog.SelectedPath;
                    string GDBTargetPath = dialog.SelectedPath + "\\KJK.gdb";

                    //copy stylefile
                    System.IO.File.Copy(System.IO.Path.Combine(StyleSourcePath, "ESRI.ServerStyle"), System.IO.Path.Combine(StyleTargetPath, "ESRI.ServerStyle"), true);
                    //copy gdb
                    if (!System.IO.Directory.Exists(GDBTargetPath))
                    {
                        System.IO.Directory.CreateDirectory(GDBTargetPath);
                    }
                    if (System.IO.Directory.Exists(GDBTargetPath))
                    {
                        string[] Files = System.IO.Directory.GetFiles(GDBSourcePath);
                        foreach (string File in Files)
                        {
                            string FileName = System.IO.Path.GetFileName(File);
                            string GDBFileDestFile = System.IO.Path.Combine(GDBTargetPath, FileName);
                            string GDBFileSourceFile = System.IO.Path.Combine(GDBSourcePath, FileName);
                            System.IO.File.Copy(GDBFileSourceFile, GDBFileDestFile, true);
                        }
                    }
                    ProjectPath = GDBTargetPath;
                    LoadGDBFile(GDBTargetPath);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }
        private bool SaveAsCommand_CanExecute()
        {
            return IsProjectOpened;
        }
        public System.Windows.Input.ICommand SaveAsCommand { get { return new RelayCommand(SaveAsCommand_Executed, SaveAsCommand_CanExecute); } }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;


namespace GUI
{
    class FileViewModel : ObservableObject
    {
        private ControlsModel _ControlsModel = new ControlsModel();
        private string _MapFileName = "null";

        public FileViewModel()
        {
            //_ControlsModel.MapControl.OleDropEnabled = true;
        }

        #region Properties
        public AxMapControl MapControl
        {
            get { return _ControlsModel.MapControl; }
        }
        
        public AxTOCControl TOCControl
        {
            get { return _ControlsModel.TOCControl; }
        }

        public string MapFileName
        {
            get { return _MapFileName; }
            set 
            { 
                _MapFileName = value;
                RaisePropertyChanged("MapFileName");
            }
        }
        #endregion

        #region AddDataCommand
        private bool AddDataCommand_CanExecute()
        {
            return true;
        }
        private void AddDataCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd;
            cmd = new ESRI.ArcGIS.Controls.ControlsAddDataCommandClass();
            cmd.OnCreate(MapControl.Object);
            cmd.OnClick();
        }

        //private IRasterLayer CreateRasterLayerFromFile(System.IO.FileInfo fileInfo)
        //{
        //    try
        //    {
        //        IWorkspaceFactory pWSF;
        //        pWSF = new RasterWorkspaceFactoryClass();

        //        IWorkspace pWS;
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
        private bool OpenFileCommand_CanExecute()
        {
            return true;
        }
        private void OpenFileCommand_Executed()
        {
            ESRI.ArcGIS.SystemUI.ICommand cmd;
           
            //ESRI.ArcGIS.SystemUI.ITool tool;
            cmd = new ESRI.ArcGIS.Controls.ControlsOpenDocCommandClass();
            //cmd = tool as ICommand;
            cmd.OnCreate(MapControl.Object);
            cmd.OnClick();
            //MapControl.CurrentTool = cmd as ITool;
        }
        public System.Windows.Input.ICommand OpenFileCommand { get { return new RelayCommand(OpenFileCommand_Executed, OpenFileCommand_CanExecute); } }

        #endregion


    }
}

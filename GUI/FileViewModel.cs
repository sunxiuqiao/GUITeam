using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;
using System.Windows.Input;

namespace GUI
{
    class FileViewModel : ObservableObject
    {
        private ControlsModel _ControlsModel = new ControlsModel();

        #region Properties
        public AxMapControl MapControl
        {
            get { return _ControlsModel.MapControl; }
        }
        
        public AxTOCControl TOCControl
        {
            get { return _ControlsModel.TOCControl; }
        }
        #endregion

        #region Commands
        private bool OpenFileCommand_CanExecute()
        {
            return true;
        }
        private void OpenFileCommand_Executed()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = "shapeFile(*.shp)|*.shp|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowReadOnly = true;

            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(openFileDialog.FileName);
                if(fileInfo != null)
                {
                    if(MapControl.CheckMxFile(fileInfo.FullName))
                    {
                        MapControl.LoadMxFile(fileInfo.FullName);
                    }
                    else if(fileInfo.Extension == ".shp")
                    {
                        MapControl.AddShapeFile(fileInfo.DirectoryName, fileInfo.Name);
                    }
                }
            }
        }
        public ICommand OpenFileCommand { get { return new RelayCommand(OpenFileCommand_Executed,OpenFileCommand_CanExecute); } }
        #endregion

    }
}

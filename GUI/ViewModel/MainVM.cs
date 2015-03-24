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
using GUI.MVVMBase;
using Microsoft.Win32;
using System.IO;
using System.Windows.Data;  



namespace GUI.ViewModel
{
    class MainVM : MVVMBase.ObservableObject
    {
        private string _MapFileName = "null";
        #region subVM
        private static ControlsViewModel controlsVM = new ControlsViewModel();
        private DBCreationVM dbCreationVM= new DBCreationVM();
        private LocalProjectVM localProjectVM = new LocalProjectVM();
        #endregion

        public MainVM()
        {
            
        }

        #region Properties
        public AxMapControl MapControl
        {
            get { return ControlsViewModel.MapControl(); }
        }
        
        public AxTOCControl TOCControl
        {
            get { return ControlsViewModel.TOCControl(); }
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

        public DBCreationVM DBCreationVM
        {
            get { return dbCreationVM; }
            set { dbCreationVM = value; }
        }

        public ControlsViewModel ControlsVM
        {
            get { return controlsVM; }
            set { controlsVM = value; }
        }
        
        public LocalProjectVM LocalProjectVM
        {
            get { return localProjectVM; }
            set { localProjectVM = value; }
        }
        #endregion


        

    }
}

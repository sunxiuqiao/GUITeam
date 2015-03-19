using GUI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    class GeoDBCreationVM : MVVMBase.ObservableObject
    {
        #region member
        string featureDatasetName;
        string coordSystem;
        string maxX;
        string maxY;
        string minX;
        string minY;
        #endregion
        #region construct
        public GeoDBCreationVM()
        {
        }
        #endregion
        #region Properties
        public string FeatureDatasetName
        {
            get { return featureDatasetName; }
            set
            { 
                featureDatasetName = value;
                RaisePropertyChanged("FeatureDatasetName");
            }
        }

        public string CoordSystem
        {
            get { return coordSystem; }
            set { 
                coordSystem = value;
                RaisePropertyChanged("CoordSystem");
            }
        }

        public string MaxX
        {
            get { return maxX; }
            set { 
                maxX = value;
                RaisePropertyChanged("MaxX");
            }
        }

        public string MaxY
        {
            get { return maxY; }
            set {
                maxY = value;
                RaisePropertyChanged("MaxY");
            }
        }

        public string MinX
        { 
            get { return minX; }
            set { 
                minX = value;
                RaisePropertyChanged("MinX");
            }
        }

        public string MinY
        {
            get { return minY; }
            set {
                minY = value;
                RaisePropertyChanged("MinY");
            }
        }
        #endregion
        #region Command
        #region ExtendFromFileCommand
        private void ExtendFromFileCommand_Excuted()
        {

        }

        private bool ExtendFromFileCommand_CanExcute()
        {
            return true;
        }

        public System.Windows.Input.ICommand ExtendFromFileCommand { get { return new MVVMBase.RelayCommand(ExtendFromFileCommand_Excuted, ExtendFromFileCommand_CanExcute); } }
        #endregion

        #region CoordFromShapeCommand
        private void CoordFromShapeCommand_Excuted()
        {

        }

        private bool CoordFromShapeCommand_CanExcute()
        {
            return true;
        }

        public System.Windows.Input.ICommand CoordFromShapeCommand { get { return new MVVMBase.RelayCommand(CoordFromShapeCommand_Excuted, CoordFromShapeCommand_CanExcute); } }
        #endregion
        #endregion

    }
}

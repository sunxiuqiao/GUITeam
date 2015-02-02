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
        #region construct
        public GeoDBCreationVM()
        {
        }
        #endregion


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
    }
}

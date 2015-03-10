using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.View;
using GUI.MVVMBase;

namespace GUI.ViewModel
{
    class DBCreationVM : MVVMBase.ObservableObject
    {
        #region SubModel 
        private DBCreationDialogVM dbCreationDialogVM = new DBCreationDialogVM();
        #endregion

        #region Properties
        public DBCreationDialogVM DBCreationDialogVM
        {
            get { return dbCreationDialogVM; }
            set { dbCreationDialogVM = value; }
        }

        //public bool IsCreateDBDialogOpen
        //{
        //    get { return dbCreationDialogVM.IsCreationDialogOpen; }
        //    set { dbCreationDialogVM.IsCreationDialogOpen = value;  }
        //}
        #endregion

        #region Commands
        //#region CreationCommand
        //private void CreationCommand_Excuted()
        //{
        //    IsCreateDBDialogOpen = true;
        //}

        //private bool CreationCommand_CanExcute()
        //{
        //    return true;
        //}
          
        //public System.Windows.Input.ICommand CreateCommand { get { return new MVVMBase.RelayCommand(CreationCommand_Excuted, CreationCommand_CanExcute); } }
        //#endregion
        #endregion

    }
}
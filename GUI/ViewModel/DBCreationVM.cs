using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.View;

namespace GUI.ViewModel
{
    class DBCreationVM
    {
        #region Properties
        #region CreationCommand
        private void CreationCommand_Excuted()
        {
            DBCreationDialogView dialog = new DBCreationDialogView();
            dialog.ShowDialog();
        }

        private bool CreationCommand_CanExcute()
        {
            return true;
        }

        public System.Windows.Input.ICommand CreateCommand { get { return new MVVMBase.RelayCommand(CreationCommand_Excuted, CreationCommand_CanExcute); } }
        #endregion
        #endregion
    }
}

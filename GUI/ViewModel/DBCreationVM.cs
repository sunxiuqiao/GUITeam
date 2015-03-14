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
        private DBTabControlVM dbTabControlVM = new DBTabControlVM();
        #endregion

        #region Properties
        public DBCreationDialogVM DBCreationDialogVM
        {
            get { return dbCreationDialogVM; }
            set { dbCreationDialogVM = value; }
        }

        public DBTabControlVM DBTabControlVM
        {
            get { return dbTabControlVM; }
            set { dbTabControlVM = value; }
        }
        #endregion

        #region Commands
        
        #endregion

    }
}
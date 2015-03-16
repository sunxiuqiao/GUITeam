using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.View;
using GUI.MVVMBase;
using Oracle.ManagedDataAccess.Client;


namespace GUI.ViewModel
{
    class DBCreationVM : MVVMBase.ObservableObject
    {
        #region SubModel 
        private DBCreationDialogVM dbCreationDialogVM = new DBCreationDialogVM();
        private DBTabControlVM dbTabControlVM = new DBTabControlVM();
        private string projectName;
        private bool isCreateButtonEnable = true;
        #endregion

        #region Properties
        public DBCreationDialogVM DBCreationDialogVM
        {
            get { return dbCreationDialogVM; }
        }

        public DBTabControlVM DBTabControlVM
        {
            get { return dbTabControlVM; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set 
            { 
                projectName = value;
                //如果ProjectName 为空ConnTest就不能click
                if (!string.IsNullOrEmpty(projectName))
                {
                    DBTabControlVM.GeoConnVM.IsCanConnTest = true;
                    DBTabControlVM.AttributeConnVM.IsCanConnTest = true;
                    DBTabControlVM.BusinessConnVM.IsCanConnTest = true;
                }
                else
                {
                    DBTabControlVM.GeoConnVM.IsCanConnTest = false;
                    DBTabControlVM.AttributeConnVM.IsCanConnTest = false;
                    DBTabControlVM.BusinessConnVM.IsCanConnTest = false;
                }
                RaisePropertyChanged("ProjectName");
            }
        }

        public bool IsCreateButtonEnable
        {
            get 
            {
                if (DBTabControlVM.GeoConnVM.IsSaved)
                    isCreateButtonEnable = true;
                else
                    isCreateButtonEnable = false ;
                return isCreateButtonEnable;
            }
            set
            {
                isCreateButtonEnable = value;
            }
        }


        #endregion

        #region Commands
        
        private void CreateCommand_Excuted()
        {
            DBCreationDialogVM.IsCreateDialogOpened = true;
        }

        private bool CreateCommand_CanExcute()
        {
            return IsCreateButtonEnable;
        }

        public System.Windows.Input.ICommand StartCreateDBCommand { get { return new MVVMBase.RelayCommand(CreateCommand_Excuted, CreateCommand_CanExcute); } }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.View;
using System.Collections.ObjectModel;
using GUI.MVVMBase;

namespace GUI.ViewModel
{
    class DBCreationDialogVM : MVVMBase.ObservableObject
    {
        #region member
        private DialogUserControl currentControl ;
        private string nextStepButtonContent = "下一步";
        System.Windows.Visibility dbCreationProgramBarVisibiliy = System.Windows.Visibility.Hidden;
        private ObservableCollection<DialogUserControl> userControls = new ObservableCollection<DialogUserControl>();
        private bool isCreationDialogOpen = false;

        private int count = 0;
        #endregion


        public DBCreationDialogVM()
        {
            userControls.Add(new GeoDBCreationDialog());
            userControls.Add(new AttributeDBCreationDialog());
            userControls.Add(new BusinessDBCreationDialog());
            currentControl = userControls.First();
        }

        #region properties
        public bool IsCreationDialogOpen
        {
            get { return isCreationDialogOpen; }
            set { isCreationDialogOpen = value;
            RaisePropertyChanged("IsCreationDialogOpen");
            }
        }
        
        public DialogUserControl CurrentControl
        {
            get { return currentControl; }
            set
            {
                currentControl = value;
                RaisePropertyChanged("CurrentControl");
            }
        }


        public string NextStepButtonContent
        {
            get 
            {
                return nextStepButtonContent;
            }
            set
            {
                nextStepButtonContent = value;
                RaisePropertyChanged("NextStepButtonContent");
            }
        }

        public System.Windows.Visibility DBCreationProgramBarVisibiliy
        {
            get { return dbCreationProgramBarVisibiliy; }
            set
            {
                dbCreationProgramBarVisibiliy = value;
                RaisePropertyChanged("DBCreationProgramBarVisibiliy");
            }
        }
        #endregion

        #region NextStepCommand
        private bool NextStepCommand_CanExecute()
        {
            if (count >= userControls.Count)
                return false;
            return true;
        }

        private void NextStepCommand_Executed()
        {
            count++;
            if (count == userControls.Count)
            {
                DBCreationProgramBarVisibiliy = System.Windows.Visibility.Visible;
            }

            else if (count == userControls.Count - 1)
            {
                NextStepButtonContent = "创建";
                CurrentControl = userControls[count];
            }
            else
            {
                CurrentControl = userControls[count];
            }
        }
        public System.Windows.Input.ICommand NextStepCommand { get { return new RelayCommand(NextStepCommand_Executed, NextStepCommand_CanExecute); } }

        #endregion

        #region BackCommand
        private bool BackCommand_CanExecute()
        {
            if (count <= 0)
                return false;
            return true;
        }

        private void BackCommand_Executed()
        {
            count--;
            CurrentControl = userControls[count];
            if (count != userControls.Count)
            {
                NextStepButtonContent = "下一步";
                CurrentControl = userControls[count];
            }
            DBCreationProgramBarVisibiliy = System.Windows.Visibility.Hidden;

        }
        public System.Windows.Input.ICommand BackCommand { get { return new RelayCommand(BackCommand_Executed, BackCommand_CanExecute); } }

        #endregion

        #region CancelCommand
        private bool CancelCommand_CanExecute()
        {
            return true;
        }
        private void CancelCommand_Executed()
        {
            IsCreationDialogOpen = false;
        }
        public System.Windows.Input.ICommand CancelCommand { get { return new RelayCommand(CancelCommand_Executed, CancelCommand_CanExecute); } }

        #endregion

        #region CreateDBCommand
        private void CreationCommand_Excuted()
        {
            IsCreationDialogOpen = true;
        }

        private bool CreationCommand_CanExcute()
        {
            return true;
        }

        public System.Windows.Input.ICommand StartCreateDBCommand { get { return new MVVMBase.RelayCommand(CreationCommand_Excuted, CreationCommand_CanExcute); } }

        #endregion

    }
}
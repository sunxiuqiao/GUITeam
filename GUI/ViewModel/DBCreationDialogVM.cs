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
        private ObservableCollection<DialogUserControl> UserControls = new ObservableCollection<DialogUserControl>();


        private int count = 0;
        #endregion


        public DBCreationDialogVM()
        {
            UserControls.Add(new GeoDBCreationDialog());
            UserControls.Add(new AttributeDBCreationDialog());
            UserControls.Add(new BusinessDBCreationDialog());
            currentControl = UserControls.First();
        }

        //#region properties


        public DialogUserControl CurrentControl
        {
            get { return currentControl; }
            set
            {
                currentControl = value;
                RaisePropertyChanged("CurrentControl");
            }
        }


        #region NextStepCommand
        private bool NextStepCommand_CanExecute()
        {
            if (count >= UserControls.Count)
                return false;
            return true;
        }

        private void NextStepCommand_Executed()
        {
            count++;
            if (count == UserControls.Count)
            {
                DBCreationProgramBarVisibiliy = System.Windows.Visibility.Visible;
            }
                
            else if(count == UserControls.Count-1)
            {
                NextStepButtonContent = "创建";
                CurrentControl = UserControls[count];
            }
            else
            {
                CurrentControl = UserControls[count];
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
            CurrentControl = UserControls[count];
            DBCreationProgramBarVisibiliy = System.Windows.Visibility.Hidden;

        }
        public System.Windows.Input.ICommand BackCommand { get { return new RelayCommand(BackCommand_Executed, BackCommand_CanExecute); } }

        #endregion

        #region NextStepButton Content
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
        #endregion

        #region ProgramBarVisibiliy
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


    }
}
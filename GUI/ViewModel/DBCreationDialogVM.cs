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
        //private AttributeDBCreationDialog attrDBCreationControl =null;
        //private GeoDBCreationDialog geoDBCreationControl = null;
        //private BusinessDBCreationDialog busiDBCreationControl = null;
        private ObservableCollection<DialogUserControl> UserControls = new ObservableCollection<DialogUserControl>();


        private int count = 0;
        #endregion


        public DBCreationDialogVM()
        {
            UserControls.Add(new GeoDBCreationDialog());
            UserControls.Add(new AttributeDBCreationDialog());
            UserControls.Add(new BusinessDBCreationDialog());
            //geoDBCreationControl = new GeoDBCreationDialog();
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

        //public System.Windows.Visibility IsCreateButtonShow
        //{
        //    get 
        //    {
        //        if (CurrentControl == UserControls.Last())
        //            return System.Windows.Visibility.Visible;
        //        else
        //            return System.Windows.Visibility.Hidden;
        //    }
        //    set
        //    {
        //        if (value == System.Windows.Visibility.Visible)
        //            isLastControl = true;
        //        else
        //            isLastControl = false;
        //        RaisePropertyChanged("IsLastControl");
        //    }
            
        //}
        //public System.Windows.Visibility IsBackButtonShow
        //{
        //    get
        //    {
        //        if (isFirstControl ==true)
        //            return System.Windows.Visibility.Hidden;
        //        else
        //            return System.Windows.Visibility.Visible;
        //    }
        //    set
        //    {
        //        if (value == System.Windows.Visibility.Hidden)
        //            isFirstControl = true;
        //        else
        //            isFirstControl = false;
        //        RaisePropertyChanged("IsNotFirstControl");
        //    }
            
        //}
        //public System.Windows.Visibility IsNextButtonShow
        //{
        //    get
        //    {
        //        if (CurrentControl == UserControls.First())
        //            return System.Windows.Visibility.Hidden;
        //        else
        //            return System.Windows.Visibility.Visible;
        //    }
        //    set
        //    {
        //        if (value == System.Windows.Visibility.Hidden)
        //            isLastControl = true;
        //        else
        //            isLastControl = false;
        //        RaisePropertyChanged("IsNotLastControl");
        //    }
            
        //}

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
                System.Windows.MessageBox.Show("haha");
            else
                CurrentControl = UserControls[count];
            
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

        }
        public System.Windows.Input.ICommand BackCommand { get { return new RelayCommand(BackCommand_Executed, BackCommand_CanExecute); } }

        #endregion


    }
}
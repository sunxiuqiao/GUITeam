﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.View;
using System.Collections.ObjectModel;
using GUI.MVVMBase;
using CreateDatabase;

namespace GUI.ViewModel
{
    class DBCreationDialogVM : MVVMBase.ObservableObject
    {
        #region member
        private DialogUserControl currentControl ;
        private string nextStepButtonContent = "下一步";
        System.Windows.Visibility dbCreationProgramBarVisibiliy = System.Windows.Visibility.Hidden;
        private ObservableCollection<DialogUserControl> userControls = new ObservableCollection<DialogUserControl>();
        private bool isCreateDialogOpened = false;
        private int count = 0;
        #endregion

        #region subModel
        GeoDBCreationVM geoDBCreationVM = new GeoDBCreationVM();
        AttriDBCreationVM attriDBCreationVM = new AttriDBCreationVM();
        BusiDBCreationVM busiDBCreationVM = new BusiDBCreationVM();

        //public GeoDBCreationVM GeoDBCreationVM
        //{
        //    get { return geoDBCreationVM; }
        //    set { value = geoDBCreationVM; }
        //}

        //public AttriDBCreationVM AttriDBCreationVM
        //{
        //    get { return attriDBCreationVM; }
        //    set { attriDBCreationVM = value; }
        //}

        //public BusiDBCreationVM BusiDBCreationVM
        //{
        //    get { return busiDBCreationVM; }
        //    set { busiDBCreationVM = value; }
        //}
        #endregion

        public DBCreationDialogVM()
        {
            GeoDBCreationDialog GeoDialog = new GeoDBCreationDialog();
            AttributeDBCreationDialog AttriDialog = new AttributeDBCreationDialog();
            BusinessDBCreationDialog BusiDialog = new BusinessDBCreationDialog();
            GeoDialog.DataContext = geoDBCreationVM;
            AttriDialog.DataContext = attriDBCreationVM;
            BusiDialog.DataContext = busiDBCreationVM;
            userControls.Add(GeoDialog);
            userControls.Add(AttriDialog);
            userControls.Add(BusiDialog);
            currentControl = userControls.First();
        }

        #region properties
        public bool IsCreateDialogOpened
        {
            get { return isCreateDialogOpened; }
            set { isCreateDialogOpened = value;
            RaisePropertyChanged("IsCreateDialogOpened");
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
                //create attributedatabase
                try
                {
                    DBCreationProgramBarVisibiliy = System.Windows.Visibility.Visible;
                    CAttributeDatabase AttributeDatabase = (CAttributeDatabase)AttributeConnVM.Database();
                    ICreation AttributeCreation = new CAttributeTableCreation();
                    AttributeDatabase.Tables.LoadSchemaFromXml("..//..//..//AttributeDBConfig.xml");
                    for (int i = 0; i < AttributeDatabase.Tables.Count; ++i)
                    {
                        AttributeDatabase.Tables[i].Creation = AttributeCreation;
                        AttributeDatabase.Tables[i].Database = AttributeDatabase;
                    }
                    AttributeDatabase.CreateTable();
                    System.Windows.Forms.MessageBox.Show("finish");

                }
                catch(Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
                
                
            }

            else if (count == userControls.Count - 1)
            {
                NextStepButtonContent = "创建";
                CurrentControl = userControls[count];
            }
            else
            {
                NextStepButtonContent = "下一步";
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
            if (count < userControls.Count)
            {
                NextStepButtonContent = "下一步";
                CurrentControl = userControls[count];
            }
            else if(count == userControls.Count)
            {
                NextStepButtonContent = "创建";
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
            IsCreateDialogOpened = false;
        }
        public System.Windows.Input.ICommand CancelCommand { get { return new RelayCommand(CancelCommand_Executed, CancelCommand_CanExecute); } }

        #endregion

        
        

    }
}
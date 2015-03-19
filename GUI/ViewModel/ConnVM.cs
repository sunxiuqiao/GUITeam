using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GUI.MVVMBase;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Windows.Input;

namespace GUI.ViewModel
{
    abstract class  ConnVM : MVVMBase.ObservableObject
    {
        #region Member
        private string serviceName ;
        private string server;
        private string user;
        private string portNumber;
        private string name;
        private string connName;
        private string passWord;

        private bool isCanSave = false;
        private bool isCanConnTest = false;
        private bool isSaved = false;

        #endregion

        #region Constructor
        public ConnVM()
        {
        }
        #endregion

        #region Properties


        public bool IsCanSave
        {
            get { return isCanSave; }
            set { isCanSave = value; RaisePropertyChanged("IsCanSave"); }
        }

        public bool IsSaved
        {
            get { return isSaved; }
            set { isSaved = value; }
        }

        public bool IsCanConnTest
        {
            get
            { return isCanConnTest; }
            set
            { isCanConnTest = value; }
        }

        public string ServiceName
        {
            get { return serviceName; }
            set 
            { 
                serviceName = value;
                RaisePropertyChanged("ServiceName");
                RaisePropertyChanged("IsCanConnTest");
            }
        }

        public string Server
        {
            get { return server; }
            set 
            { 
                server = value;
                RaisePropertyChanged("Server");
                RaisePropertyChanged("IsCanConnTest");
            }
        }

        public string User
        {
            get { return user; }
            set 
            {
                user = value;
                RaisePropertyChanged("User");
                RaisePropertyChanged("IsCanConnTest");
            }
        }

        public string PortNumber
        {
            get { return portNumber; }
            set 
            { 
                portNumber = value;
                RaisePropertyChanged("PortNumber");
                RaisePropertyChanged("IsCanConnTest");
            }
        }

        public string Name
        {
            get { return name; }
            set
            { 
                name = value;
                RaisePropertyChanged("Name");
                RaisePropertyChanged("IsCanConnTest");
            }
        }

        public string ConnName
        {
            get { return connName; }
            set 
            {
                connName = value;
                RaisePropertyChanged("ConnName");
                RaisePropertyChanged("IsCanConnTest");
            }
        }

        public string PassWord
        {
            get { return passWord; }
            set 
            {
                passWord = value;
                RaisePropertyChanged("PassWord");
                RaisePropertyChanged("IsCanConnTest");
            }
        }
        #endregion

        #region function
        #endregion

        #region Command
        protected bool ConnTestCommand_CanExecute()
        {
            return IsCanConnTest;
        }
        abstract protected void ConnTestCommand_Executed();
        public System.Windows.Input.ICommand ConnTestCommand { get { return new RelayCommand(ConnTestCommand_Executed, ConnTestCommand_CanExecute); } }

        
        abstract protected void ConnSaveCommand_Executed();
        public System.Windows.Input.ICommand ConnSaveCommand { get { return new RelayCommand(ConnSaveCommand_Executed); } }

        #endregion

    }

    class GeoConnVM : ConnVM
    {
        #region member
        private static CreateDatabase.IDatabase database = CreateDatabase.CSpatialDatabase.GetInstance();
        #endregion

        public static CreateDatabase.IDatabase Database()
        {
            return database;
        }

        protected override void ConnTestCommand_Executed()
        {
            Database().Name = Name;
            Database().Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                Database().PortNumber = 0;
            }
            else
            {
                try
                {
                    Database().PortNumber = int.Parse(PortNumber);
                }
                catch(Exception e)
                {
                    System.Windows.MessageBox.Show("亲，接口必须是数字哦！例如：5151");
                }
                
            }
            Database().Server = Server;
            Database().ServiceName = ServiceName;
            Database().User = User;
            Database().Version = "SDE.DEFAULT";

            IsCanSave = Database().Open();
            if (IsCanSave)
            {
                System.Windows.MessageBox.Show("spatial database connect is ok");
                RaisePropertyChanged("ConnSaveCommand");
            }
                
            //System.Windows.MessageBox.Show(string.Format("Server:{0},ServiceName:{1},User:{2},PortNumber:{3},PassName:{4}", SpatialDatabase.Server, SpatialDatabase.ServiceName, SpatialDatabase.User, SpatialDatabase.PortNumber, SpatialDatabase.Password));
        }

        protected override void ConnSaveCommand_Executed()
        {

            IsSaved = true;
        }

    }

    class AttributeConnVM : ConnVM
    {
        #region member
        private static CreateDatabase.IDatabase database = CreateDatabase.CAttributeDatabase.GetInstance();
        #endregion

        public static CreateDatabase.IDatabase Database()
        {
            return database;
        }

        protected override void ConnTestCommand_Executed()
        {
            Database().Name = Name;
            Database().Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                Database().PortNumber = 0;
            }
            else
            {
                Database().PortNumber = int.Parse(PortNumber);
            }
            Database().Server = Server;
            Database().ServiceName = ServiceName;
            Database().User = User;

            IsCanSave = Database().Open();
            if (IsCanSave)
                System.Windows.MessageBox.Show("Attribute database connect is ok");
        }

        protected override void ConnSaveCommand_Executed()
        {
            IsSaved = true;
        }
    }

    class BusinessConnVM : ConnVM
    {
        #region member
        private static CreateDatabase.IDatabase database = CreateDatabase.CBusinessDatabase.GetInstance();
        #endregion

        public static CreateDatabase.IDatabase Database()
        {
            return database;
        }

        

        protected override void ConnTestCommand_Executed()
        {
            Database().Name = Name;
            Database().Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                Database().PortNumber = 0;
            }
            else
            {
                Database().PortNumber = int.Parse(PortNumber);
            }
            Database().Server = Server;
            Database().ServiceName = ServiceName;
            Database().User = User;

            IsCanSave = Database().Open();
            if (IsCanSave)
                System.Windows.MessageBox.Show("Attribute database connect is ok");
        }

        protected override void ConnSaveCommand_Executed()
        {
            IsSaved = true;
        }
    }

    
}

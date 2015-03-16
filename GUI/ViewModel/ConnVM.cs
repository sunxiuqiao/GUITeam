using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GUI.MVVMBase;
using Microsoft.Win32;
using System.Windows.Forms;

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
            set { isCanSave = value; }
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

        #region Command
        protected bool ConnTestCommand_CanExecute()
        {
            return IsCanConnTest;
        }
        abstract protected void ConnTestCommand_Executed();
        public System.Windows.Input.ICommand ConnTestCommand { get { return new RelayCommand(ConnTestCommand_Executed, ConnTestCommand_CanExecute); } }


        protected bool ConnSaveCommand_CanExecute()
        {
            return IsCanSave;
        }
        abstract protected void ConnSaveCommand_Executed();
        public System.Windows.Input.ICommand ConnSaveCommand { get { return new RelayCommand(ConnSaveCommand_Executed, ConnSaveCommand_CanExecute); } }

        #endregion

    }

    class GeoConnVM : ConnVM
    {
        protected override void ConnTestCommand_Executed()
        {
            CreateDatabase.IDatabase SpatialDatabase = CreateDatabase.CSpatialDatabase.GetInstance();
            SpatialDatabase.Name = Name;
            SpatialDatabase.Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                SpatialDatabase.PortNumber = 0;
            }
            else
            {
                SpatialDatabase.PortNumber = int.Parse(PortNumber);
            }
            SpatialDatabase.Server = Server;
            SpatialDatabase.ServiceName = ServiceName;
            SpatialDatabase.User = User;
            SpatialDatabase.Version = "SDE.DEFAULT";

            IsCanSave = SpatialDatabase.Open();
            if (IsCanSave)
                System.Windows.MessageBox.Show("spatial database connect is ok");
            //System.Windows.MessageBox.Show(string.Format("Server:{0},ServiceName:{1},User:{2},PortNumber:{3},PassName:{4}", SpatialDatabase.Server, SpatialDatabase.ServiceName, SpatialDatabase.User, SpatialDatabase.PortNumber, SpatialDatabase.Password));
        }


        protected override void ConnSaveCommand_Executed()
        {

            IsSaved = true;
        }

    }

    class AttributeConnVM : ConnVM
    {
        protected override void ConnTestCommand_Executed()
        {
            CreateDatabase.IDatabase AttributeDatabase = CreateDatabase.CAttributeDatabase.GetInstance();
            AttributeDatabase.Name = Name;
            AttributeDatabase.Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                AttributeDatabase.PortNumber = 0;
            }
            else
            {
                AttributeDatabase.PortNumber = int.Parse(PortNumber);
            }
            AttributeDatabase.Server = Server;
            AttributeDatabase.ServiceName = ServiceName;
            AttributeDatabase.User = User;

            IsCanSave = AttributeDatabase.Open();
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
        protected override void ConnTestCommand_Executed()
        {
            CreateDatabase.IDatabase BusinessDatabase = CreateDatabase.CBusinessDatabase.GetInstance();
            BusinessDatabase.Name = Name;
            BusinessDatabase.Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                BusinessDatabase.PortNumber = 0;
            }
            else
            {
                BusinessDatabase.PortNumber = int.Parse(PortNumber);
            }
            BusinessDatabase.Server = Server;
            BusinessDatabase.ServiceName = ServiceName;
            BusinessDatabase.User = User;

            IsCanSave = BusinessDatabase.Open();
            if (IsCanSave)
                System.Windows.MessageBox.Show("Attribute database connect is ok");
        }

        protected override void ConnSaveCommand_Executed()
        {

            IsSaved = true;
        }
    }

    
}

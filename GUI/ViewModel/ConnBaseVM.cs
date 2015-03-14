using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GUI.ViewModel
{
    abstract class  ConnBaseVM : MVVMBase.ObservableObject
    {
        #region Member
        private string serviceName ;
        private string server;
        private string user;
        private string portNumber;
        private string name;
        private string connName;
        private string passWord;

        private bool isCanSave;
        #endregion

        #region Constructor
        public ConnBaseVM()
        {
        }
        #endregion

        #region Properties
        
        public string ServiceName
        {
            get { return serviceName; }
            set 
            { 
                serviceName = value;
                RaisePropertyChanged("ServiceName");
            }
        }
        public string Server
        {
            get { return server; }
            set 
            { 
                server = value;
                RaisePropertyChanged("Server");
            }
        }

        public string User
        {
            get { return user; }
            set 
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }

        public string PortNumber
        {
            get { return portNumber; }
            set 
            { 
                portNumber = value;
                RaisePropertyChanged("PortNumber");
            }
        }

        public string Name
        {
            get { return name; }
            set
            { 
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string ConnName
        {
            get { return connName; }
            set 
            {
                connName = value;
                RaisePropertyChanged("ConnName");
            }
        }

        public string PassWord
        {
            get { return passWord; }
            set 
            {
                passWord = value;
                RaisePropertyChanged("PassWord");
            }
        }
        #endregion

        
    }
}

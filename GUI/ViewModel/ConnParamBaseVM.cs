using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GUI.ViewModel
{
    class ConnParamBaseVM : MVVMBase.ObservableObject
    {
        private string serverNames ;

        public ConnParamBaseVM()
        {
            serverNames = "jaja";
        }

        public string ServerNames
        {
            get { return serverNames; }
            set 
            {
                serverNames = value;
                
            }
        }
    }
}

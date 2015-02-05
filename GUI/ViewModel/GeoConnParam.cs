using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    class GeoConnParam : ConnParamBaseVM
    {
        private string serverNames = "kaka" ;


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

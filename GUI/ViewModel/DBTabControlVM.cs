using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    class DBTabControlVM
    {
        private GeoConnVM geoConnVM = new GeoConnVM();

        #region Properties
        public GeoConnVM GeoConnVM
        {
            get { return geoConnVM; }
            set { geoConnVM = value; }
        }
        #endregion
    }
}

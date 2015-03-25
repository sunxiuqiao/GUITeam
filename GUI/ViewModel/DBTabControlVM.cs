using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    class DBTabControlVM
    {
        #region subVM
        private GeoConnVM geoConnVM = new GeoConnVM();
        private AttributeConnVM attributeConnVM = new AttributeConnVM();
        private BusinessConnVM businessConnVM = new BusinessConnVM();
        #endregion

        public DBTabControlVM()
        {
        }

        #region Properties
        public GeoConnVM GeoConnVM
        {
            get { return geoConnVM; }
            set { geoConnVM = value; }
        }

        public AttributeConnVM AttributeConnVM
        {
            get { return attributeConnVM; }
            set { attributeConnVM = value; }
        }

        public BusinessConnVM BusinessConnVM
        {
            get { return businessConnVM; }
            set { businessConnVM = value; }
        }

        #endregion
    }
}

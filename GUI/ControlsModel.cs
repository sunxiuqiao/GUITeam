using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;

namespace GUI
{
    class ControlsModel
    {
        private readonly AxMapControl _MapControl = new AxMapControl();
        private readonly AxTOCControl _TOCControl = new AxTOCControl();

        public AxMapControl MapControl
        {
            get
            {
                return _MapControl;
            }
        }

        public AxTOCControl TOCControl
        {
            get
            {
                return _TOCControl;
            }
        }
    }
}

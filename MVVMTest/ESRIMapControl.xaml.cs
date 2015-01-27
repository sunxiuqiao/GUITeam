using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;

namespace MVVMTest
{
    /// <summary>
    /// ESRIMapControl.xaml 的交互逻辑
    /// </summary>
    public partial class ESRIMapControl : UserControl
    {
        private AxMapControl mapControl = new AxMapControl();
        public ESRIMapControl()
        {
            InitializeComponent();
            host.Child = mapControl;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mapControl.BorderStyle = esriControlsBorderStyle.esriNoBorder;
            mapControl.KeyIntercept = (int)esriKeyIntercept.esriKeyInterceptArrowKeys;
            mapControl.AutoKeyboardScrolling = true;
            mapControl.AutoMouseWheel = true;
        }

        public void AddLayer(ILayer layer)
        {
            mapControl.AddLayer(layer);
        }

        public void DeleteLayer(ILayer layer)
        {
            for (int i = 0; i < mapControl.LayerCount; ++i)
            {
                if (mapControl.get_Layer(i) == layer)
                    mapControl.DeleteLayer(i);
            }
        }

        //public AxMapControl MapControl { get { return mapControl; } }
    }
}

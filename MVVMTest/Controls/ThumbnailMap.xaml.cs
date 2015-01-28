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
    /// ThumbnailMap.xaml 的交互逻辑
    /// </summary>
    public partial class ThumbnailMap : UserControl
    {
        protected AxMapControl mapControl = new AxMapControl();
        public ThumbnailMap()
        {
            InitializeComponent();
            host.Child = mapControl;

            //mapControl.Show();
        }

        protected ILayer dataLayer = null;
        public ILayer DataLayer
        {
            get { return dataLayer; }
            set { dataLayer = value; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mapControl.BorderStyle = esriControlsBorderStyle.esriNoBorder;
            if (dataLayer!=null)
            {
                mapControl.AddLayer(dataLayer);
            } 
        }

    }
}

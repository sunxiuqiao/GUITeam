using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using Xceed.Wpf.AvalonDock.Layout;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MapControlHost.Child = MainModel.MapControl;
            TOCControlHost.Child = MainModel.TOCControl;
            MainModel.MapControl.CreateControl();
            MainModel.TOCControl.CreateControl();
            MainModel.MapControl.OleDropEnabled = true;
            MainModel.TOCControl.SetBuddyControl(MainModel.MapControl);

        }

      
    }
        
}

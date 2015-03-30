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
using ESRI.ArcGIS.Geodatabase;

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

        private void GUIMainWindow_Closed(object sender, EventArgs e)
        {
            IMap map = ViewModel.ControlsVM.MapControl().Map;

            for (int index = 0; index < map.LayerCount; ++index)
            {
                ILayer lyr = map.get_Layer(index);
                IFeatureLayer featurelyr = lyr as IFeatureLayer;
                IFeatureClass featureClas = featurelyr.FeatureClass;
                IDataset dataset = featureClas as IDataset;
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
                if (workspaceEdit == null)
                    return;
                else
                {
                    if (workspaceEdit.IsBeingEdited())
                    {
                        if (System.Windows.Forms.MessageBox.Show("是否保存编辑？", "Save Prompt?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            workspaceEdit.StopEditOperation();
                            workspaceEdit.StopEditing(true);
                        }
                        else
                        {
                            workspaceEdit.StopEditOperation();
                            workspaceEdit.StopEditing(false);
                        }
                    }
                    else
                    {
                        workspaceEdit.StopEditOperation();
                        workspaceEdit.StopEditing(false);
                    }
                }

            }
        }

        
    }
        
}

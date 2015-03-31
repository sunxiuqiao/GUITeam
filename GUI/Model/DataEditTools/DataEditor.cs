using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Model.DataEditTools
{
    public class DataEditor
    {
        ILayer currentLayer = null;
        IWorkspaceEdit2 wksEditor = null;

        public ILayer CurrentLayer
        {
            get { return currentLayer; }
            set { currentLayer = value; }
        }

        public IWorkspaceEdit2 WKSEditor
        {
            get { return wksEditor; }
            set { wksEditor = value; }
        }


        public DataEditor()
        {
            
        }
        public void StartEdit()
        {
            if (currentLayer is IFeatureLayer)
            {
                IFeatureLayer featureLyr = currentLayer as IFeatureLayer;

                IFeatureClass featureClass = featureLyr.FeatureClass;
                IDataset dataSet = featureClass as IDataset;
                WKSEditor = dataSet.Workspace as IWorkspaceEdit2;
            }
            if (WKSEditor == null)
                return;
            if (!WKSEditor.IsBeingEdited())
                WKSEditor.StartEditing(true);
        }

        public void StopEdit()
        {
            try
            {
                if (WKSEditor == null)
                    return;
                else
                {
                    if (!WKSEditor.IsBeingEdited())
                    {
                        WKSEditor.StopEditOperation();
                        WKSEditor.StopEditing(false);
                    }
                    else
                    {
                        if (MessageBox.Show("是否保存编辑？", "Save Prompt?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            WKSEditor.StopEditOperation();
                            WKSEditor.StopEditing(true);
                        }
                        else
                        {
                            WKSEditor.StopEditOperation();
                            WKSEditor.StopEditing(false);
                        }
                    }
                }
                IActiveView activeView = ControlsVM.MapControl().ActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                // If an exception was raised, make sure the edit operation and
                // edit session are discarded.
                try
                {
                    if (WKSEditor.IsInEditOperation)
                    {
                        WKSEditor.AbortEditOperation();
                    }
                    if (WKSEditor.IsBeingEdited())
                    {
                        WKSEditor.StopEditing(false);
                    }
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show(exc.Message);
                }
            }


        }
    }
}

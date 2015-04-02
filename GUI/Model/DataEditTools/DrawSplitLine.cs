using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model.DataEditTools
{
    class DrawSplitLine : DataEditTools.DrawPolyline
    {
        private DataEditor dataEdit = null;
        public DrawSplitLine(DataEditor edit)
        {
            dataEdit = edit;
        }

        public DataEditor DataEdit
        {
            get { return dataEdit; }
            set { dataEdit = value; }
        }

        public override void OnDblClick()
        {
            DataEdit.WKSEditor.StartEditOperation();
            base.OnDblClick();
            IMap map = ControlsVM.MapControl().Map;
            ISelection selection = map.FeatureSelection;
            IEnumFeature features = selection as IEnumFeature;
            IFeature feature = features.Next();
            if (feature == null)
                return;
            IFeatureEdit2 featureEdit = feature as IFeatureEdit2;
            ISet set = featureEdit.SplitWithUpdate(m_geometry);

            ControlsVM.MapControl().ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            DataEdit.WKSEditor.StopEditOperation();
        }

    }
}

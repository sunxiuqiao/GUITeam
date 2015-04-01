using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model.DataEditTools
{
    public class DrawDKTool : DrawPolygon
    {
        protected DataEditor dataEdit = null;

        public DrawDKTool(DataEditor edit)
        {
            dataEdit = edit;
        }

        public DataEditor DataEdit
        {
            get { return dataEdit; }
            set
            {
                dataEdit = value;
            }
        }
        public override void OnDblClick()
        {
            DataEdit.WKSEditor.StartEditOperation();
            base.OnDblClick();
            if (m_geometry != null)
            {
                Draw(m_geometry);
            }
            DataEdit.WKSEditor.StopEditOperation();
        }

        private void Draw(IGeometry geometry)
        {
            try
            {
                IFeatureLayer featureLyr = DataEdit.CurrentLayer as IFeatureLayer;
                IFeatureClass featureClass = featureLyr.FeatureClass;
                IWorkspaceEdit2 workspaceEdit = DataEdit.WKSEditor;
                IFeatureBuffer featBuffer = featureClass.CreateFeatureBuffer();
                featBuffer.Shape = geometry;
                SetFieldValue(featureLyr, featBuffer, "YSDM", "211011");
                IFeatureCursor featCursor = featureClass.Insert(true);
                featCursor.InsertFeature(featBuffer);
                    
                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        private static void SetFieldValue(IFeatureLayer Layer, IFeatureBuffer Feature, string Field, object Value)
        {
            IFeatureClass pFeatureCls = Layer.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(Field);
            Feature.set_Value(pFieldIndex, Value);
        }
    }
}

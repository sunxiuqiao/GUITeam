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
    class DrawJZXTool : DataEditTools.DrawPolyline
    {
        private DataEditor dataEdit = null;

        public DrawJZXTool(DataEditor edit)
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
            if (geometry != null)
            {
                Draw(geometry);
            }
            DataEdit.WKSEditor.StopEditOperation();
        }

        

        private void Draw(IGeometry geometry)
        {
            try
            {
                ILayer layer = DataEdit.CurrentLayer;
                if (layer is IFeatureLayer)
                {
                    IFeatureLayer featureLyr = layer as IFeatureLayer;
                    IFeatureClass featureClass = featureLyr.FeatureClass;
                    IWorkspaceEdit workspaceEdit = dataEdit.WKSEditor;
                    IFeatureBuffer featBuffer = featureClass.CreateFeatureBuffer();
                    featBuffer.Shape = geometry;
                    SetFieldValue(featureLyr, featBuffer, "YSDM", "211031");
                    IFeatureCursor featCursor = featureClass.Insert(true);
                    featCursor.InsertFeature(featBuffer);

                }

                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        private int GetLayerByName(IMap Map, string LyrName)
        {
            try
            {
                int index = -1;
                for (int i = 0; i < Map.LayerCount; ++i)
                {
                    if (Map.get_Layer(i).Name == LyrName)
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return -1;
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

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
    class DrawJZDTool:DrawPoint
    {
        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            base.OnMouseDown(Button, Shift,  X,  Y);
            if (geometry != null)
            {
                Draw(geometry);
            }
        }

        private void Draw(IGeometry geometry)
        {
            try
            {
                if (geometry == null)
                    return;
                IMap map = m_hookHelper.FocusMap;
                ILayer layer = null;
                if (geometry.GeometryType.Equals(esriGeometryType.esriGeometryPoint))
                {
                    int layerIndex = GetLayerByName(map, "界址点");
                    if (layerIndex == -1)
                    {
                        System.Windows.Forms.MessageBox.Show("图层错误");
                        return;
                    }
                    layer = map.get_Layer(layerIndex);
                    if (layer is IFeatureLayer)
                    {
                        IFeatureLayer featureLyr = layer as IFeatureLayer;
                        IFeatureClass featureClass = featureLyr.FeatureClass;
                        IDataset dataSet = featureClass as IDataset;
                        IWorkspace workSpace = dataSet.Workspace;
                        IWorkspaceEdit workspaceEdit = workSpace as IWorkspaceEdit;
                        IFeatureBuffer featBuffer = featureClass.CreateFeatureBuffer();
                        featBuffer.Shape = geometry;
                        SetFieldValue(featureLyr, featBuffer, "YSDM", "211021");
                        IFeatureCursor featCursor = featureClass.Insert(true);
                        featCursor.InsertFeature(featBuffer);

                    }
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

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model.DataEditTools
{
    class FeatureMerge : BaseCommand
    {
        private HookHelper m_HookHelper = new HookHelperClass();
        private DataEditor dataEdit = null;

        public FeatureMerge(DataEditor edit)
        {
            dataEdit = edit;
        }

        public DataEditor DataEdit
        {
            get { return dataEdit; }
            set { dataEdit = value; }
        }


        public override void OnCreate(object hook)
        {
            m_HookHelper.Hook = hook;
            base.m_category = "DataEditTools/MergeCommand"; //localizable text 
            base.m_caption = "合并";  //localizable text 
            base.m_message = "合并";  //localizable text
            base.m_toolTip = "合并";  //localizable text
            base.m_name = "DataEditTools_MergeCommand";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
        }

        public override void OnClick()
        {
            dataEdit.WKSEditor.StartEditOperation();
            IEnumFeature features = m_HookHelper.FocusMap.FeatureSelection as IEnumFeature ;
            
            features.Reset();
            IFeature feature = features.Next();
            IGeometryCollection geometryBag = new GeometryBagClass();
            while (feature != null)
            {
                geometryBag.AddGeometry(feature.Shape);
                feature.Delete();
                feature = features.Next();
            }
            ITopologicalOperator topo = new PolygonClass();
            topo.ConstructUnion(geometryBag as IEnumGeometry);


            IFeatureLayer featureLyr = DataEdit.CurrentLayer as IFeatureLayer;
            IFeatureClass featureClass = featureLyr.FeatureClass;
            IDataset dataSet = featureClass as IDataset;
            
            IWorkspaceEdit2 workspaceEdit = DataEdit.WKSEditor;

            IFeatureBuffer featBuffer = featureClass.CreateFeatureBuffer();
            featBuffer.Shape = topo as IGeometry;
            SetFieldValue(featureLyr, featBuffer, "YSDM", "211011");
            IFeatureCursor featCursor = featureClass.Insert(true);
            featCursor.InsertFeature(featBuffer);

            DataEdit.WKSEditor.StopEditOperation();
            m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            
        }
        private static void SetFieldValue(IFeatureLayer Layer, IFeatureBuffer Feature, string Field, object Value)
        {
            IFeatureClass pFeatureCls = Layer.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(Field);
            Feature.set_Value(pFieldIndex, Value);
        }

    }
}

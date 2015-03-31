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
        private ILayer currentLayer = null;

        public ILayer CurrentLayer
        {
            get { return currentLayer; }
            set { currentLayer = value; }
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
            IEnumFeature features = m_HookHelper.FocusMap.FeatureSelection as IEnumFeature ;
            features.Reset();
            IFeature feature = features.Next();
            bool isCanMerge = true;
            while(feature != null)
            {
                IRelationalOperator relOp = feature.Shape as IRelationalOperator;
                IFeature nextFeature = features.Next();

                if (nextFeature == null)
                    break;
                else
                {
                    IGeometry geo = nextFeature.Shape;
                    isCanMerge = (relOp.Touches(geo)||(!relOp.Disjoint(geo)));
                }
                    
                feature = nextFeature;
            }
            if (isCanMerge == false)
                return;
            else
            {
                features.Reset();
                feature = features.Next();
                if (feature == null)
                    return;
                ITopologicalOperator topoOp = (ITopologicalOperator)feature.Shape;
                IFeature feature2 = features.Next();
                if (feature2 == null)
                    return;
                IGeometry UnionGeometry = topoOp.Union(feature2.Shape);

                IFeatureLayer featureLyr = currentLayer as IFeatureLayer;
                IFeatureClass featureClass = featureLyr.FeatureClass;
                IDataset dataSet = featureClass as IDataset;
                IWorkspace workSpace = dataSet.Workspace;
                IWorkspaceEdit2 workspaceEdit = workSpace as IWorkspaceEdit2;
                if (!(workspaceEdit.IsInEditOperation))
                    workspaceEdit.StartEditOperation();
                IFeatureBuffer featBuffer = featureClass.CreateFeatureBuffer();
                featBuffer.Shape = UnionGeometry;
                SetFieldValue(featureLyr, featBuffer, "YSDM", "211011");
                IFeatureCursor featCursor = featureClass.Insert(true);
                featCursor.InsertFeature(featBuffer);
                m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
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

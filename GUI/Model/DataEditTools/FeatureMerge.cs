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
            //features.Reset();
            //IFeature feature = features.Next();
            //bool isCanMerge = true;
            //while(feature != null)
            //{
            //    IRelationalOperator relOp = feature.Shape as IRelationalOperator;
            //    IFeature nextFeature = features.Next();

            //    if (nextFeature == null)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        IGeometry geo = nextFeature.Shape;
            //        isCanMerge = (relOp.Touches(geo)||(!relOp.Disjoint(geo)));
            //    }
                    
            //    feature = nextFeature;
            //}
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
            m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            //if (isCanMerge == false)
            //    return;
            //else
            //{
                //ITopologicalOperator2 topoOp = (IT)
                //features.Reset();
                //feature = features.Next();
                //IFeature feature2 = features.Next();
                //if (feature == null || feature2==null)
                //    return;
                //ITopologicalOperator2 topoOp1 = (ITopologicalOperator2)feature.Shape;
                //topoOp1.IsKnownSimple_2 = false;
                //topoOp1.Simplify();
                //feature.Shape.SnapToSpatialReference();
                //feature2.Shape.SnapToSpatialReference();
                ////IEnumGeometry geometrys = feature2.Shape 
                //topoOp1.ConstructUnion(feature2.ShapeCopy);
                //feature2.Delete();



                //IFeatureLayer featureLyr = DataEdit.CurrentLayer as IFeatureLayer;
                //IFeatureClass featureClass = featureLyr.FeatureClass;
                //IDataset dataSet = featureClass as IDataset;
                ////IWorkspace workSpace = dataSet.Workspace;
                //IWorkspaceEdit2 workspaceEdit = DataEdit.WKSEditor;
                //if (!(workspaceEdit.IsInEditOperation))
                //    workspaceEdit.StartEditOperation();
                //IFeatureBuffer featBuffer = featureClass.CreateFeatureBuffer();
                //featBuffer.Shape = UnionGeometry;
                //SetFieldValue(featureLyr, featBuffer, "YSDM", "211011");
                //IFeatureCursor featCursor = featureClass.Insert(true);
                //featCursor.InsertFeature(featBuffer);
                //m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            //}
            DataEdit.WKSEditor.StopEditOperation();
        }
        private static void SetFieldValue(IFeatureLayer Layer, IFeatureBuffer Feature, string Field, object Value)
        {
            IFeatureClass pFeatureCls = Layer.FeatureClass;
            int pFieldIndex = pFeatureCls.FindField(Field);
            Feature.set_Value(pFieldIndex, Value);
        }

    }
}

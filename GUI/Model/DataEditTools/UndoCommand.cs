using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace GUI.Model.DataEditTools
{
    class UndoCommand : BaseCommand
    {
        private HookHelper m_HookHelper = new HookHelperClass();
        private ILayer currentLayer = null;

        public UndoCommand(ILayer lyr)
        {
            currentLayer = lyr;
        }

        public override void OnCreate(object hook)
        {
            m_HookHelper.Hook = hook;
            base.m_category = "DataEditTools/RedoCommand"; //localizable text 
            base.m_caption = "重做";  //localizable text 
            base.m_message = "重做";  //localizable text
            base.m_toolTip = "重做";  //localizable text
            base.m_name = "DataEditTools_RedoCommand";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
        }

        public override void OnClick()
        {
            if(currentLayer is IFeatureLayer)
            {
                IFeatureLayer featureLayer = currentLayer as IFeatureLayer;
                IFeatureClass featureClass = featureLayer.FeatureClass;
                IDataset dataset = featureClass.FeatureDataset;
                IWorkspaceEdit2 workspaceEdit = dataset.Workspace as IWorkspaceEdit2;
                if (workspaceEdit.IsBeingEdited())
                {
                    workspaceEdit.StopEditOperation();
                }
                bool hasUndos = false;
                workspaceEdit.HasUndos(ref hasUndos);
                if (hasUndos)
                    workspaceEdit.UndoEditOperation();
                //m_HookHelper.OperationStack.Undo();
                m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,null,null);
            }

        }


        public override string Caption
        {
            get
            {
                return "删除要素";
            }
        }
    }
}

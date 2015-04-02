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
    public class ModifyDKBMTool:SelectFeaturesTool
    {

        public ModifyDKBMTool(DataEditor edit):base(edit)
        {

        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            base.OnMouseDown(Button,Shift,X,Y);
            IEnumFeature features = m_hookHelper.FocusMap.FeatureSelection as IEnumFeature;
            features.Reset();
            IFeature feature = features.Next();
            if (feature == null)
                return;
            ViewModel.DataEditVM.ModifyDKBMVM.IsShow = true;
        }
        
    }
}

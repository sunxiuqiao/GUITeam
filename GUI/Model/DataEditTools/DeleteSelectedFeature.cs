using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model.DataEditTools
{
    class DeleteSelectedFeature : BaseCommand
    {
        private HookHelper m_HookHelper = new HookHelperClass();

        public override void OnCreate(object hook)
        {
            base.m_category = "DataEditTools/DeleteSelectedFeature"; //localizable text 
            base.m_caption = "删除要素";  //localizable text 
            base.m_message = "删除要素";  //localizable text
            base.m_toolTip = "删除要素";  //localizable text
            base.m_name = "DataEditTools_DeleteSelectedFeature";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
        }

        public override void OnClick()
        {
            IEnumFeature enumFeature = m_HookHelper.FocusMap.FeatureSelection as IEnumFeature;
            
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

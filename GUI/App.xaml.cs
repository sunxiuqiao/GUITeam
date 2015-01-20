using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if(!RuntimeManager.Bind(ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("Unable to bind ArcGis Runtime ,Application will be shut down");
                return;
            }

            AoInitialize aoi = new AoInitializeClass();
            esriLicenseProductCode productcode = esriLicenseProductCode.esriLicenseProductCodeEngine;
            if(aoi.IsProductCodeAvailable(productcode) == esriLicenseStatus.esriLicenseAvailable)
            {
                aoi.Initialize(productcode);
            }
        }
    }
}

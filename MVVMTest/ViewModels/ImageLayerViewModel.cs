using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMTest.ViewModels
{
    public class ImageLayerViewModel:LayerViewModel
    {
        #region Constructor
        public ImageLayerViewModel(string imageFilePath, ESRIMapControl mapControl)
        {
            FilePath = imageFilePath;
            if (IsValid)
            {
                mapControl.AddLayer(DataLayer);
            }
        }

        override protected string layerName() { return "影像"; }
        #endregion


        #region FilePath
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (filePath != value)
                {
                    filePath = value;
                    RasterLayerClass dataLayer = new RasterLayerClass();
                    dataLayer.CreateFromFilePath(filePath);
                    this.dataLayer = dataLayer;
                    RaisePropertyChanged("FilePath");
                }
            }
        }
        #endregion

        #region FileName
        public string FileName
        {
            get
            {
                if (FilePath == null)
                    throw new ArgumentNullException("filePath");

                return System.IO.Path.GetFileName(FilePath);
            }
        }
        #endregion

        #region IsValid
        override protected bool isValid()
        {
            if (System.IO.File.Exists(FilePath) == false)
                return false;


            return true;
        }
        #endregion
    }
}

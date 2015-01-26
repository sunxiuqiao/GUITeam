using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using MicroMvvm;

namespace MVVMTest.ViewModels
{
    public class ImageViewModel : ObservableObject
    {
        #region Constructor
        public ImageViewModel(string imageFilePath)
        {
            FilePath = imageFilePath;
        }
        #endregion

        #region Layer
        private IRasterLayer layer;


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
                    layer = RasterLayerClass();
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
        public bool IsValid
        {
            get
            {
                if (System.IO.File.Exists(FilePath)==false)
                    return false;

                
                return true;
            }
        }
        #endregion
    }
}

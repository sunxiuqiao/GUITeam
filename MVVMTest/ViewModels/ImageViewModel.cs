﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroMvvm;

namespace MVVMTest.ViewModels
{
    public class ImageViewModel : ObservableObject
    {
        #region Constructor
        public ImageViewModel(string imageFilePath)
        {
            filePath = imageFilePath;
        }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MicroMvvm;
using MVVMTest.Models;

namespace MVVMTest.ViewModels
{
    public class ProjectViewModel : ObservableObject
    { 
        public ProjectViewModel(string path)
        {
            FilePath = path;
            Title = FileName;
        }

        public ProjectViewModel()
        {
            IsDirty = true;
            Title = FileName;
        }

        #region Title
        private string title = null;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged("Title");
                }
            }
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
                    return "Untitled" + (IsDirty ? "*" : "");

                return System.IO.Path.GetFileName(FilePath) + (IsDirty ? "*" : "");
            }
        }
        #endregion

        #region IsDirty
        private bool isDirty = false;
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;
                    RaisePropertyChanged("IsDirty");
                    RaisePropertyChanged("FilePath");
                }
            }
        }
        #endregion

    }
}

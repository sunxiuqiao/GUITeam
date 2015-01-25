using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMTest.ViewModels
{
    public class PaneViewModel : ObservableObject
    {
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.View;

namespace GUI.ViewModel
{
    class BusiDBCreationVM : MVVMBase.ObservableObject
    {
        #region construct
        public BusiDBCreationVM()
        {
        }
        #endregion

        #region member
        private bool isContinueWithError = false;
        private bool isInitDictionary = false;
        #endregion

        #region Properties
        public bool IsContinueWithError
        {
            get { return isContinueWithError; }
            set
            {
                isContinueWithError = value;
                RaisePropertyChanged("IsContinueWithError");
            }
        }

        public bool IsInitDictionary
        {
            get { return isInitDictionary; }
            set
            {
                isInitDictionary = value;
                RaisePropertyChanged("IsInitDictionary");
            }
        }
        #endregion

    }
}

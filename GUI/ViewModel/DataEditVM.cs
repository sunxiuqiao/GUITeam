using GUI.MVVMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    public class DataEditVM : MVVMBase.ObservableObject
    {
        #region member
        bool isDKDraw = false;
        bool isJZDDraw = false;
        bool isAttributeEdit = false;
        bool isAnnotationEdit = false;
        #endregion

        #region Properties
        public bool IsDKDraw
        {
            get { return isDKDraw; }
            set
            {
                isDKDraw = value;
                RaisePropertyChanged("IsDKDraw");
            }
        }

        public bool IsJZDDraw
        {
            get { return isJZDDraw; }
            set
            {
                isJZDDraw = value;
                RaisePropertyChanged("IsJZDDraw");
            }
        }

        public bool IsAttributeEdit
        {
            get { return isAttributeEdit; }
            set
            {
                isAttributeEdit = value;
                RaisePropertyChanged("IsAttributeEdit");
            }
        }

        public bool IsAnnotationEdit
        {
            get { return isAnnotationEdit; }
            set
            {
                isAnnotationEdit = value;
                RaisePropertyChanged("IsAnnotationEdit");
            }
        }
        #endregion

        #region StartDrawDKCommand
        private void StartDrawDK_Executed()
        {
            IsDKDraw = true;
        }

        private bool StartDrawDK_CanExecute()
        {
            if (IsJZDDraw || IsAnnotationEdit || IsAttributeEdit)
                return false;
            return true;
        }
        public System.Windows.Input.ICommand StartDrawDKCommand { get { return new RelayCommand(StartDrawDK_Executed, StartDrawDK_CanExecute); } } 
        #endregion

        #region StartDrawJZDCommand
        private void StartDrawJZD_Executed()
        {
            IsJZDDraw = true;
        }

        private bool StartDrawJZD_CanExecute()
        {
            if (IsDKDraw || IsAnnotationEdit || IsAttributeEdit)
                return false; 
            return true;
        }
        public System.Windows.Input.ICommand StartDrawJZDCommand { get { return new RelayCommand(StartDrawJZD_Executed, StartDrawJZD_CanExecute); } }
        #endregion

        #region StartEditAttributeCommand
        private void StartEditAttribute_Executed()
        {
            IsAttributeEdit = true;
        }

        private bool StartEditAttribute_CanExecute()
        {
            if (IsJZDDraw || IsDKDraw || IsAnnotationEdit)
                return false;
            return true;
        }
        public System.Windows.Input.ICommand StartEditAttributeCommand { get { return new RelayCommand(StartEditAttribute_Executed, StartEditAttribute_CanExecute); } }
        #endregion

        #region StartEditAnnotationCommand
        private void StartEditAnnotation_Executed()
        {
            IsAnnotationEdit = true;
        }

        private bool StartEditAnnotation_CanExecute()
        {
            if (IsJZDDraw || IsAnnotationEdit || IsDKDraw)
                return false;
            return true;
        }
        public System.Windows.Input.ICommand StartEditAnnotationCommand { get { return new RelayCommand(StartEditAnnotation_Executed, StartEditAnnotation_CanExecute); } }
        #endregion

        #region StopDrawDKCommand
        private void StopDrawDK_Executed()
        {
            IsDKDraw = false;
        }

        private bool StopDrawDK_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopDrawDKCommand { get { return new RelayCommand(StopDrawDK_Executed, StopDrawDK_CanExecute); } } 
        
        #endregion

        #region StopDrawJZDCommand
        private void StopDrawJZD_Executed()
        {
            IsJZDDraw = false;
        }

        private bool StopDrawJZD_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopDrawJZDCommand { get { return new RelayCommand(StopDrawJZD_Executed, StopDrawJZD_CanExecute); } }

        #endregion

        #region StopEditAttributeCommand
        private void StopEditAttribute_Executed()
        {
            IsAttributeEdit = false;
        }

        private bool StopEditAttribute_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopEditAttributeCommand { get { return new RelayCommand(StopEditAttribute_Executed, StopEditAttribute_CanExecute); } }

        #endregion

        #region StopEditAnnotationCommand
        private void StopEditAnnotation_Executed()
        {
            IsAnnotationEdit = false;
        }

        private bool StopEditAnnotation_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand StopEditAnnotationCommand { get { return new RelayCommand(StopEditAnnotation_Executed, StopEditAnnotation_CanExecute); } }

        #endregion

    }
}

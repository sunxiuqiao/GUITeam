using MicroMvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MVVMTest.ViewModels
{
    class MainViewModel:MicroMvvm.ObservableObject
    {
        #region Constructor
        static MainViewModel _this = new MainViewModel();

        public MainViewModel() 
        {
            //图层窗格            
        }

        public static MainViewModel This
        {
            get { return _this; }
        }
        #endregion

        #region Projects
        private ObservableCollection<ProjectViewModel> projects = new ObservableCollection<ProjectViewModel>();
        public ObservableCollection<ProjectViewModel> Projects
        {
            get {  return projects; }
            set { projects = value; }
        }
        #endregion

        #region ActiveProject
        private ProjectViewModel _activeProject = null;
        public ProjectViewModel ActiveProject
        {
            get { return _activeProject; }
            set
            {
                if (_activeProject != value)
                {
                    _activeProject = value;
                    RaisePropertyChanged("ActiveProject");
                    if (ActiveProjectChanged != null)
                        ActiveProjectChanged(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler ActiveProjectChanged;
        #endregion

        #region Layers
        private ObservableCollection<PaneViewModel> layers = new ObservableCollection<PaneViewModel>();
        public ObservableCollection<PaneViewModel> Layers
        {
            get { return layers; }
            set { layers = value; }
        }
        #endregion

        #region NewProjectCommand
        void NewProjectExcute()
        {
            ProjectViewModel prj = new ProjectViewModel();
            //prj.Images.Add(new ImageViewModel(@"C:\Users\cgz\Pictures\ChangeObject.png"));
            projects.Add(prj);
            ActiveProject = prj;
        }

        bool CanNewProjectExcute()
        {
            return true;
        }

        public ICommand NewProject { get { return new RelayCommand(NewProjectExcute, CanNewProjectExcute); } }
        #endregion

        #region OpenProjectCommand
        void OpenProjectExcute()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "项目文件(*.cdtprox)|*.cdtprox|所有文件(*.*)|*.*";
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                string filePath = dlg.FileName;
                ProjectViewModel prj = new ProjectViewModel(filePath);
                projects.Add(prj);
                ActiveProject = prj;
            }
        }

        bool CanOpenProjectExcute()
        {
            return true;
        }

        public ICommand OpenProject { get { return new RelayCommand(OpenProjectExcute, CanOpenProjectExcute); } }
        #endregion

        #region CloseProjectCommand
        void CloseActiveProjectExcute()
        {
            Close(ActiveProject);
        }

        bool CanCloseActiveProjectExcute()
        {
            return ActiveProject!=null;
        }

        public ICommand CloseActiveProject { get { return new RelayCommand(CloseActiveProjectExcute, CanCloseActiveProjectExcute); } }

        internal bool Close(ProjectViewModel projectToClose)//true 关闭; false 未关闭
        {
            if (projectToClose == null)
                return false;

            if (projectToClose.IsDirty)
            {
                MessageBoxResult ret = MessageBox.Show("项目已修改，是否保存？", "是否保存项目:" + projectToClose.Title + " ?", MessageBoxButton.YesNoCancel);
                if (ret == MessageBoxResult.OK)
                {
                    //Save();
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }

            Projects.Remove(projectToClose);
            if (Projects.Count == 0)
                ActiveProject = null;
            return true;
        }
        #endregion
    }
}

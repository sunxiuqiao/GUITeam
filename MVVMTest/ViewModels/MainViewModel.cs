using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using MicroMvvm;
using System.Windows.Input;

namespace MVVMTest.ViewModels
{
    class MainViewModel:MicroMvvm.ObservableObject
    {        
        static MainViewModel _this = new MainViewModel();

        public MainViewModel() 
        {
            projects.Add(new ProjectViewModel("hehe", "c:/hehe.path"));
            projects.Add(new ProjectViewModel("hehe2", "d:/hehe2.path"));
        }

        public static MainViewModel This
        {
            get { return _this; }
        }

        ObservableCollection<ProjectViewModel> projects = new ObservableCollection<ProjectViewModel>();
        public ObservableCollection<ProjectViewModel> Projects
        {
            get {  return projects; }
            set { projects = value; }
        }

        private ProjectViewModel _activeDocument = null;
        public ProjectViewModel ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                if (_activeDocument != value)
                {
                    _activeDocument = value;
                    RaisePropertyChanged("ActiveDocument");
                    if (ActiveProjectChanged != null)
                        ActiveProjectChanged(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler ActiveProjectChanged;


        void OpenProjectExcute()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "项目文件(*.cdtprox)|*.cdtprox|所有文件(*.*)|*.*";
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                string filePath = dlg.FileName;
                string projectName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                projects.Add(new ProjectViewModel(projectName, filePath));
            }
        }

        bool CanOpenProjectExcute()
        {
            return true;
        }

        public ICommand OpenProject { get { return new RelayCommand(OpenProjectExcute, CanOpenProjectExcute); } }
    }
}

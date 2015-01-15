using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MVVMTest.ViewModels
{
    class MainViewModel
    {
        ObservableCollection<ProjectViewModel> projects = new ObservableCollection<ProjectViewModel>();

        public MainViewModel() 
        {
            projects.Add(new ProjectViewModel("hehe", "c:/hehe.path"));
            projects.Add(new ProjectViewModel("hehe2", "d:/hehe2.path"));
        }

        public ObservableCollection<ProjectViewModel> Projects
        {
            get {  return projects; }
            set { projects = value; }
        }
    }
}

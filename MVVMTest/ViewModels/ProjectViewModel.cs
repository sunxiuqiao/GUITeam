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
        Project project;

        public ProjectViewModel(string name, string path)
        {
            project = new Project { ProjectName = name, FilePath = path };
        }

        public Project Project 
        {
            get { return project; }
            set { project = value; }
        }

        public string ProjectName
        {
            get { return project.ProjectName; }
            set
            {
                if (project.ProjectName != value)
                {
                    project.ProjectName = value;
                    RaisePropertyChanged("ProjectName");
                }
            }
        }

        public string FilePath
        {
            get { return project.FilePath; }
            set 
            {
                if (project.FilePath != value)
                {
                    project.FilePath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        }

        void UpdateProjectNameExecute(string newName)
        {
            ProjectName = newName;
        }

        bool CanUpdateProjectNameExecute(string newName)
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand<string>(UpdateProjectNameExecute,CanUpdateProjectNameExecute); } }
    }
}

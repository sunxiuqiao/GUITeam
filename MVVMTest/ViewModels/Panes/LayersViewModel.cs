﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MVVMTest.ViewModels
{
    public class LayersViewModel : PaneViewModel
    {
        public LayersViewModel()
        {
            Title = "Layers";
            MainViewModel.This.ActiveProjectChanged += new EventHandler(OnActiveProjectChanged);
        }

        void OnActiveProjectChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("Images");
        }

        public ObservableCollection<ImageViewModel> Images
        {
            get
            {
                ProjectViewModel prj = MainViewModel.This.ActiveProject;
                if (prj == null) return null;
                return prj.Images;
            }
        }
    }
}

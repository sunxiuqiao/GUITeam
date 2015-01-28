﻿using MVVMTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock;

namespace MVVMTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainViewModel.This;
            LayersPaneViewModel layer = new LayersPaneViewModel();
            MainViewModel.This.Layers.Add(layer);
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (MainViewModel.This.ActiveProject != null)
            {
                if (MainViewModel.This.Close(MainViewModel.This.Projects[0]) == false)
                {                    
                    e.Cancel = true;
                    break;
                }
            }
        }
    }
}

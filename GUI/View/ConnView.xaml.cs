﻿using System;
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

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for ConnView.xaml
    /// </summary>
    public partial class ConnView : UserControl
    {
        public ConnView()
        {
            InitializeComponent();
            
        }

        private void ConnNameComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConnSaveButton.IsEnabled = false;
        }

    }
}

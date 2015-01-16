using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MVVMTest
{
    public class ActiveContentToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return System.Windows.Visibility.Hidden;

            if (value.GetType().ToString() == "MVVMTest.ViewModels.ProjectViewModel")
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new ArgumentException("Can't convert from Visibility to this");
        }
    }
}

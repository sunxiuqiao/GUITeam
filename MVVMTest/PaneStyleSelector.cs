using MVVMTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MVVMTest
{
    class PanesStyleSelector : StyleSelector
    {
        public Style LayersStyle
        {
            get;
            set;
        }

        public Style ProjectStyle
        {
            get;
            set;
        }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is LayersPaneViewModel)
                return LayersStyle;

            if (item is ProjectViewModel)
                return ProjectStyle;

            return base.SelectStyle(item, container);
        }
    }
}

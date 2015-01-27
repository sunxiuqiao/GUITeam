using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MVVMTest.ViewModels;
using Xceed.Wpf.AvalonDock.Layout;

namespace MVVMTest
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public PanesTemplateSelector()
        {

        }

        public DataTemplate ProjectViewTemplate
        {
            get;
            set;
        }

        public DataTemplate LayersViewTemplate
        {
            get;
            set;
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;

            if (item is LayersPaneViewModel)
                return LayersViewTemplate;

            if (item is ProjectViewModel)
                return ProjectViewTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}

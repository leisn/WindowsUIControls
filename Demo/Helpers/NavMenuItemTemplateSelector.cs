using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Demo.Models;

namespace Demo.Helpers
{
    public class NavMenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MainItemTemplate { get; set; }
        public DataTemplate SecondItemTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item is ControlCategorie ? MainItemTemplate :
                item is ControlItem ? SecondItemTemplate : SeparatorTemplate;
        }

    }
}

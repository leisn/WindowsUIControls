using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Demo.Microsofts
{
    public class SuggestItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringItemTemplate { get; set; }
        public DataTemplate SymbolItemTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return  item is Symbol ? SymbolItemTemplate : StringItemTemplate;
        }

    }
}

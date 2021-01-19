using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leisn.UI.Xaml.Controls
{

    public class SelectorStateChangedEventArgs
    {
        public bool Selected { get; }

        public SelectorStateChangedEventArgs(bool selected)
        {
            this.Selected = selected;
        }
    }
}

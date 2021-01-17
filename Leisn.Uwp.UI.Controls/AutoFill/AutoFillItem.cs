using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml;

namespace Leisn.Uwp.UI.Controls
{
    internal class AutoFillItem
    {
        public bool ShouldDisplay { get; set; }
        public int Index { get; }
        public Rect Bounds => new Rect(Left, Top, Width, Height);

        public double Left { get; internal set; }
        public double Top { get; internal set; }
        public double Width { get; internal set; }
        public double Height { get; internal set; }

        public UIElement Element { get; internal set; }

        public AutoFillItem(int index)
        {
            Index = index;
        }
    }
}

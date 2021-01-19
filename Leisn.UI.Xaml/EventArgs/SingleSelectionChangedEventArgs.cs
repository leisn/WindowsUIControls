using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leisn.UI.Xaml.Controls
{
    public class SingleSelectionChangedEventArgs : EventArgs
    {
        public int OldIndex { get; }
        public int NewIndex { get; }
        public object OldValue { get; }
        public object NewValue { get; }

        public SingleSelectionChangedEventArgs(
            int oldIndex, int newIndex,
            object oldValue, object newValue)
        {
            this.OldIndex = oldIndex;
            this.NewIndex = newIndex;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leisn.UI.Xaml.Extensions;

using Microsoft.UI.Xaml.Controls;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Leisn.UI.Xaml.Controls
{
    internal class AutoFillLayoutState
    {
        VirtualizingLayoutContext _Context;

        List<AutoFillItem> _Items { get; } = new List<AutoFillItem>();
        RectClips _AvailableRects { get; } = new RectClips();

        public bool Initialized { get; private set; }

        public AutoFillLayoutState(VirtualizingLayoutContext context)
        {
            this._Context = context;
            this.Initialized = false;
        }

        #region save old state
        public double ClientWidth { get; internal set; }
        public double ClientHeight { get; internal set; }

        Size AvailableSize { get; set; }
        Thickness Padding { get; set; }
        Orientation Orientation { get; set; }
        double HorizontalSpacing { get; set; }
        double VerticalSpacing { get; set; }

        public bool SavePropertiesIfChange(Orientation ori, Thickness padding,
            double hSpacing, double vSpacing, Size availableSize)
        {
            var changed = this.Orientation != ori || this.Padding != padding
                || this.HorizontalSpacing != hSpacing || this.VerticalSpacing != vSpacing
                || this.AvailableSize != availableSize;
            if (changed)
            {
                this.Orientation = ori;
                this.Padding = padding;
                this.HorizontalSpacing = hSpacing;
                this.VerticalSpacing = vSpacing;
                this.AvailableSize = availableSize;
            }
            return changed;
        }

        #endregion


        public int ItemCount => _Items.Count;

        public AutoFillItem this[int index]
        {
            get => _Items[index];
            set => _Items[index] = value;
        }

        public AutoFillItem GetOrCreateItemAt(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException("< 0");
            if (index < _Items.Count)
                return _Items[index];
            else
            {
                var item = new AutoFillItem(index);
                this._Items.Add(item);
                return item;
            }
        }

        public void Reset()
        {
            //System.Diagnostics.Debug.WriteLine("Rest states");
            _Items.Clear();
            _AvailableRects.Clear();
            Initialized = false;
        }

        public void Initialize(Rect canvasBounds)
        {
            if (!Initialized)
                _AvailableRects.MergeItem(canvasBounds);
            Initialized = true;
        }

        public Size GetTotalSize()
        {
            var rects = from item in _Items select item.Bounds;
            return rects.GetOutBounds().Size();
        }

        public Size GetVirtualizedSize()
        {
            var rects = from item in _Items
                        where item.ShouldDisplay
                        select item.Bounds;
            return rects.GetOutBounds().Size();
        }

        internal void RemoveItemsFrom(int index)
        {
            if (index >= ItemCount)
                return;
            List<Rect> backRects = new List<Rect>();
            for (int i = index; i < ItemCount; i++)
            {
                var item = _Items[i];
                var bounds = item.Bounds;
                if (!bounds.IsEmpty())
                {
                    bounds.Width += HorizontalSpacing;
                    bounds.Height += VerticalSpacing;
                    backRects.Add(bounds);
                }
            }
            _Items.RemoveRange(index, ItemCount - index);
            if (backRects.Count > 0)
            {
                _AvailableRects.MergeItems(backRects);
                sortAvailableRects();
            }
        }

        internal void RecycleElementAt(int index)
        {
            _Context.RecycleElement(_Context.GetOrCreateElementAt(index));
        }

        internal void FitItem(AutoFillItem item)
        {
            var calSize = new Size(item.Width + HorizontalSpacing, item.Height + VerticalSpacing);
            var (index, rect) = findAvailableRect(calSize);
            Debug.Assert(index != -1);
            var (target, clipRight, clipBottom) = rect.Clip(calSize);
            item.Left = target.Left;
            item.Top = target.Top;

            _AvailableRects.RemoveAt(index);
            _AvailableRects.CutThenMergeOthers(target,
                clipRight.GetValueOrDefault(),
                clipBottom.GetValueOrDefault());
            sortAvailableRects();
        }


        #region handle available rects

        (int Index, Rect Area) findAvailableRect(Size size)
        {
            return Rects.FindEnoughArea(_AvailableRects, size);
        }

        void sortAvailableRects()
        {
            if (Orientation == Orientation.Horizontal)
                _AvailableRects.Sort(compareRectTop);
            else
                _AvailableRects.Sort(compareRectLeft);
        }

        int compareRectTop(Rect rect1, Rect rect2)
        {
            var offset = rect1.Top - rect2.Top;
            if (offset == 0)
                return (int)(rect1.Left - rect2.Left);
            if (offset < 0)
                return -1;
            else
                return 1;
        }
        int compareRectLeft(Rect rect1, Rect rect2)
        {
            var offset = rect1.Left - rect2.Left;
            if (offset == 0)
                return (int)(rect1.Top - rect2.Top);
            if (offset < 0)
                return -1;
            else
                return 1;
        }

        #endregion
    }
}

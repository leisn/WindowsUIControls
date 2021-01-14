using System;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Leisn.Uwp.UI.Extensions;
using System.Diagnostics;

namespace Leisn.Uwp.UI.Controls
{
    public class AutoFillPanel : Panel
    {
        #region Properties
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                nameof(Padding),
                typeof(Thickness),
                typeof(AutoFillPanel),
                new PropertyMetadata(default(Thickness), LayoutPropertyChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(AutoFillPanel),
                new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));

        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        public static readonly DependencyProperty HorizontalSpacingProperty =
            DependencyProperty.Register(
                nameof(HorizontalSpacing),
                typeof(double),
                typeof(AutoFillPanel),
                new PropertyMetadata(4d, LayoutPropertyChanged));

        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        public static readonly DependencyProperty VerticalSpacingProperty =
            DependencyProperty.Register(
                nameof(VerticalSpacing),
                typeof(double),
                typeof(AutoFillPanel),
                new PropertyMetadata(4d, LayoutPropertyChanged));

        private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoFillPanel ap)
            {
                ap.InvalidateMeasure();
                ap.InvalidateArrange();
            }
        }
        #endregion

        readonly List<Rect> arrageRects = new List<Rect>();
        readonly RectClips availableRects = new RectClips();

        internal int CompareRectTop(Rect rect1, Rect rect2)
        {
            var offset = rect1.Top - rect2.Top;
            if (offset == 0)
                return (int)(rect1.Left - rect2.Left);
            if (offset < 0)
                return -1;
            else
                return 1;
        }
        internal int CompareRectLeft(Rect rect1, Rect rect2)
        {
            var offset = rect1.Left - rect2.Left;
            if (offset == 0)
                return (int)(rect1.Top - rect2.Top);
            if (offset < 0)
                return -1;
            else
                return 1;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            arrageRects.Clear();

            var requiredSize = new Size(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
            var clientSize = new Size(
                  availableSize.Width - Padding.Left - Padding.Right,
                   availableSize.Height - Padding.Top - Padding.Bottom);

            if (Children.Count < 1) return requiredSize;

            if (Orientation == Orientation.Horizontal)
                availableRects.UnionItem(new Rect(Padding.Left, Padding.Top,
                 clientSize.Width + HorizontalSpacing, double.PositiveInfinity));
            else
                availableRects.UnionItem(new Rect(Padding.Left, Padding.Top,
                   double.PositiveInfinity, clientSize.Height + VerticalSpacing));

            foreach (var child in Children)
            {
                child.Measure(clientSize);
                var childSize = child.DesiredSize;

                childSize.Width += HorizontalSpacing;
                childSize.Height += VerticalSpacing;

                var (index, rect) = Rects.FindEnoughArea(availableRects, childSize);
                Debug.Assert(index != -1);
                var (target, clipRight, clipBottom) = rect.Clip(childSize);

                availableRects.RemoveAt(index);

                clipAfterArrange(target);

                availableRects.UnionItem(clipRight.GetValueOrDefault());
                availableRects.UnionItem(clipBottom.GetValueOrDefault());

                target.Width -= HorizontalSpacing;
                target.Height -= VerticalSpacing;
                arrageRects.Add(target);

                if (Orientation == Orientation.Horizontal)
                    availableRects.Sort(CompareRectTop);
                else
                    availableRects.Sort(CompareRectLeft);
            }

            availableRects.Clear();

            var bounds = arrageRects.GetOutBounds();
            requiredSize.Width += bounds.Width;
            requiredSize.Height += bounds.Height;
            return requiredSize;
        }

        private void clipAfterArrange(Rect arrange)
        {
            for (int i = availableRects.Count - 1; i >= 0; i--)
            {
                var x = availableRects[i].Clip(arrange);
                if (x.Clipped)
                {
                    availableRects.RemoveAt(i);
                    var clips = x.ClipResult.Clips;
                    foreach (var item in clips)
                    {
                        availableRects.UnionItem(item);
                    }
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = Children.Count;
            Debug.Assert(arrageRects.Count == count);

            for (int i = 0; i < count; i++)
            {
                var child = Children[i];
                var rect = arrageRects[i];
                child.Arrange(rect);
            }
            return finalSize;
        }

    }
}

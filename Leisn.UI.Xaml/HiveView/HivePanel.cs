
using System;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Leisn.UI.Xaml.Extensions;
using System.Diagnostics;
using Windows.UI.Xaml.Automation.Peers;

namespace Leisn.UI.Xaml.Controls
{
    public class HivePanel : HivePanelBase
    {
        readonly protected List<Rect> arrageRects = new List<Rect>();

        #region public properties
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(HivePanel),
                new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            arrageRects.Clear();

            var requiredSize = new Size(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
            var clientSize = new Size(
                  availableSize.Width - Padding.Left - Padding.Right,
                   availableSize.Height - Padding.Top - Padding.Bottom);

            if (Children.Count < 1) return requiredSize;


            var fixedSize = GetChildrenMaxSize(clientSize);

            double left = Padding.Left;
            double top = Padding.Top;
            Rect rect;
            int col = 0;
            foreach (var child in Children)
            {
                child.Measure(fixedSize);
                rect = new Rect(left, top, fixedSize.Width, fixedSize.Height);
                arrageRects.Add(rect);
                if (rect.Right + HorizontalSpacing + fixedSize.Width * 3 / 4 > clientSize.Width)
                {
                    if (col % 2 == 0)
                        top += fixedSize.Height + Spacing;
                    else
                        top += fixedSize.Height / 2 + VerticalSpacing;
                    left = Padding.Left;
                    col = 0;
                }
                else
                {
                    left += fixedSize.Width * 3 / 4 + HorizontalSpacing;
                    if (col % 2 == 0)
                        top += fixedSize.Height / 2 + VerticalSpacing;
                    else
                        top -= fixedSize.Height / 2 + VerticalSpacing;
                    col++;
                }
            }

            var bounds = arrageRects.GetOutBounds();
            requiredSize.Width += bounds.Width;
            requiredSize.Height += bounds.Height;
            return requiredSize;
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

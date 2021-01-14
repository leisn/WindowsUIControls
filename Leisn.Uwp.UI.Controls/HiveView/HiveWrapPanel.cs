using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Leisn.Uwp.UI.Controls
{
    public class HiveWrapPanel : HivePanel
    {
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
                typeof(HiveWrapPanel),
                new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));
        #endregion

        public double ItemWidth => 2 * FixedEdge;
        public double ItemHeight => Math.Sqrt(3) * FixedEdge;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            int count = Children.Count;
            var requiredSize = new Size(
              Padding.Left + Padding.Right,
              Padding.Top + Padding.Bottom);

            if (count == 0 || FixedEdge == 0)
                return requiredSize;

            var clientSize = new Size(
              availableSize.Width - Padding.Left - Padding.Right,
              availableSize.Height - Padding.Top - Padding.Bottom);

            var halfHeight = ItemHeight / 2;
            var halfEdge = FixedEdge / 2;

            int row = 0, col = 0;

            var childSize = new Size(ItemWidth, ItemHeight);
            foreach (var child in Children)
            {
                child.Measure(childSize);
            }

            if (Orientation == Orientation.Horizontal)
            {
                //只显示一列
                if (clientSize.Width < ItemWidth + HorizontalSpacing + FixedEdge + halfEdge)
                {
                    requiredSize.Width += ItemWidth;
                    requiredSize.Height += ItemHeight * count + Spacing * (count - 1);
                    return requiredSize;
                }

                double left = Padding.Left + halfEdge + FixedEdge + HorizontalSpacing;
                for (int i = 0; i < count; i++)
                {

                    if (i != count - 1 &&
                        left + ItemWidth + HorizontalSpacing + FixedEdge + HorizontalSpacing
                        + ItemWidth + Padding.Right
                        > clientSize.Width)
                    {
                        requiredSize.Width = Math.Max(requiredSize.Width, left + ItemWidth + Padding.Right);
                        row++;
                        requiredSize.Height += halfHeight + VerticalSpacing;
                        col = 0;
                        if ((row & 1) == 1)
                            left = Padding.Left;
                        else
                            left = Padding.Left + halfEdge + FixedEdge + HorizontalSpacing;
                        continue;
                    }
                    left += ItemWidth + HorizontalSpacing + FixedEdge + HorizontalSpacing;
                    col++;
                }
                requiredSize.Height += ItemHeight;
            }
            else
            {
                //只显示一行
                if (clientSize.Height < ItemHeight + VerticalSpacing + halfHeight)
                {
                    requiredSize.Height += ItemHeight;
                    requiredSize.Width += ItemWidth * count + Spacing * (count - 1);
                    return requiredSize;
                }

                double top = Padding.Top + halfHeight + VerticalSpacing;
                for (int i = 0; i < count; i++)
                {
                    if (i != count - 1 &&
                        top + ItemHeight + Spacing + ItemHeight + Padding.Bottom
                        > clientSize.Height)
                    {
                        requiredSize.Height = Math.Max(requiredSize.Height,
                            top + ItemHeight + Padding.Bottom);
                        col++;
                        requiredSize.Width += halfEdge + FixedEdge + HorizontalSpacing;
                        row = 0;
                        if ((col & 1) == 1)
                            top = Padding.Top;
                        else
                            top = Padding.Top + halfHeight + VerticalSpacing;
                        continue;
                    }
                    top += ItemHeight + Spacing;
                    row++;
                }
                requiredSize.Width += ItemWidth;
            }
            //Debug.WriteLine(requiredSize);
            return requiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Debug.WriteLine("final " + finalSize);
            var clientSize = new Size(
                finalSize.Width - Padding.Left - Padding.Right,
                finalSize.Height - Padding.Top - Padding.Bottom);

            int count = Children.Count;

            var halfHeight = ItemHeight / 2;
            var halfEdge = FixedEdge / 2;

            double left = Padding.Left, top = Padding.Top;

            if (Orientation == Orientation.Horizontal)
            {
                //只显示一列
                if (clientSize.Width < ItemWidth + HorizontalSpacing + FixedEdge + halfEdge)
                {
                    for (int k = 0; k < count; k++)
                    {
                        var child = Children[k];
                        child.Arrange(new Rect
                        {
                            X = left,
                            Y = top,
                            Width = ItemWidth,
                            Height = ItemHeight
                        });
                        top += ItemHeight + Spacing;
                    }
                    return finalSize;
                }

                int row = 0;

                left = Padding.Left + halfEdge + FixedEdge + HorizontalSpacing;
                for (int i = 0; i < count; i++)
                {
                    var child = Children[i];
                    child.Arrange(new Rect
                    {
                        X = left,
                        Y = top,
                        Width = ItemWidth,
                        Height = ItemHeight
                    });

                    if (left + ItemWidth + HorizontalSpacing + FixedEdge + HorizontalSpacing
                        + ItemWidth + Padding.Right
                        > clientSize.Width)
                    {
                        row++;
                        top += halfHeight + VerticalSpacing;
                        if ((row & 1) == 1)
                            left = Padding.Left;
                        else
                            left = Padding.Left + halfEdge + FixedEdge + HorizontalSpacing;
                        continue;
                    }
                    left += ItemWidth + HorizontalSpacing + FixedEdge + HorizontalSpacing;
                }
            }
            else
            {
                //只显示一行
                if (clientSize.Height < ItemHeight + VerticalSpacing + halfHeight)
                {
                    for (int k = 0; k < count; k++)
                    {
                        var child = Children[k];
                        child.Arrange(new Rect
                        {
                            X = left,
                            Y = top,
                            Width = ItemWidth,
                            Height = ItemHeight
                        });
                        left += ItemWidth + Spacing;
                    }
                    return finalSize;
                }

                int col = 0;
                top = Padding.Top + halfHeight + VerticalSpacing;
                for (int i = 0; i < count; i++)
                {
                    var child = Children[i];
                    child.Arrange(new Rect
                    {
                        X = left,
                        Y = top,
                        Width = ItemWidth,
                        Height = ItemHeight
                    });

                    if (top + ItemHeight + Spacing + ItemHeight + Padding.Bottom
                        > clientSize.Height)
                    {
                        col++;
                        left += halfEdge + FixedEdge + HorizontalSpacing;
                        if ((col & 1) == 1)
                            top = Padding.Top;
                        else
                            top = Padding.Top + halfHeight + VerticalSpacing;
                        continue;
                    }
                    top += ItemHeight + Spacing;
                }
            }
            return finalSize;
        }

    }
}

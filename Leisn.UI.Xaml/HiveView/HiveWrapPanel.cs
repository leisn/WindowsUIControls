using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Leisn.UI.Xaml.Extensions;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Leisn.UI.Xaml.Controls
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

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            arrageRects.Clear();

            int count = Children.Count;
            var requiredSize = new Size(
              Padding.Left + Padding.Right,
              Padding.Top + Padding.Bottom);

            if (count == 0) return requiredSize;

            var clientSize = new Size(
              availableSize.Width - Padding.Left - Padding.Right,
              availableSize.Height - Padding.Top - Padding.Bottom);

            var fixedSize = GetChildrenMaxSize(clientSize);

            foreach (var child in Children)
            {
                child.Measure(fixedSize);
            }

            var itemWidth = fixedSize.Width;
            var itemHeight = fixedSize.Height;
            var itemEdge = itemWidth / 2;
            var halfHeight = itemHeight / 2;
            var unitWidth = fixedSize.Width * 3 / 4;
            double left = Padding.Left, top = Padding.Top;

            if (Orientation == Orientation.Horizontal)
            {
                //只显示一列
                if (clientSize.Width < itemWidth + HorizontalSpacing + unitWidth)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var child = Children[i];
                        child.Measure(fixedSize);
                        arrageRects.Add(new Rect
                        {
                            X = left,
                            Y = top,
                            Width = itemWidth,
                            Height = itemHeight
                        });
                        top += itemHeight + Spacing;
                    }
                }
                else
                {
                    int row = 0;
                    left = Padding.Left + unitWidth + HorizontalSpacing;
                    for (int i = 0; i < count; i++)
                    {
                        var child = Children[i];
                        child.Measure(fixedSize);
                        arrageRects.Add(new Rect
                        {
                            X = left,
                            Y = top,
                            Width = itemWidth,
                            Height = itemHeight
                        });

                        if (left + itemWidth + HorizontalSpacing + itemEdge + HorizontalSpacing
                            + itemWidth + Padding.Right
                            > clientSize.Width)
                        {
                            row++;
                            top += halfHeight + VerticalSpacing;
                            if ((row & 1) == 1)
                                left = Padding.Left;
                            else
                                left = Padding.Left + unitWidth + HorizontalSpacing;
                            continue;
                        }
                        left += itemWidth + HorizontalSpacing + itemEdge + HorizontalSpacing;
                    }
                }
            }
            else
            {
                //只显示一行
                if (clientSize.Height < itemHeight + VerticalSpacing + halfHeight)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var child = Children[i];
                        child.Measure(fixedSize);
                        arrageRects.Add(new Rect
                        {
                            X = left,
                            Y = top,
                            Width = itemWidth,
                            Height = itemHeight
                        });
                        left += itemWidth + Spacing;
                    }
                }
                else
                {
                    int col = 0;
                    top = Padding.Top + halfHeight + VerticalSpacing;
                    for (int i = 0; i < count; i++)
                    {
                        var child = Children[i];
                        child.Measure(fixedSize);
                        arrageRects.Add(new Rect
                        {
                            X = left,
                            Y = top,
                            Width = itemWidth,
                            Height = itemHeight
                        });

                        if (top + itemHeight + Spacing + itemHeight + Padding.Bottom
                            > clientSize.Height)
                        {
                            col++;
                            left += unitWidth + HorizontalSpacing;
                            if ((col & 1) == 1)
                                top = Padding.Top;
                            else
                                top = Padding.Top + halfHeight + VerticalSpacing;
                            continue;
                        }
                        top += itemHeight + Spacing;
                    }
                }
            }

            var bounds = arrageRects.GetOutBounds();
            requiredSize.Width += bounds.Width;
            requiredSize.Height += bounds.Height;
            return requiredSize;
        }
    }
}

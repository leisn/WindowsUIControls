
using System;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Leisn.Uwp.UI.Extensions;
using System.Diagnostics;

namespace Leisn.Uwp.UI.Controls
{
    public class HivePanel : Panel
    {
        internal double GetHeightFromEdge(double edge) => Math.Sqrt(3) * edge;
        internal double GetHeightFromWidth(double width) => GetHeightFromEdge(width / 2);
        internal double GetEdgeFromHeight(double height) => height / Math.Sqrt(3);
        internal double GetWidthFromHeight(double height) => GetEdgeFromHeight(height) * 2;
        internal double GetWidthFromEdge(double edge) => edge * 2;

        internal double HorizontalSpacing => Spacing / Math.Sin(Math.PI / 3);
        internal double VerticalSpacing => Spacing / 2;

        #region DependencyProperties
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                nameof(Spacing),
                typeof(double),
                typeof(HivePanel),
                new PropertyMetadata(4d, LayoutPropertyChanged));

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                nameof(Padding),
                typeof(Thickness),
                typeof(HivePanel),
                new PropertyMetadata(default(Thickness), LayoutPropertyChanged));

        public double FixedEdge
        {
            get { return (double)GetValue(FixedEdgeProperty); }
            set { SetValue(FixedEdgeProperty, value); }
        }

        public static readonly DependencyProperty FixedEdgeProperty =
            DependencyProperty.Register(
                nameof(FixedEdge),
                typeof(double),
                typeof(HivePanel),
                new PropertyMetadata(30d, LayoutPropertyChanged));

        #endregion

        protected static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HivePanel hp)
            {
                hp.InvalidateMeasure();
                hp.InvalidateArrange();
            }
        }

        readonly List<Rect> arrageRects = new List<Rect>();

        protected override Size MeasureOverride(Size availableSize)
        {
            arrageRects.Clear();

            var requiredSize = new Size(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
            var clientSize = new Size(
                  availableSize.Width - Padding.Left - Padding.Right,
                   availableSize.Height - Padding.Top - Padding.Bottom);

            if (Children.Count < 1 || FixedEdge == 0) return requiredSize;

            {
                var fixedSize = new Size(GetWidthFromEdge(FixedEdge), GetHeightFromEdge(FixedEdge));
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

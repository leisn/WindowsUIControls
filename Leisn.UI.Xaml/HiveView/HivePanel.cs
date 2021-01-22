
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
    public class HivePanel : Panel
    {
        internal static double GetHeightFromEdge(double edge) => Math.Sqrt(3) * edge;
        internal static double GetHeightFromWidth(double width) => GetHeightFromEdge(width / 2);
        internal static double GetEdgeFromHeight(double height) => height / Math.Sqrt(3);
        internal static double GetWidthFromHeight(double height) => GetEdgeFromHeight(height) * 2;
        internal static double GetWidthFromEdge(double edge) => edge * 2;

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
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("FixedEdge < 0");
                SetValue(FixedEdgeProperty, value);
            }
        }

        public static readonly DependencyProperty FixedEdgeProperty =
            DependencyProperty.Register(
                nameof(FixedEdge),
                typeof(double),
                typeof(HivePanel),
                new PropertyMetadata(0d, LayoutPropertyChanged));
        #endregion

        protected static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HivePanel hp)
            {
                hp.InvalidateMeasure();
                //hp.InvalidateArrange();
            }
        }

        readonly protected List<Rect> arrageRects = new List<Rect>();


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

        protected virtual Size GetChildrenMaxSize(Size clientSize)
        {
            var fixedSize = new Size();
            if (FixedEdge == 0 || double.IsNaN(FixedEdge))
            {
                var maxSize = new Size();
                foreach (var child in Children)
                {
                    child.Measure(clientSize);
                    maxSize.Width = Math.Max(child.DesiredSize.Width, maxSize.Width);
                    maxSize.Height = Math.Max(child.DesiredSize.Height, maxSize.Height);
                }

                var tempWidth = GetWidthFromHeight(maxSize.Height);
                if (tempWidth > maxSize.Width)
                {
                    fixedSize.Width = tempWidth;
                    fixedSize.Height = maxSize.Height;
                }
                else
                {
                    fixedSize.Width = maxSize.Width;
                    fixedSize.Height = GetHeightFromWidth(maxSize.Width);
                }
            }
            else
            {
                fixedSize = new Size(GetWidthFromEdge(FixedEdge), GetHeightFromEdge(FixedEdge));
            }
            return fixedSize;
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

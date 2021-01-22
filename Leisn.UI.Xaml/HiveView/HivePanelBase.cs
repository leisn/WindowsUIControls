
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
    public abstract class HivePanelBase : Panel
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
                typeof(HivePanelBase),
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
                typeof(HivePanelBase),
                new PropertyMetadata(default(Thickness), LayoutPropertyChanged));

        public double FixedEdge
        {
            get { return (double)GetValue(FixedEdgeProperty); }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("FixedEdge < 0");
                SetValue(FixedEdgeProperty, value);
            }
        }

        public static readonly DependencyProperty FixedEdgeProperty =
            DependencyProperty.Register(
                nameof(FixedEdge),
                typeof(double),
                typeof(HivePanelBase),
                new PropertyMetadata(0d, LayoutPropertyChanged));

        protected static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HivePanelBase hp)
            {
                hp.InvalidateMeasure();
                //hp.InvalidateArrange();
            }
        }
        #endregion

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
    }
}

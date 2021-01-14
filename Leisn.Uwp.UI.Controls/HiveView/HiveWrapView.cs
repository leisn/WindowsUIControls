using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Leisn.Uwp.UI.Extensions;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Leisn.Uwp.UI.Controls
{
    public class HiveWrapView : HiveView
    {
        public HiveWrapView()
        {
            this.DefaultStyleKey = typeof(HiveWrapView);
        }

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
                typeof(HiveWrapView),
                new PropertyMetadata(Orientation.Horizontal, OnHivePropertyChanged));

        #endregion

        private static void OnHivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (HiveWrapView)d;
            var panel = view.FindDescendant<HiveWrapPanel>();
            if (panel == null) return;
            if (e.Property == OrientationProperty)
            {
                view.SetOrientation((Orientation)e.NewValue);
            }
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            var panel = this.FindDescendant<HiveWrapPanel>();
            if (panel == null)
                throw new InvalidCastException("ItemsPanel must be a HiveWrapPanel");
            SetOrientation(Orientation);
        }

        internal void SetOrientation(Orientation orientation)
        {
            var panel = this.FindDescendant<HiveWrapPanel>();
            if (panel == null)
                throw new InvalidCastException("ItemsPanel must be a HiveWrapPanel");
            panel.SetValue(HiveWrapPanel.OrientationProperty, Orientation);
            bool isVer = orientation == Orientation.Horizontal;
            SetValue(ScrollViewer.HorizontalScrollModeProperty, isVer ? ScrollMode.Disabled : ScrollMode.Enabled);
            SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, isVer ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto);
            SetValue(ScrollViewer.VerticalScrollModeProperty, isVer ? ScrollMode.Enabled : ScrollMode.Disabled);
            SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, isVer ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
        }
    }
}

using System;

using Leisn.Uwp.UI.Extensions;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Leisn.Uwp.UI.Controls
{
    public class HiveGridView : HiveView
    {
        public HiveGridView()
        {
            this.DefaultStyleKey = typeof(HiveGridView);
            FixedEdge = 0;
        }


        #region DependencyProperty
        public static readonly DependencyProperty RowCountProperty =
          DependencyProperty.Register(
              nameof(RowCount),
              typeof(int),
              typeof(HiveGridView),
              new PropertyMetadata(1, OnHivePropertyChanged));
        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        public static readonly DependencyProperty ColumnCountProperty =
          DependencyProperty.Register(
              nameof(ColumnCount),
              typeof(int),
              typeof(HiveGridView),
              new PropertyMetadata(1, OnHivePropertyChanged));
        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set
            { SetValue(ColumnCountProperty, value); }
        }

        #endregion

        private static void OnHivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (HiveGridView)d;
            var panel = view.FindDescendant<HiveGrid>();
            if (panel == null) return;
            if (e.Property == RowCountProperty)
            {
                panel.SetValue(HiveGrid.RowCountProperty, e.NewValue);
            }
            else if (e.Property == ColumnCountProperty)
            {
                panel.SetValue(HiveGrid.ColumnCountProperty, e.NewValue);
            }
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);
            var panel = this.FindDescendant<HiveGrid>();
            if (panel == null)
                throw new InvalidCastException("ItemsPanel must be a HiveGrid");

            panel.SetValue(HiveGrid.RowCountProperty, RowCount);
            panel.SetValue(HiveGrid.ColumnCountProperty, ColumnCount);
        }
    }
}

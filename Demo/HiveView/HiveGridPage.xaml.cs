using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Demo.Helpers;
using Demo.Models;
using Leisn.UI.Xaml.Controls;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Demo.HiveView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HiveGridPage : Page
    {
        HiveGridPanelViewModel viewModel;
        public HiveGridPage()
        {
            this.InitializeComponent();
            viewModel = new HiveGridPanelViewModel();
            this.Loaded += HiveGridPage_Loaded;
        }

        private void HiveGridPage_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < viewModel.PanelRowCont; i++)
            {
                var row = viewModel.PanelRowCont - i;
                for (int j = 0; j < viewModel.PanelRowCont; j++)
                {
                    var col = viewModel.PanelColCont - j;
                    var item = new HiveViewItem();
                    item.SetValue(HiveGrid.RowProperty, row - 1);
                    item.SetValue(HiveGrid.ColumnProperty, col - 1);
                    item.Content = $"{i * viewModel.PanelColCont + j + 1} ({row},{col})";
                    item.DataContext = viewModel;
                    item.SetBinding(HiveViewItem.StrokeThicknessProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeThickness"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeDashArrayProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeDashArray"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeDashCapProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeDashCap"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeDashOffsetProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeDashOffset"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeLineJoinProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeLineJoin"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeEndLineCapProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeEndLineCap"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeMiterLimitProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeMiterLimit"),
                        Mode = BindingMode.OneWay
                    });
                    item.SetBinding(HiveViewItem.StrokeStartLineCapProperty, new Binding
                    {
                        Path = new PropertyPath("StrokeStartLineCap"),
                        Mode = BindingMode.OneWay
                    });
                    this.hiveGrid.Children.Add(item);
                }
            }
        }
    }
}

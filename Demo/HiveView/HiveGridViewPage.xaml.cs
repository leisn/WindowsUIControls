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
    public sealed partial class HiveGridViewPage : Page
    {
        HiveGridViewModel viewModel;
        public HiveGridViewPage()
        {
            this.InitializeComponent();
            viewModel = new HiveGridViewModel();
            viewModel.OnOrientaionChanged += ViewModel_OnOrientaionChanged;
            this.Loaded += HiveGridViewPage_Loaded;
        }
        private void ViewModel_OnOrientaionChanged(Orientation ori)
        {
            bool isVer = ori == Orientation.Horizontal;
            this.hiveGridView.SetValue(ScrollViewer.VerticalScrollModeProperty, isVer ? ScrollMode.Enabled : ScrollMode.Disabled);
            this.hiveGridView.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, isVer ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
            this.hiveGridView.SetValue(ScrollViewer.HorizontalScrollModeProperty, isVer ? ScrollMode.Disabled : ScrollMode.Enabled);
            this.hiveGridView.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, isVer ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto);
        }
        private async void HiveGridViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            var panel = this.hiveGridView.ItemsPanelRoot as ItemsHiveGrid;
            panel.DataContext = viewModel;
            await viewModel.LoadItems();
        }

        private void hiveGridView_SelectionChanged(object sender, Leisn.UI.Xaml.Controls.SingleSelectionChangedEventArgs e)
        {
            viewModel.Selection = ($"index = {e.OldIndex}, value = {e.OldValue}  -->  index = {e.NewIndex}, value = {e.NewValue}");
        }

    }
}

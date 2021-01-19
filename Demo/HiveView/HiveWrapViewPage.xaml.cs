using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Demo.Models;

using Leisn.UI.Xaml.Controls;

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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Demo.HiveView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HiveWrapViewPage : Page
    {
        HiveWrapViewModel viewModel;
        public HiveWrapViewPage()
        {
            this.InitializeComponent();
            viewModel = new HiveWrapViewModel();
            viewModel.OnOrientaionChanged += ViewModel_OnOrientaionChanged;
            this.Loaded += HiveViewDemoPage_Loaded;

        }
        private void ViewModel_OnOrientaionChanged(Orientation ori)
        {
            bool isVer = ori == Orientation.Horizontal;
            this.hiveWrapView.SetValue(ScrollViewer.VerticalScrollModeProperty, isVer ? ScrollMode.Enabled : ScrollMode.Disabled);
            this.hiveWrapView.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, isVer ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
            this.hiveWrapView.SetValue(ScrollViewer.HorizontalScrollModeProperty, isVer ? ScrollMode.Disabled : ScrollMode.Enabled);
            this.hiveWrapView.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, isVer ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto);
        }

        private async void HiveViewDemoPage_Loaded(object sender, RoutedEventArgs e)
        {

            var panel = this.hiveWrapView.ItemsPanelRoot as HiveWrapPanel;
            panel.DataContext = viewModel;

            await viewModel.LoadItems();
        }

        private void hiveWrapView_SelectionChanged(object sender, SingleSelectionChangedEventArgs e)
        {
            viewModel.WrapSelection = ($"index = {e.OldIndex}, value = {e.OldValue}  -->  index = {e.NewIndex}, value = {e.NewValue}");
        }
    }
}

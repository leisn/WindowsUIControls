using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Demo.Helpers;
using Demo.Models;
using Leisn.UI.Xaml.Extensions;
using Leisn.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Demo.HiveView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HiveViewPage : Page
    {
        HiveViewModel viewModel;
        public HiveViewPage()
        {
            this.InitializeComponent();
            viewModel = new HiveViewModel();
            this.Loaded += HiveViewPage_Loaded;
        }

        private async void HiveViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadItems();
        }

        private void hiveView_Loaded(object sender, RoutedEventArgs e)
        {
            var panel = this.hiveView.ItemsPanelRoot as HivePanel;
            panel.DataContext = viewModel;
        }
    }
}

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

        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri uri))
                await Launcher.LaunchUriAsync(uri);
        }

        private void hiveView_SelectionChanged(object sender, Leisn.Uwp.UI.Controls.SingleSelectionChangedEventArgs e)
        {
            viewModel.ViewSelected = ($"index = {e.OldIndex}, value = {e.OldValue}  -->  index = {e.NewIndex}, value = {e.NewValue}");
        }
    }
}

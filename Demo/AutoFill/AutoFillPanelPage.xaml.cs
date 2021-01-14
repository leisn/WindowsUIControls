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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Demo.AutoFill
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AutoFillPanelPage : Page
    {
        AutoFillPanelViewModel viewModel;
        public AutoFillPanelPage()
        {
            this.InitializeComponent();

            viewModel = new AutoFillPanelViewModel();
            viewModel.OnOrientaionChanged += ViewModel_OnOrientaionChanged;
        }

        private void ViewModel_OnOrientaionChanged(Orientation ori)
        {
            bool isVer = ori == Orientation.Horizontal;
            scrollViewer.VerticalScrollMode = isVer ? ScrollMode.Enabled : ScrollMode.Disabled;
            scrollViewer.VerticalScrollBarVisibility = isVer ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
            scrollViewer.HorizontalScrollMode = isVer ? ScrollMode.Disabled : ScrollMode.Enabled;
            scrollViewer.HorizontalScrollBarVisibility = isVer ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri uri))
                await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}

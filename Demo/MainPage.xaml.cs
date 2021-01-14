using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Demo.Models;
using Demo.ViewModel;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using muxc = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModel viewModel;
        AppSettings settings;
        public MainPage()
        {
            settings = AppSettings.Current;

            this.InitializeComponent();
            viewModel = new MainViewModel();
            
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadItems();
        }

        private void navigationView_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Pages.SettingsPage), null,
                    args.RecommendedNavigationTransitionInfo);
            }
            else
            {
                var selectedItem = (MenuItemBase)args.SelectedItem;
                if (selectedItem != null)
                {
                    string pageName = "Demo." + selectedItem.Page;

                    var parameter = new NavigationParameter
                    {
                        Frame = this.contentFrame,
                        Content = selectedItem.Title,
                        NavigationView = navigationView
                    };

                    contentFrame.Navigate(Type.GetType(pageName),
                        parameter,
                        args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private void navigationView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            if (!contentFrame.CanGoBack)
                return;
            contentFrame.GoBack();
        }

        private void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            navigationView.IsBackEnabled = contentFrame.CanGoBack;
            if (contentFrame.SourcePageType == typeof(Pages.SettingsPage))
                navigationView.Header = new NavHeader { Title = "Settings" };
            else if (contentFrame.SourcePageType != null)
            {
                var item = navigationView.SelectedItem;

                var header = new NavHeader();
                if (item is MenuItemBase)
                {
                    header.Title = (item as MenuItemBase)?.Title;
                    header.Desc = (item as ControlItem)?.Desc;
                }
                else
                    header.Title = ((muxc.NavigationViewItem)navigationView.SelectedItem)?.Content?.ToString();
                navigationView.Header = header;
            }
        }

        private async void contentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                CloseButtonText = "OK",
                Title = "Navigtion Failed",
                Content = "Failed to load Page " + e.SourcePageType.FullName + ". " + Environment.NewLine + e.Exception.ToString()
            };
            await dialog.ShowAsync();
        }
    }
}

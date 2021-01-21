using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Demo.Helpers;
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
        public static MainPage Current { get; private set; }

        MainViewModel viewModel;
        AppSettings settings;

        public MainPage()
        {
            Current = this;
            settings = AppSettings.Current;

            this.InitializeComponent();
            viewModel = new MainViewModel();

            this.Loaded += MainPage_Loaded;
        }

        public void NavigateToItem(MenuItemBase menu)
        {
            this.navigationView.SelectedItem = menu;
        }


        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadItems();
            contentFrame.Navigate(typeof(Pages.StartPage));
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

                    contentFrame.Navigate(Type.GetType(pageName),
                        selectedItem,
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
            else if (contentFrame.SourcePageType == typeof(Pages.StartPage))
            {
                navigationView.Header =
                    new NavHeader
                    {
                        Title = "Start",
                        Desc = "This is a [AutoFillPanel](#AutoFill.AutoFillPanelPage) use AcrylicBrush as background."
                    };
            }
            else if (contentFrame.SourcePageType != null)
            {
                var header = new NavHeader();

                object item = e.Parameter;

                if (item != null)
                {
                    var menuItem = item as MenuItemBase;
                    header.Title = menuItem.Title;
                    header.Desc = menuItem.Desc;
                }
                else
                {
                    item = navigationView.SelectedItem;
                    header.Title = ((muxc.NavigationViewItem)item)?.Content?.ToString();
                }

                navigationView.Header = header;
                if (item != navigationView.SelectedItem)
                {
                    //just set selected state
                    var oldItem = navigationView.ContainerFromMenuItem(navigationView.SelectedItem)
                         as muxc.NavigationViewItem;
                    if (oldItem != null)
                        oldItem.IsSelected = false;
                    var newItem = navigationView.ContainerFromMenuItem(item)
                             as muxc.NavigationViewItem;
                    if (newItem != null)
                        newItem.IsSelected = true;
                }
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

        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            var markdown = sender as Microsoft.Toolkit.Uwp.UI.Controls.MarkdownTextBlock;
            var link = e.Link;
            if (link.StartsWith("#"))
            {
                var uri = e.Link.Substring(1, e.Link.Length - 1);
                foreach (var ca in viewModel.Categories)
                {
                    if (ca.Page == uri)
                    {
                        navigationView.SelectedItem = ca;
                        return;
                    }
                    foreach (var item in ca.Items)
                    {
                        if (item.Page == uri)
                        {
                            navigationView.SelectedItem = item;
                            return;
                        }
                    }
                }
            }
            else
            {
                if (!link.StartsWith("http"))
                    link = markdown.UriPrefix + e.Link;
                if (Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                    await Windows.System.Launcher.LaunchUriAsync(uri);
            }
        }

    }
}

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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Demo.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ControlListPage : Page
    {
        ControlListViewModel viewModel;


        public ControlListPage()
        {
            this.InitializeComponent();
            viewModel = new ControlListViewModel();

            this.Loaded += OnLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel.Categorie = (e.Parameter as MenuItemBase)?.Title;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadItems();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.Current.NavigateToItem(e.ClickedItem as ControlItem);
        }
    }
}

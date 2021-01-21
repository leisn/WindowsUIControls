using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Demo.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Leisn.UI.Xaml.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Demo.Microsofts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SymbolsPage : Page
    {
        SymbolsViewModel viewModel;
        public SymbolsPage()
        {
            this.InitializeComponent();
            viewModel = new SymbolsViewModel();
            this.Loaded += SymbolsPage_Loaded;
        }

        private async void SymbolsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadItems();
        }

        private void gridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = this.gridView.SelectedItem; ;
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(item.ToString());
            Clipboard.SetContent(dataPackage);

            notification.Show(1567);
        }


        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //do nothing we won't change gridView
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            viewModel.FilterChanged(sender.Text);
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var item = args.SelectedItem;
            if(item is string)
            {
                sender.Text = "";
                return;
            }
            this.gridView.SelectedItem = item;
            this.gridView.MakeVisible(new SemanticZoomLocation { Item = item });
        }
    }
}

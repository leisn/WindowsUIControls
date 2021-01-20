using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.Models;
using Demo.ViewModel;

using Windows.UI.Xaml.Controls;

namespace Demo.Microsofts
{
    public class SymbolsViewModel : BaseViewModel
    {
        public ObservableCollection<ItemsGroup> Groups;

        public SymbolsViewModel()
        {
            Groups = new ObservableCollection<ItemsGroup>();
        }

        public async Task LoadItems()
        {
            IsBusy = true;

            Groups.Clear();
            await Task.Delay(100);
            var syms = Enum.GetValues(typeof(Symbol));
            var list = new List<object>();
            foreach (var item in syms)
            {
                list.Add(item);
            }
            var groups = from sym in list
                         group sym by sym.ToString().Substring(0, 1) into g
                         orderby g.Key
                         select new ItemsGroup(g);

            foreach (var item in groups)
            {
                Groups.Add(item);
            }

            IsBusy = false;
        }
    }
}

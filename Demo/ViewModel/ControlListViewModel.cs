using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.Models;

namespace Demo.ViewModel
{
    public class ControlListViewModel : BaseViewModel
    {
        public ObservableCollection<ControlItem> Items;

        public string Categorie { get; set; }

        public ControlListViewModel()
        {
            Items = new ObservableCollection<ControlItem>();
        }

        public async Task LoadItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Items.Clear();
                if (string.IsNullOrWhiteSpace(Categorie))
                    return;

                var categorie = await DataStore.GetControlCategorieAsync(Categorie);

                if (categorie != null)
                {
                    foreach (var item in categorie.Items)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

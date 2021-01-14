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
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<ControlCategorie> Categories;

        public MainViewModel()
        {
            Categories = new ObservableCollection<ControlCategorie>();
        }

        public async Task LoadItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Categories.Clear();

                var categories = await DataStore.GetControlCategoriesAsync();

                if (categories != null)
                {
                    foreach (var item in categories)
                    {
                        Categories.Add(item);
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
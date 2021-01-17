using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.Models;

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Demo.ViewModel
{
    public class StartViewModel : BaseViewModel
    {
        public ObservableCollection<Size> ControlSizes;

        public StartViewModel()
        {
            ControlSizes = new ObservableCollection<Size>();
        }

        readonly Size[] Sizes = new Size[]
        {
            new Size(100,100),
            new Size(100,48),new Size(100,48),
            new Size(48,48), new Size(48,48), new Size(48,48),new Size(48,48),new Size(48,48),
            new Size(22,22),  new Size(22,22), new Size(22,22),
        };

        public async Task LoadItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                ControlSizes.Clear();

                var sizes = await RandomItems();

                foreach (var item in sizes)
                {
                    ControlSizes.Add(item);
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

        async Task<List<Size>> RandomItems()
        {
            return await Task.Run(() =>
            {
                Random random = new Random();
                List<Size> items = new List<Size> {
                    new Size
                    {
                        Width=100,
                        Height=100
                    }
                };
                for (int i = 0; i < 99; i++)
                {
                    var size = Sizes[random.Next(0, Sizes.Length - 1)];
                    items.Add(size);
                }
                return items;
            });
        }

    }
}

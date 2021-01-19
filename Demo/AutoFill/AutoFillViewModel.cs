using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.ViewModel;

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Demo.AutoFill
{
    public class AutoFillViewModel : BaseViewModel
    {
        public Array Orientations;

        public ObservableCollection<Item> Items;

        public AutoFillViewModel()
        {
            Orientations = Enum.GetValues(typeof(Orientation));
            Items = new ObservableCollection<Item>();
        }

        double hSpacing = 4;
        public double HSpacing
        {
            get { return hSpacing; }
            set { SetProperty(ref hSpacing, value); }
        }

        double vSpacing = 4;
        public double VSpacing
        {
            get { return vSpacing; }
            set { SetProperty(ref vSpacing, value); }
        }

        public event Action<Orientation> OnOrientaionChanged;
        Orientation orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                SetProperty(ref orientation, value,
                onChanged: () => { OnOrientaionChanged?.Invoke(value); });
            }
        }

        public async Task LoadItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Items.Clear();

                var sizes = await RandomItems();

                foreach (var item in sizes)
                {
                    Items.Add(item);
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


        readonly Size[] Sizes = new Size[]
        {
            new Size(204,204),
            new Size(204,100),new Size(204,100),
            new Size(100,100),  new Size(100,100), new Size(100,100), new Size(100,100), new Size(100,100),
            new Size(48,48),  new Size(48,48), new Size(48,48),
        };
        async Task<List<Item>> RandomItems()
        {
            return await Task.Run(() =>
            {
                Random random = new Random();
                List<Item> items = new List<Item>();
                var count = random.Next(300, 567);
                for (int i = 0; i < count; i++)
                {
                    var size = Sizes[random.Next(0, Sizes.Length - 1)];
                    var item = new Item
                    {
                        Index = i,
                        Width = size.Width,
                        Height = size.Height,
                        //Width=random.Next(50,300),
                        //Height=random.Next(50,300),
                        Color = Color.FromArgb(255, (byte)random.Next(3, 253),
                        (byte)random.Next(3, 253), (byte)random.Next(3, 253))
                    };
                    items.Add(item);
                }
                return items;
            });
        }

        public class Item
        {
            public int Index { get; internal set; }

            public double Width { get; internal set; }

            public double Height { get; internal set; }

            public Color Color { get; internal set; }
        }
    }
}

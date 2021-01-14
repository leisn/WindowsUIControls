using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.ViewModel;

using Windows.UI.Xaml.Controls;

namespace Demo.HiveView
{
    public class HiveViewModel : BaseViewModel
    {

        public ObservableCollection<int> Numbers;

        public HiveViewModel()
        {
            Numbers = new ObservableCollection<int>();
        }
        public string viewSelected;

        public string ViewSelected
        {
            get { return viewSelected; }
            set { SetProperty(ref viewSelected, value); }
        }

        double viewSpacing = 4;
        public double ViewSpacing
        {
            get { return viewSpacing; }
            set { SetProperty(ref viewSpacing, value); }
        }

        double viewEdge = 30;
        public double ViewEdge
        {
            get { return viewEdge; }
            set { SetProperty(ref viewEdge, value); }
        }

        public async Task LoadItems()
        {
            await Task.Delay(1);
            Numbers.Clear();
            for (int i = 1; i < 17; i++)
            {
                Numbers.Add(i);
            }
        }
    }
}

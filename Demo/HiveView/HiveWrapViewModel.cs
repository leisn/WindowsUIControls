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
    public class HiveWrapViewModel : BaseViewModel
    {
        public ObservableCollection<int> Numbers;
        public Array Orientations;
        public HiveWrapViewModel()
        {
            Numbers = new ObservableCollection<int>();
            Orientations = Enum.GetValues(typeof(Orientation));
        }

        string wrapSelection = "";
        public string WrapSelection
        {
            get { return wrapSelection; }
            set { SetProperty(ref wrapSelection, value); }
        }

        double wrapSpacing = 4;
        public double WrapSpacing
        {
            get { return wrapSpacing; }
            set { SetProperty(ref wrapSpacing, value); }
        }
        double wrapEdge = 30;
        public double WrapEdge
        {
            get { return wrapEdge; }
            set { SetProperty(ref wrapEdge, value); }
        }
        Orientation wrapOrientation = Orientation.Horizontal;
        public Orientation WrapOrientation
        {
            get { return wrapOrientation; }
            set { SetProperty(ref wrapOrientation, value); }
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

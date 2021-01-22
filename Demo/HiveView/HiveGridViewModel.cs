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
    public class HiveGridViewModel : BaseViewModel
    {
        public ObservableCollection<int> Numbers;
        public Array Orientations;
        public HiveGridViewModel()
        {
            Numbers = new ObservableCollection<int>();
            Orientations = Enum.GetValues(typeof(Orientation));
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

        double gridFixedEdge = 30;
        public double GridFixedEdge
        {
            get { return gridFixedEdge; }
            set { SetProperty(ref gridFixedEdge, value); }
        }

        int gridCont = 4;
        public int GridCount
        {
            get { return gridCont; }
            set { SetProperty(ref gridCont, value); }
        }


        double gridSpacing = 4;
        public double GridSpacing
        {
            get { return gridSpacing; }
            set { SetProperty(ref gridSpacing, value); }
        }

        string selection = "";
        public string Selection
        {
            get { return selection; }
            set { SetProperty(ref selection, value); }
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

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
        public ObservableCollection<string> Points;

        public HiveGridViewModel()
        {
            Points = new ObservableCollection<string>();
        }


        double gridFixedEdge = 30;
        public double GridFixedEdge
        {
            get { return gridFixedEdge; }
            set { SetProperty(ref gridFixedEdge, value); }
        }

        int gridRowCont = 4;
        public int GridRowCont
        {
            get { return gridRowCont; }
            set { SetProperty(ref gridRowCont, value); }
        }

        int gridColCont = 4;
        public int GridColCont
        {
            get { return gridColCont; }
            set { SetProperty(ref gridColCont, value); }
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
            Points.Clear();
            for (int i = 1; i < 17; i++)
            {
                Points.Add($"{(i - 1) / 4}, {(i - 1) % 4}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.ViewModel;

using Windows.UI.Xaml.Controls;

namespace Demo.AutoFill
{
    public class AutoFillPanelViewModel : BaseViewModel
    {
        public Array Orientations;

        public AutoFillPanelViewModel()
        {
            Orientations = Enum.GetValues(typeof(Orientation));
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

    }
}

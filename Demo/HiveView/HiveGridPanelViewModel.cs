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
    public class HiveGridPanelViewModel : BaseViewModel
    {
        bool autocell = false;
        public bool Autocell
        {
            get { return autocell; }
            set { SetProperty(ref autocell, value); }
        }

        double panelSpacing = 4;
        public double PanelSpacing
        {
            get { return panelSpacing; }
            set { SetProperty(ref panelSpacing, value); }
        }

        double panelFixedEdge = 0;
        public double PanelFixedEdge
        {
            get { return panelFixedEdge; }
            set { SetProperty(ref panelFixedEdge, value); }
        }

        int panelRowCont = 4;
        public int PanelRowCont
        {
            get { return panelRowCont; }
            set { SetProperty(ref panelRowCont, value); }
        }

        int panelColCont = 7;
        public int PanelColCont
        {
            get { return panelColCont; }
            set { SetProperty(ref panelColCont, value); }
        }
    }
}

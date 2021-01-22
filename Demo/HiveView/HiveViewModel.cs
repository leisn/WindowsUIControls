using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo.ViewModel;
using Demo.Models;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Demo.HiveView
{

    public class HiveViewModel : BaseViewModel
    {
        public Array Orientations;
        public ObservableCollection<ItemsGroup> Groups;

        public HiveViewModel()
        {
            Groups = new ObservableCollection<ItemsGroup>();
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
            Groups.Clear();

            Groups.Add(new ItemsGroup("A", new List<string>
            {
                "Alexander","Andrew","Alexis","Aaron","Anna","Avery","Antonio"
            }));
            Groups.Add(new ItemsGroup("B", new List<string>
            {
                "Benjamin","Brianna","Blake","Bailey","Bella","Bennett","Brynn"
            }));
            Groups.Add(new ItemsGroup("C", new List<string>
            {
                "Chloe","Christian","Connor","Charles","Carlos","Claire","Cooper","Colin"
            }));
            Groups.Add(new ItemsGroup("D", new List<string>
            {
                "Daniel","David","Dylan","Diana","Dawson","Dean","Drake"
            }));
            Groups.Add(new ItemsGroup("E", new List<string>
            {
                "Emily","Ethan","Emma","Ella","Edward","Eva","Elise","Ezra"
            }));
            Groups.Add(new ItemsGroup("F", new List<string>
            {
                "Francisco","Faith","Finn","Felix","Frank","Finlay","Faye"
            }));
            Groups.Add(new ItemsGroup("G", new List<string>
            {
                "Grace","Gabriel","Gavin","George","Grayson","Georgia","Grant"
            }));
            Groups.Add(new ItemsGroup("H", new List<string>
            {
                "Hannah","Henry","Hayden","Holly","Heidi","Hanna","Harmony"
            }));
            Groups.Add(new ItemsGroup("J", new List<string>
            {
                "Joshua","James","Jack","John","Jackson","Jordan","Jonathan"
            }));
            Groups.Add(new ItemsGroup("K", new List<string>
            {
                "Kevin","Kayla","Kyle","Kaitlyn","Katherine","Kiara","Kyla"
            }));
            Groups.Add(new ItemsGroup("L", new List<string>
            {
                "Luke","Lucas","Lily","Leah","Levi","Leo","Laura"
            }));
        }
    }
}

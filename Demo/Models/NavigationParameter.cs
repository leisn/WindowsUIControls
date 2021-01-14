using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Demo.Models
{
    public class NavigationParameter
    {
        public Frame Frame { get; set; }
        public Microsoft.UI.Xaml.Controls.NavigationView NavigationView { get; set; }
        public string Content { get; set; }

        public object ExtraParameter { get; set; }
    }
}

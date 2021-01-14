using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ControlItem : MenuItemBase
    {
        public string Path { get; set; }
        public string Desc { get; set; }

        //public Geometry Parse(string data)
        //{
        //    var xaml = "<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
        //    + "<Path.Data>" + data + "</Path.Data>" + "</Path>";
        //    var path = XamlReader.Load(xaml) as Path;
        //    return path.Data;
        //}
    }
}

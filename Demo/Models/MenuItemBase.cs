using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class MenuItemBase
    {
        public string Title { get; set; }
        public string Page { get; set; }
        public string Desc { get; set; }
    }

    public class NavHeader
    {
        public string Title;
        public string Desc;
    }
}

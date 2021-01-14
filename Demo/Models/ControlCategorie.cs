using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ControlCategorie : MenuItemBase
    {
        public string Glyph { get; set; }
        public ControlItem[] Items { get; set; }
    }
}

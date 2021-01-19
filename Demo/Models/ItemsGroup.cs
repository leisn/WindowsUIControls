using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ItemsGroup
    {
        public ObservableCollection<string> Items { get; }

        public string Title { get; set; }

        public ItemsGroup()
        {
            Items = new ObservableCollection<string>();
        }

        public ItemsGroup(string key, IEnumerable<string> items) : this()
        {
            this.Title = key;
            foreach (var item in items)
                this.Items.Add(item);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

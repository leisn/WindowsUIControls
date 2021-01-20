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
        public ObservableCollection<object> Items { get; }

        public string Title { get; set; }

        public ItemsGroup()
        {
            Items = new ObservableCollection<object>();
        }

        public ItemsGroup(string key, IEnumerable<object> items) : this()
        {
            this.Title = key;
            foreach (var item in items)
                this.Items.Add(item);
        }

        public ItemsGroup(IGrouping<string,object> grouping) : this()
        {
            this.Title = grouping.Key;
            foreach (var item in grouping)
                this.Items.Add(item);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

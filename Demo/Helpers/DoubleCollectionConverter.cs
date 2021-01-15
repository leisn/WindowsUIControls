using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Demo.Helpers
{
    public class DoubleCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = value as DoubleCollection;
            var str = "";
            if (val == null)
                return str;
            for (int i = 0; i < val.Count; i++)
            {
                str += val[i].ToString();
                if (i != val.Count - 1)
                    str += ",";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var val = (string)value;
            val = val.Replace(',', ' ');
            var db = new DoubleCollection();

            var sp = val.Split(' ');
            for (int i = 0; i < sp.Length; i++)
            {
                var temp = sp[i].Trim();
                if (double.TryParse(temp, out double d))
                    db.Add(d);
            }
            if (db.Count == 0)
                return null;
            return db;
        }
    }
}
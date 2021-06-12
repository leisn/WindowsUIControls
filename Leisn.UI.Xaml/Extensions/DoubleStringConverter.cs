using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Data;

namespace Leisn.UI.Xaml.Extensions
{
    public class DoubleStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double val;
            if (!double.TryParse(value.ToString(), out val))
            {
                Console.WriteLine("Parse double failed:" + value);
                return "NaN";
            }

            if (parameter != null)
            {
                return val.ToString(parameter.ToString());
            }
            return val.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double val;
            if (!double.TryParse(value.ToString(), out val))
            {
                Console.WriteLine("Parse double failed:" + value);
                return double.NaN;
            }
            return val;
        }
    }
}

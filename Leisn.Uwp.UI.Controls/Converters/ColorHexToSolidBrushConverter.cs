using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leisn.Uwp.UI.Extensions;

using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Leisn.Uwp.UI.Controls
{
    public class ColorHexToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                //return new SolidColorBrush(Color.FromHex("#004F8A"));
                return null;
            var brush = new SolidColorBrush(ColorConverter.FromHex((string)value));
            //System.Diagnostics.Debug.WriteLine(brush.Color.ToHex());
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "";
            if (!(value is SolidColorBrush))
                throw new ArgumentException($"类型错误，'{value.GetType()}'不是SolidColorBrush");
            return ((SolidColorBrush)value).Color.ToHex();
        }
    }
}

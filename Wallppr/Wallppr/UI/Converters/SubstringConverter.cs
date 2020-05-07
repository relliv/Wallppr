using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Wallppr.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class SubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var content = (string)value;
            var length = int.Parse((string)parameter);

            if (content.Length > length)
            {
                return content.Substring(0, length);
            }

            return content;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

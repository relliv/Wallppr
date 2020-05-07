using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wallppr.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ThisMonthUnderlineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var month = (int)value;

            if (DateTime.Now.Month == month)
            {
                return TextDecorations.Underline;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

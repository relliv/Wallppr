using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wallppr.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NullToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

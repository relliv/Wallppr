using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Wallppr.UI.Converters
{
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class PathIsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

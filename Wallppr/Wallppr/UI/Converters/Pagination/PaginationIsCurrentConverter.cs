using Wallppr.Models.Common.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wallppr.UI.Converters.Pagination
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PaginationIsCurrentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is PageType val)
            {
                if (val == PageType.Current)
                    return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
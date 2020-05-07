using Wallppr.Models.Common.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wallppr.UI.Converters.Pagination
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PaginationPrevNextNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Hidden;

            if (value != null && value is PageType val)
            {
                if (val == PageType.Normal || val == PageType.Current)
                    visibility = Visibility.Visible;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
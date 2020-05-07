using Wallppr.Models.Common.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wallppr.UI.Converters.Pagination
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PaginationPrevNextIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Geometry path = null;

            if (value != null && value is PageType val)
            {
                if (val == PageType.Start)
                {
                    path = (Application.Current.FindResource("ChevronDoubleLeft") as Path).Data;
                }
                else if (val == PageType.End)
                {
                    path = (Application.Current.FindResource("ChevronDoubleRight") as Path).Data;
                }
                else if (val == PageType.Next)
                {
                    path = (Application.Current.FindResource("ChevronRight") as Path).Data;
                }
                else if (val == PageType.Previous)
                {
                    path = (Application.Current.FindResource("ChevronLeft") as Path).Data;
                }
            }

            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
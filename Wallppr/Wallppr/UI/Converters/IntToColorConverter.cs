using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Wallppr.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class IntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var val = value is long valLong && valLong > 0
                ? Math.Abs(valLong) % 10
                : (value is int valInt && valInt > 0 ? Math.Abs(valInt) % 10 : 0);

            var color = (SolidColorBrush)new BrushConverter().ConvertFrom("#e2252a"); // red - rose

            switch (val)
            {
                case 0:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#e2252a"); // red - rose
                    break;
                case 1:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#52b1c0"); // blue - sapphire
                    break;
                case 2:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#e9c17a"); // tan - latte
                    break;
                case 3:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#05c04a"); // green - parakeet
                    break;
                case 4:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#fdae1d"); // orange - merigold
                    break;
                case 5:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#a91a0d"); // red - apple
                    break;
                case 6:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#5cbc63"); // green - fern
                    break;
                case 7:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#7d7c9c"); // grey - flint
                    break;
                case 8:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#a32cc3"); // purple - purple
                    break;
                case 9:
                    color = (SolidColorBrush)new BrushConverter().ConvertFrom("#c7a951"); // tan - fawn
                    break;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

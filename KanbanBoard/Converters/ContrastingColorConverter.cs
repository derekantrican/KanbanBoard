using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace KanbanBoard.Converters
{
    public class ContrastingColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Color color)
            {
                Color foreColor = PerceivedBrightness(color) > 130 ? System.Drawing.Color.Black.Convert() : System.Drawing.Color.White.Convert();
                return new SolidColorBrush(foreColor);
            }

            return AvaloniaProperty.UnsetValue;
        }

        private int PerceivedBrightness(Color c) //Credit: https://stackoverflow.com/a/2241471/2246411
        {
            return (int)Math.Sqrt(
            c.R * c.R * .299 +
            c.G * c.G * .587 +
            c.B * c.B * .114);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not used
            return AvaloniaProperty.UnsetValue;
        }
    }
}

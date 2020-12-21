using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace KanbanBoard.Converters
{
    public class IsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not used
            return AvaloniaProperty.UnsetValue;
        }
    }
}

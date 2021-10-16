using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DayZTediratorToolz.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case true:
                    return Visibility.Visible;
                default:
                    return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
               case Visibility.Visible:
                   return true;
               default:
                   return false;
            }
        }
    }
}
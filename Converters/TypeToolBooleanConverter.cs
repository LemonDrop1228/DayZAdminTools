using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using DayZTediratorToolz.Helpers;

namespace DayZTediratorToolz.Converters
{
    public class TypeToolBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(parameter.ToString()) == (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
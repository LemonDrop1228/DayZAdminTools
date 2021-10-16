using System;
using System.Windows.Data;

namespace DayZTediratorToolz.Converters
{
    public class OneZeroToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch(value.ToString().ToLower())
            {
                case "1":
                    return true;
                case "0":
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is bool)
            {
                if((bool)value == true)
                    return "1";
                else
                    return "0";
            }
            return "no";
        }
    }
}
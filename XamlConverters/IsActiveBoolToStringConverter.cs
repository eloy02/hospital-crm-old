using System;
using System.Globalization;
using System.Windows.Data;

namespace XamlConverters
{
    public class IsActiveBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool active)
            {
                if (active)
                    return "Доступно";
                else return "Недоступно";
            }
            else return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
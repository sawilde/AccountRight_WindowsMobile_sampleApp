using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MYOB.Sample.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                       CultureInfo culture)
        {
            parameter = parameter ?? Visibility.Visible;
            if (value is bool)
            {
                var booleanValue = (bool)value;
                var visibility = (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString(), true);
                if (booleanValue) return visibility;
                return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DemoResource.converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Reverse { get; set; }

        public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue)) return value;
            if (boolValue)
            {
                return Reverse ? FalseVisibility : Visibility.Visible;
            }
            else
            {
                return Reverse ? Visibility.Visible : FalseVisibility;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility visiValue)) return value;
            return visiValue == Visibility.Visible ? !Reverse : Reverse;
        }
    }
}

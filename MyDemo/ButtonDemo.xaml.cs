using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDemo
{
    /// <summary>
    /// ButtonDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonDemo : UserControl
    {
        public ButtonDemo()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("click");
        }
    }

    public class Thickness2ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Thickness t) || !(parameter is string p)) return value;

            var p2 = p.Trim().ToLower();
            var result = new Thickness();
            if (p2.Contains("l"))
            {
                result.Left = t.Left;
            }
            else if (p2.Contains("r"))
            {
                result.Right = t.Right;
            }
            else if (p2.Contains("t"))
            {
                result.Top = t.Top;
            }
            else if (p2.Contains("b"))
            {
                result.Bottom = t.Bottom;
            }
            else
            {
                result = t;
            }
            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

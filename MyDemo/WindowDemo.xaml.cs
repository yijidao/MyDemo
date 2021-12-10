using System;
using System.Collections.Generic;
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
    /// WindowDemo.xaml 的交互逻辑
    /// </summary>
    public partial class WindowDemo : UserControl
    {
        private Window _window = new Window
        {
            Width = 100, Height = 100, Background = new SolidColorBrush(Colors.LightBlue)
        };

        public WindowDemo()
        {
            InitializeComponent();
            _window.Show();
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var p = Mouse.GetPosition(App.Current.MainWindow);
            
            _window.Left= p.X;
            _window.Top = p.Y;

        }
    }
}

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

namespace ReactiveUIDemo.Vlc
{
    /// <summary>
    /// MonitorView2.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorView2 : UserControl
    {
        public MonitorView2()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new Window()
            {
                Height = 600,
                Width = 1200,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = new SolidColorBrush()
                {
                    Color = Colors.Blue,
                    Opacity = 0.2
                },
                Content = new Grid(),
            };
            window.MouseRightButtonDown += (o, args) => window.Close();
            window.ShowDialog();
        }
    }
}

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
using System.Windows.Shapes;

namespace WpfApp9
{
    /// <summary>
    /// MainWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
        }

        private int _index = 0;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var target = (UIElement)e.OriginalSource;
            var p = target.TransformToAncestor(innerContainer).Transform(new Point(0, 0));
            var detail = GetDetail(p.X, p.Y);
            innerContainer.Children.Add(detail);

        }

        private Border GetDetail(double x, double y)
        {
            var detail = new Border
            {
                Child = new TextBlock
                {
                    Text = $"{_index++}"
                },
                Background = Brushes.Aquamarine,
                Width = 120,
                Height = 80,
                Margin = new Thickness(x, y - 80, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.White
            };
            Panel.SetZIndex(detail, ++_zIndex);
            detail.MouseDown += DetailOnMouseDown;
            return detail;
        }

        private int _zIndex = 0;

        private void DetailOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var ctl = (UIElement)sender;
            Panel.SetZIndex(ctl, ++_zIndex);

        }
    }
}

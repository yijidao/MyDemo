using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace ReactiveUIDemo
{
    /// <summary>
    /// MainWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();

            _window = new Window() { Width = 1800, Height = 900 };
            Loaded += OnLoaded;
        }

        private Window _window;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var demoType in Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IDemo))))
            {
                var btn = new Button()
                {
                    Content = demoType.Name,
                    Height = 30,
                    Width = 110
                };
                btn.Click += (o, args) =>
                {
                    _window.Content = Activator.CreateInstance(demoType);
                    if (!_window.IsVisible)
                    {
                        _window.Show();
                    }
                };
                buttonContainer.Children.Add(btn);
            }
        }
    }
}

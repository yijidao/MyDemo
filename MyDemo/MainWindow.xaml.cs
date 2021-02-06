using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public Window ContentHost { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var name = (sender as ContentControl).Content.ToString();
            var content = (UserControl)Activator.CreateInstance(Type.GetType($"MyDemo.{name}"));
            //controlContainer.Content = content;
            if (ContentHost == null)
            {
                ContentHost = new Window
                {
                    Height = 800,
                    Width = 1600
                };
                ContentHost.Owner = this;
                ContentHost.Closing += (o, args) =>
                {
                    args.Cancel = true;
                    ContentHost.Hide();
                };
            }

            ContentHost.Content = content;
            ContentHost.Show();
        }

        
    }
}

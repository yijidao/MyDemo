using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Grpc.Core;
using GrpcGreeterClient;

namespace WpfApp6
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server _server;

        public MainWindow()
        {
            InitializeComponent();

            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _server = new Server
            {
                Services =
                {
                    Greeter.BindService(new GreeterService()),
                    WpfCommunication.BindService(new WpfCommunicationService()),
                },
                Ports = { new ServerPort("localhost", 8099, ServerCredentials.Insecure) }
            };
            _server.Start();

            
            //var host = new ControlHost(ActualHeight, ActualWidth);
            //host.Visibility = Visibility.Hidden;
            //Content = host;

            //var window = new Window();

            //window.Content = host;
            //window.Show();

            //grid.Children.Add(new ControlHost());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _server.ShutdownAsync().Wait();
            base.OnClosing(e);
        }
    }
}

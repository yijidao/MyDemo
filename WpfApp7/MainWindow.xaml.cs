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
using Grpc.Core;
using GrpcGreeterClient;

namespace WpfApp7
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Channel _channel;
        private Greeter.GreeterClient _client;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

            request.Click += (sender, args) =>
            {
                var content = input.Text;
                input.Text = string.Empty;
                var response = _client.SayHello(new HelloRequest { Name = content });
                message.AppendText(response.Message + Environment.NewLine);
            };
            handle.Click += (sender, args) =>
            {
                //var client = new WpfCommunication.WpfCommunicationClient(_channel);
                //var response = client.GetUserControl1(new WpfRequest());
                //message.AppendText(response.Handle.ToString() + Environment.NewLine);
                //var host = new ControlHost(response.Handle);

                var host = new ControlHost(_channel);

                border.Child = host;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _channel = new Channel("localhost", 8099, ChannelCredentials.Insecure);
            _client = new Greeter.GreeterClient(_channel);
        }
    }
}

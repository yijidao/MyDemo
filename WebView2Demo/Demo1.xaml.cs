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

namespace WebView2Demo
{
    /// <summary>
    /// Demo1.xaml 的交互逻辑
    /// </summary>
    public partial class Demo1 : UserControl
    {
        public Demo1()
        {
            InitializeComponent();
            webView.Source = new Uri("http://www.google.com");
            webView.NavigationStarting += (sender, args) => Log($"[{nameof(webView.NavigationStarting)}]: {args.Uri}");
            webView.NavigationCompleted += (sender, args) => Log($"[{nameof(webView.NavigationCompleted)}]: {args.WebErrorStatus}");
            webView.ContentLoading += (sender, args) => Log($"[{nameof(webView.ContentLoading)}]: {args.IsErrorPage}");;
            webView.SourceChanged += (sender, args) => Log($"[{nameof(webView.ContentLoading)}]: {args.IsNewDocument}"); ;
            webView.WebMessageReceived += (sender, args) => { };

            navigate.Click += (sender, args) =>
            {
                var url = address.Text;
                if (string.IsNullOrWhiteSpace(url))
                {
                    webView.CoreWebView2.ExecuteScriptAsync($"alert(`url is null`)");
                    return;
                }
                if (!url.StartsWith("http://") || !url.StartsWith("https://")) url = $"https://{url}";
                Log($"[navigate]: {url}");
                webView.CoreWebView2.Navigate(url);
            };

        }

        private void Log(string message) => logTextBox.AppendText($"{message}{Environment.NewLine}");
    }
}

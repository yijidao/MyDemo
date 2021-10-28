using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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

namespace WebView2Demo
{
    /// <summary>
    /// AutoRefreshDemo.xaml 的交互逻辑
    /// </summary>
    public partial class AutoRefreshDemo : UserControl
    {
        public AutoRefreshDemo()
        {
            InitializeComponent();
            input.Text = "https://www.baidu.com";
            webView2.Source = new Uri(input.Text);
            refresh.Click += (sender, args) =>
            {
                var url = input.Text;
                if (string.IsNullOrWhiteSpace(url))
                {
                    webView2.CoreWebView2.ExecuteScriptAsync($"alert(`url is null`)");
                    return;
                }

                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = $"https://{url}";
                    input.Text = url;
                }

                webView2.CoreWebView2.Navigate(url);
            };

            var o = Observable.FromEventPattern(isAuto, nameof(isAuto.Checked));

            var o2 = Observable.FromEventPattern(isAuto, nameof(isAuto.Unchecked));

            Observable.Interval(TimeSpan.FromSeconds(120))
                .SkipUntil(o)
                .TakeUntil(o2)
                .Repeat()
                .ObserveOnDispatcher()
                .Subscribe(_ =>
                {
                    refresh.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                });

            isAuto.IsChecked = true;

        }
    }
}

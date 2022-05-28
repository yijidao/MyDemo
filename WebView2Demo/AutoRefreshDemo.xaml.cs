using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using UserControl = System.Windows.Controls.UserControl;

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

            //webView2


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

                if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("file://"))
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

            printPdf.Click += async  (sender, args) =>
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "txt files (*.pdf)|*.pdf",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    FileName = $"test.pdf"
                };
                var result = saveFileDialog.ShowDialog();

                if (result != DialogResult.OK)
                    return;

                var printSetting = webView2.CoreWebView2.Environment.CreatePrintSettings();
                printSetting.ShouldPrintBackgrounds = true;

                // c 盘根目录因为权限控制可能无法生成文件，但是 c 盘子目录就可以了。
                var saveResult = await webView2.CoreWebView2.PrintToPdfAsync($"{saveFileDialog.FileName}", printSetting);
                
            };

            printPdf2.Click += async (sender, args) =>
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "txt files (*.pdf)|*.pdf",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    FileName = $"test.pdf"
                };
                var result = saveFileDialog.ShowDialog();

                if (result != DialogResult.OK)
                    return;
                var browser = new WebView2();
                browser.Source = new Uri(@"C:\cache\test_pdf.html");

                var printSetting = webView2.CoreWebView2.Environment.CreatePrintSettings();
                printSetting.ShouldPrintBackgrounds = true;

                var saveResult = await browser.CoreWebView2.PrintToPdfAsync($"{saveFileDialog.FileName}", printSetting);

            };
        }
    }
}

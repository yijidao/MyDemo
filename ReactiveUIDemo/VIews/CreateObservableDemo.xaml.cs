using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
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

namespace ReactiveUIDemo.Views
{
    /// <summary>
    /// CreateObservableDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CreateObservableDemo : UserControl, IDemo
    {
        public CreateObservableDemo()
        {
            InitializeComponent();

            // Rx.Net 将标准事件模式转换成 Observable 的两种方法。
            var clicks1 = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => button1.Click += h, h => button1.Click -= h);
            clicks1.Subscribe(_ =>
            {
                Debug.WriteLine("Click1");
            });

            var clicks2 = Observable.FromEventPattern(button1, nameof(Button.Click));
            clicks2.Subscribe(_ =>
            {
                Debug.WriteLine("Click2");
            });
        }
    }
}

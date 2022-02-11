using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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
using Prism.Ioc;

using ScheduleDemo.Services;

namespace ScheduleDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var testClass = new ScheduleServiceTestClass();
            //button1.Click += (sender, args) =>
            //{
                //var observable = testClass.Demo1();

                //observable.ObserveOnDispatcher()
                //    .Subscribe(x =>
                //{
                //    textBox1.Text += $"-{string.Join("-", x)}";
                //});
            //};

            button1.Click += (sender, args) =>
            {
                var o2 = ScheduleServiceTestClass.Demo2(MockServiceClass.MockService,
                            (oldValue, newValue) => newValue.Except(oldValue).ToArray(),
                            TimeSpan.FromSeconds(2))
                        .ObserveOnDispatcher()
                .Subscribe(x => textBox1.Text += $"-{string.Join("-", x)}");
            };
        }
    }
}

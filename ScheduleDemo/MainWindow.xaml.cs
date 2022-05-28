using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            IDisposable subject = null;

            //button2.Click += (sender, args) =>
            //{
            //    subject = ScheduleServiceTestClass.MockSubject1().ObserveOnDispatcher()
            //        .Subscribe(x => textBox2.Text += $"-{string.Join("-", x)}");
            //};

            //cancel2.Click += (sender, args) =>
            //{
            //    subject?.Dispose();
            //};

            button2.Click += (sender, args) =>
            {
                StringBuilder sb = new StringBuilder(textBox2.Text);
                subject = ScheduleServiceTestClass.MockSubject2()
                    .ObserveOnDispatcher()

                    .Subscribe(x =>
                    {
                        Debug.WriteLine($"{string.Join("-", x)}");
                        //sb.Append($"-{string.Join("-", x)}");
                        //textBox2.Text = sb.ToString();
                    });
                //subject = ScheduleServiceTestClass.MockSubject2().ObserveOnDispatcher()
                //    .Subscribe(x => textBox2.Text += $"-{string.Join("-", x)}");
            };

            cancel2.Click += (sender, args) =>
            {
                subject?.Dispose();
                GC.Collect();
            };

            button3.Click += (sender, args) =>
            {
                ScheduleServiceTestClass.MockSubject1()
                    .ObserveOnDispatcher()

                    .Subscribe(x => textBox3.Text += $"-{string.Join("-", x)}");
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace MyDemo
{
    /// <summary>
    /// BackgroundWorkerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class BackgroundWorkerDemo : UserControl
    {
        public BackgroundWorkerDemo()
        {
            InitializeComponent();

            var worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += (sender, args) =>
            {
                var w = sender as BackgroundWorker;
                //status.Content = "start";
                for (var i = 1; i <= 5; i++)
                {
                    if (w.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        w.ReportProgress(i * 20);
                    }
                }
            };
            worker.ProgressChanged += (sender, args) =>
            {
                status.Content = "doing";
                progress.Content = $"{args.ProgressPercentage}%";
            };
            worker.RunWorkerCompleted += (sender, args) =>
            {
                status.Content = args.Cancelled ? "canceled" : "completed";
            };
            start.Click += (sender, args) =>
            {
                if (worker.IsBusy) return;
                worker.RunWorkerAsync();
            };
            cancel.Click += (sender, args) =>
            {
                if (!worker.WorkerSupportsCancellation) return;
                worker.CancelAsync();
            };
        }
    }
}

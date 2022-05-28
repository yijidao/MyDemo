using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace SimpleThreadApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _continueCalculating;
        private bool _notAPrime;
        private long _num = 3000000000000000;


        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void StartOrStop(object sender, RoutedEventArgs e)
        {
            if (_continueCalculating)
            {
                _continueCalculating = false;
                startOrStopButton.Content = "Resume";
            }
            else
            {
                _continueCalculating = true;
                startOrStopButton.Content = "Stop";
                startOrStopButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal, CheckNextNumber);
            }
        }

        private void CheckNextNumber()
        {
            var x = new Stopwatch();
            x.Start();

            _notAPrime = false;

            for (long i = 3;  i<= Math.Sqrt(_num); i++)
            {
                if (_num % i == 0)
                {
                    _notAPrime = true;
                    break;
                }
            }

            if (!_notAPrime)
            {
                x.Stop();
                elapsed.Text = x.ElapsedMilliseconds.ToString();
                bigPrime.Text = _num.ToString();
            }

            _num += 2;
            if (_continueCalculating)
            {
                startOrStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, CheckNextNumber);
            }
        }
    }
}

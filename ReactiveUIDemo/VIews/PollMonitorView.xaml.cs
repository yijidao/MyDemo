using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using ReactiveUIDemo.ViewModels;

namespace ReactiveUIDemo.Views
{
    /// <summary>
    /// PollMonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class PollMonitorView : UserControl, IDemo
    {
        public PollMonitorView()
        {
            InitializeComponent();
        }

        private void MoreButton_OnClick(object sender, RoutedEventArgs e) => moreList.IsOpen = true;
    }

}

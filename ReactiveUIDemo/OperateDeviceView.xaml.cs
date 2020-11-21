using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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
using ReactiveUI;

namespace ReactiveUIDemo
{
    /// <summary>
    /// OperateDeviceView.xaml 的交互逻辑
    /// </summary>
    public partial class OperateDeviceView
    {
        public OperateDeviceView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Regions, v => v.devices.ItemsSource).DisposeWith(d);
            });
        }
    }
}

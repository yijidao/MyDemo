using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
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
    /// PublishPIDSView.xaml 的交互逻辑
    /// </summary>
    public partial class PublishPIDSView : ReactiveUserControl<PublishPIDSViewModel>
    {
        public PublishPIDSView()
        {
            InitializeComponent();

            ViewModel = new PublishPIDSViewModel(); ;
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Lines, v => v.lines.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedAll, v => v.selectedAll.IsChecked).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SelectedLines, v => v.stations.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedAllStation, v => v.selectedAllStation.IsChecked).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SelectedStations, v => v.regions.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedAllRegions, v => v.selectedAllRegion.IsChecked).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SelectedDevices, v => v.targets.ItemsSource).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ShowDevices, v => v.operateDevices).DisposeWith(d);
            });
        }
    }
}

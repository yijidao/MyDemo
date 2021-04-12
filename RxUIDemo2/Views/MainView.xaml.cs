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
using RxUIDemo2.ViewModels;

namespace RxUIDemo2.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.GoBackCommand, v => v.goBack.Command).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.GoNextCommand, v => v.goNext.Command).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Router, v => v.routedViewHost.Router).DisposeWith(d);
            });
        }
    }
}

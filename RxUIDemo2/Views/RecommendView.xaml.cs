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
    /// RecommendView.xaml 的交互逻辑
    /// </summary>
    public partial class RecommendView
    {
        public RecommendView()
        {
            InitializeComponent();
            ViewModel = new RecommendViewModel();


            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Username, v => v.username.Text).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.CloseCommand, v => v.close).DisposeWith(d);
            });
        }
    }
}

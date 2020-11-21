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
    /// LineDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class LineDetailView
    {
        public LineDetailView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Selected, v => v.isSelect.IsChecked).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Name, v => v.lineName.Text).DisposeWith(d);
            });
        }
    }
}

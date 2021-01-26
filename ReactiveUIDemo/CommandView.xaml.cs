using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
    /// CommandView.xaml 的交互逻辑
    /// </summary>
    public partial class CommandView
    {
        public CommandView()
        {
            InitializeComponent();
            ViewModel = new CommandViewModel();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.TextContent, v => v.tb1.Text).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.GenerateList1Command, v => v.btn2).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.List1, v => v.lb1.ItemsSource).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ButtonCommand, v => v.btn1).DisposeWith(d);

                //btn1.Events().MouseDown.Select(x => Unit.Default).InvokeCommand(ViewModel.ButtonCommand);
            });
        }
    }
}

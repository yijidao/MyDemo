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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<AppViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new AppViewModel();

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                        viewModel => viewModel.IsAvailable,
                        view => view.searchResultsListBox.Visibility) // 这里使用了一个默认的转换器，bool 转 Visibility
                    .DisposeWith(disposableRegistration);


                // ReactiveUI 绑定 ItemsControl 的操作很骚，注意一下
                // 当在 XAML 平台上使用 Reactive Binding 时，如果没有设置 ItemTemplate，它将在我们的依赖注入中查找 IViewFor < t > 并使用该控件显示条目
                this.OneWayBind(ViewModel,
                        viewModel => viewModel.SearchResults,
                        view => view.searchResultsListBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                        viewModel => viewModel.SearchTerm,
                        view => view.searchTextBox.Text)
                    .DisposeWith(disposableRegistration);
            });
        }
    }
}

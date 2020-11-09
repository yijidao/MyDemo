using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Security.Policy;
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
    /// NuGetDetailsView.xaml 的交互逻辑
    /// </summary>
    public partial class NuGetDetailsView : ReactiveUserControl<NugetDetailsViewModel>
    {
        public NuGetDetailsView()
        {
            InitializeComponent();

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IconUri,
                    view => view.iconImage.Source,
                    url => url == null ? null : new BitmapImage(url))  // 这里手动将 uri 转换为 BitmapImage
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                        viewModel => viewModel.Title,
                        view => view.titleRun.Text)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                        viewModel => viewModel.Description,
                        view => view.descriptionRun.Text)
                    .DisposeWith(disposableRegistration);

                this.BindCommand(ViewModel,
                        viewModel => viewModel.OpenPage,
                        view => view.openButton)
                    .DisposeWith(disposableRegistration);

            });


        }
    }
}

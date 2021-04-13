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
    /// InteractionView.xaml 的交互逻辑
    /// </summary>
    public partial class InteractionView
    {
        public InteractionView()
        {
            InitializeComponent();
            ViewModel = new InteractionViewModel();

            this.WhenActivated(d =>
            {
                this.ViewModel.Confirm.RegisterHandler(async interation =>
                {
                    //var deleteIt = await this.DisplayAlert
                }).DisposeWith(d);

                
            });
        }
    }
}

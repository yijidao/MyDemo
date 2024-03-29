﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ReactiveUI;
using RxUIDemo2.ViewModels;

namespace RxUIDemo2.Views
{
    /// <summary>
    /// MainView2.xaml 的交互逻辑
    /// </summary>
    public partial class MainView2
    {
        public MainView2()
        {
            InitializeComponent();
            ViewModel = new MainViewModel2();
            
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.ClickCommand, v => v.click1, Observable.Return("click1")).DisposeWith(d);
                //this.BindCommand(ViewModel, vm => vm.ClickCommand, v => v.click1).DisposeWith(d);
            });
        }
    }
}

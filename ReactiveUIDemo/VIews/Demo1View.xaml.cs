﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using ReactiveUIDemo.ViewModels;

namespace ReactiveUIDemo.Views
{
    /// <summary>
    /// Demo1View.xaml 的交互逻辑
    /// </summary>
    public partial class Demo1View : UserControl, IDemo
    {
        public Demo1View()
        {
            InitializeComponent();

            DataContext = new Demo1ViewModel();
        }
    }
}

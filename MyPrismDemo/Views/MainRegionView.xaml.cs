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
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace MyPrismDemo.Views
{
    /// <summary>
    /// MainRegionView.xaml 的交互逻辑
    /// </summary>
    public partial class MainRegionView : UserControl
    {
        public MainRegionView()
        {
            InitializeComponent();

            var dialogService = ContainerLocator.Current.Resolve<IDialogService>();

            dialog.Click += (sender, args) =>
            {
                dialogService.ShowDialog(nameof(DialogView));
            };

            subdialog.Click += (sender, args) =>
            {
                dialogService.ShowDialog("subdialog");
            };

        }
    }
}

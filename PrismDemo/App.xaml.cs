using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using PrismDemo.Service;

namespace PrismDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITest, Test1>();
        }

        protected override Window CreateShell() => new MainWindow();

    }
}

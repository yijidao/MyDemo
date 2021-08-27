using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MyPrismDemo.Service;
using MyPrismDemo.Service.Impl;
using MyPrismDemo.ViewModels;
using MyPrismDemo.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace MyPrismDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<IRegionManager, RegionManager>();
            containerRegistry.RegisterForNavigation<MainRegionView>();
            containerRegistry.RegisterForNavigation<MoqView>();
            containerRegistry.RegisterForNavigation<DetailRegionView>();

            //containerRegistry.RegisterSingleton<ITestService, TestService>();
            containerRegistry.RegisterSingleton<ITestService>(() => new MockTestService(new TestService()));

            containerRegistry.RegisterDialog<DialogView, SubDialogViewModel>("subdialog");
            containerRegistry.RegisterDialog<DialogView>();


        }

        //protected override Window CreateShell() => new MainWindow2(); //new MainWindow();
        protected override Window CreateShell() => new MainWindow(); //new MainWindow();

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<VlcPrismModule.VlcPrismModuleModule>();
        }
    }
}

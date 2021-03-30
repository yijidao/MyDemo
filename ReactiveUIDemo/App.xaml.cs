using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using LibVLCSharp.Shared;
using Prism.Ioc;
using Prism.Unity;
using ReactiveUI;
using Splat;

namespace ReactiveUIDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            //this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            //this.StartupUri = new Uri("MainWindow2.xaml", UriKind.Relative);

            Core.Initialize();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override Window CreateShell() => new MainWindow2();
    }
}

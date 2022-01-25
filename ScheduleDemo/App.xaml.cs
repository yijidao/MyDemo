using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using ScheduleDemo.Services;

namespace ScheduleDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ITestService, TestService>();
            containerRegistry.Register<IScheduleService, ScheduleService>();
        }

        protected override Window CreateShell() => new MainWindow();
    }
}

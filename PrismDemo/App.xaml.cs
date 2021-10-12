using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DryIoc;
using Microsoft.Extensions.Caching.Memory;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using PrismAopModuleDemo;
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
            //containerRegistry.Intercept<ITest, ExceptionInterceptor>();
            //containerRegistry.Intercept<ITest, LoggingInterceptor>();
            //containerRegistry.InterceptAsync<ITest, AsyncMethodLogInterceptor>();

            containerRegistry.RegisterSingleton<IMemoryCache>(provider => new MemoryCache(new MemoryCacheOptions()));

            containerRegistry.InterceptAsync<ITest, CacheInterceptor>();
        }

        protected override Window CreateShell() => new MainWindow();

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.WinForms;
using Prism.Ioc;
using Prism.Unity;

namespace MyDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 对Winfrom控件启用系统自带得现代化样式
            System.Windows.Forms.Application.EnableVisualStyles();

            InitCefsharp();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IVideoService, VideoService>();
        }

        //protected override Window CreateShell()
        //{
        //    return new MainWindow();
        //}
        protected override Window CreateShell() => new MainWindow2();

        private void InitCefsharp()
        {
            //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"
            var settings = new CefSettings();
            settings.BrowserSubprocessPath = System.IO.Path.GetFullPath(@"x86\CefSharp.BrowserSubprocess.exe");
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }


    }
}

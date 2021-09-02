using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp3
{
    class Startup
    {
        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        static void Main()
        {
            //var app = new App();
            //app.MainWindow = new Window1();
            //app.MainWindow.Show();
            //app.Run();

            var domain = AppDomain.CreateDomain("first domain");
            var domain2 = AppDomain.CreateDomain("second domain");
            domain2.UnhandledException += (sender, args) =>
            {
                Debug.WriteLine("domain2 exception");
            };
            CrossAppDomainDelegate action = () =>
            {
                var thread = new Thread(() =>
                {
                    var app = new App {MainWindow = new Window1()};
                    app.MainWindow.Show();
                    app.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            };

            CrossAppDomainDelegate action2 = () =>
            {
                var thread = new Thread(() =>
                {
                    var app = new App { MainWindow = new Window2() };

                    app.DispatcherUnhandledException += (sender, args) =>
                    {
                        //args.Handled = true;
                        Debug.WriteLine("DispatcherUnhandledException");
                    };
                    
                    app.MainWindow.Show();
                    app.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            };
            domain.DoCallBack(action);
            domain2.DoCallBack(action2);

            var a = new App
            {
                MainWindow = new Window1()
            };
            a.MainWindow.Show();
            a.Run();

        }
    }
}

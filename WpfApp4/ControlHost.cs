using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp4
{
    class ControlHost : HwndHost
    {
        const int WS_CHILD = 0x40000000;

        public IntPtr Handle { get; set; }
        

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            throw new Exception();
            //var p = new HwndSourceParameters("usercontrol1")
            //{
            //    ParentWindow = hwndParent.Handle,
            //    WindowStyle = WS_CHILD
            //};
            //var source = new HwndSource(p)
            //{
            //    RootVisual = _window
            //};
            //HwndSource
            //return new HandleRef(this, source.Handle);
        }


        //protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        //{
        //    var p = new HwndSourceParameters("usercontrol1")
        //    {
        //        ParentWindow = hwndParent.Handle,
        //        WindowStyle = WS_CHILD,
        //    };
        //    var source = new HwndSource(p) { RootVisual = new UserControl1() };


        //    return new HandleRef(this, source.Handle);
        //}

        //protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        //{
        //    var domain = AppDomain.CreateDomain("domain1");
        //    var wrapElement = (WrapElement)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "WpfApp4.WrapElement");

        //    domain.DoCallBack(() =>
        //    {
        //        var thread = new Thread(() =>
        //        {
        //            var app = new App { MainWindow = new Window2() };

        //            app.DispatcherUnhandledException += (sender, args) =>
        //            {
        //                //args.Handled = true;
        //                Debug.WriteLine("DispatcherUnhandledException");
        //            };

        //            //app.MainWindow.Show();
        //            app.Run();
        //        });
        //        thread.SetApartmentState(ApartmentState.STA);
        //        thread.Start();
        //    });

        //    domain.UnhandledException += (sender, args) =>
        //    {

        //    };
        //    return new HandleRef(this, wrapElement.GetUserControl1(hwndParent.Handle));
        //}

        //protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        //{
        //    //Process.Start()
        //}



        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            //throw new NotImplementedException();
        }
    }
}

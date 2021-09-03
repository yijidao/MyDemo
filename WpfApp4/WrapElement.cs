using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp4
{
    class WrapElement : MarshalByRefObject
    {
        const int WS_CHILD = 0x40000000;


        private Window _window;
        public IntPtr GetUserControl1(IntPtr hwndParent)
        {
            //_window = new Window1();
            ////window.WindowState = WindowState.Maximized;
            //_window.WindowStyle = WindowStyle.None;
            //_window.Content = new UserControl1();
            //_window.Show();
            //var helper= new WindowInteropHelper(_window);
            //return helper.Handle;

            var p = new HwndSourceParameters("usercontrol1")
            {
                ParentWindow = hwndParent,
                WindowStyle = WS_CHILD,
            };
            var source = new HwndSource(p) { RootVisual = new UserControl1() };
            return source.Handle;
        }

        
    }
}

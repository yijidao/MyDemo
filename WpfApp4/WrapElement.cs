using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp4
{
    class WrapElement : MarshalByRefObject
    {
        private Window _window;
        public IntPtr GetUserControl1()
        {
            _window = new Window1();
            //window.WindowState = WindowState.Maximized;
            _window.WindowStyle = WindowStyle.None;
            _window.Content = new UserControl1();
            _window.Show();
            var helper= new WindowInteropHelper(_window);
            return helper.Handle;
        }

        //public void Show() => _window.Show();

        //public Application GetApp()
        //{

        //}


        //public Application GetApp2()
        //{

        //}
    }
}

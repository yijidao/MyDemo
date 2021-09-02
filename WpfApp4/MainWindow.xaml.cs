using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("user32.dll")]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]

        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);
        public MainWindow()
        {
            InitializeComponent();



            //var domain = AppDomain.CreateDomain("first domain");
            //var domain2 = AppDomain.CreateDomain("second domain");
            //domain2.UnhandledException += (sender, args) =>
            //{

            //    Debug.WriteLine("domain2 exception");
            //};
            //CrossAppDomainDelegate action = () =>
            //{
            //    var thread = new Thread(() =>
            //    {
            //        var app = new App { MainWindow = new Window1() };
            //        app.MainWindow.Show();
            //        app.Run();
            //    });
            //    thread.SetApartmentState(ApartmentState.STA);
            //    thread.Start();
            //};

            //CrossAppDomainDelegate action2 = () =>
            //{
            //    var thread = new Thread(() =>
            //    {
            //        var app = new App { MainWindow = new Window2() };

            //        app.DispatcherUnhandledException += (sender, args) =>
            //        {
            //            //args.Handled = true;
            //            Debug.WriteLine("DispatcherUnhandledException");
            //        };

            //        app.MainWindow.Show();
            //        app.Run();
            //    });
            //    thread.SetApartmentState(ApartmentState.STA);
            //    thread.Start();
            //};
            //domain.DoCallBack(action);
            //domain2.DoCallBack(action2);
            //CompositionContainer
            //var i = new WindowInteropHelper(this).Handle;

            //var wrapElement = (WrapElement)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "WpfApp4.WrapElement");
            //wrapElement.GetUserControl1();
            //var tabItem = GetTabItem(Assembly.GetExecutingAssembly().FullName, "WpfApp4.WrapElement");
            //tabControl.Items.Add(tabItem);
            //GetTabItem(Assembly.GetExecutingAssembly().FullName, "WpfApp4.WrapElement");
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            GetTabItem(Assembly.GetExecutingAssembly().FullName, "WpfApp4.WrapElement");

            //var tabItem = GetTabItem(Assembly.GetExecutingAssembly().FullName, "WpfApp4.WrapElement");
            //tabControl.Items.Add(tabItem);
        }

        private void GetTabItem(string assemblyName, string typeName)
        {
            //var host = new WindowsFormsHost();
            //var panel = new System.Windows.Forms.Panel();
            //panel.Width = (int) ActualWidth;
            //panel.Height = (int) ActualHeight;
            var domain = AppDomain.CreateDomain($"{assemblyName}");
            domain.UnhandledException += (sender, args) =>
            {

            };
            
            var wrapElement = (WrapElement)domain.CreateInstanceAndUnwrap(assemblyName, typeName);
            var handel = wrapElement.GetUserControl1();
            //host.Child = panel;
            SetParent(handel, panel.Handle);
            //MoveWindow(handel, 0, 0, 400, 400, true);
            MoveWindow(handel, 0, 0, panel.Width, panel.Height, true);
            

            //var tabItem = new TabItem();
            //tabItem.Content = host;
            //tabControl.Items.Add(tabItem);
            //tabItem.IsSelected = true;
            //wrapElement.Show();
            //return tabItem;
        }

    }
}

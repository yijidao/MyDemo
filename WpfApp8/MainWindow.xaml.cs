using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary2;

namespace WpfApp8
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Process> _processes = new List<Process>();

        public MainWindow()
        {
            InitializeComponent();

            plugin1.Click += (sender, args) =>
            {
                var loader = new PluginLoader();
                var path = $@"{Directory.GetCurrentDirectory()}\WpfControlLibrary1.dll";
                var p = loader.LoadPlugin(path);
                _processes.Add(p);
                var control = loader.GetView();

                border1.Child = control;
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _processes.Where(x => !x.HasExited).ToList().ForEach(x => x.Kill());
            base.OnClosing(e);
        }
    }
}

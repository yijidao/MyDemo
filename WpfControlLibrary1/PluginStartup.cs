using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlLibrary1
{
    public class PluginStartup
    {
        public FrameworkElement CreateView()
        {
            return new UserControl1()
            {
            };
        }
    }
}

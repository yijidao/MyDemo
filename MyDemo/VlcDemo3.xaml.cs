using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MyDemo
{
    /// <summary>
    /// VlcDemo3.xaml 的交互逻辑
    /// </summary>
    public partial class VlcDemo3 : UserControl
    {
        public VlcDemo3()
        {
            InitializeComponent();
            Loaded += (sender, args) => panelView.Play(new Tuple<int, long[]>(10, new long[] { 1, 2, 3, 4 }));
            //panelView.Play(new Tuple<int, long[]>(10, new long[] { 1, 2, 3, 4 }));
        }
    }
}

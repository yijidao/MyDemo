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
    /// MyWrapPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class MyWrapPanelView : UserControl
    {
        public MyWrapPanelView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.Source;


            MyWrapPanel.SetLineBreakBefore(button, !MyWrapPanel.GetLineBreakBefore(button));

        }
    }
}

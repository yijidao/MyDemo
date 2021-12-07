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
    /// VlcDemo2.xaml 的交互逻辑
    /// </summary>
    public partial class VlcDemo2 : UserControl
    {
        public VlcDemo2()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                videoPlayerView.Play(@"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4");
            };
            Unloaded += (sender, args) =>
            {
                videoPlayerView.Stop();
            };
        }


    }
}

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

namespace VlcPrismModule.Views
{
    /// <summary>
    /// MonitorView2.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorView2 : UserControl
    {
        public string[] Datas { get; set; } = new[]
            {"Item1", "Item2", "Item3", "Item4", "Item5", "Item6", "Item7", "Item8", "Item9"};

        public MonitorView2()
        {
            InitializeComponent();

            //itemsControl1.ItemsSource = Datas;

            itemsControl1.ItemsSource = new MediaPlayerView[]
            {
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView(), 
                new MediaPlayerView()
            };

            Unloaded += (sender, args) =>
            {

            };
        }
    }
}

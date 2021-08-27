using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
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
using Windows.Media.Core;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //mediaPlayerElement.Source =
            //mediaPlayerElement.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri("http://1011.hlsplay.aodianyun.com/demo/game.flv"));


            //D:\佳都\Code\dev\HJMos_NCC_Client\Work\Videos

            //var files = Directory.GetFiles(@"D:\佳都\Code\dev\HJMos_NCC_Client\Work\Videos", "*.mp4");

            //Observable.Interval(TimeSpan.FromSeconds(5))
            //    .Select(x => x % files.Length)
            //    .ObserveOnDispatcher()
            //    .Subscribe(value =>
            //    {
            //        mediaPlayerElement.Source = files[value];

            //    });



        }
    }
}

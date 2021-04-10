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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibVLCSharp.Shared;
using MediaPlayer = System.Windows.Media.MediaPlayer;

namespace ReactiveUIDemo.Views
{
    /// <summary>
    /// MediaView.xaml 的交互逻辑
    /// </summary>
    public partial class MediaView : UserControl, IDemo
    {
        static MediaView()
        {
            Core.Initialize();
        }
        public MediaView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
            
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var libVlc = new LibVLC(enableDebugLogs: true);
            videoView.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(libVlc);
            using (var media = new Media(libVlc, new Uri("rtmp://58.200.131.2:1935/livetv/hunantv")))
            {
                videoView.MediaPlayer.Play(media);
            }
        }
    }
}

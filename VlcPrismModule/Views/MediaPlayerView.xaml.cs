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
using ClassLibrary1;
using LibVLCSharp.Shared;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace VlcPrismModule.Views
{
    /// <summary>
    /// MediaPlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class MediaPlayerView : UserControl
    {

        static MediaPlayerView()
        {
            Core.Initialize();
        }

        public MediaPlayerView()
        {
            InitializeComponent();
            var class1 = new Class1();
            class1.Test();
            var source = new[]
            {
                "rtmp://58.200.131.2:1935/livetv/gxtv",
                "rtmp://58.200.131.2:1935/livetv/hunantv",
                "http://1011.hlsplay.aodianyun.com/demo/game.flv"

            };

            var index = 0;
            var libVlc = new LibVLC(enableDebugLogs: true);
            videoView.MediaPlayer = new MediaPlayer(libVlc);

            Background = Brushes.LightBlue;

            playButton.Click += (sender, args) =>
            {
                ValidateIndex();
                using (var media = new Media(libVlc, new Uri(source[index++])))
                {
                    videoView.MediaPlayer.Play(media);
                }
            };
            stopButton.Click += (sender, args) => videoView.MediaPlayer.Stop();

            IsVisibleChanged += (sender, args) =>
            {
                if (args.NewValue is bool visible && visible)
                {
                    if (videoView != null)
                    {

                    }
                }
            };

            playButton.IsVisibleChanged += (sender, args) =>
            {
                if (videoView != null)
                {

                }
            };

            void ValidateIndex() => index = index >= source.Length ? 0 : index;

            //Unloaded += (sender, args) =>
            //{
            //    videoView.MediaPlayer?.Stop();
            //    videoView.MediaPlayer?.Dispose();
            //    videoView.Dispose();
            //};
        }

    }
}

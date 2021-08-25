using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using LibVLCSharp.WPF;

namespace WpfApp1
{
    /// <summary>
    /// VlcDemo.xaml 的交互逻辑
    /// </summary>
    public partial class VlcDemo : UserControl
    {
        public VlcDemo()
        {
            InitializeComponent();

            Core.Initialize();
            // var libVlc = new LibVLC("--input-repeat=1");
            // var libVlc = new LibVLC("--input-repeat=1");

            var libVlc = new LibVLC();


            //for (var i = 0; i < 3; i++)
            //{
            //    grid.ColumnDefinitions.Add(new ColumnDefinition());
            //    grid.RowDefinitions.Add(new RowDefinition());
            //}

            var medias = new List<VideoView>();

            //for (int r = 0; r < 3; r++)
            //{
            //    for (int c = 0; c < 3; c++)
            //    {
            //        var libVlc = new LibVLC();

            //        var player = new MediaPlayer(libVlc);

            //        var videoView = new VideoView
            //        {
            //            MediaPlayer = player
            //        };

            //        videoView.MediaPlayer.Stopped += (sender, args) =>
            //        {
            //            ThreadPool.QueueUserWorkItem((state => player.Play()) );

            //        };
            //        //videoView.MediaPlayer.EndReached += MediaPlayerOnEndReached;
            //        Grid.SetRow(videoView, r);
            //        Grid.SetColumn(videoView, c);
            //        grid.Children.Add(videoView);
            //        medias.Add(videoView);
            //    }
            //}

            var urls = Directory.GetFiles(@"D:\\佳都\\Code\\dev\\HJMos_NCC_Client\\Work\\Videos", "*.ts");
            var queue = new Queue<string>(urls);
            var queue2 = new Queue<string>();
            var videoView = new VideoView();
            var player = new MediaPlayer(libVlc);
            videoView.MediaPlayer = player;

            player.Stopped += (sender, args) =>
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var url = queue.Dequeue();
                    queue2.Enqueue(url);
                    using (var media = new Media(libVlc, new Uri(url)))
                    {
                        player.Play(media);
                    }

                });
            };
            player.EndReached += (sender, args) =>
            {
                
            };
            grid.Children.Add(videoView);
            medias.Add(videoView);

            
            changeSource.Click += (sender, args) =>
            {
                medias.ForEach(x =>
                {
                    if (queue.Count == 0)
                    {
                        queue = queue2;
                        queue2 = new Queue<string>();
                    }
                    var libVlc = new LibVLC();

                    var url = queue.Dequeue();
                    queue2.Enqueue(url);
                    using (var media = new Media(libVlc, new Uri(url)))
                    {
                        x.MediaPlayer.Play(media);
                    }
                });
            };

            paush.Click += (sender, args) =>
            {
                medias.ForEach(x =>
                {
                    x.MediaPlayer.SetPause(true);
                });
            };
            resume.Click += (sender, args) =>
            {
                medias.ForEach(x =>
                {
                    x.MediaPlayer.SetPause(false);
                });
            };
            stop.Click += (sender, args) =>
            {
                medias.ForEach(x =>
                {
                    x.MediaPlayer.Stop();
                });
            };
        }

        private void MediaPlayerOnEndReached(object? sender, EventArgs e)
        {

        }

        private void MediaPlayerOnStopped(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

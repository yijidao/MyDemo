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
using System.Windows.Threading;
using LibVLCSharp.Shared;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace ReactiveUIDemo.Vlc
{
    /// <summary>
    /// MediaView.xaml 的交互逻辑
    /// </summary>
    public partial class MediaView : UserControl
    {
        public static readonly RoutedEvent SplitEvent = EventManager.RegisterRoutedEvent("Split",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaView));
        public event RoutedEventHandler Split
        {
            add => AddHandler(SplitEvent, value);
            remove => RemoveHandler(SplitEvent, value);
        }

        public VlcView ParentView { get; }

        private LibVLC LibVlcInstance { get; set; }

        private MediaPlayer MediaPlayerElement { get; set; }

        //public MediaView()
        //{
        //    InitializeComponent();
        //}

        public MediaView(VlcView parentView)
        {
            ParentView = parentView;

            InitializeComponent();

            parentView.Loaded += VlcViewOnLoaded;
            playButton.Click += PlayButtonOnClick;
            stopButton.Click += StopButtonOnClick;
            pauseButton.Click += PauseButtonOnClick;
            fullButton.Click += FullButtonOnClick;
            Unloaded += OnUnloaded;
            splitButton.Click += (_, __) =>
            {
                RaiseEvent(new RoutedEventArgs(MediaView.SplitEvent));
            };

            AllowDrop = true;

            MouseMove += (sender, args) =>
            {
                if (args.LeftButton != MouseButtonState.Pressed) return;
            };

            playButton.IsVisibleChanged += (sender, args) =>
            {

            };
            IsVisibleChanged += (sender, args) =>
            {

            };
        }

        private void FullButtonOnClick(object sender, RoutedEventArgs e)
        {
            MediaPlayerElement.ToggleFullscreen();
        }

        private void PauseButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (MediaPlayerElement.CanPause)
            {
                MediaPlayerElement.Pause();
            }
            else
            {
                MediaPlayerElement.Play();
            }
        }


        public string[] Sources { get; set; } = new[]
        {
            "rtmp://58.200.131.2:1935/livetv/gxtv",
            "rtmp://58.200.131.2:1935/livetv/hunantv",
            "http://1011.hlsplay.aodianyun.com/demo/game.flv",
        };


        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            MediaPlayerElement.Stop();
            MediaPlayerElement.Dispose();
            LibVlcInstance.Dispose();
        }

        private void StopButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (ParentView?.videoView?.MediaPlayer == null || !ParentView.videoView.MediaPlayer.IsPlaying) return;
            ParentView.videoView.MediaPlayer.Stop();
        }

        private void PlayButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (ParentView?.videoView?.MediaPlayer == null || ParentView.videoView.MediaPlayer.IsPlaying) return;
            using (var media = new Media(LibVlcInstance, new Uri(Sources[0])))
            {
                var retuslt = ParentView.videoView.MediaPlayer.Play(media);
            }
        }

        public void Play(string source)
        {
            //var result = false;
            try
            {
                //if (MediaPlayerElement.IsPlaying)
                //{
                //    MediaPlayerElement.Stop();
                //}
                MediaPlayerElement.Stop();

                if (!MediaPlayerElement.IsPlaying)
                {
                    using (var media = new Media(LibVlcInstance, new Uri(source)))
                    {
                        MediaPlayerElement.Play(media);

                    }
                }
                //if (MediaPlayerElement != null)
                //{
                //    StopMediaPlayer(MediaPlayerElement);
                //}

                //var player = CreateMediaPlayer(LibVlcInstance);
                //using (var media = new Media(LibVlcInstance, new Uri(source)))
                //{
                //    player.Play(media);

                //}

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
                System.Diagnostics.Debug.WriteLine(e);
            }
            //return result;
        }

        private void VlcViewOnLoaded(object sender, RoutedEventArgs e)
        {
            LibVlcInstance = new LibVLC(enableDebugLogs: true);
            //ParentView.videoView.MediaPlayer = CreateMediaPlayer(LibVlcInstance);
            MediaPlayerElement = new MediaPlayer(LibVlcInstance);
            ParentView.videoView.MediaPlayer = MediaPlayerElement;

            MediaPlayerElement.EnableMouseInput = true;
        }

        private MediaPlayer CreateMediaPlayer(LibVLC libVlc)
        {
            var mediaPlayer = new MediaPlayer(libVlc);
            ParentView.videoView.MediaPlayer = mediaPlayer;
            return mediaPlayer;
        }

        private void StopMediaPlayer(MediaPlayer player)
        {
            player.Stop();
            ParentView.videoView.MediaPlayer = null;
            player.Dispose();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

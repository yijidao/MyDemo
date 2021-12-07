using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibVLCSharp.Shared;
using Prism.Ioc;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;


namespace MyDemo
{
    /// <summary>
    /// VideoPlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class VideoPlayerView : UserControl
    {

        private static readonly LibVLC _libVlc;

        public static readonly RoutedEvent MouseClickEvent = EventManager.RegisterRoutedEvent(nameof(MouseClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoPlayerView));
        public static readonly RoutedEvent FullScreenClickEvent = EventManager.RegisterRoutedEvent(nameof(FullScreenClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoPlayerView));
        public static readonly RoutedEvent RestoreScreenClickEvent = EventManager.RegisterRoutedEvent(nameof(RestoreScreenClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoPlayerView));


        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public event RoutedEventHandler MouseClick
        {
            add => AddHandler(MouseClickEvent, value);
            remove => RemoveHandler(MouseClickEvent, value);
        }

        /// <summary>
        /// 全屏按钮点击事件
        /// </summary>
        public event RoutedEventHandler FullScreenClick
        {
            add => AddHandler(FullScreenClickEvent, value);
            remove => RemoveHandler(FullScreenClickEvent, value);
        }

        /// <summary>
        /// 还原按钮点击事件
        /// </summary>
        public event RoutedEventHandler RestoreScreenClick
        {
            add => AddHandler(RestoreScreenClickEvent, value);
            remove => RemoveHandler(RestoreScreenClickEvent, value);
        }

        /// <summary>
        /// 被选中时的边框颜色
        /// </summary>
        public static SolidColorBrush ActiveBrush { get; set; } = new SolidColorBrush(Color.FromRgb(18, 255, 243));

        /// <summary>
        /// 手动停止
        /// </summary>
        public bool ManualStop { get; set; }

        private readonly Subject<string> _subject = new Subject<string>();

        static VideoPlayerView()
        {
            Core.Initialize(); // 确保只执行一次
            _libVlc = new LibVLC();
        }

        public VideoPlayerView()
        {
            InitializeComponent();


            var player = new MediaPlayer(_libVlc);
            videoView.MediaPlayer = player;

            //Unloaded += (sender, args) =>
            //{
            //    Dispose();
            //};

            Observable.FromEventPattern<EventArgs>(player, nameof(MediaPlayer.Playing)).Select(_ => "play")
                .Merge(Observable.FromEventPattern<EventArgs>(player, nameof(MediaPlayer.Stopped))
                    .Select(_ => ManualStop ? "stop" : "error").Do(x =>
                    {
                        if (x == "error")
                        {
                            LogHelper.Error($"连接视频失败，url:{Url}");
                        }
                    }))
                .StartWith("stop")
                .Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged()
                .ObserveOnDispatcher()
                .Subscribe(value =>
                {
                    switch (value)
                    {
                        case "play":
                            stopTip.Visibility = Visibility.Collapsed;
                            faultTip.Visibility = Visibility.Collapsed;
                            break;
                        case "stop":
                            stopTip.Visibility = Visibility.Visible;
                            faultTip.Visibility = Visibility.Collapsed;
                            ManualStop = false;
                            break;
                        case "error":
                            stopTip.Visibility = Visibility.Collapsed;
                            faultTip.Visibility = Visibility.Visible;
                            break;
                    }
                }, ex => LogHelper.Error($"视频控件发生异常，{ex}"));

            Observable.FromEventPattern<RoutedEventArgs>(fullScreenButton, nameof(fullScreenButton.Click))
                .Select(_ => "full")
                .Merge(Observable
                    .FromEventPattern<RoutedEventArgs>(restoreScreenButton, nameof(restoreScreenButton.Click))
                    .Select(_ => "restore"))
                .StartWith("restore")
                .Throttle(TimeSpan.FromMilliseconds(200))
                .DistinctUntilChanged()
                .ObserveOnDispatcher()
                .Subscribe(value =>
                {
                    switch (value)
                    {
                        case "full":
                            fullScreenButton.Visibility = Visibility.Collapsed;
                            restoreScreenButton.Visibility = Visibility.Visible;
                            RaiseEvent(new RoutedEventArgs(FullScreenClickEvent, this));
                            break;
                        case "restore":
                            fullScreenButton.Visibility = Visibility.Visible;
                            restoreScreenButton.Visibility = Visibility.Collapsed;
                            RaiseEvent(new RoutedEventArgs(RestoreScreenClickEvent, this));
                            break;
                    }
                }, ex => LogHelper.Error($"视频控件发生异常，{ex}"));


            player.EndReached += (sender, args) => ThreadPool.QueueUserWorkItem(_ =>
            {
                using (var media = new Media(_libVlc, new Uri(Url)))
                {
                    player.Play(media);
                }
            });

            Observable.FromEventPattern<RoutedEventArgs>(backPanel, nameof(backPanel.MouseLeftButtonDown))
                .ObserveOnDispatcher()
                .Subscribe(_ =>
                {
                    videoView.BorderBrush = ActiveBrush;
                    RaiseEvent(new RoutedEventArgs(MouseClickEvent, this));
                }, ex => LogHelper.Error($"视频控件发生异常，{ex}"));

            _subject.Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged()
                .ObserveOnDispatcher()
                .Subscribe(value =>
                {
                    if (string.IsNullOrWhiteSpace(value) || value == "stop")
                    {
                        ManualStop = true;
                        player.Stop();
                    }
                    else
                    {
                        using (var media = new Media(_libVlc, new Uri(value)))
                        {
                            player.Play(media);
                        }
                    }
                }, ex => LogHelper.Error($"视频控件发生异常，{ex}"));
        }

        public void HideBorder() => videoView.BorderBrush = Brushes.Transparent;

        private string Url { get; set; }

        public void Play(string url)
        {
            Url = url;
            _subject.OnNext(url);
        }

        //public async Task PlayForId(long id)
        //{
        //    var result = await ContainerLocator.Container.Resolve<IVideoService>()?.PlayVideo(id);
        //    var url = result.FirstOrDefault()?.Url;
        //    Play(url);
        //}

        public void Replay() => Play(Url);

        public void Stop() => _subject.OnNext("stop");


        //public void Dispose()
        //{
        //    //_libVlc?.Dispose();
        //    videoView?.Dispose();
        //    Debug.WriteLine("------  释放摄像头资源  ------");
        //}
    }


    static class LogHelper{
        public static void Error(string ex)
        {
            Debug.WriteLine(ex);
        }
    }
}

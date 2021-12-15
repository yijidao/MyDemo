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
    public partial class VideoPlayerView : UserControl, IDisposable
    {

        internal static readonly LibVLC _libVlc;

        public static readonly RoutedEvent MouseClickEvent = EventManager.RegisterRoutedEvent(nameof(MouseClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoPlayerView));
        public static readonly RoutedEvent FullScreenClickEvent = EventManager.RegisterRoutedEvent(nameof(FullScreenClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoPlayerView));
        public static readonly RoutedEvent RestoreScreenClickEvent = EventManager.RegisterRoutedEvent(nameof(RestoreScreenClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoPlayerView));

        private readonly Subject<string> _subject = new Subject<string>();

        /// <summary>
        /// 被选中时的边框颜色
        /// </summary>
        public static SolidColorBrush ActiveBrush { get; set; } = new SolidColorBrush(Color.FromRgb(18, 255, 243));

        /// <summary>
        /// 手动停止
        /// </summary>
        private bool ManualStop { get; set; }

        private string Url { get; set; }

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

        static VideoPlayerView()
        {
            Core.Initialize(); // 确保只执行一次
            _libVlc = new LibVLC();
        }

        private MediaPlayer _player;

        public VideoPlayerView()
        {
            InitializeComponent();

            _player = new MediaPlayer(_libVlc);

            //var _player = new MediaPlayer(_libVlc);
            videoView.MediaPlayer = _player;

            Observable.FromEventPattern<EventArgs>(_player, nameof(MediaPlayer.Playing)).Select(_ => "play")
                .Merge(Observable.FromEventPattern<EventArgs>(_player, nameof(MediaPlayer.Stopped))
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


            _player.EndReached += (sender, args) => ThreadPool.QueueUserWorkItem(_ =>
            {
                using (var media = new Media(_libVlc, new Uri(Url)))
                {
                    _player.Play(media);
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
                        Task.Run(() => _player.Stop()); 
                    }
                    else
                    {
                        using (var media = new Media(_libVlc, new Uri(value)))
                        {
                            _player.Play(media);
                        }
                    }
                }, ex => LogHelper.Error($"视频控件发生异常，{ex}"));


            IsVisibleChanged += (sender, args) =>
            {
                if (args.NewValue is bool value)
                {
                    backPanel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            };
        }

        public void HideBorder() => videoView.BorderBrush = Brushes.Transparent;

        /// <summary>
        /// 直接传入 url  进行播放
        /// </summary>
        /// <param name="url"></param>
        public void Play(string url)
        {
            Url = url;
            _subject.OnNext(url);
        }

        /// <summary>
        /// 传入 id 进行播放，这个方法只是为了向后兼容才添加的，建议还是调用传入 url 的方法进行播放，这样控件比较纯粹，不会涉及业务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public async Task PlayForId(long id)
        //{
        //    var result = await ContainerLocator.Container.Resolve<IVideoService>()?.PlayVideo(id);
        //    var url = result.FirstOrDefault()?.Url;
        //    Play(url);
        //}

        /// <summary>
        /// 重播上次播放的视频
        /// </summary>
        public void Replay() => Play(Url);

        /// <summary>
        /// 停止视频，建议视频控件在用不到的情况下就停止播放，这样节省内存
        /// </summary>
        public void Stop() => _subject.OnNext("stop");

        #region IDispose

        private bool _dispose = false;

        //public void Dispose()
        //{

        //    if (_dispose) return;
        //    _dispose = true;
        //    videoView.MediaPlayer?.Stop();
        //    videoView.Dispose();
        //    _subject.Dispose();
        //    //Debug.WriteLine("------  释放摄像头资源  ------");
        //}
        public void Dispose()
        {

            if (_dispose) return;
            _dispose = true;

            Task.Run(() => _player.Stop());
            videoView.Dispose();
            _subject.Dispose();

            //Debug.WriteLine("------  释放摄像头资源  ------");
        }

        #endregion



    }

    static class LogHelper
    {
        public static void Error(string ex)
        {
            Debug.WriteLine(ex);
        }
    }
}

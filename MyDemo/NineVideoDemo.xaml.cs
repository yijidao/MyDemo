using System;
using System.Collections.Generic;
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
using ImTools;
using Prism.Ioc;

namespace MyDemo
{
    /// <summary>
    /// NineVideoDemo.xaml 的交互逻辑
    /// </summary>
    public partial class NineVideoDemo : UserControl
    {
        private readonly Dictionary<VideoPlayerView, List<PlayVideoResponse>> _playerDic = new Dictionary<VideoPlayerView, List<PlayVideoResponse>>();


        public NineVideoDemo()
        {
            InitializeComponent();

            Loaded += (sender, args) => InitEvent();

            //InitEvent();
            //Dialog.SetWindowStyle(this, (Style)Resources["CommonDialogStyle"]);
        }

        private void InitEvent()
        {
            EventManager.RegisterClassHandler(typeof(VideoPlayerView), VideoPlayerView.MouseClickEvent, new RoutedEventHandler(MediaPlayerViewOnMouseClick));
            EventManager.RegisterClassHandler(typeof(VideoPlayerView), VideoPlayerView.FullScreenClickEvent, new RoutedEventHandler(MediaPlayerViewOnFullScreenClick));
            EventManager.RegisterClassHandler(typeof(VideoPlayerView), VideoPlayerView.RestoreScreenClickEvent, new RoutedEventHandler(MediaPlayerViewOnRestoreScreenClick));

            Screens = new VideoPlayerView[9];

            for (var i = 0; i < Screens.Length; i++)
            {
                var player = new VideoPlayerView { Margin = new Thickness(6) };
                screenContain.Children.Add(player);
                Screens[i] = player;
                _playerDic.Add(player, null);
            }


            niceSplit.IsChecked = true; // 初始选中九分屏

            playButton.Checked += PlayAllPlayer;
            playButton.Unchecked += StopAllPlayer;

           

            Observable.FromEventPattern<RoutedEventArgs>(this, nameof(Loaded)).Select(_ => "loaded")
                .Merge(Observable.FromEventPattern<RoutedEventArgs>(this, nameof(Unloaded)).Select(_ => "unloaded"))
                .Throttle(TimeSpan.FromMilliseconds(400))
                .DistinctUntilChanged()
                //.Skip(1)
                .Subscribe(value =>
                {
                    switch (value)
                    {
                        case "loaded":
                            Screens.ForEach(x => x.Replay());
                            break;
                        case "unloaded":
                            Screens.ForEach(x => x.Stop());
                            //GC.Collect();
                            break;
                    }
                });


            foreach (var player in _playerDic)
            {
                player.Key.Play(@"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4");
            }
        }

        /// <summary>
        /// 选中的摄像头
        /// </summary>
        public VideoPlayerView SelectedPlayer
        {
            get => (VideoPlayerView)GetValue(SelectedPlayerProperty);
            set => SetValue(SelectedPlayerProperty, value);
        }

        public static readonly DependencyProperty SelectedPlayerProperty =
            DependencyProperty.Register("SelectedPlayer", typeof(VideoPlayerView), typeof(NineVideoDemo), new PropertyMetadata(null));

        private VideoPlayerView[] Screens { get; set; }

        /// <summary>
        /// 显示的摄像头数量
        /// </summary>
        public int ScreenCount { get; private set; }

        private void SplitScreenButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (Equals(e.OriginalSource, niceSplit))
            {
                NineSplitScreen();
            }
            else if (Equals(e.OriginalSource, fourSplit))
            {
                FourSplitScreen();
            }
            else if (Equals(e.OriginalSource, oneSplit))
            {
                OneSplitScreen();
            }
            Screens.FirstOrDefault()?.RaiseEvent(new RoutedEventArgs(VideoPlayerView.MouseClickEvent, Screens.FirstOrDefault()));
        }

        private void OneSplitScreen()
        {
            screenContain.Rows = 1;
            for (var i = 0; i < Screens.Length; i++)
            {
                Screens[i].Visibility = i == 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            ScreenCount = 1;
        }

        private void FourSplitScreen()
        {
            screenContain.Rows = 2;
            for (var i = 0; i < Screens.Length; i++)
            {
                Screens[i].Visibility = i < 4 ? Visibility.Visible : Visibility.Collapsed;
            }

            ScreenCount = 4;
        }

        private void NineSplitScreen()
        {
            screenContain.Rows = 3;
            foreach (var screen in Screens)
            {
                screen.Visibility = Visibility.Visible;
            }

            ScreenCount = 9;
        }

        

        /// <summary>
        /// 单击播放按钮，播放所有视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayAllPlayer(object sender, RoutedEventArgs e)
        {
            foreach (var player in Screens)
            {
                player.Replay();
            }
        }

        /// <summary>
        /// 单击暂停按钮，播放所有视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopAllPlayer(object sender, RoutedEventArgs e)
        {
            foreach (var player in Screens)
            {
                player.Stop();
            }
        }

        private void MediaPlayerViewOnMouseClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is VideoPlayerView playerView)
            {
                SelectedPlayer = playerView;
            }
            foreach (var screen in Screens.Where(x => !Equals(x, e.OriginalSource)))
            {
                screen.HideBorder();
            }
        }

        private void MediaPlayerViewOnFullScreenClick(object sender, RoutedEventArgs e)
        {
            if (SelectedPlayer == null)
                return;

            screenContain.Rows = 1;

            foreach (var screen in Screens.Where(x => !Equals(x, e.OriginalSource)))
            {
                screen.Visibility = Visibility.Collapsed;
            }
        }

        private void MediaPlayerViewOnRestoreScreenClick(object sender, RoutedEventArgs e)
        {
            switch (ScreenCount)
            {
                case 9:
                    NineSplitScreen();
                    break;
                case 4:
                    FourSplitScreen();
                    break;
                case 1:
                    OneSplitScreen();
                    break;
            }
        }
    }
}

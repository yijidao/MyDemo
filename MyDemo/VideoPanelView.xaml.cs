using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
using Prism.Ioc;

namespace MyDemo
{
    /// <summary>
    /// VideoPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class VideoPanelView : UserControl, IDisposable
    {
        private List<VideoPlayerView> Players { get; set; } = new List<VideoPlayerView>(4);

        private readonly Subject<Tuple<int, long[]>> _subject = new Subject<Tuple<int, long[]>>();
        private readonly Subject<IObservable<List<long>>> _idsSubject = new Subject<IObservable<List<long>>>();

        private List<PlayVideoResponse> Responses { get; set; }

        public VideoPanelView()
        {
            InitializeComponent();

            grid.Rows = 2;

            for (var r = 0; r < 2; r++)
            {
                for (var c = 0; c < 2; c++)
                {
                    var player = new VideoPlayerView
                    {
                        Margin = new Thickness(6)
                    };
                    //Grid.SetRow(player, r);
                    //Grid.SetColumn(player, c);
                    grid.Children.Add(player);
                    Players.Add(player);
                }
            }

            EventManager.RegisterClassHandler(typeof(VideoPlayerView), VideoPlayerView.MouseClickEvent, new RoutedEventHandler((
                (sender, args) =>
                {
                    foreach (var screen in Players.Where(x => !Equals(x, args.OriginalSource)))
                    {
                        screen.HideBorder();
                    }
                })));
            EventManager.RegisterClassHandler(typeof(VideoPlayerView), VideoPlayerView.FullScreenClickEvent, new RoutedEventHandler((
                (sender, args) =>
                {
                    grid.Rows = 1;
                    foreach (var screen in Players.Where(x => !Equals(x, args.OriginalSource)))
                    {
                        screen.Visibility = Visibility.Collapsed;
                    }
                })));
            EventManager.RegisterClassHandler(typeof(VideoPlayerView), VideoPlayerView.RestoreScreenClickEvent, new RoutedEventHandler((
                (sender, args) =>
                {
                    grid.Rows = 2;
                    foreach (var screen in Players)
                    {
                        screen.Visibility = Visibility.Visible;
                    }
                })));

            var loadOrUnload = Observable.FromEventPattern<RoutedEventArgs>(this, nameof(UserControl.Loaded))
                .Select(_ => "loaded")
                .Merge(Observable.FromEventPattern<RoutedEventArgs>(this, nameof(UserControl.Unloaded))
                    .Select(_ => "unloaded"))
                .Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged();

            loadOrUnload.ObserveOnDispatcher()
                .Skip(1)
                .Subscribe(value =>
                {
                    Players.ForEach(x =>
                    {
                        switch (value)
                        {
                            case "loaded":
                                x.Replay();
                                break;
                            case "unloaded":
                                x.Stop();
                                break;
                        }
                    });
                }, ex => LogHelper.Error($"视频面板发生异常，{ex}"));


            _idsSubject.Switch()
                .SkipUntil(loadOrUnload.Where(x => x == "loaded"))
                .TakeUntil(loadOrUnload.Where(x => x == "unloaded"))
                .Repeat()
                .ObserveOnDispatcher()
                .Subscribe(async x =>
                {
                    if (Responses?.Count > 0)
                    {
                        var stopVideoRequest = new StopVideoRequest
                        {
                            PlayList = Responses.Select(y => new PlayListItem
                            {
                                Url = y.Url,
                                CameraId = y.CameraId,
                                SessionId = y.SessionId
                            }).ToList()
                        };
                        await ContainerLocator.Container.Resolve<IVideoService>()?.StopVideo(stopVideoRequest);
                        Responses = null;
                    }


                    if (x.Count == 0)
                    {
                        Players.ForEach(player => player.Stop());
                        return;
                    }
                    var result = await ContainerLocator.Container.Resolve<IVideoService>()?.PlayVideo(x.ToArray());
                    Responses = result;
                    for (var i = 0; i < Players.Count; i++)
                    {
                        if (result.Count > i)
                        {
                            Players[i].Play(result[i].Url);
                        }
                        else
                        {
                            Players[i].Stop();
                        }
                    }
                }, ex => LogHelper.Error($"视频面板发生异常，{ex}"));

            _subject.Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged(x =>
                {
                    var builder = new StringBuilder();
                    var (interval, ids) = x;
                    builder.Append(interval);

                    foreach (var id in ids)
                    {
                        builder.Append(id);
                    }

                    //ids.ForEach(y => builder.Append(y));
                    return builder.ToString();
                })
                .Select(x =>
                {
                    var (interval, ids) = x;
                    var o = Observable.Interval(TimeSpan.FromSeconds(interval))
                        .Select(tick => tick + 1)
                        .StartWith(0)
                        .Select(tick =>
                        {
                            if (ids.Length == 0)
                            {
                                return new List<long>();
                            }

                            var start = (4 * tick) % ids.Length;
                            var end = start + 4 > ids.Length ? ids.Length : start + 4;
                            var list = new List<long>(4);

                            for (var i = start; i < end; i++)
                            {
                                list.Add(ids[i]);
                            }
                            return list;
                        });
                    return o;
                })
                .Subscribe(x =>
                {
                    _idsSubject.OnNext(x);
                }, ex => LogHelper.Error($"视频面板发生异常，{ex}"));

        }



        public Tuple<int, long[]> DeviceInfos
        {
            get => (Tuple<int, long[]>)GetValue(DeviceInfosProperty);
            set => SetValue(DeviceInfosProperty, value);
        }

        public static readonly DependencyProperty DeviceInfosProperty =
            DependencyProperty.Register("DeviceInfos", typeof(Tuple<int, long[]>), typeof(VideoPanelView), new PropertyMetadata(null,
                (o, args) =>
                {
                    if (args.NewValue == null) return;
                    //((VideoPanelView)o).Play((Tuple<int, long[]>)args.NewValue);
                }));



        public void Play(int interval, long[] ids) => _subject.OnNext(new Tuple<int, long[]>(interval, ids));

        public void Play(Tuple<int, long[]> value) => _subject.OnNext(value);

        public void Dispose()
        {
            _subject?.Dispose();
            _idsSubject?.Dispose();

        }
    }

    public interface IVideoService
    {
        Task StopVideo(StopVideoRequest stopVideoRequest);
        Task<List<PlayVideoResponse>> PlayVideo(long[] toArray);
    }

    class VideoService : IVideoService
    {
        public Task StopVideo(StopVideoRequest stopVideoRequest)
        {
            return Task.FromResult("ok");
        }

        public Task<List<PlayVideoResponse>> PlayVideo(long[] toArray)
        {
            return Task.FromResult(toArray.Select(x => new PlayVideoResponse
            {
                CameraId = x,
                SessionId = string.Empty,
                Url = @"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4"

            }).ToList());

            //return Task.FromResult(new List<PlayVideoResponse>
            //{
            //    new PlayVideoResponse
            //    {
            //        CameraId = 0,
            //        SessionId = "",
            //        Url = @"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4"
            //    }
            //});
        }
    }

    public class PlayVideoResponse
    {
        //[JsonProperty("cameraId")]
        public long CameraId { get; set; }

        //[JsonProperty("sessionId")]
        public string SessionId { get; set; }

        //[JsonProperty("url")]
        public string Url { get; set; }
    }

    public class StopVideoRequest
    {
        //[JsonProperty("playList")]
        public List<PlayListItem> PlayList { get; set; }
    }

    public class PlayVideoRequest
    {
        //[JsonProperty("playList")]
        public List<PlayListItem> PlayList { get; set; }
    }

    public class PlayListItem
    {
        //[JsonProperty("cameraId")]
        public long CameraId { get; set; }

        //[JsonProperty("sessionId")]
        public string SessionId { get; set; }

        public string Url { get; set; }
    }
}

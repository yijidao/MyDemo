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

namespace ReactiveUIDemo.Vlc
{
    /// <summary>
    /// VlcView.xaml 的交互逻辑
    /// </summary>
    public partial class VlcView : UserControl
    {
        public MediaView MediaPlayerElement { get; set; }

        public VlcView()
        {
            InitializeComponent();
            MediaPlayerElement = new MediaView(this);
            videoView.Content = MediaPlayerElement;
            Unloaded += OnUnloaded;

            MediaPlayerElement.Split += (sender, args) => RaiseEvent(new RoutedEventArgs(VlcView.SplitEvent));
        }

        public static readonly RoutedEvent SplitEvent = EventManager.RegisterRoutedEvent("Split",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VlcView));

        public event RoutedEventHandler Split
        {
            add => AddHandler(SplitEvent, value);
            remove => RemoveHandler(SplitEvent, value);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            videoView.Dispose();
        }

        private void VideoView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

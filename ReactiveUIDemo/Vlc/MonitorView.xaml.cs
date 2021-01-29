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
    /// MonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorView : UserControl
    {
        public MonitorView()
        {
            InitializeComponent();

            sourceListBox.ItemsSource = new[]
            {
                "rtmp://58.200.131.2:1935/livetv/gxtv",
                "rtmp://58.200.131.2:1935/livetv/hunantv",
                "http://1011.hlsplay.aodianyun.com/demo/game.flv"
            };
            sourceListBox.SelectionChanged += SourceListBoxOnSelectionChanged;


            AddHandler(VlcView.SplitEvent, new RoutedEventHandler((sender, e) =>
            {

            }));
        }

        private void SourceListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sourceListBox.SelectedItem?.ToString())) return;
            foreach (VlcView vlcView in mediaPlays.Children)
            {
                if (vlcView.MediaPlayerElement is MediaView mediaView)
                {
                    mediaView.Play(sourceListBox.SelectedItem.ToString());
                }
            }
        }
    }
}

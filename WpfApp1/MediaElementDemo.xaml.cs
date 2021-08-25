using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// MediaElementDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MediaElementDemo : UserControl
    {
        public MediaElementDemo()
        {
            InitializeComponent();

            for (var i = 0; i < 3; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }

            var medias = new List<MediaElement>();

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    var media = new MediaElement();
                    media.LoadedBehavior = MediaState.Manual;
                    media.UnloadedBehavior = MediaState.Manual;
                    Grid.SetRow(media, r);
                    Grid.SetColumn(media, c);
                    grid.Children.Add(media);
                    medias.Add(media);
                }
            }
            var urls = Directory.GetFiles(@"D:\\佳都\\Code\\dev\\HJMos_NCC_Client\\Work\\Videos", "*.ts");
            var queue = new Queue<string>(urls);
            var queue2 = new Queue<string>();

            changeSource.Click += (sender, args) =>
            {
                medias.ForEach(x =>
                {
                    if (x.Source != null)
                    {
                        x.Stop();
                        queue2.Enqueue(x.Source.AbsolutePath);
                    }

                    if (queue.Count == 0)
                    {
                        queue = queue2;
                        queue2 = new Queue<string>();
                    }

                    var url = queue.Dequeue();

                    x.Source = new Uri(url);
                    x.Play();
                });
            };
        }
    }
}

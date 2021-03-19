using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDemo
{
    /// <summary>
    /// MediaDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MediaDemo 
    {
        public MediaDemo()
        {
            InitializeComponent();
            media.LoadedBehavior = MediaState.Manual;
            media.MediaEnded += MediaOnMediaEnded;
        }

        private void MediaOnMediaEnded(object sender, RoutedEventArgs e)
        {
            media.Stop();
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = @"MP3 files (*.mp3)|*.mp3";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Path = dialog.FileName;
                    
                }
            }
        }

        public string Path { get; set; }

        private void Play_OnClick(object sender, RoutedEventArgs e)
        {
            media.Source = new Uri(Path);
            media.Play();
        }

        private void Stop_OnClick(object sender, RoutedEventArgs e)
        {
            media.Stop();
            //media.
        }
    }
}

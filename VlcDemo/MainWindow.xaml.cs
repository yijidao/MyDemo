using System;
using System.Windows;
using LibVLCSharp.Shared;

namespace VlcDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeComponent();

            Core.Initialize();
            //using (var libvlc = new LibVLC(enableDebugLogs:true))
            //{
            //    using (var media = new Media(libvlc, new Uri(@"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4")))
            //    {
            //        using (var player = new MediaPlayer(media))
            //        {
            //            player.Play();
            //        }
            //    }
            //}

            var libvlc = new LibVLC();
            videoView.MediaPlayer = new MediaPlayer(libvlc);
            //using (var media = new Media(libvlc, new Uri(@"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4")))
            //{
            //    videoView.MediaPlayer.Play(media);
            //}
            Loaded += (sender, args) =>
            {
                using (var media = new Media(libvlc,
                           new Uri(
                               @"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4")))
                {
                    videoView.MediaPlayer.Play(media);
                }
            };
        }

    }
}

using System;
using System.Diagnostics;
using System.Windows.Controls;
using LibVLCSharp.Shared;

namespace MyDemo
{
    /// <summary>
    /// VlcDemo.xaml 的交互逻辑
    /// </summary>
    public partial class VlcDemo : UserControl
    {
        //private static readonly  LibVLC _libVlc;
        private readonly LibVLC _libVlc;


        static VlcDemo()
        {
            Core.Initialize();
            //_libVlc = new LibVLC();
        }

        public VlcDemo()
        {
            //C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos
            InitializeComponent();

            _libVlc = new LibVLC();
            


            videoView.MediaPlayer = new MediaPlayer(_libVlc);


            Loaded += (sender, args) =>
            {
                using (var media = new Media(_libVlc,
                           new Uri(
                               @"C:\pci\HJMos_NCC_Client\HJMos_NCC_Client_V1.0\src\Hjmos.Ncc.WSV1\bin\Debug\Videos\01.mp4")))
                {
                    videoView.MediaPlayer.Play(media);
                }
            };

            Unloaded += (sender, args) =>
            {
                videoView.MediaPlayer.Stop(); // 如果播放了视频，需要调用 stop()，不然内存无法释放
                Debug.WriteLine("unload");
                //videoView.Dispose();
                GC.Collect();
            };
        }
    }
}

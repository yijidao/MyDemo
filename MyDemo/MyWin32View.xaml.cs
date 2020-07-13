using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace MyDemo
{
    /// <summary>
    /// MyWin32View.xaml 的交互逻辑
    /// </summary>

    public partial class MyWin32View : UserControl
    {
        public struct POINT
        {
            public int X { get; set; }
            public int Y { get; set; }

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public MyWin32View()
        {
            InitializeComponent();

            var timer = new DispatcherTimer();
            timer.Tick += GetMousePosition;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Start();

            void GetMousePosition(object sender, EventArgs e)
            {
                GetCursorPos(out POINT position);
                CursorPosition.Text = $"x: {position.X}, y: {position.Y}";
                var color = GetColorAt(new Point { X = position.X, Y = position.Y });
                ColorBrush.Color = System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
                HexColor.Text = $"#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";
            }
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT point);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        public System.Drawing.Color GetColorAt(Point location)
        {
            using (var gdest = Graphics.FromImage(screenPixel))
            using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hSrcDC = gsrc.GetHdc();
                IntPtr hDC = gdest.GetHdc();
                int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                gdest.ReleaseHdc();
                gsrc.ReleaseHdc();
            }
            return screenPixel.GetPixel(0, 0);
        }

    }
}

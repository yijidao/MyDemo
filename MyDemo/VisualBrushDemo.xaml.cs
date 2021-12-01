using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MyDemo
{
    /// <summary>
    /// VisualBrushDemo.xaml 的交互逻辑
    /// </summary>
    public partial class VisualBrushDemo : UserControl
    {
        public VisualBrushDemo()
        {
            InitializeComponent();

            var p = default(Point);
            var maxXOffset = canvas.Width - slide.Width;
            Observable.FromEventPattern(slide, nameof(slide.MouseMove))
                .SkipUntil(Observable.FromEventPattern(slide, nameof(slide.MouseLeftButtonDown)).Do(_ => p = Mouse.GetPosition(slide)))
                .TakeUntil(Observable.FromEventPattern(slide, nameof(slide.MouseLeftButtonUp)))
                .Repeat()
                .Subscribe(e =>
                {
                    var p2 = Mouse.GetPosition(canvas);
                    var left = p2.X - p.X;
                    if (left < 0) left = 0;
                    if (left > maxXOffset) left = maxXOffset;

                    Canvas.SetLeft(slide, left);

                    var offset = content.Width / canvas.Width * left;

                    scrollViewer.ScrollToHorizontalOffset(offset);
                });





        }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Debug.WriteLine($"HS: {e.HorizontalOffset}");
            //Debug.WriteLine($"{e.ViewportWidth}");
            //Debug.WriteLine($"{e.ExtentWidth}");
            //e.HorizontalOffset / e.
            //var hs = e.HorizontalOffset;
            var offset = slide.Width / e.ViewportWidth * e.HorizontalOffset;
            //Debug.WriteLine(offset);
            Canvas.SetLeft(slide, offset);

        }
    }
}

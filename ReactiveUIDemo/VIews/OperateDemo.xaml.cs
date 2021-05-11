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

namespace ReactiveUIDemo.Views
{
    /// <summary>
    /// OperateDemo.xaml 的交互逻辑
    /// </summary>
    public partial class OperateDemo : UserControl, IDemo
    {
        public OperateDemo()
        {
            InitializeComponent();

            var mouseDown = Observable.FromEventPattern<MouseButtonEventArgs>(canvas, nameof(canvas.MouseDown));
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(canvas, nameof(canvas.MouseUp));
            var movements = Observable.FromEventPattern<MouseEventArgs>(canvas, nameof(canvas.MouseMove));

            //var line = new Polyline()
            //{
            //    Stroke = Brushes.Black,
            //    StrokeThickness = 2
            //};
            //canvas.Children.Add(line);

            //movements.Select(m => m.EventArgs.GetPosition(canvas))
            //    .SkipUntil(mouseDown)
            //    .TakeUntil(mouseUp)
            //    .Repeat()
            //    .Subscribe(pos => line.Points.Add(pos));

            Polyline line = null;
            movements.SkipUntil(mouseDown.Do(_ =>
                {
                    line = new Polyline {StrokeThickness = 2, Stroke = Brushes.Black};
                    canvas.Children.Add(line);
                }))
                .TakeUntil(mouseUp)
                .Select(m => m.EventArgs.GetPosition(canvas))
                .Repeat()
                .Subscribe(pos => line.Points.Add(pos));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDemo
{
    /// <summary>
    /// MyPathAnimationView.xaml 的交互逻辑
    /// </summary>
    public partial class MyPathAnimationView : UserControl
    {
        public MyPathAnimationView()
        {
            InitializeComponent();


            var pathDescriptor1 = DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Path));
            var pathDescriptor2 = DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Path));

            pathDescriptor1.AddValueChanged(path2, Path2TopChanged);
            pathDescriptor1.AddValueChanged(path2, Path2LeftChanged);

            void Path2TopChanged(object sender, EventArgs e)
            {

            }

            void Path2LeftChanged(object sender, EventArgs e)
            {

            }

        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //var animation = new PointAnimationUsingPath
            //{
            //    Duration = TimeSpan.FromSeconds(2),
            //    PathGeometry = (PathGeometry)FindResource("path1"),
            //};
            //ellipse1.BeginAnimation(EllipseGeometry.CenterProperty, animation);
            var pathGeometry = CreatePath(ellipse1.Center, e.GetPosition(path2));

            var animation = new PointAnimationUsingPath
            {
                Duration = TimeSpan.FromSeconds(2),
                PathGeometry = pathGeometry,
                FillBehavior = FillBehavior.HoldEnd
            };
            ellipse1.BeginAnimation(EllipseGeometry.CenterProperty, animation);


        }

        public Point[] Points { get; set; } = new Point[]
        {
            new Point(100,0),
            new Point(100,50),
            new Point(150,50),
            new Point(150,0),
            new Point(200,0)
        };


        private PathGeometry CreatePath(Point from, Point to)
        {
            to = new Point(Math.Round(to.X), Math.Round(to.Y));
            // M0,0 100,0 100,50 150,50 150,0 200,0

            if (from.X > to.X)
            {
                from = new Point(0, 0);
            }

            var points1 = Points.Where(x => x.X >= (from.X - 2) && x.X <= (to.X + 2));

            if (points1.FirstOrDefault().X - 2 <= from.X && from.X <= points1.FirstOrDefault().X + 2)
            {
                points1 = points1.Skip(1);
            }
            if (points1.LastOrDefault().X - 2 <= to.X && to.X <= points1.LastOrDefault().X + 2)
            {
                points1 = points1.Take(points1.Count() - 1);
            }

            var path = new PathGeometry();
            var figure = new PathFigure();
            figure.StartPoint = from;
            foreach (var p in points1)
            {
                figure.Segments.Add(new LineSegment(p, false));
            }
            figure.Segments.Add(new LineSegment(to, false));
            path.Figures.Add(figure);
            return path;
        }
    }
}

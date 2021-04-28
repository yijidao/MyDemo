using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyPrismDemo.Views
{
    public class LineRender : FrameworkElement
    {
        public LineRender()
        {
        }
    }

    public class TrainLine
    {
        public PathGeometry MyPathGeometry { get; set; }

        public PathFigure MyPathFigure { get; set; }

        public TrainLine(Point start)
        {
            MyPathFigure = new PathFigure()
            {
                StartPoint = start,
                Segments = new PathSegmentCollection()
            };
            var figures = new PathFigureCollection { MyPathFigure };
            MyPathGeometry.Figures = figures;
        }

        public void Sample()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(10, 50);

            LineSegment myLineSegment = new LineSegment();
            myLineSegment.Point = new Point(200, 70);

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myLineSegment);

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myPathGeometry;
        }


        public void Run(Point next)
        {
            var segment = new LineSegment() { Point = next };
            MyPathFigure.Segments.Add(segment);
        }
    }

}

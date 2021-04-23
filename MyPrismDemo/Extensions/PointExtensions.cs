using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyPrismDemo.Extensions
{
    public static class PointExtensions
    {
        public static Point Change(this Point point, double x, double y) => new Point(point.X + x, point.Y + y);
    }
}

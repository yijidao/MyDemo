using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MyPrismDemo.Extensions;

namespace MyPrismDemo.Views
{
    public class Render : FrameworkElement
    {
        public DrawingGroup Group { get; set; }

        public Render(EllipseBounce[] ellipse)
        {
            Group = new DrawingGroup();

            foreach (var item in ellipse)
            {
                var eg = item.Ellipse;
                var col = Brushes.Black;
                col.Freeze();

                var gd = new GeometryDrawing(col, null, eg);
                Group.Children.Add(gd);
            }
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawDrawing(Group);
        }
    }

    public class SimpleBounce2D
    {
        public Rect Stage { get; }
        public Point Position { get; set; }
        public Point Velocity { get; set; }

        public double X => Position.X;

        public double Y => Position.Y;

        public SimpleBounce2D(Rect stage, Point position, Point velocity)
        {
            Stage = stage;
            Position = position;
            Velocity = velocity;
        }

        public virtual void Update()
        {
            UpdatePosition();
            BoundaryCheck();
        }

        private void UpdatePosition() => Position = Position.Change(Velocity.X, Velocity.Y);

        private void BoundaryCheck()
        {
            if (Position.X > Stage.Width + Stage.X)
            {
                Velocity = new Point(-Velocity.X, Velocity.Y);
                Position = new Point(Stage.Width + Stage.X, Position.Y);
            }

            if (Position.X < Stage.X)
            {
                Velocity = new Point(-Velocity.X, Velocity.Y);
                Position = new Point(Stage.X, Position.Y);
            }

            if (Position.Y > Stage.Height + Stage.Y)
            {
                Velocity = new Point(Velocity.X, -Velocity.Y);
                Position = new Point(Position.X, Stage.Height + Stage.Y);
            }

            if (Position.Y < Stage.Y)
            {
                Velocity = new Point(Velocity.X, -Velocity.Y);
                Position = new Point(Position.X, Stage.Y);
            }
        }
    }

    public class EllipseBounce : SimpleBounce2D
    {
        public EllipseGeometry Ellipse { get; set; }

        public EllipseBounce(Rect stage, Point position, Point velocity, float radius) : base(stage, position, velocity)
        {
            Ellipse = new EllipseGeometry(position, radius, radius);
        }

        public override void Update()
        {
            base.Update();
            Ellipse.Center = Position;
        }
    }
}

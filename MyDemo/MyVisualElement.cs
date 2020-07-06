using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyDemo
{
    class MyVisualElement : Decorator
    {
        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(MyVisualElement), new FrameworkPropertyMetadata
            {
                DefaultValue = Colors.DarkGray,
                AffectsRender = true
            });

        protected override Size MeasureOverride(Size constraint)
        {
            if(Child != null)
            {
                Child.Measure(constraint);
                return Child.DesiredSize;
            }
            else
            {
                return new Size();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var bounds = new Rect(0, 0, ActualWidth, ActualHeight);
            drawingContext.DrawRectangle(GetForegroundBrush(), null, bounds);
        }

        private Brush GetForegroundBrush()
        {
            if (!IsMouseOver)
            {
                return new SolidColorBrush(BackgroundColor);
            }
            else
            {
                var brush = new RadialGradientBrush(Colors.White, BackgroundColor);
                var position = Mouse.GetPosition(this);
                var origin = new Point(position.X / ActualWidth, position.Y / ActualHeight);
                brush.GradientOrigin = origin;
                brush.Center = origin;
                brush.RadiusX = 1;
                brush.RadiusY = 1;
                return brush;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            InvalidateVisual();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            InvalidateVisual();
        }

    }
}

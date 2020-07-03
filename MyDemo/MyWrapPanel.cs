using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MyDemo
{
    class MyWrapPanel : Panel
    {
        public static bool GetLineBreakBefore(DependencyObject obj)
        {
            return (bool)obj.GetValue(LineBreakBeforeProperty);
        }

        public static void SetLineBreakBefore(DependencyObject obj, bool value)
        {
            obj.SetValue(LineBreakBeforeProperty, value);
        }

        // Using a DependencyProperty as the backing store for LineBreakBefore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineBreakBeforeProperty =
            DependencyProperty.RegisterAttached("LineBreakBefore", typeof(bool), typeof(MyWrapPanel), new FrameworkPropertyMetadata
            {
                AffectsParentMeasure = true,
                AffectsParentArrange = true,
            });


        protected override Size MeasureOverride(Size availableSize)
        {

            var currentSize = new Size();
            var panelSize = new Size();

            foreach (UIElement element in base.InternalChildren)
            {
                element.Measure(availableSize);
                var desiredSize = element.DesiredSize;

                if (GetLineBreakBefore(element) || currentSize.Width + desiredSize.Width > availableSize.Width )
                {
                    panelSize.Width = Math.Max(currentSize.Width, panelSize.Width);
                    panelSize.Height += currentSize.Height;
                    currentSize = desiredSize;

                    if (desiredSize.Width > availableSize.Width)
                    {
                        panelSize.Width = Math.Max(desiredSize.Width, panelSize.Width);
                        panelSize.Height += desiredSize.Height;
                        currentSize = new Size();
                    }
                }
                else
                {
                    currentSize.Width += desiredSize.Width;
                    currentSize.Height = Math.Max(currentSize.Height, desiredSize.Height);
                }
            }

            panelSize.Width = Math.Max(panelSize.Width, currentSize.Width);
            panelSize.Height += currentSize.Height;
            return panelSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var firstInLine = 0;
            var currentSize = new Size();
            var accumulateHeight = 0d;
            var elements = base.InternalChildren;

            for (var i = 0; i < elements.Count; i++)
            {
                var desiredSize = elements[i].DesiredSize;
                if (GetLineBreakBefore(elements[i]) || desiredSize.Width + currentSize.Width > finalSize.Width)
                {
                    ArrageLine(accumulateHeight, currentSize.Height, firstInLine, i);
                    accumulateHeight += currentSize.Height;
                    currentSize = desiredSize;

                    if (desiredSize.Width > finalSize.Width)
                    {
                        ArrageLine(accumulateHeight, desiredSize.Height, i, ++i);
                        accumulateHeight += currentSize.Height;
                        currentSize = new Size();
                    }
                    firstInLine = i;
                }
                else
                {
                    currentSize.Height = Math.Max(desiredSize.Height, currentSize.Height);
                    currentSize.Width += desiredSize.Width;
                }
            }

            if(firstInLine < elements.Count)
            {
                ArrageLine(accumulateHeight, currentSize.Height, firstInLine, elements.Count);
            }

            return finalSize;
        }

        private void ArrageLine(double y, double lineHeight, int start, int end)
        {
            double x = 0;
            var childrens = base.InternalChildren;
            for (var i = start; i < end; i++)
            {
                var child = childrens[i];
                child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineHeight));
                x += child.DesiredSize.Width;
            }
        }
    }
}

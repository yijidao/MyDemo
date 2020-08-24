using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// MyLineView.xaml 的交互逻辑
    /// </summary>
    public partial class MyLineView : UserControl
    {
        public MyLineView()
        {
            InitializeComponent();
        }

        private void ellipse1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var cur = e.GetPosition(Path1);
                var p = cur - Ellipse1Point;

                Canvas.SetTop(Path1, Canvas.GetTop(Path1) + p.Y);
                Canvas.SetLeft(Path1, Canvas.GetLeft(Path1) + p.X);

                Line1X = ellipse1.Center.X + Canvas.GetLeft(Path1);
                Line2Y = ellipse1.Center.Y + Canvas.GetTop(Path1);

                CenterX = ellipse1.Center.X + Canvas.GetLeft(Path1);
                CenterY = ellipse1.Center.Y + Canvas.GetTop(Path1);

                LeftTextX = Canvas.GetLeft(Path1) / 2;
                TopTextY = Canvas.GetTop(Path1) / 2;

            }
        }



        public double Line1X
        {
            get { return (double)GetValue(Line1XProperty); }
            set { SetValue(Line1XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Line1X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Line1XProperty =
            DependencyProperty.Register("Line1X", typeof(double), typeof(MyLineView), new PropertyMetadata(0d));



        public double Line2Y
        {
            get { return (double)GetValue(Line2YProperty); }
            set { SetValue(Line2YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Line2Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Line2YProperty =
            DependencyProperty.Register("Line2Y", typeof(double), typeof(MyLineView), new PropertyMetadata(0d));



        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterXProperty =
            DependencyProperty.Register("CenterX", typeof(double), typeof(MyLineView), new PropertyMetadata(0d));



        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterYProperty =
            DependencyProperty.Register("CenterY", typeof(double), typeof(MyLineView), new PropertyMetadata(0d));



        public double LeftTextX
        {
            get { return (double)GetValue(LeftTextXProperty); }
            set { SetValue(LeftTextXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftTextX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftTextXProperty =
            DependencyProperty.Register("LeftTextX", typeof(double), typeof(MyLineView), new PropertyMetadata(0d));



        public double TopTextY
        {
            get { return (double)GetValue(TopTextYProperty); }
            set { SetValue(TopTextYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopTextY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopTextYProperty =
            DependencyProperty.Register("TopTextY", typeof(double), typeof(MyLineView), new PropertyMetadata(0d));




        public Point Ellipse1Point { get; set; }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse1Point = e.GetPosition(Path1);
        }
        public Point Ellipse2Point { get; set; }

        private void Ellipse2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse2Point = e.GetPosition(Ellipse2);
        }

        private void Ellipse2_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var cp = e.GetPosition(Ellipse2);
                var p = cp - Ellipse2Point;

                var y = Canvas.GetTop(Ellipse2) + p.Y;
                var x = Canvas.GetLeft(Ellipse2) + p.X;

                //Debug.WriteLine($"{x}, {y}");

                Canvas.SetTop(Ellipse2, y);
                Canvas.SetLeft(Ellipse2, x);
            }
        }
    }
}

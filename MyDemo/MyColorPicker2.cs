﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyDemo
{
    class MyColorPicker2 : Control
    {
        static MyColorPicker2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyColorPicker2), new FrameworkPropertyMetadata(typeof(MyColorPicker2)));
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(MyColorPicker1), new FrameworkPropertyMetadata(Colors.Black, (d, e) =>
            {
                var color = (Color)e.NewValue;
                var oldColor = (Color)e.OldValue;

                var colorPicker = (MyColorPicker1)d;
                colorPicker.Red = color.R;
                colorPicker.Green = color.G;
                colorPicker.Blue = color.B;
                colorPicker.PreviousColor = oldColor;

                var args = new RoutedPropertyChangedEventArgs<Color>(oldColor, color);
                args.RoutedEvent = MyColorPicker1.ColorChangedEvent;
                colorPicker.RaiseEvent(args);
            }));

        public Color? PreviousColor { get; set; }

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Red.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register("Red", typeof(byte), typeof(MyColorPicker1), new FrameworkPropertyMetadata((d, e) =>
            {
                var colorPicker = (MyColorPicker1)d;
                var color = colorPicker.Color;
                color.R = (byte)e.NewValue;
                colorPicker.Color = color;
            }));

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Blue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register("Blue", typeof(byte), typeof(MyColorPicker1), new FrameworkPropertyMetadata((d, e) =>
            {
                var colorPicker = (MyColorPicker1)d;
                var color = colorPicker.Color;
                color.B = (byte)e.NewValue;
                colorPicker.Color = color;
            }));

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Green.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register("Green", typeof(byte), typeof(MyColorPicker1), new FrameworkPropertyMetadata((d, e) =>
            {
                var colorPicker = (MyColorPicker1)d;
                var color = colorPicker.Color;
                color.G = (byte)e.NewValue;
                colorPicker.Color = color;
            }));

    }
}

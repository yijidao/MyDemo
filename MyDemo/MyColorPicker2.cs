using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
            DependencyProperty.Register("Color", typeof(Color), typeof(MyColorPicker2), new FrameworkPropertyMetadata(Colors.Black, (d, e) =>
            {
                var color = (Color)e.NewValue;
                var oldColor = (Color)e.OldValue;

                var colorPicker = (MyColorPicker2)d;
                colorPicker.Red = color.R;
                colorPicker.Green = color.G;
                colorPicker.Blue = color.B;
                colorPicker.PreviousColor = oldColor;

                //var args = new RoutedPropertyChangedEventArgs<Color>(oldColor, color);
                //args.RoutedEvent = MyColorPicker2.ColorChangedEvent;
                //colorPicker.RaiseEvent(args);
            }));

        public Color? PreviousColor { get; set; }

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Red.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register("Red", typeof(byte), typeof(MyColorPicker2), new FrameworkPropertyMetadata((d, e) =>
            {
                var colorPicker = (MyColorPicker2)d;
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
            DependencyProperty.Register("Blue", typeof(byte), typeof(MyColorPicker2), new FrameworkPropertyMetadata((d, e) =>
            {
                var colorPicker = (MyColorPicker2)d;
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
            DependencyProperty.Register("Green", typeof(byte), typeof(MyColorPicker2), new FrameworkPropertyMetadata((d, e) =>
            {
                var colorPicker = (MyColorPicker2)d;
                var color = colorPicker.Color;
                color.G = (byte)e.NewValue;
                colorPicker.Color = color;
            }));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_sdRed") is RangeBase redSlider)
            {
                var binding = new Binding(nameof(MyColorPicker2.Red));
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                redSlider.SetBinding(RangeBase.ValueProperty, binding );
            }

            if (GetTemplateChild("PART_sdGreen") is RangeBase greenSlider)
            {
                var binding = new Binding(nameof(MyColorPicker2.Green));
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                greenSlider.SetBinding(RangeBase.ValueProperty, binding);
            }

            if (GetTemplateChild("PART_sdBlue") is RangeBase blueSlider)
            {
                var binding = new Binding(nameof(MyColorPicker2.Blue));
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                blueSlider.SetBinding(RangeBase.ValueProperty, binding);
            }

            if (GetTemplateChild("PART_PreviewBrush") is SolidColorBrush previewBrush)
            {
                var binding = new Binding(nameof(MyColorPicker2.Color));
                binding.Source = previewBrush;
                binding.Mode = BindingMode.OneWayToSource;
                this.SetBinding(MyColorPicker2.ColorProperty, binding);
            }
        }

    }
}

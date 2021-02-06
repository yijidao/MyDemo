using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyDemo
{
    public class ListBoxWithImage : ListBox
    {
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ListBoxWithImage), new PropertyMetadata(null));

        static ListBoxWithImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxWithImage), new FrameworkPropertyMetadata(typeof(ListBoxWithImage)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


        }
    }
}

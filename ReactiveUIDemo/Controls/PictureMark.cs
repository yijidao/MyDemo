using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;

namespace ReactiveUIDemo.Controls
{
    public class PictureMark : Control
    {
        public MarkInfo Mark
        {
            get => (MarkInfo)GetValue(MarkProperty);
            set => SetValue(MarkProperty, value);
        }

        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register("Mark", typeof(MarkInfo), typeof(PictureMark),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (o, args) =>
                {
                    var ctl = (PictureMark)o;
                    if (args.NewValue is MarkInfo markInfo)
                    {
                        Canvas.SetLeft(ctl, markInfo.X);
                        Canvas.SetTop(ctl, markInfo.Y);
                    }
                }));

        public Visibility Editing
        {
            get => (Visibility)GetValue(EditingProperty);
            set => SetValue(EditingProperty, value);
        }

        public static readonly DependencyProperty EditingProperty =
            DependencyProperty.Register("Editing", typeof(Visibility), typeof(PictureMark), new PropertyMetadata(Visibility.Collapsed));

        public static readonly RoutedEvent RemoveEvent = EventManager.RegisterRoutedEvent("Remove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PictureMark));

        public event RoutedEventHandler Remove
        {
            add => AddHandler(RemoveEvent, value);
            remove => RemoveHandler(RemoveEvent, value);
        }

        static PictureMark()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PictureMark), new FrameworkPropertyMetadata(typeof(PictureMark)));

        }

        public PictureMark()
        {
            DataContextChanged += (sender, args) =>
            {
                if (args.NewValue is MarkInfo markInfo)
                {
                    Mark = markInfo;
                }
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var remove = GetTemplateChild("remove") as Button;
            remove.Click += (sender, args) =>
            {
                RaiseEvent(new RoutedEventArgs(RemoveEvent, this));
            };
        }
    }

    public class MarkInfo : ReactiveObject
    {
        private string _info;
        public string Info
        {
            get => _info;
            set => this.RaiseAndSetIfChanged(ref _info, value);
        }

        private double _x;
        public double X
        {
            get => _x;
            set => this.RaiseAndSetIfChanged(ref _x, value);
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => this.RaiseAndSetIfChanged(ref _y, value);
        }

        public long Id { get; set; }

        public MarkInfo(long id, string info, double x, double y)
        {
            _info = info;
            _x = x;
            _y = y;
            Id = id;
        }

    }
}

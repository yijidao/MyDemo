using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ReactiveUI;

namespace ReactiveUIDemo.Controls
{
    public class MonitorMark : Control
    {
        public MarkInfo Mark
        {
            get => (MarkInfo)GetValue(MarkProperty);
            set => SetValue(MarkProperty, value);
        }

        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register("Mark", typeof(MarkInfo), typeof(MonitorMark),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (o, args) =>
                {
                    var ctl = (MonitorMark)o;
                    if (args.NewValue is MarkInfo markInfo)
                    {
                        Canvas.SetLeft(ctl, markInfo.X);
                        Canvas.SetTop(ctl, markInfo.Y);
                    }
                }));

        public bool Editing
        {   
            get => (bool)GetValue(EditingProperty);
            set => SetValue(EditingProperty, value);
        }

        public static readonly DependencyProperty EditingProperty =
            DependencyProperty.Register("Editing", typeof(bool), typeof(MonitorMark), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (o, args) =>
                {
                    var ctl = (MonitorMark) o;
                    var editing = (bool) args.NewValue;
                    ctl.ChangeVisualState(editing ? "IsEdit" : "UnEdit");
                }));

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(bool), typeof(MonitorMark), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (o, args) =>
                {
                    var ctl = (MonitorMark)o;
                    var selected = (bool) args.NewValue;
                    if (selected)
                    {
                        ctl.ChangeVisualState("Selected");
                        ctl.UpdateElementSet();
                    }
                    else
                    {
                        ctl.ChangeVisualState("Normal");
                    }
                }));



        public static readonly RoutedEvent RemoveEvent = EventManager.RegisterRoutedEvent("Remove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MonitorMark));

        public event RoutedEventHandler Remove
        {
            add => AddHandler(RemoveEvent, value);
            remove => RemoveHandler(RemoveEvent, value);
        }

        private static readonly Lazy<List<WeakReference<MonitorMark>>> Singleton = new Lazy<List<WeakReference<MonitorMark>>>(LazyThreadSafetyMode.ExecutionAndPublication);

        private List<WeakReference<MonitorMark>> ElementList => Singleton.Value;
        static MonitorMark()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonitorMark), new FrameworkPropertyMetadata(typeof(MonitorMark)));
        }

        public MonitorMark()
        {
            DataContextChanged += (sender, args) =>
            {
                if (args.NewValue is MarkInfo markInfo)
                {
                    Mark = markInfo;
                }
            };

            ElementList.Add(new WeakReference<MonitorMark>(this));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState("Normal");
            ChangeVisualState("UnEdit");
        }

        private void ChangeVisualState(string stateName, bool useTransitions = false)
        {
            VisualStateManager.GoToState(this, stateName, useTransitions);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (Selected) return;
            ChangeVisualState("Hover");
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (Selected) return;
            ChangeVisualState("Normal");
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Selected = true;
        }

        private void UpdateElementSet()
        {
            lock (ElementList)
            {
                var index = 0;
                var window = Window.GetWindow(this);

                while (index < ElementList.Count)
                {
                    if (ElementList[index].TryGetTarget(out var pictureMark))
                    {
                        index++;
                        if (pictureMark != this && Window.GetWindow(pictureMark) == window)
                        {
                            pictureMark.Selected = false;
                        }
                    }
                    else
                    {
                        ElementList.RemoveAt(index);
                    }
                }


            }

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

    class PictureMarkMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)value;
            return new Thickness(width, -6, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

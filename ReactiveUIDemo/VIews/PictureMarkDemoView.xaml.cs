using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
using Microsoft.Win32.SafeHandles;
using ReactiveUI;
using ReactiveUIDemo.Controls;
using ReactiveUIDemo.ViewModels;

namespace ReactiveUIDemo.Views
{
    /// <summary>
    /// PictureMarkDemo.xaml 的交互逻辑
    /// </summary>
    public partial class PictureMarkDemoView : UserControl, IDemo
    {
        public PictureMarkDemoView()
        {
            InitializeComponent();
            var viewModel = new PictureMarkDemoViewModel();
            DataContext = viewModel;
            var bimg = new BitmapImage(new Uri(@"D:\Code\MyDemo\ReactiveUIDemo\Images\map.jpg", UriKind.RelativeOrAbsolute));
            Image.Source = bimg;

            var downSubject = new Subject<PictureMark>();
            var upSubject = new Subject<PictureMark>();

            var move = Observable.FromEventPattern<MouseEventArgs>(this.MarkContainer,
                nameof(this.MarkContainer.MouseMove));

            IDisposable d = null;
            IDisposable d2 = null;
            IDisposable d3 = null;

            MarkContainer.AddHandler(PictureMark.RemoveEvent, new RoutedEventHandler(((o, eventArgs) =>
            {
                var ctl = (PictureMark) eventArgs.OriginalSource;
                MarkContainer.Children.Remove(ctl);
                viewModel.Marks.Remove(ctl.Mark);
            })));

            Edit.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Hidden;

            Edit.Click += (sender, args) =>
            {
                Edit.Visibility = Visibility.Hidden;
                Save.Visibility = Visibility.Visible;

                foreach (UIElement element in MarkContainer.Children)
                {
                    if (element is PictureMark pictureMark)
                    {
                        pictureMark.Editing = Visibility.Visible;
                    }
                }

                PictureMark current = null;
                d = move.SkipUntil(downSubject.Do(x => current = x))
                    .TakeUntil(upSubject.Do(_ => current = null))
                    .Repeat()
                    .Select(x => x.EventArgs.GetPosition(MarkContainer))
                    .Subscribe(p =>
                    {
                        Debug.WriteLine($"------  X:{p.X}  Y:{p.Y}  ------");
                        Canvas.SetLeft(current, p.X);
                        Canvas.SetTop(current, p.Y);
                    });


                d2 = Observable.FromEventPattern<MouseEventArgs>(monitors, nameof(monitors.MouseMove))
                    .Where(x => x.EventArgs.LeftButton == MouseButtonState.Pressed && monitors.SelectedItem is Controls.MarkInfo)
                    .Subscribe(_ =>
                    {
                        var data = new DataObject();
                        data.SetData("info", (MarkInfo)monitors.SelectedItem);
                        DragDrop.DoDragDrop(monitors as DependencyObject, data, DragDropEffects.Copy);
                    });
                d3 = Observable.FromEventPattern<DragEventArgs>(MarkContainer, nameof(MarkContainer.Drop))
                    .Where(x => x.EventArgs.Data.GetData("info") is MarkInfo markInfo && !viewModel.Marks.Contains(markInfo))
                    .Subscribe(x =>
                    {
                        var markInfo = x.EventArgs.Data.GetData("info") as MarkInfo;
                        var p = x.EventArgs.GetPosition(MarkContainer);

                        markInfo.X = p.X;
                        markInfo.Y = p.Y;

                        var pictureMark = new PictureMark
                        {
                            DataContext = markInfo,
                            Width = 36,
                            Height = 36,
                            Background = new SolidColorBrush(Colors.LightBlue),
                            Editing = Visibility.Visible
                        };

                        pictureMark.MouseLeftButtonDown += (o, args) => downSubject.OnNext((PictureMark)o);
                        pictureMark.MouseLeftButtonUp += (o, args) => upSubject.OnNext((PictureMark)o);
                        MarkContainer.Children.Add(pictureMark);
                        viewModel.Marks.Add(markInfo);
                    });

            };

            Save.Click += (sender, args) =>
            {
                viewModel.Save();

                foreach (UIElement element in MarkContainer.Children)
                {
                    if (element is PictureMark pictureMark)
                    {
                        pictureMark.Editing = Visibility.Collapsed;
                    }
                }

                Save.Visibility = Visibility.Hidden;
                Edit.Visibility = Visibility.Visible;

                d?.Dispose();
                d = null;

                d2?.Dispose();
                d2 = null;

                d3?.Dispose();
                d3 = null;
            };
        }
    }
}

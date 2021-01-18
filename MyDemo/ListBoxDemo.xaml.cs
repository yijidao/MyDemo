using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// ListBoxDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxDemo : UserControl
    {
        public ListBoxDemo()
        {
            InitializeComponent();
        }

        public static string[] Generate()
        {
            return new string[] { "虹猫", "蓝兔", "莎丽", "大奔", "逗逗", "跳跳", "达达" };
        }



        public Schedule SelectedsSchedule
        {
            get { return (Schedule)GetValue(SelectedsScheduleProperty); }
            set { SetValue(SelectedsScheduleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedsSchedule.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedsScheduleProperty =
            DependencyProperty.Register("SelectedsSchedule", typeof(Schedule), typeof(ListBoxDemo), new PropertyMetadata(null,
                (o, args) =>
                {
                    var value = args.NewValue;

                }));


    }

    public class IsLastItemInContainerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            DependencyObject item = (DependencyObject)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);

            return ic.ItemContainerGenerator.IndexFromContainer(item)
                   == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// MyDataViewView.xaml 的交互逻辑
    /// </summary>
    public partial class MyDataViewView : UserControl
    {
        public MyDataViewView()
        {
            InitializeComponent();

            var data = new ObservableCollection<Schedule>(Schedule.Generate());

            listBox1.ItemsSource = data;
            var view1 = (ListCollectionView)CollectionViewSource.GetDefaultView(listBox1.ItemsSource);

            view1.SortDescriptions.Add(new SortDescription(nameof(Schedule.WorkingHour), ListSortDirection.Ascending));
            view1.SortDescriptions.Add(new SortDescription(nameof(Schedule.StartDate), ListSortDirection.Ascending));

            //普通分组
            //view1.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Schedule.Tag)));
            //范围分组
            var grouper = new WorkingHourGroup { GroupInterval = 40 };
            view1.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Schedule.WorkingHour), grouper));
            view1.Filter = x =>
            {
                var schedule = (Schedule)x;
                if (schedule.Tag == "学习") return false;
                else return true;
            };
        }
    }

    public class WorkingHourGroup : IValueConverter
    {
        public int GroupInterval { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hour = (int)value;
            if (hour < GroupInterval)
            {
                return String.Format("工时 < {0}", hour);
            }
            else
            {
                var interval = hour / GroupInterval;
                var lowerLimit = interval * GroupInterval;
                var upperLimit = (interval + 1) * GroupInterval;
                return string.Format("{0} <= 工时 <= {1}", lowerLimit, upperLimit);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

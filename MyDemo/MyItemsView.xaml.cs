using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Xml;

namespace MyDemo
{
    /// <summary>
    /// MyItemsView.xaml 的交互逻辑
    /// </summary>
    public partial class MyItemsView : UserControl
    {
        public MyItemsView()
        {
            InitializeComponent();

            var list = new List<Schedule>{

            new Schedule { Title = "WPF研究", Tag = "学习" },
            new Schedule { Title = "Win32研究", Tag = "学习" },
            new Schedule { Title = "大屏系统开发", Tag = "工作" },
            };

            ComboBox1.ItemsSource = list;
        }
    }

    public class Schedule
    {
        public string Title { get; set; }

        public string Tag { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int WorkingHour { get; set; }

        public string Content { get; set; }

        public Schedule[] Details { get; set; }

        public Schedule()
        {

        }

        public Schedule(string title, string tag, DateTime startDate, DateTime endDate, int workingHour, string content = "")
        {
            Title = title;
            Tag = tag;
            StartDate = startDate;
            EndDate = endDate;
            WorkingHour = workingHour;
            Content = content;
        }

        public Schedule(string title, Schedule[] details)
        {
            Title = title;
            Details = details;
            StartDate = details.Select(x => x.StartDate).Min();
            EndDate = details.Select(x => x.EndDate).Max();
            WorkingHour = details.Select(x => x.WorkingHour).Sum();
        }

        public static Schedule[] Generate()
        {
            var models = new List<Schedule>();

            models.Add(new Schedule("WPF研究", "学习", new DateTime(2020, 7, 1), DateTime.Today, 100, "笔记同步进行"));
            models.Add(new Schedule("Win32研究", "学习", new DateTime(2020, 7, 15), DateTime.Today, 150, "笔记同步进行"));
            models.Add(new Schedule("大屏开发", "工作", new DateTime(2020, 7, 1), new DateTime(2020, 7, 5), 40, "可配置开发"));
            models.Add(new Schedule("框架开发", "工作", new DateTime(2020, 7, 27), DateTime.Today, 200, "架构设计"));

            return models.ToArray();
        }

        public static Schedule[] GenerateHierarchical()
        {
            var models1 = new List<Schedule>();
            models1.Add(new Schedule("WPF研究", "学习", new DateTime(2020, 7, 1), DateTime.Today, 100, "笔记同步进行"));
            models1.Add(new Schedule("Win32研究", "学习", new DateTime(2020, 7, 15), DateTime.Today, 150, "笔记同步进行"));

            var models2 = new List<Schedule>();
            models2.Add(new Schedule("大屏开发", "工作", new DateTime(2020, 7, 1), new DateTime(2020, 7, 5), 40, "可配置开发"));
            models2.Add(new Schedule("框架开发", "工作", new DateTime(2020, 7, 27), DateTime.Today, 200, "架构设计"));

            var models = new List<Schedule>();
            models.Add(new Schedule("项目内", models1.ToArray()));
            models.Add(new Schedule("项目外", models2.ToArray()));

            return models.ToArray();
        }

    }

    public class SingleCriteriaHighlightStyleSelector : StyleSelector
    {
        public Style DefaultStyle { get; set; }

        public Style HighlightStyle { get; set; }

        public string PropertyToEvaluate { get; set; }

        public string PropertyToHighlight { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {

            var element = item as XmlElement;

            var evaluate = element[PropertyToEvaluate].InnerText;

            if (evaluate == PropertyToHighlight)
            {
                return HighlightStyle;
            }
            else
            {
                return DefaultStyle;
            }
        }

    }

}

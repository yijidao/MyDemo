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

            if(evaluate == PropertyToHighlight)
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

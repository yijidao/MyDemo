using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// CharDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CharDemo : UserControl
    {
        public CharDemo()
        {
            InitializeComponent();

            var o = new ObservableCollection<CharViewModel>();
            o.Add(new SendViewModel { Message = "Hello" });
            o.Add(new ReceiveViewModel { Message = "优秀", Title = "优秀的标题" });
            charWindow.ItemsSource = o;

            
            send.Click += (sender, args) =>
            {
                var value = input.Text;
                if (string.IsNullOrWhiteSpace(value)) return;
                o.Add(new SendViewModel { Message = value });
                input.Text = "";
                o.Add(new ReceiveViewModel { Message = "自动回复...", Title = "优秀的标题" });
                var sv = charWindow.Template.FindName("scrollViewer", charWindow) as ScrollViewer;
                sv?.ScrollToBottom();
            };
        }
    }

    public class CharViewModel
    {
        public string Message { get; set; }
    }

    public class SendViewModel : CharViewModel
    {

    }

    public class ReceiveViewModel : CharViewModel
    {
        public string Title { get; set; }
    }

    public class CharItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is FrameworkElement element)) return null;

            switch (item)
            {
                case SendViewModel _:
                    return element.FindResource("SendChar") as DataTemplate;
                case ReceiveViewModel _:
                    return element.FindResource("ReceiveChar") as DataTemplate;
                default:
                    return null;
            }
        }
    }

}

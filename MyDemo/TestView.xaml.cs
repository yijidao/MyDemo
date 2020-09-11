using System;
using System.Collections.Generic;
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
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : UserControl
    {



        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(TestView), new FrameworkPropertyMetadata(0));



        public TestView()
        {
            InitializeComponent();

            //var schedule = new Schedule { Title = "按钮绑定测试" };

            //var binding = new Binding("Title");
            //binding.Source = schedule;
            //BindingOperations.SetBinding(Button1, ContentProperty, binding);

            //var binding1 = new Binding(nameof(Schedule.IsComplete));
            //binding1.Source = schedule;

            //BindingOperations.SetBinding(Button1, Button.IsDefaultedProperty, binding1);


            var isAllow = (Button.IsDefaultedProperty.DefaultMetadata as FrameworkPropertyMetadata).IsDataBindingAllowed;

        }
    }
}

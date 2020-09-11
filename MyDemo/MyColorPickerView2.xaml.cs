using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// MyColorPickerView2.xaml 的交互逻辑
    /// </summary>
    public partial class MyColorPickerView2 : UserControl
    {
        public MyColorPickerView2()
        {
            InitializeComponent();
        }


        [Browsable(true)]
        //[Description("测试属性面板属性"), Category("自定义"), DefaultValue(false)]
        public bool TestProperty
        {
            get { return (bool)GetValue(TestPropertyProperty); }
            set { SetValue(TestPropertyProperty, value); }
        }


        public static readonly DependencyProperty TestPropertyProperty =
            DependencyProperty.Register("TestProperty", typeof(bool), typeof(MyColorPicker2), new PropertyMetadata(false));


    }
}

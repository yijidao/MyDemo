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

namespace MyPrismDemo.Views
{
    /// <summary>
    /// BusinessView.xaml 的交互逻辑
    /// </summary>
    [DisplayName("业务控件1"), Description("用来拖拽的简单的业务控件")]
    public partial class BusinessView : UserControl
    {
        public BusinessView()
        {
            InitializeComponent();
        }
    }
}

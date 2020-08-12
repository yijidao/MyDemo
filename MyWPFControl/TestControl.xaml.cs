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

namespace MyWPFControl
{
    /// <summary>
    /// TestControl.xaml 的交互逻辑
    /// </summary>
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();

            Carouse.ItemsSource = new ObservableCollection<object>
            {
                new TClass{ Text1 = "11", Text2 = "22" },
                new TClass{ Text1 = "33", Text2 = "44" },
            };
        }
    }

    public class TClass
    {
        public string Text1 { get; set; }

        public string Text2 { get; set; }
    }
}

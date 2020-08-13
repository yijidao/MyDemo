using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Carouse.ItemsSource = new ObservableCollection<TClass>
            //{
            //    new TClass{Text1 = "11", Text2 = "22"},
            //    new TClass{Text1 = "33", Text2 = "44"}
            //};
            //C1.DataContext = new TClass { Text1 = "11", Text2 = "22" };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CarouseData).Add();
        }
    }

    public class TClass : System.ComponentModel.INotifyPropertyChanged
    {

        //public string Text1 { get; set; }

        //public string Text2 { get; set; }


        private string _Text1;

        public string Text1
        {
            get { return _Text1; }
            set
            {
                _Text1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text1)));
            }
        }
        private string _Text2;

        public string Text2
        {
            get { return _Text2; }
            set
            {
                _Text2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text2)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

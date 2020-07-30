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
    /// MyDataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class MyDataGridView : UserControl
    {
        public MyDataGridView()
        {
            InitializeComponent();

            DataGrid1.ItemsSource = new ObservableCollection<Schedule>(Schedule.Generate()); 
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var rowData = e.Row.DataContext as Schedule;
            if (rowData.Tag == "工作") e.Row.Background = new SolidColorBrush(Colors.LightBlue);
            else e.Row.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}

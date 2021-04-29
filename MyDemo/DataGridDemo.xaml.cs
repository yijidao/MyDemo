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
    /// DataGridDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridDemo : UserControl 
    {
        public DataGridDemo()
        {
            InitializeComponent();

            grid.ItemsSource = DataGridModel.GetDataGridModels(20);
        }
    }


    class DataGridModel
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public bool Checked { get; set; }

        public DataGridModel(string name, int value, bool check)
        {
            Name = name;
            Value = value;
            Checked = check;
        }

        public static DataGridModel[] GetDataGridModels(int count)
        {
            var list = new List<DataGridModel>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new DataGridModel($"name_{i}", i, i % 2 == 0));
            }
            return list.ToArray();
        }

    }
}

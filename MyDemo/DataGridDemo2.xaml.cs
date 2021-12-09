using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// DataGridDemo2.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridDemo2 : UserControl
    {
        private int _index = 0;
        private ObservableCollection<DataGridDemo2Model> _dic = new ObservableCollection<DataGridDemo2Model>();

        public DataGridDemo2()
        {
            InitializeComponent();

            dataGrid.ItemsSource = _dic;
            add.Click += (sender, args) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _dic.Add(new DataGridDemo2Model
                    {
                        Index = _index++

                    });
                }
            };
        }

        private double currentChange = 0;

        private void DataGrid_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Debug.WriteLine("--------------------------------------------");
            Debug.WriteLine($"VerticalChange: {e.VerticalChange}");
            Debug.WriteLine($"VerticalOffset: {e.VerticalOffset}");
            Debug.WriteLine($"ExtentHeight: {e.ExtentHeight}");
            Debug.WriteLine($"ViewportHeight: {e.ViewportHeight}");
            Debug.WriteLine("--------------------------------------------");

            //e.

            //if (Math.Abs(e.VerticalOffset + e.ViewportHeight - e.ExtentHeight) <= 0 )
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        _dic.Add(new DataGridDemo2Model
            //        {
            //            Index = _index++

            //        });
            //    }
            //}
            

        }
    }

    class DataGridDemo2Model
    {
        public int Index { get; set; }
    }
}

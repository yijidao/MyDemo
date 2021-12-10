using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Common;

namespace MyDemo
{
    /// <summary>
    /// DataGridDemo2.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridDemo2 : UserControl
    {
        private int _index = 0;
        private readonly ObservableCollection<DataGridDemo2Model> _dic = new ObservableCollection<DataGridDemo2Model>();

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


        private void DataGrid_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            Debug.WriteLine("--------------------------------------------");
            Debug.WriteLine($"VerticalChange: {e.VerticalChange}");
            Debug.WriteLine($"VerticalOffset: {e.VerticalOffset}");
            Debug.WriteLine($"ExtentHeight: {e.ExtentHeight}");
            Debug.WriteLine($"ViewportHeight: {e.ViewportHeight}");
            Debug.WriteLine("--------------------------------------------");

            e.Handled = true;
            if (e.ViewportHeight > 0 && e.VerticalChange > 0 && Math.Abs(e.VerticalOffset + e.ViewportHeight - e.ExtentHeight) < 1)
            {

                for (int i = 0; i < 10; i++)
                {
                    _dic.Add(new DataGridDemo2Model
                    {
                        Index = _index++

                    });
                }
                dataGrid.ScrollIntoView(_dic[(int)e.VerticalOffset]); // 设置一下滚动行，不然会一直滚动到底
            }
        }
    }

    class DataGridDemo2Model
    {
        public long Index { get; set; }
    }
}

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
    /// MyListViewView.xaml 的交互逻辑
    /// </summary>
    public partial class MyListViewView : UserControl
    {
        public MyListViewView()
        {
            InitializeComponent();
        }

        public ViewBase GridView1 { get; set; }
        private int _Num;

        public int Num
        {
            get { return _Num; }
            set
            {
                if (value == 3)
                {
                    value = 0;
                }
                _Num = value;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GridView1 == null) GridView1 = ListView1.View;
            Num++;
            switch (Num)
            {
                case 0:
                    ListView1.View = GridView1;
                    break;
                case 1:
                    ListView1.View = FindResource("TileView1") as ViewBase;
                    break;
                case 2:
                    ListView1.View = FindResource("TileView2") as ViewBase;
                    break;
            }

        }
    }
}

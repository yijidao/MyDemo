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

namespace MyDemo
{
    /// <summary>
    /// MyDataViewView.xaml 的交互逻辑
    /// </summary>
    public partial class MyDataViewView : UserControl
    {
        public MyDataViewView()
        {
            InitializeComponent();

            var data = new ObservableCollection<Schedule>(Schedule.Generate());

            listBox1.ItemsSource = data;
            var view1 = (ListCollectionView)CollectionViewSource.GetDefaultView(listBox1.ItemsSource);

            view1.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Schedule.Tag)));
            

        }
    }
}

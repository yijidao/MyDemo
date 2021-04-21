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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyPrismDemo.ViewModels;

namespace MyPrismDemo.Views
{
    /// <summary>
    /// TrainTimeTableView.xaml 的交互逻辑
    /// </summary>
    public partial class TrainTimeTableView : UserControl
    {
        public TrainTimeTableView()
        {
            InitializeComponent();

            //((TrainTimeTableViewModel) DataContext).PropertyChanged += (sender, args) =>
            //{

            //};

        }

        //void AddColumns(string[] newColumnNames)
        //{
        //    foreach (string name in newColumnNames)
        //    {
        //        grid.Columns.Add(new DataGridTextColumn
        //        {
        //            // bind to a dictionary property
        //            Binding = new Binding("Custom[" + name + "]"),
        //            Header = name
        //        });
        //    }
        //}

        
}
}

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
using MyPrismDemo.ViewModels;

namespace MyPrismDemo.Views
{
    /// <summary>
    /// DesignView.xaml 的交互逻辑
    /// </summary>
    public partial class DesignView : UserControl
    {
        public DesignView()
        {
            InitializeComponent();
        }

        private void DragControl_ToolBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed ||
                !(toolBox.SelectedItem is ToolBoxItemViewModel item)) return;
            var dataObject = new DataObject();
            dataObject.SetData("type", item.ItemType);
            DragDrop.DoDragDrop(toolBox, dataObject, DragDropEffects.Copy);
        }

        private void Container_GridDrop(object sender, DragEventArgs e)
        {

            var type = e.Data.GetData("type");


            var point = e.GetPosition(container);

            var column = GetColumn(point.X);
            var row = GetRow(point.Y);

            System.Diagnostics.Debug.WriteLine($"c: {column}, r: {row}");

            int GetColumn(double x)
            {
                int index = 0;
                foreach (var columnDefinition in container.ColumnDefinitions)
                {
                    if (x >= columnDefinition.Offset && x < columnDefinition.Offset + columnDefinition.ActualWidth)
                    {
                        return index;
                    }
                    index++;
                }

                return 0;
            }

            int GetRow(double y)
            {
                int index = 0;
                foreach (var rowDefinition in container.RowDefinitions)
                {
                    if (y >= rowDefinition.Offset && y < rowDefinition.Offset + rowDefinition.ActualHeight)
                    {
                        return index;
                    }

                    index++;
                }

                return 0;
            }

            //int GetColumn(double x)
            //{
            //    int index = 0;
            //    foreach (var columnDefinition in container.ColumnDefinitions)
            //    {
            //        if (x >= columnDefinition.Offset && x < columnDefinition.Offset + columnDefinition.ActualWidth)
            //        {
            //            return index;
            //        }
            //        index++;
            //    }

            //    return 0;
            //}
        }


        private void CreateControl_GridOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}

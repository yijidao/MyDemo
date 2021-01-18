using System;
using System.Collections;
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
    /// GridDemo.xaml 的交互逻辑
    /// </summary>
    public partial class GridDemo : UserControl
    {
        public GridDemo()
        {
            InitializeComponent();

            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            var backRect = GenerateCellMask();
            Grid.SetColumn(backRect, grid1.ColumnDefinitions.Count - 1);
            grid1.Children.Add(backRect);
            var textBlock = GenerateCellMaskText();
            Grid.SetColumn(textBlock, grid1.ColumnDefinitions.Count - 1);
            grid1.Children.Add(textBlock);
            //grid1.ColumnDefinitions.Add(new ColumnDefinition());
            //grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition());
            //grid1.RowDefinitions.Add(new RowDefinition());
            //grid1.RowDefinitions.Add(new RowDefinition());
        }

        private void PopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            popup1.IsOpen = true;
        }

        private void AddColumnDefinition_ButtonClick(object sender, RoutedEventArgs e)
        {
            var columnSplitter = new GridSplitter
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 1
            };
            SetGridSplitterStyle(ref columnSplitter);
            Grid.SetColumn(columnSplitter, grid1.ColumnDefinitions.Count - 1);
            grid1.Children.Add(columnSplitter);
            grid1.ColumnDefinitions.Add(new ColumnDefinition
            {
                MinWidth = 10
            });
            var backRect = GenerateCellMask();
            Grid.SetColumn(backRect, grid1.ColumnDefinitions.Count - 1);
            grid1.Children.Add(backRect);
            var textBlock = GenerateCellMaskText();
            Grid.SetColumn(textBlock, grid1.ColumnDefinitions.Count - 1);
            grid1.Children.Add(textBlock);

        }

        private void RemoveColumnDefinition_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (grid1.ColumnDefinitions.Count <= 1) return;

            var stack = new Stack<UIElement>();
            
            foreach (UIElement element in grid1.Children)
            {
                if (Grid.GetColumn(element) != grid1.ColumnDefinitions.Count - 1) continue;
                stack.Push(element);
            }

            while (stack.Count > 0)
            {
                grid1.Children.Remove(stack.Pop());
            }

            grid1.ColumnDefinitions.RemoveAt(grid1.ColumnDefinitions.Count - 1);
        }

        private void AddRowDefinition_ButtonClick(object sender, RoutedEventArgs e)
        {
            grid1.RowDefinitions.Add(new RowDefinition());
        }

        private void RemoveRowDefinition_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (grid1.RowDefinitions.Count <= 1) return;
            grid1.RowDefinitions.RemoveAt(grid1.RowDefinitions.Count - 1);
        }

        private void SetGridSplitterStyle(ref GridSplitter splitter)
        {
            splitter.Focusable = false;
            splitter.Background = Brushes.LightGray;
        }

        private void CloseGridDesign_ButtonClick(object sender, RoutedEventArgs e)
        {
            popup1.IsOpen = false;
        }

        private Rectangle GenerateCellMask() => new Rectangle
        {
            Fill = Brushes.LightCyan,
            Opacity = .1
        };

        private TextBlock GenerateCellMaskText() => new TextBlock
        {
            Text = $"column{grid1.ColumnDefinitions.Count}",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brushes.White,
            FontSize = 14
        };
    }
}

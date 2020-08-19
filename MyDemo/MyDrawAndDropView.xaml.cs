using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDemo
{
    /// <summary>
    /// MyDrawAndDropView.xaml 的交互逻辑
    /// </summary>
    public partial class MyDrawAndDropView : UserControl
    {
        public MyDrawAndDropView()
        {
            InitializeComponent();
        }

        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var data = new DataObject();
                var shape = (e.OriginalSource as Ellipse);
                data.SetData(DataFormats.StringFormat, shape.Fill.ToString());
                data.SetData("Object", shape);
                DragDrop.DoDragDrop(shape, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void ellipse_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                var dataString = e.Data.GetData(DataFormats.StringFormat) as string;
                var converter = new BrushConverter();
                if (converter.IsValid(dataString))
                {
                    var newFill = converter.ConvertFromString(dataString) as Brush;
                    var ellipse = (e.Source as Ellipse);
                    ellipse.Fill = newFill;

                    if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
            }
            e.Handled = true;
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                var ellipse = e.Data.GetData("Object") as Ellipse;
                var panel = e.OriginalSource as StackPanel;
                if(ellipse != null && panel != null)
                {
                    var parent= VisualTreeHelper.GetParent(ellipse) as Panel;
                    if(parent != null)
                    {
                        if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                        {
                            var xaml = XamlWriter.Save(ellipse);
                            var newEllipse = XamlReader.Parse(xaml) as FrameworkElement;
                            newEllipse.Name = "";
                            panel.Children.Add(newEllipse);
                            e.Effects = DragDropEffects.Copy;
                        }
                        else
                        {
                            parent.Children.Remove(ellipse);
                            panel.Children.Add(ellipse);
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                }
            }
            e.Handled = true;
        }

    }
}

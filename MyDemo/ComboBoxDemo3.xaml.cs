using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace MyDemo
{
    /// <summary>
    /// ComboBoxDemo3.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxDemo3 : UserControl
    {
        public ComboBoxDemo3()
        {
            InitializeComponent();
        }

        private void Dropdown_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("d");
        }

        private void Arrow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("a");
        }
    }

    public class MyPopup : Popup
    {
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            bool isOpen = this.IsOpen;
            base.OnPreviewMouseLeftButtonDown(e);

            if (isOpen && !this.IsOpen)
                e.Handled = true;
        }
    }

}

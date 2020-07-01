using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Xml;
using Xceed.Wpf.Toolkit;

namespace MyDemo
{
    /// <summary>
    /// MyTemplateBrower.xaml 的交互逻辑
    /// </summary>
    public partial class MyTemplateBrower : UserControl
    {
        public MyTemplateBrower()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var controlType = typeof(Control);

            var types = new List<Type>();

            foreach (var type in Assembly.GetAssembly(controlType).GetTypes()
                .Where(x => x.IsSubclassOf(controlType) && !x.IsAbstract && x.IsPublic))
            {
                types.Add(type);
            }

            types.OrderBy(x => x.Name);

            lsbControls.ItemsSource = types;
        }

        private void lsbControls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var controlType = (Type)lsbControls.SelectedItem;
                var info = controlType.GetConstructor(System.Type.EmptyTypes);

                var control = (Control)info.Invoke(null);
                control.Visibility = Visibility.Collapsed;
                grid1.Children.Add(control);

                var template = control.Template;
                var settings = new XmlWriterSettings();
                settings.Indent = true;

                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb, settings))
                {
                    XamlWriter.Save(template, writer);

                    tbTemplate.Text = sb.ToString();

                    grid1.Children.Remove(control);
                }
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString());
            }

        }
    }
}

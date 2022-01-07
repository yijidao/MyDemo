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
using ConfigDemo.ViewModels;

namespace ConfigDemo.Views
{
    /// <summary>
    /// ConfigView.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();
        }
    }

    public class ConfigItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WebDataTemplate { get; set; }

        public DataTemplate CommonDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case WebConfigSection webConfigSection:
                    return WebDataTemplate;
                case CommonConfigSection commonConfigSection:
                    return CommonDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}

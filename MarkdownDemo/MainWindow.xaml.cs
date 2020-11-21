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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MarkdownSharp;

namespace MarkdownDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //var reader = XamlReader.Parse(markdown.Transform(writer.Text));
            var html = MDConverter.Transform(writer.Text);
            reader.NavigateToString(html);

        }

        public Markdown MDConverter { get; set; } = new Markdown();

        private void Writer_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var html = MDConverter.Transform(writer.Text);
            reader?.NavigateToString(html);
        }
    }
}

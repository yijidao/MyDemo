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
    /// CardDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CardDemo : UserControl
    {
        public CardDemo()
        {
            InitializeComponent();

            var cardModel = new CardModel
            {
                Title = "1号线综合监控",
            };

            for (int i = 1; i <= 16; i++)
            {
                cardModel.CardItems.Add($"Item{i}");
            }

            container.DataContext = cardModel;

        }
    }

    public class CardModel 
    {
        public string Title { get; set; }
        public List<string> CardItems { get; set; } = new List<string>();
    }
}

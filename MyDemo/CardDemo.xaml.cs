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
using System.Windows.Media.Effects;
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

            var list = Create(cardModel, 7);
            Grid.SetRow(list, 1);
            container.Children.Add(list);
        }

        /// <summary>
        /// 根据列动态生成卡片布局
        /// </summary>
        /// <param name="cardModel"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Grid Create(CardModel cardModel, int column = 5)
        {
            var count = cardModel.CardItems.Count;
            var row = (count / column + (count % column == 0 ? 0 : 1)) * 2;

            var grid = new Grid
            {
                Margin = new Thickness(12, 0, 12, 12)
            };
            for (int i = 0; i < column; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < row; i++)
            {
                var value = new RowDefinition
                {
                    Height = new GridLength(32),

                };
                grid.RowDefinitions.Add(value);
                var backBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(15, 59, 89))
                };

                Grid.SetColumnSpan(backBorder, column);
                Grid.SetRow(backBorder, i / 2 * 2);

                grid.Children.Add(backBorder);
            }

            for (int i = 0; i < cardModel.CardItems.Count; i++)
            {
                var item = cardModel.CardItems[i];
                var textBlock = new TextBlock
                {
                    Text = item,
                    VerticalAlignment = VerticalAlignment.Center

                };
                Grid.SetRow(textBlock, i / column * 2); // 1/5 =0;(0) 6/5=1;(2) 11 / 5 = 2;(4) 16/5 = 3;(6)
                Grid.SetColumn(textBlock, i % column);
                grid.Children.Add(textBlock);

                var border = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(62, 196, 26)),
                    Width = 12,
                    Height = 12,
                    CornerRadius = new CornerRadius(6),
                    Effect = new BlurEffect() { KernelType = KernelType.Gaussian, Radius = 2 }
                };
                Grid.SetRow(border, i / column * 2 + 1);
                Grid.SetColumn(border, i % column);
                grid.Children.Add(border);
            }

            return grid;
        }

        public void CreateCardItem(string itemName)
        {


        }

    }

    public class CardModel
    {
        public string Title { get; set; }
        public List<string> CardItems { get; set; } = new List<string>();
    }
}

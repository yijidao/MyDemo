using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace MyWPFControl
{
    /// <summary>
    /// PCICarouse.xaml 的交互逻辑
    /// </summary>
    public partial class PCICarouse : Selector
    {
        public PCICarouse()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ContentPanel = GetTemplateChild("ContentPanel") as StackPanel;
            RadioPanel = GetTemplateChild("radioPanel") as StackPanel;
            ButtonPanel = GetTemplateChild("ButtonPanel") as StackPanel;
            ButtonPanel.MouseLeftButtonDown += ButtonPanel_MouseLeftButtonDown;

            var prevPage = GetTemplateChild("PrevPage") as Border;

            prevPage.MouseLeftButtonDown += Border_MouseDown;

            var NextPage = GetTemplateChild("NextPage") as Border;
            NextPage.MouseLeftButtonDown += Border_MouseDown_1;

            if(ItemsSource != null)
            {
                GenerateItems(ItemsSource);
                PageIndex = 0;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (ItemsSource != null && ContentPanel!=null)
            {
                GenerateItems(ItemsSource);
                PageIndex = 0;
            }
        }

        private void GenerateItems(IEnumerable list)
        {
            foreach (var item in list)
            {
                var c = new ContentControl();
                c.ContentTemplate = ItemTemplate;
                c.Content = item;
                var itemPanel = new Grid();
                itemPanel.Children.Add(c);
                itemPanel.Width = Width;
                itemPanel.Height = Height;
                ContentPanel.Children.Add(itemPanel);

                var pageButton = new RadioButton();
                pageButton.Click += (s, e) =>
                {
                    var index = RadioPanel.Children.IndexOf(e.OriginalSource as RadioButton);
                    PageIndex = index;

                };
                RadioPanel.Children.Add(pageButton);
            }
        }

        private StackPanel ContentPanel { get; set; }
        private StackPanel RadioPanel { get; set; }
        private StackPanel ButtonPanel { get; set; }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                GenerateItems(e.NewItems);
                if (PageIndex == -1)
                {
                    PageIndex = 0;
                }
            }
        }

        private int _PageIndex = -1;

        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                _PageIndex = value;
                if (RadioPanel?.Children.Count > value)
                {
                    (RadioPanel.Children[value] as RadioButton).IsChecked = true;
                }
                ContentPanel.Margin = new Thickness(-ContentPanel.Children.Cast<Grid>().Take(value).Sum(x => x.DesiredSize.Width), 0, 0, 0);
                if (Items?.Count > value)
                {
                    SelectedItem = Items[value];
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PageIndex == 0)
            {
                PageIndex = Items.Count - 1;
            }
            else
            {
                PageIndex--;
            }
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (PageIndex == Items.Count - 1)
            {
                PageIndex = 0;
            }
            else
            {
                PageIndex++;
            }
        }

        private void ButtonPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is RadioButton pageButton)
            {
                var index = ButtonPanel.Children.IndexOf(pageButton);
                PageIndex = (index - 1);
            }
        }

    }
}

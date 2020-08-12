using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace MyWPFControl
{
    /// <summary>
    /// PCICarousel1.xaml 的交互逻辑
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    public partial class PCICarousel1 : UserControl
    {
        public PCICarousel1()
        {
            InitializeComponent();

            Items = new ObservableCollection<FrameworkElement>();
            Items.CollectionChanged += (s, e) =>
            {
                if (e.NewItems?.Count > 0)
                {
                    foreach (var item in e.NewItems)
                    {

                        var panel = new Grid { Width = this.Width };
                        panel.Children.Add((FrameworkElement)item);
                        ContentPanel.Children.Add(panel);

                        var pageButton = new RadioButton();
                        pageButton.Click += (s, e) =>
                        {
                            var index = radioPanel.Children.IndexOf(e.OriginalSource as RadioButton);
                            PageIndex = index;

                        };
                        radioPanel.Children.Add(pageButton);
                    }
                    PageIndex = 0;
                }
            };
        }



        //public ObservableCollection<FrameworkElement> Source
        //{
        //    get { return (ObservableCollection<FrameworkElement>)GetValue(SourceProperty); }
        //    set { SetValue(SourceProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SourceProperty =
        //    DependencyProperty.Register("Source", typeof(ObservableCollection<FrameworkElement>), typeof(ownerclass), new PropertyMetadata(0));





        public IEnumerable<RadioButton> RadioButtonList { get; set; }


        public ObservableCollection<FrameworkElement> Items { get; private set; }


        public FrameworkElement CurrentItem
        {
            get { return (FrameworkElement)GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem", typeof(FrameworkElement), typeof(PCICarousel1), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private int _PageIndex = -1;

        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                _PageIndex = value;
                if (radioPanel?.Children.Count > value)
                {
                    (radioPanel.Children[value] as RadioButton).IsChecked = true;
                }
                ContentPanel.Margin = new Thickness(-ContentPanel.Children.Cast<Grid>().Take(value).Sum(x => x.DesiredSize.Width), 0, 0, 0);
                if (Items?.Count > value)
                {
                    CurrentItem = Items[value];
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

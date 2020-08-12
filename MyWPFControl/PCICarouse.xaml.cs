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
    public partial class PCICarouse : UserControl
    {
        public PCICarouse()
        {
            InitializeComponent();

        }

        public ObservableCollection<object> ItemsSource
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<object>), typeof(PCICarouse), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (d, e) =>
                {
                    if (e.NewValue is ObservableCollection<object> list)
                    {
                        var ctl = (PCICarouse)d;


                        foreach (var item in list)
                        {
                            //var panel = new Grid { Width = ctl.Width };
                            //panel.Children.Add((FrameworkElement)item);
                            var contentControl = new ContentControl();
                            contentControl.DataContext = item;
                            if(ctl.ItemTemplate != null)
                            {
                                contentControl.ContentTemplate = ctl.ItemTemplate;
                            }
                            ctl.ContentPanel.Children.Add(contentControl);
                        }

                        //list.CollectionChanged += ctl.ItemsSourceCollectionChanged;

                        var pageButton = new RadioButton();
                        pageButton.Click += (s, e) =>
                        {
                            var index = ctl.radioPanel.Children.IndexOf(e.OriginalSource as RadioButton);
                            ctl.PageIndex = index;

                        };
                        ctl.radioPanel.Children.Add(pageButton);
                        ctl.PageIndex = 0;
                    }
                }));


        //private void ItemsSourceCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems?.Count > 0)
        //    {
        //        foreach (var item in e.NewItems)
        //        {

        //            //var panel = new Grid { Width = this.Width };
        //            //panel.Children.Add((FrameworkElement)item);
        //            var contentControl = new ContentControl();
        //            contentControl.DataContext = item;

        //            ContentPanel.Children.Add(contentControl);

        //            var pageButton = new RadioButton();
        //            pageButton.Click += (s, e) =>
        //            {
        //                var index = radioPanel.Children.IndexOf(e.OriginalSource as RadioButton);
        //                PageIndex = index;

        //            };
        //            radioPanel.Children.Add(pageButton);
        //        }
                
        //    }
        //}

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PCICarouse), new PropertyMetadata(null));



        public object CurrentItem
        {
            get { return (object)GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem", typeof(object), typeof(PCICarouse), new PropertyMetadata(null));

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
                if (ItemsSource?.Count > value)
                {
                    CurrentItem = ItemsSource[value];
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PageIndex == 0)
            {
                PageIndex = ItemsSource.Count - 1;
            }
            else
            {
                PageIndex--;
            }
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (PageIndex == ItemsSource.Count - 1)
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

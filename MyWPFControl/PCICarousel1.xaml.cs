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
    public partial class PCICarousel1 : UserControl
    {
        public PCICarousel1()
        {
            InitializeComponent();
            init();

            void init()
            {
                foreach (var item in Items)
                {
                    ContentPanel.Children.Add(item);
                }
                CurrentItem = Items.FirstOrDefault();
                PageIndex = 0;

            }
        }

        public IEnumerable<RadioButton> RadioButtonList { get; set; }

        public ObservableCollection<FrameworkElement> Items
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<FrameworkElement>), typeof(PCICarousel1), new PropertyMetadata(new ObservableCollection<FrameworkElement>
            {
                new Rectangle{ Width=400, Fill= new SolidColorBrush( Colors.Yellow) },
                new Rectangle{ Width=400, Fill= new SolidColorBrush( Colors.Blue) },
                new Rectangle{ Width=400, Fill= new SolidColorBrush( Colors.Green) },
                new Rectangle{ Width=400, Fill= new SolidColorBrush( Colors.Red) },
            }));



        public FrameworkElement CurrentItem
        {
            get { return (FrameworkElement)GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem", typeof(FrameworkElement), typeof(PCICarousel1), new PropertyMetadata(null));




        private int _PageIndex = -1;

        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                _PageIndex = value;

                (radioPanel.Children[value] as RadioButton).IsChecked = true;

                
                ContentPanel.Margin = new Thickness(-Items.Take(value).Sum(x => x.DesiredSize.Width), 0, 0, 0);
            }
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PageIndex == 0)
            {
                PageIndex = Items.Count - 1;
                //ContentPanel.Margin = new Thickness(-Items.Sum(x => x.DesiredSize.Width) + Items.FirstOrDefault().DesiredSize.Width, 0, 0, 0);
            }
            else
            {
                PageIndex--;
                //ContentPanel.Margin = new Thickness(ContentPanel.Margin.Left + CurrentItem.DesiredSize.Width, 0, 0, 0);
            }
            CurrentItem = Items[PageIndex];
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {



            if (PageIndex == Items.Count - 1)
            {
                PageIndex = 0;
                //ContentPanel.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                PageIndex++;
                //ContentPanel.Margin = new Thickness(ContentPanel.Margin.Left - CurrentItem.DesiredSize.Width, 0, 0, 0);
            }
            CurrentItem = Items[PageIndex];

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

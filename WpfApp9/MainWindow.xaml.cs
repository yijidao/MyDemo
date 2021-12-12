using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Prism.Mvvm;

namespace WpfApp9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            DataContext = vm;
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var btn = (Button)e.OriginalSource;
        //    //
        //    var p = btn.TransformToAncestor(this).Transform(new Point(0, 0));

        //    var window = new Window
        //    {
        //        Width = 100,
        //        Height = 100,
        //        Background = Brushes.LightBlue,
        //        Owner = this,
        //        Top = this.Top + p.Y - btn.Height,
        //        Left = this.Left + p.X
        //    };

        //    window.Show();
        //}


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            var btn = (Button)e.OriginalSource;
            var p = btn.TransformToAncestor(grid).Transform(new Point(0, 0));

            var b = new Border
            {
                Width = 100,
                Height = 100,
                Background = Brushes.LightBlue,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(p.X, p.Y - 100, 0, 0)

            };
            grid.Children.Add(b);

        }
    }

    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<ItemModel> _items;
        public ObservableCollection<ItemModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public MainWindowViewModel()
        {
            var list = new List<ItemModel>();

            //Items = new ObservableCollection<ItemModel>();

            for (int i = 1; i <= 10; i++)
            {
                list.Add(new ItemModel
                {
                    StationName = $"车站{i}",
                    PopupItem = $"详细内容{i}"
                });
            }

            Items = new ObservableCollection<ItemModel>(list);
        }

    }
    public class ItemModel : BindableBase
    {
        private string? _stationName;

        public string? StationName
        {
            get => _stationName;
            set => SetProperty(ref _stationName, value);
        }

        private string? _popupItem;

        public string? PopupItem
        {
            get => _popupItem;
            set => SetProperty(ref _popupItem, value);
        }

        private bool _showPopup;
        public bool ShowPopup
        {
            get => _showPopup;
            set => SetProperty(ref _showPopup, value);
        }
    }
}



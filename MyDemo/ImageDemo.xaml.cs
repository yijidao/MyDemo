using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// ImageDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ImageDemo : UserControl
    {
        public ImageDemo()
        {
            InitializeComponent();

            //LoadImage("https://i.pinimg.com/474x/bf/1f/4e/bf1f4e5c43193730f356dd73e45a524e.jpg");

            //ImageSources = GetImageSource(Links);
            //images.ItemsSource = ImageSources;
            images.ItemsSource = Links;
        }

        public string[] Links { get; set; } = new[]
        {
            "https://i.pinimg.com/474x/bf/1f/4e/bf1f4e5c43193730f356dd73e45a524e.jpg",
            "https://i.pinimg.com/474x/bf/1f/4e/bf1f4e5c43193730f356dd73e45a524e.jpg",
            "https://i.pinimg.com/474x/bf/1f/4e/bf1f4e5c43193730f356dd73e45a524e.jpg",
            "https://i.pinimg.com/474x/bf/1f/4e/bf1f4e5c43193730f356dd73e45a524e.jpg"
        };

        public IEnumerable<BitmapImage> ImageSources { get; set; }

        private IEnumerable<BitmapImage> GetImageSource(IEnumerable<string> links)
        {
            return links.Select(x =>
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(x, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                return bitmapImage;
            }).ToArray();
        }

        private void LoadImage(string url)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            //image.Source = bitmapImage;
        }
    }
}

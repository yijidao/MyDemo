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
using MyPrismDemo.Views;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using VlcPrismModule.Views;

namespace MyPrismDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var manager = (Application.Current as PrismApplication)?.Container.Resolve<IRegionManager>();
            var contentRegion = "ContentRegion";
            regionButton1.Click += (sender, args) => manager?.RequestNavigate(contentRegion, nameof(MainRegionView));
            regionButton2.Click += (sender, args) => manager?.RequestNavigate(contentRegion, nameof(DetailRegionView));
            vlcRegion.Click += (sender, args) => manager?.RequestNavigate(contentRegion, nameof(MediaPlayerView));
            monitorRegion.Click += (sender, args) => manager?.RequestNavigate(contentRegion, nameof(MonitorView));
            moq.Click += (sender, args) => manager?.RequestNavigate(contentRegion, nameof(MoqView));
        }

    }
}

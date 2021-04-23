using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace MyPrismDemo.Views
{
    /// <summary>
    /// RenderView.xaml 的交互逻辑
    /// </summary>
    public partial class RenderView : UserControl
    {
        public EllipseBounce[] Ellipses { get; set; }
        public DispatcherTimer UpdateTimer { get; set; }

        public RenderView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Ellipses = new EllipseBounce[100];

            var stage = new Rect(0, 0, 1000, 500);
            var rand = new Random();
            for (int i = 0; i < Ellipses.Length; i++)
            {

                var posX = (float)(rand.NextDouble() * stage.Width + stage.X);
                var posY = (float)(rand.NextDouble() * stage.Height + stage.Y);
                var pos = new Point(posX, posY);

                var velX = (float)(rand.NextDouble() * 5 - 2.5);
                var velY = (float)(rand.NextDouble() * 5 - 2.5);
                var vel = new Point(velX, velY);
                Ellipses[i] = new EllipseBounce(stage, pos, vel, 2);
            }

            var rd = new Render(Ellipses);
            grid.Children.Add(rd);
            UpdateTimer = new DispatcherTimer();
            UpdateTimer.Tick += UpdateTimerOnTick;
            UpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60); // fps60
            UpdateTimer.Start();
        }

        private void UpdateTimerOnTick(object sender, EventArgs e)
        {
            foreach (var item in Ellipses)
            {
                item.Update();
            }
        }
    }
}

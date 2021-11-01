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
    /// RouterDemo.xaml 的交互逻辑
    /// </summary>
    //[DefaultProperty("Content")]
    //[System.Windows.Markup.ContentProperty("Content")]
    public partial class RouterDemo : UserControl
    {
        public RouterDemo()
        {
            InitializeComponent();
        }





    }

    public class Router : ContentControl
    {

    }
    [System.Windows.Markup.ContentProperty("Target")]
    public class Route : ContentControl
    {


        public AuthorityRoute Authority
        {
            get { return (AuthorityRoute)GetValue(AuthorityProperty); }
            set { SetValue(AuthorityProperty, value); }
        }

        public static readonly DependencyProperty AuthorityProperty =
            DependencyProperty.Register("Authority", typeof(AuthorityRoute), typeof(Route), new PropertyMetadata(AuthorityRoute.None));



        public DependencyObject Target
        {
            get { return (DependencyObject)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(DependencyObject), typeof(Route));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            switch (Authority)
            {
                case AuthorityRoute.LineNet:
                    //route.Content = args.NewValue;
                    break;
                case AuthorityRoute.Line:
                    Content = Target;

                    break;
                case AuthorityRoute.Station:
                    break;
            }
        }
    }

    public enum AuthorityRoute
    {
        None,
        LineNet,
        Line,
        Station
        
    }
}

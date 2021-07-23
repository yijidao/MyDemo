using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Primitives;

namespace MyDemo
{
    public abstract class ChatBubble : ContentControl
    {
        public static readonly DependencyProperty ProfilePictureProperty =
            DependencyProperty.Register("ProfilePicture", typeof(ImageSource), typeof(ChatBubble), new PropertyMetadata(null));

        public static readonly DependencyProperty UpdateTimeProperty =
            DependencyProperty.Register("UpdateTime", typeof(DateTime), typeof(ChatBubble), new PropertyMetadata(DateTime.Now));


        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(ChatBubble), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 用户头像
        /// </summary>
        public ImageSource ProfilePicture
        {
            get => (ImageSource)GetValue(ProfilePictureProperty);
            set => SetValue(ProfilePictureProperty, value);
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get => (DateTime)GetValue(UpdateTimeProperty);
            set => SetValue(UpdateTimeProperty, value);
        }


        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
    }

    public class ReceiveChatBubble : ChatBubble
    {
        #region 图标

        private static DrawingImage helpful = new DrawingImage
        {
            Drawing = new GeometryDrawing
            {
                Brush = Brushes.White,
                Geometry = new PathGeometry
                {
                    Figures = PathFigureCollection.Parse(
                        "M214.558094 410.41984c5.798867 0.199961 40.692052 2.899434 40.692052 41.391916V1001.804335c-0.299941 3.999219-2.999414 22.195665-29.094317 22.195665H84.083577c-5.198985-0.299941-33.993361-3.099395-33.99336-34.693224L16.296817 443.513376s0.499902-33.093536 34.193322-33.093536h164.067955zM593.983988 0.09998S772.449131 19.996095 666.769772 410.219879h273.346612c7.098614-0.399922 67.78676-1.499707 67.78676 65.58719v0.799844c-0.299941 10.297989-5.198985 119.376684-101.280219 446.312829l-1.599687 4.899043c-6.898653 20.296036-34.893185 96.181215-68.286663 96.181215H426.916618c-28.594415 0-101.380199-19.196251-101.380199-72.685804V410.219879S528.196837 260.749072 528.196837 69.88635C533.995704 59.888303 518.29877 0.09998 593.983988 0.09998z")
                }
            }
        };
        private static DrawingImage unhelpful = new DrawingImage
        {
            Drawing = new GeometryDrawing
            {
                Brush = Brushes.White,
                Geometry = new PathGeometry
                {
                    Figures = PathFigureCollection.Parse(
                        "M809.57094 613.740064c-5.799434-0.19998-40.696026-2.899717-40.696025-41.395957V22.297822c0.299971-3.999609 2.999707-22.197832 29.097158-22.197832h142.086124c5.199492 0.299971 33.99668 3.099697 33.99668 34.696612l33.69671 545.746704s-0.499951 33.096768-34.196661 33.096768l-163.983986 0.09999z m-379.362952 410.259936s-178.48257-19.898057-72.792892-410.159945L84.041793 613.940045c-7.099307 0.399961-67.79338 1.499854-67.79338-65.593594v-0.799922c0.299971-10.298994 5.199492-119.388341 101.290109-446.356411l1.599843-4.899521C126.037692 75.992579 154.034958 0.09999 187.431696 0.09999h409.659994c28.597207 0 101.390099 19.198125 101.390099 72.692901V613.940045S495.801582 763.425447 495.801582 954.306806c-5.699443 9.899033 9.999024 69.693194-65.593594 69.693194z")
                }
            }
        };

        private static DrawingImage helpfulHighlignt = new DrawingImage
        {
            Drawing = new GeometryDrawing
            {
                Brush = new SolidColorBrush((Color)(ColorConverter.ConvertFromString("#F8E71C"))),
                Geometry = new PathGeometry
                {
                    Figures = PathFigureCollection.Parse(
                        "M214.558094 410.41984c5.798867 0.199961 40.692052 2.899434 40.692052 41.391916V1001.804335c-0.299941 3.999219-2.999414 22.195665-29.094317 22.195665H84.083577c-5.198985-0.299941-33.993361-3.099395-33.99336-34.693224L16.296817 443.513376s0.499902-33.093536 34.193322-33.093536h164.067955zM593.983988 0.09998S772.449131 19.996095 666.769772 410.219879h273.346612c7.098614-0.399922 67.78676-1.499707 67.78676 65.58719v0.799844c-0.299941 10.297989-5.198985 119.376684-101.280219 446.312829l-1.599687 4.899043c-6.898653 20.296036-34.893185 96.181215-68.286663 96.181215H426.916618c-28.594415 0-101.380199-19.196251-101.380199-72.685804V410.219879S528.196837 260.749072 528.196837 69.88635C533.995704 59.888303 518.29877 0.09998 593.983988 0.09998z")
                }
            }
        };
        private static DrawingImage unhelpfulHighlignt = new DrawingImage
        {
            Drawing = new GeometryDrawing
            {
                Brush = new SolidColorBrush((Color)(ColorConverter.ConvertFromString("#F8E71C"))),
                Geometry = new PathGeometry
                {
                    Figures = PathFigureCollection.Parse(
                        "M809.57094 613.740064c-5.799434-0.19998-40.696026-2.899717-40.696025-41.395957V22.297822c0.299971-3.999609 2.999707-22.197832 29.097158-22.197832h142.086124c5.199492 0.299971 33.99668 3.099697 33.99668 34.696612l33.69671 545.746704s-0.499951 33.096768-34.196661 33.096768l-163.983986 0.09999z m-379.362952 410.259936s-178.48257-19.898057-72.792892-410.159945L84.041793 613.940045c-7.099307 0.399961-67.79338 1.499854-67.79338-65.593594v-0.799922c0.299971-10.298994 5.199492-119.388341 101.290109-446.356411l1.599843-4.899521C126.037692 75.992579 154.034958 0.09999 187.431696 0.09999h409.659994c28.597207 0 101.390099 19.198125 101.390099 72.692901V613.940045S495.801582 763.425447 495.801582 954.306806c-5.699443 9.899033 9.999024 69.693194-65.593594 69.693194z")
                }
            }
        };

        private static DrawingImage profilePicture = new DrawingImage
        {
            Drawing = new GeometryDrawing
            {
                Brush = Brushes.White,
                Geometry = new PathGeometry
                {
                    Figures = PathFigureCollection.Parse(
                        "M118.3 354.6V591c0 32.6-26.5 59.1-59.1 59.1S0.1 623.7 0.1 591V354.6c0-32.6 26.5-59.1 59.1-59.1s59.1 26.4 59.1 59.1z m905.8 0V591c0 32.6-26.4 59.1-59.1 59.1s-59.1-26.4-59.1-59.1V354.6c0-32.6 26.4-59.1 59.1-59.1s59.1 26.4 59.1 59.1zM382.9 846.9h258.3c17 0 32 10.8 37.4 26.9l13.1 39.4c6.9 20.6-4.3 42.9-24.9 49.8-4 1.3-8.2 2-12.5 2H369.8c-21.8 0-39.4-17.6-39.4-39.4 0-4.2 0.7-8.4 2-12.5l13.1-39.4c5.4-16 20.5-26.8 37.4-26.8z m341.9-472.6H299.4c-13.1 0-23.6 10.6-23.6 23.6v70.9c0 13.1 10.6 23.6 23.6 23.6h425.4c13.1 0 23.6-10.6 23.6-23.6v-70.9c0-13-10.6-23.6-23.6-23.6zM351 59.2v119.5h322.2V59.2h64.4v119.5c34.2 0 67 12.5 91.1 34.9 24.2 22.4 37.8 52.8 37.8 84.5v358.4c0 66-57.7 119.5-128.9 119.5H286.5c-34.2 0-66.9-12.5-91.1-34.9-24.2-22.4-37.8-52.8-37.8-84.5V298.1c0-66 57.7-119.5 128.9-119.5V59.2H351z")
                }
            }
        };

        #endregion

        static ReceiveChatBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReceiveChatBubble), new FrameworkPropertyMetadata(typeof(ReceiveChatBubble)));
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ReceiveChatBubble), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsHelpfulProperty =
            DependencyProperty.Register("IsHelpful", typeof(bool?), typeof(ReceiveChatBubble), new PropertyMetadata(null));

        public static readonly DependencyProperty HelpfulIconProperty =
            DependencyProperty.Register("HelpfulIcon", typeof(DrawingImage), typeof(ReceiveChatBubble), new PropertyMetadata(helpful));

        public static readonly DependencyProperty UnhelpfulIconProperty =
            DependencyProperty.Register("UnhelpfulIcon", typeof(DrawingImage), typeof(ReceiveChatBubble), new PropertyMetadata(unhelpful));

        public static readonly DependencyProperty HelpfulCommandProperty =
            DependencyProperty.Register("HelpfulCommand", typeof(ICommand), typeof(ReceiveChatBubble), new PropertyMetadata(null));

        public static readonly DependencyProperty UnhelpfulCommandProperty =
            DependencyProperty.Register("UnhelpfulCommand", typeof(ICommand), typeof(ReceiveChatBubble), new PropertyMetadata(null));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// 是否有帮助
        /// </summary>
        public bool? IsHelpful
        {
            get => (bool?)GetValue(IsHelpfulProperty);
            set => SetValue(IsHelpfulProperty, value);
        }

        /// <summary>
        /// 有帮助的图标
        /// </summary>
        public DrawingImage HelpfulIcon
        {
            get => (DrawingImage)GetValue(HelpfulIconProperty);
            set => SetValue(HelpfulIconProperty, value);
        }

        /// <summary>
        /// 没有帮助的图标
        /// </summary>
        public DrawingImage UnhelpfulIcon
        {
            get => (DrawingImage)GetValue(UnhelpfulIconProperty);
            set => SetValue(UnhelpfulIconProperty, value);
        }


        public ICommand HelpfulCommand
        {
            get => (ICommand)GetValue(HelpfulCommandProperty);
            set => SetValue(HelpfulCommandProperty, value);
        }

        public ICommand UnhelpfulCommand
        {
            get => (ICommand)GetValue(UnhelpfulCommandProperty);
            set => SetValue(UnhelpfulCommandProperty, value);
        }

        private UIElement _helpfulImage;
        private UIElement _unhelpfulImage;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ProfilePicture = profilePicture;

            if (_helpfulImage != null) _helpfulImage.MouseLeftButtonDown -= HelpfulImageClick;
            if (_unhelpfulImage != null) _unhelpfulImage.MouseLeftButtonDown -= UnhelpfulImageClick;
            if (GetTemplateChild("helpfulImage") is UIElement element)
            {
                _helpfulImage = element;
                _helpfulImage.MouseLeftButtonDown += HelpfulImageClick;
            }

            if (GetTemplateChild("unhelpfulImage") is UIElement element2)
            {
                _unhelpfulImage = element2;
                _unhelpfulImage.MouseLeftButtonDown += UnhelpfulImageClick;
            }
        }


        private void HelpfulImageClick(object sender, MouseButtonEventArgs e)
        {
            if (IsHelpful != null) return;
            HelpfulCommand?.Execute(null);
            IsHelpful = true;
            HelpfulIcon = helpfulHighlignt;
        }
        private void UnhelpfulImageClick(object sender, MouseButtonEventArgs e)
        {
            if (IsHelpful != null) return;
            UnhelpfulCommand?.Execute(null);
            IsHelpful = false;
            UnhelpfulIcon = unhelpfulHighlignt;
        }

    }

    public class SendChatBubble : ChatBubble
    {
        static SendChatBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SendChatBubble), new FrameworkPropertyMetadata(typeof(SendChatBubble)));
        }

        public static readonly DependencyProperty ArrivedProperty =
            DependencyProperty.Register("Arrived", typeof(bool), typeof(SendChatBubble), new PropertyMetadata(false));

        /// <summary>
        /// 已送达
        /// </summary>
        public bool Arrived
        {
            get => (bool)GetValue(ArrivedProperty);
            set => SetValue(ArrivedProperty, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Primitives;

namespace MyDemo
{
    public abstract class ChatBubble : ContentControl
    {
        public static readonly DependencyProperty ProfilePictureProperty =
            DependencyProperty.Register("ProfilePicture", typeof(Image), typeof(ChatBubble), new PropertyMetadata(null));

        public static readonly DependencyProperty UpdateTimeProperty =
            DependencyProperty.Register("UpdateTime", typeof(DateTime), typeof(ChatBubble), new PropertyMetadata(DateTime.Now));


        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(ChatBubble), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 用户头像
        /// </summary>
        public Image ProfilePicture
        {
            get => (Image)GetValue(ProfilePictureProperty);
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
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ReceiveChatBubble), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsHelpfulProperty =
            DependencyProperty.Register("IsHelpful", typeof(bool?), typeof(ReceiveChatBubble), new PropertyMetadata(null));

        public static readonly DependencyProperty HelpfulIconProperty =
            DependencyProperty.Register("HelpfulIcon", typeof(Image), typeof(ReceiveChatBubble), new PropertyMetadata(null));

        public static readonly DependencyProperty UnhelpfulIconProperty =
            DependencyProperty.Register("UnhelpfulIcon", typeof(Image), typeof(ReceiveChatBubble), new PropertyMetadata(null));

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
        public Image HelpfulIcon
        {
            get => (Image)GetValue(HelpfulIconProperty);
            set => SetValue(HelpfulIconProperty, value);
        }

        /// <summary>
        /// 没有帮助的图标
        /// </summary>
        public Image UnhelpfulIcon
        {
            get => (Image)GetValue(UnhelpfulIconProperty);
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
    }

    public class SendChatBubble : ChatBubble
    {
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

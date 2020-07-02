using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MyDemo
{

    [TemplateVisualState(Name = "Normal", GroupName = "ViewStates")]
    [TemplateVisualState(Name = "Flipped", GroupName = "ViewStates")]
    [TemplatePart(Name = "FlipButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "FlipButtonAlternate", Type = typeof(ToggleButton))]
    class MyFlipPanel : Control
    {
        static MyFlipPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyFlipPanel), new FrameworkPropertyMetadata(typeof(MyFlipPanel)));
        }

        public object FrontContent
        {
            get { return (object)GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(object), typeof(MyFlipPanel), null);

        public object BackContent
        {
            get { return (object)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(MyFlipPanel), null);



        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set
            {
                SetValue(IsFlippedProperty, value);
                ChangeVisualState(true);
            }
        }

        // Using a DependencyProperty as the backing store for IsFilped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFlippedProperty =
            DependencyProperty.Register("IsFlipped", typeof(bool), typeof(MyFlipPanel), new PropertyMetadata(null));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MyFlipPanel), null);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("FlipButton") is ToggleButton filpButton)
            {
                filpButton.Click += OnFlipButtonClick;
            }
            ChangeVisualState(false);

            void OnFlipButtonClick(object sender, RoutedEventArgs e)
            {
                IsFlipped = !IsFlipped;
                ChangeVisualState(true);
            }
        }

        /// <summary>
        /// 从一个状态变化到另一个状态
        /// </summary>
        /// <param name="useTransitions">是否应用过渡</param>
        private void ChangeVisualState(bool useTransitions)
        {
            if (!IsFlipped)
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);
            }
        }

    }
}

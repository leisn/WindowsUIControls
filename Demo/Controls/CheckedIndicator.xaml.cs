using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Demo.Controls
{
    public sealed partial class CheckedIndicator : UserControl
    {
        public static readonly DependencyProperty IsActiveProperty =
           DependencyProperty.Register(
               "IsActive", typeof(bool), typeof(CheckedIndicator),
               new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChanged)));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        static void IsActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((CheckedIndicator)sender).OnIsActiveChanged(Convert.ToBoolean(e.NewValue));
        }

        Storyboard animation;
        public CheckedIndicator()
        {
            this.InitializeComponent();
            animation = (Storyboard)Resources["storyBoard"];
            animation.Completed += Animation_Completed;
        }

        private void Animation_Completed(object sender, object e)
        {
            IsActive = false;
            if (animation.RepeatBehavior.HasCount)
                Root.Visibility = Visibility.Collapsed;
            animation.RepeatBehavior = RepeatBehavior.Forever;
        }

        public void AnimateOnce()
        {
            animation.RepeatBehavior = new RepeatBehavior(1);
            IsActive = true;
        }

        void OnIsActiveChanged(bool newValue)
        {
            if (newValue)
            {
                animation.Begin();
            }
            else
            {
                animation.Stop();
            }
        }
    }
}

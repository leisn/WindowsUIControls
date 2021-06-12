using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Leisn.UI.Xaml.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "reduceButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "increaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_Title", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Value", Type = typeof(TextBlock))]
    public class NumberBox : Control
    {
        const string NormalState = "Normal";
        const string PointerOverState = "PointerOver";
        const string TextEditorState = "TextEditor";
        const string DisabledState = "Disabled";

        RepeatButton reduceButton;
        RepeatButton increaseButton;
        TextBox textBox;

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NumberBox),
                new PropertyMetadata(""));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumberBox), new PropertyMetadata(0d));


        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(NumberBox), new PropertyMetadata(1d));


        public NumberBox()
        {
            this.DefaultStyleKey = typeof(NumberBox);
        }

        protected override void OnApplyTemplate()
        {
            textBox = GetTemplateChild("PART_TextBox") as TextBox;
            reduceButton = GetTemplateChild("reduceButton") as RepeatButton;
            increaseButton = GetTemplateChild("increaseButton") as RepeatButton;

            reduceButton.Click -= this.ReduceButton_Click;
            increaseButton.Click -= this.IncreaseButton_Click;

            base.OnApplyTemplate();

            reduceButton.Click += this.ReduceButton_Click;
            increaseButton.Click += this.IncreaseButton_Click;

            UpdateState();
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

        private void ReduceButton_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }


        #region Visual states
        bool isEditorOn = false;
        bool isMouseOver = false;
        void UpdateState(bool useTransitions = false)
        {
            if (IsEnabled)
            {
                if (isEditorOn)
                {
                    bool visable = textBox.Visibility == Visibility.Visible;
                    if (VisualStateManager.GoToState(this, TextEditorState, useTransitions)
                        && !visable)
                    {
                        textBox.Focus(FocusState.Programmatic);
                        textBox.SelectAll();
                    }
                }
                else if (isMouseOver)
                    VisualStateManager.GoToState(this, PointerOverState, useTransitions);
                else
                    VisualStateManager.GoToState(this, NormalState, useTransitions);
                return;
            }
            VisualStateManager.GoToState(this, DisabledState, useTransitions);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            isEditorOn = false;
            UpdateState();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.Handled)
                return;

            var rp = e.GetPosition(reduceButton);
            var ip = e.GetPosition(increaseButton);
            var reduceSize = reduceButton.ActualSize;
            var increaseSize = increaseButton.ActualSize;
            if ((rp.X < 0 || rp.X > reduceSize.X || rp.Y < 0 || rp.Y > reduceSize.Y)
                && (ip.X < 0 || ip.X > increaseSize.X || ip.Y < 0 || ip.Y > increaseSize.Y))
            {
                isEditorOn = true;
                UpdateState();
                e.Handled = true;
            }
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            isMouseOver = true;
            UpdateState();
        }
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            isMouseOver = false;
            UpdateState();
        }
        #endregion
    }
}

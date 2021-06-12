using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;

namespace Leisn.UI.Xaml.Controls
{
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Thumb", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Title", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Value", Type = typeof(TextBlock))]

    [TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "PointerOver")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "TextEditor")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "Disabled")]
    public sealed class RangeBox : RangeBase
    {
        const string NormalState = "Normal";
        const string PointerOverState = "PointerOver";
        const string TextEditorState = "TextEditor";
        const string DisabledState = "Disabled";


        Grid rootGrid;
        Rectangle thumb;
        TextBox textBox;
        TextBlock titleBlock;
        TextBlock valueBlock;


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RangeBox),
                new PropertyMetadata(""));

        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), typeof(RangeBox),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public RangeBox()
        {
            this.DefaultStyleKey = typeof(RangeBox);
            this.SizeChanged += (_, __) => updateThumbWidth();
            RegisterPropertyChangedCallback(IsEnabledProperty, PropertyChanged);
        }


        private void PropertyChanged(DependencyObject d, DependencyProperty e)
        {
            var box = d as RangeBox;
            box.UpdateState();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            updateThumbWidth();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            updateThumbWidth();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            updateThumbWidth();
        }

        void updateThumbWidth()
        {
            if (thumb == null || null == rootGrid)
                return;
            thumb.Width = rootGrid.ActualWidth * (Value - Minimum) / Maximum - Minimum;
        }


        protected override void OnApplyTemplate()
        {
            rootGrid = GetTemplateChild("RootGrid") as Grid;
            thumb = GetTemplateChild("PART_Thumb") as Rectangle;
            textBox = GetTemplateChild("PART_TextBox") as TextBox;
            titleBlock = GetTemplateChild("PART_Title") as TextBlock;
            valueBlock = GetTemplateChild("PART_Value") as TextBlock;

            base.OnApplyTemplate();

            UpdateState();
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
            base.OnTapped(e);
            isEditorOn = true;
            UpdateState();
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

        #region Drag
        Point startPoint;
        double startValue;
        bool dragging;
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            var isLeftButton = e.GetCurrentPoint(this)?.Properties?.IsLeftButtonPressed == true;
            if (!isLeftButton)
                return;
            this.CapturePointer(e.Pointer);
            startPoint = e.GetCurrentPoint(this).Position;
            startValue = Value;
            dragging = true;
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            if (!dragging)
                return;
            updateValueWhenDrag(e.GetCurrentPoint(this).Position);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (dragging)
                updateValueWhenDrag(e.GetCurrentPoint(this).Position);
            dragging = false;
            this.ReleasePointerCapture(e.Pointer);
        }

        void updateValueWhenDrag(Point endPoint)
        {
            var xOffset = endPoint.X - startPoint.X;
            var totalWidth = this.rootGrid.ActualWidth;
            var scale = totalWidth / (Maximum - Minimum);
            var valOffset = xOffset / scale;
            if (Math.Abs(valOffset) < 0.001)
                return;
            Value = Math.Round(startValue + valOffset, 3);
        }
        #endregion

        //protected override void OnKeyDown(KeyRoutedEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    switch (e.Key)
        //    {
        //        case VirtualKey.Left:
        //            Value -= 0.001;
        //            break;
        //        case VirtualKey.Right:
        //            Value += 0.001;
        //            break;
        //        case VirtualKey.Up:
        //            Value -= 0.01;
        //            break;
        //        case VirtualKey.Down:
        //            Value += 0.01;
        //            break;
        //        case VirtualKey.PageUp:
        //            Value -= 0.1;
        //            break;
        //        case VirtualKey.PageDown:
        //            Value += 0.1;
        //            break;
        //        case VirtualKey.Home:
        //            Value = Minimum;
        //            break;
        //        case VirtualKey.End:
        //            Value = Maximum;
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}

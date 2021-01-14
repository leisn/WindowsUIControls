using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;

namespace Leisn.Uwp.UI.Controls
{
    [TemplatePart(Name = "PART_Polygon", Type = typeof(Polygon))]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    [TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "Selected")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "PointerOver")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "PointerOverSelected")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "PointerOverPressed")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "Pressed")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "PressedSelected")]
    [TemplateVisualState(GroupName = "DisabledStates", Name = "Enabled")]
    [TemplateVisualState(GroupName = "DisabledStates", Name = "Disabled")]
    public class HiveViewItem : SelectorItem
    {
        protected const string PointerOverState = "PointerOver";
        protected const string PointerOverSelectedState = "PointerOverSelected";
        protected const string PointerOverPressedState = "PointerOverPressed";
        protected const string PressedState = "Pressed";
        protected const string PressedSelectedState = "PressedSelected";
        protected const string SelectedState = "Selected";
        protected const string NormalState = "Normal";
        protected const string DisabledState = "Disabled";

        protected Polygon polygon;
        protected ContentPresenter content;

        public event EventHandler<SelectorStateChangedEventArgs> Selected;

        private long isSelectedCallbackToken;

        public HiveViewItem()
        {
            DefaultStyleKey = typeof(HiveViewItem);
            isSelectedCallbackToken =
                RegisterPropertyChangedCallback(IsSelectedProperty, OnItemPropertyChanged);
            RegisterPropertyChangedCallback(IsEnabledProperty, OnItemPropertyChanged);
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            this.Unloaded += OnUnloaded;
        }

        #region DependencyProperties

        public bool CanUnselect
        {
            get { return (bool)GetValue(CanUnselectProperty); }
            set { SetValue(CanUnselectProperty, value); }
        }

        public static readonly DependencyProperty CanUnselectProperty =
            DependencyProperty.Register(
                nameof(CanUnselect),
                typeof(bool),
                typeof(HiveViewItem),
                new PropertyMetadata(true));

        public Brush SelectedForegroundBrush
        {
            get { return (Brush)GetValue(SelectedForegroundBrushProperty); }
            set { SetValue(SelectedForegroundBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedForegroundBrushProperty =
            DependencyProperty.Register(
                nameof(SelectedForegroundBrush),
                typeof(Brush),
                typeof(HiveViewItem),
                new PropertyMetadata(null));

        public Brush SelectedBackgroundBrush
        {
            get { return (Brush)GetValue(SelectedBackgroundBrushProperty); }
            set { SetValue(SelectedBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundBrushProperty =
            DependencyProperty.Register(
                nameof(SelectedBackgroundBrush),
                typeof(Brush),
                typeof(HiveViewItem),
                new PropertyMetadata(null));
        #endregion

        private void OnItemPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            var view = sender as HiveViewItem;
            if (dp == IsSelectedProperty)
            {
                var isSelected = (bool)sender.GetValue(dp);
                if (!view.IsEnabled) return;

                Selected?.Invoke(this, new SelectorStateChangedEventArgs(isSelected));
                VisualStateManager.GoToState(view, isSelected ? SelectedState : NormalState, false);
            }
            else if (dp == IsEnabledProperty)
            {
                var isEnalbed = (bool)sender.GetValue(dp);
                var isSeleted = view.IsSelected;
                VisualStateManager.GoToState(view, isEnalbed ?
                    (isSeleted ? SelectedState : NormalState) : DisabledState, false);
            }
        }

        public void SetSelctedStateOnly(bool selected)
        {
            UnregisterPropertyChangedCallback(IsSelectedProperty, isSelectedCallbackToken);
            IsSelected = selected;
            VisualStateManager.GoToState(this, IsEnabled ?
                (IsSelected ? SelectedState : NormalState) : DisabledState, false);
            isSelectedCallbackToken =
                RegisterPropertyChangedCallback(IsSelectedProperty, OnItemPropertyChanged);
        }

        protected override void OnApplyTemplate()
        {
            polygon = GetTemplateChild("PART_Polygon") as Polygon;
            content = GetTemplateChild("PART_Content") as ContentPresenter;

            VisualStateManager.GoToState(this, IsEnabled ?
                (IsSelected ? SelectedState : NormalState) : DisabledState, false);
            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);
            if (content != null)
                return content.DesiredSize;
            return size;
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.SizeChanged -= OnSizeChanged;
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var width = this.ActualWidth;
            var height = this.ActualHeight;
            updatePolygon(width, height);
        }

        private void updatePolygon(double width, double height)
        {
            var points = new PointCollection();
            points.Add(new Point(0, height / 2));
            points.Add(new Point(width / 4, 0));
            points.Add(new Point(width * 3 / 4, 0));
            points.Add(new Point(width, height / 2));
            points.Add(new Point(width * 3 / 4, height));
            points.Add(new Point(width / 4, height));
            this.polygon.Points = points;
        }

        private Size getPolygonSize()
        {
            var points = polygon.Points;
            double x0 = 0, x1 = 0, y0 = 0, y1 = 0;
            foreach (var item in points)
            {
                x0 = Math.Min(x0, item.X);
                y0 = Math.Min(y0, item.Y);
                x1 = Math.Max(x1, item.X);
                y1 = Math.Max(y1, item.Y);
            }
            return new Size(x1 - x0, y1 - y0);
        }
        protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            var width = this.ActualWidth;
            var height = this.ActualHeight;
            var size = getPolygonSize();

            if (Math.Abs(width - size.Width) > 1 || Math.Abs(height - size.Height) > 1)
                updatePolygon(width, height);
        }

        #region states
        /// <inheritdoc/>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);

            var pressed = e.GetCurrentPoint(this)?.Properties?.IsLeftButtonPressed == true;

            VisualStateManager.GoToState(this,
                IsSelected ? PointerOverSelectedState : (pressed ? PointerOverPressedState : PointerOverState),
                true);
        }

        /// <inheritdoc/>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, IsSelected ? SelectedState : NormalState, true);
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, IsSelected ? PressedSelectedState : PressedState, true);
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            if (CanUnselect)
                IsSelected = !IsSelected;
            else
                IsSelected = true;
        }

        #endregion

    }
}

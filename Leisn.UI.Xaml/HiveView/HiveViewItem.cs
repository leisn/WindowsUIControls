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

namespace Leisn.UI.Xaml.Controls
{
    [TemplatePart(Name = "PART_Hexagon", Type = typeof(Polygon))]
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

        protected Polygon hexagon;
        protected ContentPresenter content;

        public event EventHandler<SelectorStateChangedEventArgs> Selected;

        private long isSelectedCallbackToken;

        public HiveViewItem()
        {
            DefaultStyleKey = typeof(HiveViewItem);
            isSelectedCallbackToken =
                RegisterPropertyChangedCallback(IsSelectedProperty, OnItemPropertyChanged);
            RegisterPropertyChangedCallback(IsEnabledProperty, OnItemPropertyChanged);
        }

        #region DependencyProperties

        /// <summary>
        /// Whether the item can unselect
        /// </summary>
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

        /// <summary>
        /// Force this be a regular hexagon<br/>
        /// When true, this item will always be a regular hexagon, no matter what size of content<br/>
        /// When false, this item will be a regular hexagon only when content's width equals it's height<br/>
        /// But if you specific the width and height can override this
        /// </summary>
        public bool ForceRegularHexagon
        {
            get { return (bool)GetValue(ForceRegularHexagonProperty); }
            set { SetValue(ForceRegularHexagonProperty, value); }
        }

        public static readonly DependencyProperty ForceRegularHexagonProperty =
            DependencyProperty.Register(
                nameof(ForceRegularHexagon),
                typeof(bool),
                typeof(HiveViewItem),
                new PropertyMetadata(true, HiveViewItemPropertyChanged));

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

        #region stroke properties
        public Brush StrokeBrush
        {
            get { return (Brush)GetValue(StrokeBrushProperty); }
            set { SetValue(StrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty StrokeBrushProperty =
            DependencyProperty.Register(
                nameof(StrokeBrush),
                typeof(Brush),
                typeof(HiveViewItem),
                new PropertyMetadata(null));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(double),
                typeof(HiveViewItem),
                new PropertyMetadata(1));
        public double StrokeMiterLimit
        {
            get { return (double)GetValue(StrokeMiterLimitProperty); }
            set { SetValue(StrokeMiterLimitProperty, value); }
        }
        public static readonly DependencyProperty StrokeMiterLimitProperty =
            DependencyProperty.Register(
                nameof(StrokeMiterLimit),
                typeof(double),
                typeof(HiveViewItem),
                new PropertyMetadata(10));
        public double StrokeDashOffset
        {
            get { return (double)GetValue(StrokeDashOffsetProperty); }
            set { SetValue(StrokeDashOffsetProperty, value); }
        }
        public static readonly DependencyProperty StrokeDashOffsetProperty =
            DependencyProperty.Register(
                nameof(StrokeDashOffset),
                typeof(double),
                typeof(HiveViewItem),
                new PropertyMetadata(0));
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty StrokeDashArrayProperty =
           DependencyProperty.Register(
               nameof(StrokeDashArray),
               typeof(DoubleCollection),
               typeof(HiveViewItem),
               new PropertyMetadata(null, HiveViewItemPropertyChanged));

        public PenLineCap StrokeStartLineCap
        {
            get { return (PenLineCap)GetValue(StrokeStartLineCapProperty); }
            set { SetValue(StrokeStartLineCapProperty, value); }
        }
        public static readonly DependencyProperty StrokeStartLineCapProperty =
           DependencyProperty.Register(
               nameof(StrokeStartLineCap),
               typeof(PenLineCap),
               typeof(HiveViewItem),
               new PropertyMetadata(PenLineCap.Flat));

        public PenLineCap StrokeEndLineCap
        {
            get { return (PenLineCap)GetValue(StrokeEndLineCapProperty); }
            set { SetValue(StrokeEndLineCapProperty, value); }
        }
        public static readonly DependencyProperty StrokeEndLineCapProperty =
           DependencyProperty.Register(
               nameof(StrokeEndLineCap),
               typeof(PenLineCap),
               typeof(HiveViewItem),
               new PropertyMetadata(PenLineCap.Flat));

        public PenLineCap StrokeDashCap
        {
            get { return (PenLineCap)GetValue(StrokeDashCapProperty); }
            set { SetValue(StrokeDashCapProperty, value); }
        }
        public static readonly DependencyProperty StrokeDashCapProperty =
           DependencyProperty.Register(
               nameof(StrokeDashCap),
               typeof(PenLineCap),
               typeof(HiveViewItem),
               new PropertyMetadata(PenLineCap.Flat));
        public PenLineJoin StrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(StrokeLineJoinProperty); }
            set { SetValue(StrokeLineJoinProperty, value); }
        }
        public static readonly DependencyProperty StrokeLineJoinProperty =
           DependencyProperty.Register(
               nameof(StrokeLineJoin),
               typeof(PenLineJoin),
               typeof(HiveViewItem),
               new PropertyMetadata(PenLineJoin.Miter));
        #endregion

        #endregion
        protected static void HiveViewItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (HiveViewItem)d;
            if (e.Property == StrokeDashArrayProperty)
            {
                var dc = (DoubleCollection)e.NewValue;
                if (view.hexagon != null)
                {
                    var dashArray = view.hexagon.StrokeDashArray;
                    dashArray.Clear();
                    if (dc != null)
                    {
                        foreach (var dv in dc)
                            dashArray.Add(dv);
                    }
                }
            }
            else if (e.Property == ForceRegularHexagonProperty)
            {
                view.InvalidateMeasure();
            }
        }
        protected void OnItemPropertyChanged(DependencyObject sender, DependencyProperty dp)
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
            hexagon = GetTemplateChild("PART_Hexagon") as Polygon;
            content = GetTemplateChild("PART_Content") as ContentPresenter;

            VisualStateManager.GoToState(this, IsEnabled ?
                (IsSelected ? SelectedState : NormalState) : DisabledState, false);
            base.OnApplyTemplate();
        }

        const double XScale = 1.5773503112793;
        const double YScale = 1.366025390625;
        protected override Size MeasureOverride(Size availableSize)
        {
            Size requestSize = availableSize;
            if (content != null)
            {
                content.Measure(availableSize);
                var contentSize = content.DesiredSize;

                if (ForceRegularHexagon)
                {
                    var halfWidth = contentSize.Width / 2 + contentSize.Height / 2 / Math.Tan(Math.PI * 60 / 180);
                    requestSize = new Size(HivePanel.GetWidthFromEdge(halfWidth), HivePanel.GetHeightFromEdge(halfWidth));
                    if (requestSize.Height < contentSize.Height)
                    {
                        requestSize.Width = HivePanel.GetWidthFromHeight(contentSize.Height);
                        requestSize.Height = contentSize.Height;
                    }
                }
                else
                {
                    requestSize.Width = contentSize.Width * XScale;
                    requestSize.Height = contentSize.Height * YScale;
                }
            }

            //requestSize.Width += StrokeThickness * 2;
            //requestSize.Height += StrokeThickness * 2;

            base.MeasureOverride(requestSize);
            return requestSize;
        }

        #region Deprecated justify polygon size
        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //updatePolygon(finalSize);
        //    return base.ArrangeOverride(finalSize);
        //}

        //private void updatePolygon(Size arrangeSize)
        //{
        //    var width = arrangeSize.Width;
        //    var height = arrangeSize.Height;

        //    var points = polygon.Points;
        //    double x0 = 0, x1 = 0, y0 = 0, y1 = 0;
        //    foreach (var item in points)
        //    {
        //        x0 = Math.Min(x0, item.X);
        //        y0 = Math.Min(y0, item.Y);
        //        x1 = Math.Max(x1, item.X);
        //        y1 = Math.Max(y1, item.Y);
        //    }
        //    var polygonWidth = x1 - x0;
        //    var polygonHeight = y1 - y0;

        //    if (Math.Abs(width - polygonWidth) < StrokeThickness
        //        && Math.Abs(height - polygonHeight) < StrokeThickness)
        //        return;

        //    points.Clear();
        //    points.Add(new Point(0, height / 2));
        //    points.Add(new Point(width / 4, 0));
        //    points.Add(new Point(width * 3 / 4, 0));
        //    points.Add(new Point(width, height / 2));
        //    points.Add(new Point(width * 3 / 4, height));
        //    points.Add(new Point(width / 4, height));
        //}
        #endregion

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

using System;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Leisn.Uwp.UI.Extensions;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Specialized;

namespace Leisn.Uwp.UI.Controls
{
    public class AutoFillLayout : VirtualizingLayout
    {
        #region Properties
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                nameof(Padding),
                typeof(Thickness),
                typeof(AutoFillLayout),
                new PropertyMetadata(default(Thickness), LayoutPropertyChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(AutoFillLayout),
                new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));

        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        public static readonly DependencyProperty HorizontalSpacingProperty =
            DependencyProperty.Register(
                nameof(HorizontalSpacing),
                typeof(double),
                typeof(AutoFillLayout),
                new PropertyMetadata(4d, LayoutPropertyChanged));

        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        public static readonly DependencyProperty VerticalSpacingProperty =
            DependencyProperty.Register(
                nameof(VerticalSpacing),
                typeof(double),
                typeof(AutoFillLayout),
                new PropertyMetadata(4d, LayoutPropertyChanged));

        private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine(e.Property + " changed");
            if (d is AutoFillLayout ap)
            {
                ap.InvalidateMeasure();
                ap.InvalidateArrange();
            }
        }
        #endregion

        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = new AutoFillLayoutState(context);
            base.InitializeForContextCore(context);
        }

        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = null;
            base.UninitializeForContextCore(context);
        }

        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            var state = (AutoFillLayoutState)context.LayoutState;
            //Debug.WriteLine(args.Action);
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    state.RemoveItemsFrom(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    state.RemoveItemsFrom(Math.Min(args.OldStartingIndex, args.NewStartingIndex));
                    state.RecycleElementAt(args.OldStartingIndex);
                    state.RecycleElementAt(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    state.RemoveItemsFrom(args.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    state.RemoveItemsFrom(args.NewStartingIndex);
                    state.RecycleElementAt(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    state.Reset();
                    break;
            }
            base.OnItemsChangedCore(context, source, args);
        }


        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var requiredSize = new Size(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);

            if (context.ItemCount == 0
                || context.RealizationRect.Width == 0
                || context.RealizationRect.Height == 0)
                return requiredSize;

            var state = (AutoFillLayoutState)context.LayoutState;
            var clientSize = new Size(
                   availableSize.Width - Padding.Left - Padding.Right,
                   availableSize.Height - Padding.Top - Padding.Bottom);

            //Save layout properties
            //If changed every thing need redo
            if (state.SavePropertiesIfChange(Orientation, Padding,
                HorizontalSpacing, VerticalSpacing, availableSize))
            {
                state.Reset();
            }

            if (!state.Initialized)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    state.Initialize(
                        new Rect(Padding.Left, Padding.Top,
                        clientSize.Width + HorizontalSpacing, double.PositiveInfinity)
                    );
                }
                else
                {
                    state.Initialize(
                       new Rect(Padding.Left, Padding.Top,
                       double.PositiveInfinity, clientSize.Height + VerticalSpacing)
                    );
                }
            }

            for (int i = 0; i < context.ItemCount; i++)
            {
                var item = state.GetOrCreateItemAt(i);

                bool measured = false;

                if (item.Bounds.IsEmpty())//measure empty
                {
                    item.Element = context.GetOrCreateElementAt(i);
                    item.Element.Measure(clientSize);
                    item.Width = item.Element.DesiredSize.Width;
                    item.Height = item.Element.DesiredSize.Height;
                    state.FitItem(item);
                    measured = true;
                }

                item.ShouldDisplay = context.RealizationRect.IsIntersect(item.Bounds);
                if (!item.ShouldDisplay)
                {
                    if (item.Element != null)
                    {
                        context.RecycleElement(item.Element);
                        item.Element = null;
                    }

                }
                else if (!measured)//measure in view rect
                {
                    item.Element = context.GetOrCreateElementAt(i);
                    item.Element.Measure(clientSize);
                    var childSize = item.Element.DesiredSize;
                    if (item.Width != childSize.Width || item.Height != childSize.Height)  //size changed
                    {
                        item.Width = childSize.Width;
                        item.Height = childSize.Height;
                        state.RemoveItemsFrom(i + 1);//remove after this
                        state.FitItem(item);
                    }
                }
            }

            var size = state.GetTotalSize();
            //var size = state.GetVirtualizedSize();
            requiredSize.Width += size.Width;
            requiredSize.Height += size.Height;
            return requiredSize;
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if (context.RealizationRect.Width == 0
                || context.RealizationRect.Height == 0)
                return finalSize;

            var viewRect = context.RealizationRect;
            var state = (AutoFillLayoutState)context.LayoutState;
            Debug.Assert(context.ItemCount == state.ItemCount);

            for (int i = 0; i < context.ItemCount; i++)
            {
                var item = state[i];
                var bounds = item.Bounds;

                if (viewRect.IsIntersect(bounds))
                {
                    var child = context.GetOrCreateElementAt(item.Index);
                    item.Element = child;
                    child.Arrange(bounds);
                }
            }
            return finalSize;
        }

    }
}

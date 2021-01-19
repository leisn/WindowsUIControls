using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using Leisn.UI.Xaml.Extensions;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Leisn.UI.Xaml.Controls
{

    [WebHostHidden]
    public class HiveView : ItemsControl, ISemanticZoomInformation
    {
        public HiveView() : base()
        {
            this.DefaultStyleKey = typeof(HiveView);
        }

        #region public
        public event EventHandler<SingleSelectionChangedEventArgs> SelectionChanged;
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <returns>The selected item. The default is null.</returns>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(HiveView), new PropertyMetadata(null, OnHivePropertyChanged));

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        /// <returns>The index of the selected item. The default is -1.</returns>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(HiveView), new PropertyMetadata(-1, OnHivePropertyChanged));

        public HiveViewItem SelectedViewItem => SelectedIndex == -1 ? null : (HiveViewItem)ContainerFromIndex(SelectedIndex);
        #endregion

        #region handlers
        private static void OnHivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (HiveView)d;
            if (e.Property == SelectedIndexProperty)
            {
                int newIndex = (int)e.NewValue;
                var oldIndex = (int)e.OldValue;
                var newValue = newIndex == -1 ? null : view.Items[newIndex];
                var oldValue = e.OldValue == null ? null : view.Items.ElementAtOrDefault(oldIndex);
                if (oldValue != null)
                {
                    var item = (HiveViewItem)view.ContainerFromIndex(oldIndex);
                    if (item != null)
                        item.SetSelctedStateOnly(false);
                }

                bool isNew = oldValue != newValue;
                if (newValue != null)
                {
                    var item = (HiveViewItem)view.ContainerFromIndex(newIndex);
                    if (item != null)
                        item.SetSelctedStateOnly(true);
                    item?.Focus(FocusState.Pointer);
                }
                if (isNew)
                {
                    view.SetValue(SelectedItemProperty, newValue);
                    view.FireSelectionChanged(new SingleSelectionChangedEventArgs(oldIndex, newIndex, oldValue, newValue));
                }
            }
            else if (e.Property == SelectedItemProperty)
            {
                var index = e.NewValue == null ? -1 : view.Items.IndexOf(e.NewValue);
                if (view.SelectedIndex != index)
                {
                    view.SetValue(SelectedIndexProperty, index);
                }
            }
        }

        internal void FireSelectionChanged(SingleSelectionChangedEventArgs args)
        {
            NotifySemanticZoomSelectionChanged();
            SelectionChanged?.Invoke(this, args);
        }

        private void HiveViewItem_Selected(object sender, SelectorStateChangedEventArgs e)
        {
            SelectedItem = e.Selected ? ItemFromContainer((HiveViewItem)sender) : null;
        }

        #endregion

        #region overrides
        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is HiveViewItem;
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HiveViewItem();
        }

        /// <inheritdoc/>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            var hitem = (HiveViewItem)element;
            hitem.Selected -= HiveViewItem_Selected;
        }
        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var hiveItem = (HiveViewItem)element;
            hiveItem.Selected += HiveViewItem_Selected;
        }

        #endregion


        #region ISemanticZoomInformation

        //As ZoomedInView
        //   InitializeViewChange -> StartViewChangeFrom -> CompleteViewChangeFrom -> CompleteViewChange
        //   ZoomedOutView Shown -> ZoomedOutView ToggleActiveView
        //   InitializeViewChange -> StartViewChangeTo -> CompleteViewChangeTo -> CompleteViewChange
        //   ZoomedInView Shown
        //
        //As ZoomedOutView
        //   InitializeViewChange -> StartViewChangeTo -> MakeVisible -> CompleteViewChangeTo -> CompleteViewChange
        //   ZoomedOutView Shown  -> ZoomedOutView ToggleActiveView
        //   InitializeViewChange -> StartViewChangeFrom -> CompleteViewChangeFrom -> CompleteViewChange
        //   ZoomedInView Shown
        //

        bool isSemanticViewChanging = false;
        protected virtual void NotifySemanticZoomSelectionChanged()
        {
            if (IsActiveView && !isSemanticViewChanging)
            {
                if (!IsZoomedInView)
                    SemanticZoomOwner.ToggleActiveView();
            }
        }

        public void InitializeViewChange()
        {
            isSemanticViewChanging = true;
        }
        public void CompleteViewChange()
        {
            isSemanticViewChanging = false;
        }

        public void StartViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            if (!IsZoomedInView)
                this.SelectedItem = destination.Item;
        }
        public void CompleteViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
        }
        public void StartViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            if (!IsZoomedInView)
            {
                source.Item = this.SelectedItem;
                var group = SelectedItem as ICollectionViewGroup;
                if (group != null && group.GroupItems.Count > 0)
                    destination.Item = group.GroupItems[0];
                else
                    destination.Item = this.SelectedItem;
            }
        }
        public void CompleteViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
        }

        public void MakeVisible(SemanticZoomLocation item)
        {
            var container = this.ContainerFromItem(item.Item) as UIElement;
            container?.StartBringIntoView();
        }

        public bool IsActiveView
        {
            get { return (bool)GetValue(IsActiveViewProperty); }
            set { SetValue(IsActiveViewProperty, value); }
        }

        public static readonly DependencyProperty IsActiveViewProperty =
            DependencyProperty.Register(
                nameof(IsActiveView),
                typeof(bool),
                typeof(HiveView),
                new PropertyMetadata(false, OnSemantiZoomPropertyChanged));

        public bool IsZoomedInView
        {
            get { return (bool)GetValue(IsZoomedInViewProperty); }
            set
            {
                if (value)
                    throw new InvalidOperationException("As ZoominView not supported");
                SetValue(IsZoomedInViewProperty, value);
            }
        }

        public static readonly DependencyProperty IsZoomedInViewProperty =
            DependencyProperty.Register(
                nameof(IsZoomedInView),
                typeof(bool),
                typeof(HiveView),
                new PropertyMetadata(false, OnSemantiZoomPropertyChanged));

        public SemanticZoom SemanticZoomOwner
        {
            get { return (SemanticZoom)GetValue(SemanticZoomOwnerProperty); }
            set { SetValue(SemanticZoomOwnerProperty, value); }
        }

        public static readonly DependencyProperty SemanticZoomOwnerProperty =
            DependencyProperty.Register(
                nameof(SemanticZoomOwner),
                typeof(SemanticZoom),
                typeof(HiveView),
                new PropertyMetadata(false, OnSemantiZoomPropertyChanged));

        private static void OnSemantiZoomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsActiveViewProperty)
            {

            }
            else if (e.Property == IsZoomedInViewProperty)
            {

            }
            else if (e.Property == SemanticZoomOwnerProperty)
            {

            }
        }

        #endregion
    }
}

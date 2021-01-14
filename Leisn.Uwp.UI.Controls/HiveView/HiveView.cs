using System;
using System.Collections.Generic;
using System.Linq;

using Leisn.Uwp.UI.Extensions;

using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;


namespace Leisn.Uwp.UI.Controls
{
    [WebHostHidden]
    public class HiveView : ItemsControl, ISemanticZoomInformation
    {
        public HiveView() : base()
        {
            this.DefaultStyleKey = typeof(HiveView);
            this.Loaded += OnLoaded;
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
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }

            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(HiveView), new PropertyMetadata(-1, OnHivePropertyChanged));

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                nameof(Spacing),
                typeof(double),
                typeof(HiveView),
                new PropertyMetadata(4d, OnHivePropertyChanged));

        public static readonly DependencyProperty FixedEdgeProperty =
           DependencyProperty.Register(
               nameof(FixedEdge),
               typeof(double),
               typeof(HiveView),
               new PropertyMetadata(30d, OnHivePropertyChanged));

        public double FixedEdge
        {
            get { return (double)GetValue(FixedEdgeProperty); }
            set { SetValue(FixedEdgeProperty, value); }
        }
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
                    view.SelectionChanged?.Invoke(view,
                        new SingleSelectionChangedEventArgs(oldIndex, newIndex, oldValue, newValue));
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
            else if (e.Property == SpacingProperty)
            {
                var panel = view.FindDescendant<HivePanel>();
                panel?.SetValue(HivePanel.SpacingProperty, e.NewValue);
            }
            else if (e.Property == FixedEdgeProperty)
            {
                var panel = view.FindDescendant<HivePanel>();
                panel?.SetValue(HivePanel.FixedEdgeProperty, e.NewValue);
            }
        }

        private void HiveViewItem_Selected(object sender, SelectorStateChangedEventArgs e)
        {
            SelectedItem = e.Selected ? ItemFromContainer((HiveViewItem)sender) : null;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            var panel = this.FindDescendant<HivePanel>();
            if (panel == null)
                throw new InvalidCastException("ItemsPanel must be a HivePanel");
            panel.SetValue(HivePanel.SpacingProperty, Spacing);
            panel.SetValue(HivePanel.FixedEdgeProperty, FixedEdge);
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
        public void InitializeViewChange()
        {
        }

        public void CompleteViewChange()
        {
        }

        //ZoomedInView -> ZoomedOutView Start
        public void StartViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
        }
        //ZoomedInView -> ZoomedOutView Complete
        public void CompleteViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
        }

        //ZoomedOutView -> ZoomedInView Start
        public void StartViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            var item = source.Item as ICollectionViewGroup;
            destination.Item = item.Group;
        }
        //ZoomedOutView -> ZoomedInView Complete
        public void CompleteViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        // ZoomedOutView -> ZoomedInView
        public void MakeVisible(SemanticZoomLocation item)
        {
        }


        private bool isActiveView;
        public bool IsActiveView
        {
            get => isActiveView;
            set => isActiveView = value;
        }

        private bool isZoomedInView;
        public bool IsZoomedInView
        {
            get => isZoomedInView;
            set { isZoomedInView = value; }
        }
        private SemanticZoom zoomer;
        public SemanticZoom SemanticZoomOwner
        {
            get => zoomer;
            set => zoomer = value;
        }

        #endregion
    }
}

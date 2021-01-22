using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Leisn.UI.Xaml.Extensions;

namespace Leisn.UI.Xaml.Controls
{
    public class HiveGrid : HivePanelBase
    {
        #region DependencyProperty
        public static readonly DependencyProperty RowCountProperty =
          DependencyProperty.Register(
              nameof(RowCount),
              typeof(int),
              typeof(HiveGrid),
              new PropertyMetadata(1, CellPropertyChanged));
        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("RowCount < 0");
                SetValue(RowCountProperty, value); 
            }
        }

        public static readonly DependencyProperty ColumnCountProperty =
          DependencyProperty.Register(
              nameof(ColumnCount),
              typeof(int),
              typeof(HiveGrid),
              new PropertyMetadata(1, CellPropertyChanged));
        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("ColumnCount < 0");
                SetValue(ColumnCountProperty, value); 
            }
        }

        public static readonly DependencyProperty AutoCellProperty =
            DependencyProperty.Register(
                nameof(AutoCell),
                typeof(bool),
                typeof(HiveGrid),
                new PropertyMetadata(false, CellPropertyChanged));

        public bool AutoCell
        {
            get { return (bool)GetValue(AutoCellProperty); }
            set { SetValue(AutoCellProperty, value); }
        }

        protected static void CellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HiveGrid hg)
            {
                hg.RequestMeasure();
            }
        }
        #endregion

        #region  Attached DependencyProperty

        public static readonly DependencyProperty ColumnProperty =
                       DependencyProperty.RegisterAttached(
                             "Column",
                             typeof(int),
                             typeof(HiveGrid),
                             new PropertyMetadata(0, OnCellAttachedPropertyChanged));
        public static void SetColumn(UIElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(ColumnProperty, value);
        }
        public static int GetColumn(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return ((int)element.GetValue(ColumnProperty));
        }

        public static readonly DependencyProperty RowProperty =
                DependencyProperty.RegisterAttached(
                      "Row",
                      typeof(int),
                      typeof(HiveGrid),
                      new PropertyMetadata(0, OnCellAttachedPropertyChanged));
        public static void SetRow(UIElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(RowProperty, value);
        }
        public static int GetRow(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return ((int)element.GetValue(RowProperty));
        }

        private static void OnCellAttachedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (VisualTreeHelper.GetParent(d) is HiveGrid grid)
            {
                if (grid != null)
                {
                    grid.RequestMeasure();
                }
            }
        }
        #endregion

        protected int measureId;
        protected volatile int requestMeasure = 0;
        protected bool measureInProgress = false;
        protected SimpleMatrix<Rect> Cells;

        protected void RequestMeasure()
        {
            requestMeasure++;
            if (!measureInProgress)
                InvalidateMeasure();
        }

        protected bool isDirty()
        {
            return Cells == null
                || Cells.RowCount != RowCount
                || Cells.ColumnCount != ColumnCount
                || requestMeasure != measureId;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            //Debug.WriteLine(requestMeasure + " " + measureId);

            int count = Children.Count;
            var requiredSize = new Size(
              Padding.Left + Padding.Right,
              Padding.Top + Padding.Bottom);

            if (count == 0 || ColumnCount == 0 || RowCount == 0)
                return requiredSize;
            try
            {
                measureInProgress = true;
                measureId = requestMeasure;

                Cells = new SimpleMatrix<Rect>(RowCount, ColumnCount);
                double itemWidth;
                double itemHeight;

                #region cal itemWidth and itemHeight
                if (double.IsNaN(FixedEdge) || FixedEdge == 0)
                {
                    var clientSize = new Size(
                         availableSize.Width - Padding.Left - Padding.Right,
                         availableSize.Height - Padding.Top - Padding.Bottom);

                    if (ColumnCount == 1)
                    {
                        var heightFromClient = (clientSize.Height - (RowCount - 1) * Spacing) / RowCount;
                        var heightFromClientWidth = GetHeightFromWidth(clientSize.Width);
                        itemHeight = Math.Min(heightFromClient, heightFromClientWidth);
                        itemWidth = GetWidthFromHeight(itemHeight);
                    }
                    else
                    {
                        if (double.IsInfinity(clientSize.Width)
                              && double.IsInfinity(clientSize.Height))
                        {
                            var size = GetChildrenMaxSize(clientSize);
                            itemWidth = size.Width;
                            itemHeight = size.Height;
                        }
                        else if (double.IsInfinity(clientSize.Width))
                        {
                            itemHeight = (clientSize.Height - (RowCount - 1) * Spacing - VerticalSpacing) / (RowCount + 0.5);
                            itemWidth = GetWidthFromHeight(itemHeight);
                        }
                        else if (double.IsInfinity(clientSize.Height))
                        {
                            itemWidth = (clientSize.Width - (ColumnCount - 1) * HorizontalSpacing) / (3 * ColumnCount + 1) * 4;
                            itemHeight = GetHeightFromWidth(itemWidth);
                        }
                        else
                        {
                            var bestWidth = (clientSize.Width - (ColumnCount - 1) * HorizontalSpacing) / (3 * ColumnCount + 1) * 4;
                            var bestHeight = (clientSize.Height - (RowCount - 1) * Spacing - VerticalSpacing) / (RowCount + 0.5);
                            var widthFromBestHeight = GetWidthFromHeight(bestHeight);
                            itemWidth = Math.Min(bestWidth, widthFromBestHeight);
                            itemHeight = GetHeightFromWidth(itemWidth);
                        }
                    }
                }
                else
                {
                    itemWidth = GetWidthFromEdge(FixedEdge);
                    itemHeight = GetHeightFromEdge(FixedEdge);
                }
                #endregion

                initCells(itemWidth, itemHeight);
                var itemSize = new Size(itemWidth, itemHeight);
                foreach (var item in Children)
                    item.Measure(itemSize);

                var bounds = Cells.GetOutBounds();
                requiredSize.Width += bounds.Width;
                requiredSize.Height += bounds.Height;
                return requiredSize;
            }
            finally
            {
                measureInProgress = false;
                if (isDirty())
                    InvalidateMeasure();
                else
                {
                    measureId = 0;
                    requestMeasure = 0;
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = Children.Count;

            if (count == 0 || RowCount == 0 || ColumnCount == 0)
                return finalSize;

            int cellCount = RowCount * ColumnCount;
            int ites = Math.Max(cellCount, count);
            for (int i = 0; i < ites; i++)
            {
                if (i >= count)
                    break;
                var row = i / ColumnCount;
                var col = i % ColumnCount;
                row = Math.Min(RowCount - 1, row);
                col = Math.Min(ColumnCount - 1, col);
                {
                    var child = Children[i];
                    row = AutoCell ? row : GetRow(child);
                    col = AutoCell ? col : GetColumn(child);
                    row = Math.Min(RowCount - 1, Math.Max(0, row));
                    col = Math.Min(ColumnCount - 1, Math.Max(0, col));
                    child.Arrange(Cells[row, col]);
                }
            }
            return finalSize;
        }

        private void initCells(double itemWidth, double itemHeight)
        {
            double left = Padding.Left, top = Padding.Top;

            if (ColumnCount == 1)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    Cells[i, 0] = new Rect
                    {
                        X = left,
                        Y = top,
                        Width = itemWidth,
                        Height = itemHeight
                    };
                    top += itemHeight + Spacing;
                }
            }
            else
            {
                top += itemHeight / 2 + VerticalSpacing;
                for (int i = 0; i < RowCount * ColumnCount; i++)
                {
                    var row = i / ColumnCount;
                    var col = i % ColumnCount;

                    Cells[row, col] = new Rect
                    {
                        X = left,
                        Y = top,
                        Width = itemWidth,
                        Height = itemHeight
                    };

                    col++;

                    if (col == ColumnCount)
                    {
                        left = Padding.Left;
                        top += itemHeight + Spacing;
                        if (col % 2 == 0)
                            top += itemHeight / 2 + VerticalSpacing;
                    }
                    else
                    {
                        if (col % 2 == 0)
                            top += itemHeight / 2 + VerticalSpacing;
                        else
                            top -= itemHeight / 2 + VerticalSpacing;

                        left += itemWidth * 3 / 4 + HorizontalSpacing;
                    }
                }
            }
        }
    }
}

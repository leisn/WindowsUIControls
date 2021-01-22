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
    public class ItemsHiveGrid : HivePanel
    {
        #region public properties
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(ItemsHiveGrid),
                new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));

        public static readonly DependencyProperty MaximumRowsOrColumnsProperty =
          DependencyProperty.Register(
              nameof(MaximumRowsOrColumns),
              typeof(int),
              typeof(ItemsHiveGrid),
              new PropertyMetadata(1, LayoutPropertyChanged));
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("RowsOrColumns < 0");
                SetValue(MaximumRowsOrColumnsProperty, value);
            }
        }
        #endregion

        protected SimpleMatrix<Rect> Cells;
        protected override Size MeasureOverride(Size availableSize)
        {
            int count = Children.Count;
            var requiredSize = new Size(
              Padding.Left + Padding.Right,
              Padding.Top + Padding.Bottom);

            if (count == 0)
            {
                Cells = null;
                return requiredSize;
            }

            int row, col;
            double itemWidth;
            double itemHeight;

            #region cal item size and row col 
            var clientSize = new Size(
                    availableSize.Width - Padding.Left - Padding.Right,
                    availableSize.Height - Padding.Top - Padding.Bottom);

            if (Orientation == Orientation.Horizontal)
            {
                col = MaximumRowsOrColumns;
                row = (int)Math.Ceiling((double)count / col);
            }
            else
            {
                row = MaximumRowsOrColumns;
                col = (int)Math.Ceiling((double)count / row);
            }

            if (double.IsNaN(FixedEdge) || FixedEdge == 0)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (double.IsInfinity(clientSize.Width))
                        itemWidth = GetChildrenMaxSize(clientSize).Width;
                    else
                        itemWidth = (clientSize.Width - (col - 1) * HorizontalSpacing) / (3 * col + 1) * 4;
                    itemHeight = GetHeightFromWidth(itemWidth);
                }
                else
                {
                    if (double.IsInfinity(clientSize.Height))
                        itemHeight = GetChildrenMaxSize(clientSize).Height;
                    else
                        itemHeight = (clientSize.Height - (row - 1) * Spacing - VerticalSpacing) / (row + 0.5);
                    itemWidth = GetWidthFromHeight(itemHeight);
                }
            }
            else
            {
                itemWidth = GetWidthFromEdge(FixedEdge);
                itemHeight = GetHeightFromEdge(FixedEdge);

                if (Orientation == Orientation.Horizontal)
                {
                    if (!double.IsInfinity(clientSize.Width))
                    {
                        var needCol = (int)Math.Floor(
                            (clientSize.Width - itemWidth / 4 + HorizontalSpacing)
                             / (3 * itemWidth / 4 + HorizontalSpacing));

                        col = Math.Min(needCol, MaximumRowsOrColumns);
                        row = (int)Math.Ceiling((double)count / col);
                    }
                }
                else
                {
                    if (!double.IsInfinity(clientSize.Height))
                    {
                        var needRow = (int)Math.Floor(
                             (clientSize.Height - itemHeight * 0.5 + Spacing - VerticalSpacing)
                             / (itemHeight + 1));
                        row = Math.Min(needRow, MaximumRowsOrColumns);
                        col = (int)Math.Ceiling((double)count / row);
                    }
                }
            }
            #endregion

            Cells = new SimpleMatrix<Rect>(row, col);
            initCells(itemWidth, itemHeight);

            var itemSize = new Size(itemWidth, itemHeight);
            foreach (var item in Children)
                item.Measure(itemSize);

            var bounds = Cells.GetOutBounds();
            requiredSize.Width += bounds.Width;
            requiredSize.Height += bounds.Height;
            return requiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = Children.Count;

            if (count == 0 || MaximumRowsOrColumns == 0)
                return finalSize;

            var rowCount = Cells.RowCount;
            var colCount = Cells.ColumnCount;
            var mod = colCount;
            if (Orientation == Orientation.Vertical)
                mod = rowCount;
            for (int i = 0; i < count; i++)
            {
                var row = i / mod;
                var col = i % mod;
                var cell = Orientation == Orientation.Horizontal
                    ? Cells[row, col] : Cells[col, row];
                Children[i].Arrange(cell);
            }
            return finalSize;
        }

        private void initCells(double itemWidth, double itemHeight)
        {
            double left = Padding.Left, top = Padding.Top;
            var rowCount = Cells.RowCount;
            var colCount = Cells.ColumnCount;

            if (Orientation == Orientation.Horizontal)
            {
                if (colCount == 1)
                {
                    for (int i = 0; i < rowCount; i++)
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
                    return;
                }
            }

            top += itemHeight / 2 + VerticalSpacing;
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    Cells[j, i] = new Rect
                    {
                        X = left,
                        Y = top,
                        Width = itemWidth,
                        Height = itemHeight
                    };
                    top += itemHeight + Spacing;
                }
                left += itemWidth * 3 / 4 + HorizontalSpacing;
                top = Padding.Top;
                if ((i & 1) == 1)
                    top += itemHeight / 2 + VerticalSpacing;
            }

        }
    }
}

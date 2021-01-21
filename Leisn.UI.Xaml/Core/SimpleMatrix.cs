using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leisn.UI.Xaml
{
    public class SimpleMatrix<T> : IEnumerable<T>, IEnumerable
    {
        public T[,] Dimensions { get; }

        public int RowCount { get; }
        public int ColumnCount { get; }

        public int Length => RowCount * ColumnCount;

        public T Current => throw new NotImplementedException();

        public SimpleMatrix(int rowCount, int columnCount)
        {
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
            this.Dimensions = new T[rowCount, columnCount];
        }

        public T this[int row, int col]
        {
            get => Dimensions[row, col];
            set => Dimensions[row, col] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dimensions.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in Dimensions)
            {
                yield return item;
            }
        }
    }
}

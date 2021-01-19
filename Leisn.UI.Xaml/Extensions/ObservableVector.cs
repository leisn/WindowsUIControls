using System;
using System.Collections;
using System.Collections.Generic;

using Windows.Foundation.Collections;

namespace Leisn.UI.Xaml.Extensions
{
    public class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        public CollectionChange CollectionChange { get; }
        public uint Index { get; }

        public VectorChangedEventArgs(CollectionChange change, int index)
        {
            CollectionChange = change;
            Index = (uint)index;
        }
    }

    public class ObservableVector<T> : IObservableVector<T>
    {
        readonly List<T> source = new List<T>();

        public T this[int index]
        {
            get => source[index];
            set => source[index] = value;
        }

        public int Count => source.Count;

        public bool IsReadOnly => true;

        public event VectorChangedEventHandler<T> VectorChanged;

        public void Add(T item)
        {
            source.Add(item);
            VectorChanged?.Invoke(this, new VectorChangedEventArgs(CollectionChange.ItemInserted, Count - 1));
        }

        public void Clear()
        {
            source.Clear();
            VectorChanged?.Invoke(this, new VectorChangedEventArgs(CollectionChange.Reset, 0));
        }

        public bool Contains(T item) => source.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => source.GetEnumerator();

        public int IndexOf(T item) => source.IndexOf(item);

        public void Insert(int index, T item)
        {
            source.Insert(index, item);
            VectorChanged?.Invoke(this, new VectorChangedEventArgs(CollectionChange.ItemInserted, index));
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index != -1)
            {
                source.Remove(item);
                VectorChanged?.Invoke(this, new VectorChangedEventArgs(CollectionChange.ItemRemoved, index));
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            source.RemoveAt(index);
            VectorChanged?.Invoke(this, new VectorChangedEventArgs(CollectionChange.ItemRemoved, index));
        }

        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();
    }
}

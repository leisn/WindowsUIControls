using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

namespace Leisn.Uwp.UI.Extensions
{
    public class RectClips : ISet<Rect>, IList<Rect>
    {

        private readonly List<Rect> list = new List<Rect>();

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public Rect this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public void Insert(int index, Rect item)
        {
            int i = list.IndexOf(item);
            if (i != -1)
            {
                list.RemoveAt(i);
                //TODO a better way?
                index = index < Count ? index : Count - 1;
            }
            list.Insert(index, item);
        }

        public bool Add(Rect item)
        {
            int index = list.IndexOf(item);
            if (index == -1)
                list.Add(item);
            else
                list[index] = item;
            return true;
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public bool UnionItem(Rect item, bool ignoreEmpty = true)
        {
            if (ignoreEmpty && item.IsEmpty)
                return false;
            for (int i = 0; i < Count; i++)
            {
                var rect = list[i];
                if (rect.Contains(item))
                    return false;
                if (item.Contains(rect))
                {
                    list[i] = item;
                    return true;
                }
            }
            list.Add(item);
            return true;
        }

        public void Sort(Comparison<Rect> comparison)
        {
            list.Sort(comparison);
        }
        public void Sort(IComparer<Rect> comparer)
        {
            list.Sort(comparer);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Rect item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Rect[] array, int arrayIndex)
        {
            for (int i = 0; i < list.Count; i++)
            {
                array[i + arrayIndex] = list[i];
            }
        }

        public void ExceptWith(IEnumerable<Rect> other)
        {
            foreach (var item in other)
            {
                list.Remove(item);
            }
        }

        public int IndexOf(Rect item)
        {
            return list.IndexOf(item);
        }

        public bool GetIfExists(Rect item, out Rect temp)
        {
            int index = IndexOf(item);
            if (index != -1)
            {
                temp = list[index];
                return true;
            }
            temp = default;
            return false;
        }

        public IEnumerator<Rect> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<Rect> other)
        {
            var temp = new List<Rect>(other);
            foreach (var item in temp)
            {
                if (!list.Contains(item))
                    list.Remove(item);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<Rect> other)
        {
            var temp = new List<Rect>(other);
            foreach (var item in list)
            {
                if (!temp.Contains(item))
                    return false;
            }
            return true;
        }

        public bool IsProperSupersetOf(IEnumerable<Rect> other)
        {
            foreach (var item in other)
            {
                if (!list.Contains(item))
                    return false;
            }
            return true;
        }

        public bool IsSubsetOf(IEnumerable<Rect> other)
        {
            return IsProperSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<Rect> other)
        {
            return IsProperSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<Rect> other)
        {
            foreach (var item in other)
            {
                if (list.Contains(item))
                    return true;
            }
            return false;
        }

        public bool Remove(Rect item)
        {
            return list.Remove(item);
        }

        public bool SetEquals(IEnumerable<Rect> other)
        {
            var temp = new List<Rect>(other);
            if (temp.Count != list.Count)
                return false;
            foreach (var item in temp)
            {
                if (!list.Contains(item))
                    return false;
            }
            return true;
        }

        public void SymmetricExceptWith(IEnumerable<Rect> other)
        {
            var temp = new List<Rect>(other);
            foreach (var item in temp)
            {
                if (list.Contains(item))
                    list.Remove(item);
                else
                    list.Add(item);
            }
        }

        public void UnionWith(IEnumerable<Rect> other)
        {
            if (other == null)
                return;
            foreach (var item in other)
            {
                int index = list.IndexOf(item);
                if (index != -1)
                    list[index] = item;
                else
                    list.Add(item);
            }
        }

        void ICollection<Rect>.Add(Rect item)
        {
            this.Add(item);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}

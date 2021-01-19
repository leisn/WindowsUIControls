using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

namespace Leisn.UI.Xaml.Extensions
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
            if (index != -1)
                return false;
            list.Add(item);
            return true;
        }

        public void Add(params Rect[] items) => AddAll(items, false);

        public void AddIfNotEmpty(params Rect[] items) => AddAll(items, true);

        public void AddAll(IEnumerable<Rect> items, bool ignoreEmpty = true)
        {
            foreach (var item in items)
                if (!(ignoreEmpty && item.IsEmpty()))
                    Add(item);
        }

        public bool MergeItem(params Rect[] items) => MergeItems(items);

        /// <summary>
        /// 添加对象，后对集合中每一项尝试合并为更大的矩形，
        /// 直到当前集合内容不能再合并
        /// </summary>
        /// <returns> 只添加返回true，合并false</returns>
        public bool MergeItems(IEnumerable<Rect> items)
        {
            AddAll(items, true);
            var oldCount = Count;
            int newCount = MergeEachOthers();
            return newCount == oldCount;
        }

        /// <summary>
        /// 整理合并集合中所有项,使每一项不能再互相合并
        /// </summary>
        /// <returns>当前集合的项总数</returns>
        public int MergeEachOthers()
        {
            for (int i = Count - 1; i > 0; i--)
            {
                bool merged = false;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (list[j].TryMerge(list[i], out Rect target))
                    {
                        list[j] = target;
                        merged = true;
                    }
                }
                if (merged)//被合并就没有必要存在了
                    list.RemoveAt(i);
            }
            return Count;
        }

        /// <summary>
        /// 对集合中所有项进行剪切，排除剪切区域，合并其他区域到集合
        /// </summary>
        public void CutThenMergeOthers(Rect target, params Rect[] attachedItems)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                var x = list[i].Clip(target);
                if (x.Clipped)
                {
                    list.RemoveAt(i);
                    AddIfNotEmpty(x.ClipResult.Clips);
                }
            }
            AddIfNotEmpty(attachedItems);
            MergeEachOthers();
        }


        public void RemoveAt(int index) => list.RemoveAt(index);


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

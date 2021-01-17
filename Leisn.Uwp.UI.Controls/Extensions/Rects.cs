
using System;
using System.Collections;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Leisn.Uwp.UI.Extensions
{
    public static class Rects
    {
        public static bool IsEmpty(this Rect self)
        {
            return self.IsEmpty || self.Width == 0 || self.Height == 0;
        }

        public static Size Size(this Rect self)
        {
            return new Size(self.Width, self.Height);
        }


        public static bool Contains(this Rect self, Size size)
        {
            return !(size.Width > self.Width || size.Height > self.Height);
        }


        public static bool Contains(this Rect self, Rect rect)
        {
            return !(rect.X < self.X || rect.Y < self.Y ||
                     rect.Right > self.Right || rect.Bottom > self.Bottom);
        }

        public static bool IsIntersect(this Rect self, Rect rect)
        {
            return !(self.Right < rect.Left || self.Left > rect.Right || self.Bottom < rect.Top || self.Top > rect.Bottom);
        }

        public static bool TryMerge(this Rect self, Rect rect, out Rect Target)
        {
            bool merged = false;
            if (self.Contains(rect))
            {
                Target = self;
                merged = true;
            }
            else if (rect.Contains(self))
            {
                Target = rect;
                merged = true;
            }
            else if (self.IsIntersect(rect))
            {
                if (self.Top == rect.Top && self.Bottom == rect.Bottom)
                {
                    var left = Math.Min(self.Left, rect.Left);
                    var right = Math.Max(self.Right, rect.Right);
                    Target = new Rect(left, self.Top, right - left, self.Height);
                    merged = true;
                }
                else if (self.Left == rect.Left && self.Right == rect.Right)
                {
                    var top = Math.Min(self.Top, rect.Top);
                    var bottom = Math.Max(self.Bottom, rect.Bottom);
                    Target = new Rect(self.Left, top, self.Width, bottom - top);
                    merged = true;
                }
            }
            return merged;
        }

        public static (bool Merged, Rect Target) Merge(this Rect self, Rect rect)
        {
            if (self.TryMerge(rect, out Rect target))
                return (true, target);
            return (false, default);
        }

        public static (bool Overlap, Rect Clip) ClipOverlap(this Rect self, Rect rect)
        {
            var left = rect.Left - self.Left < 0 ? self.Left : rect.Left;
            var top = rect.Top - self.Top < 0 ? self.Top : rect.Top;
            var right = rect.Right - self.Right < 0 ? rect.Right : self.Right;
            var bottom = rect.Bottom - self.Bottom < 0 ? rect.Bottom : self.Bottom;

            var width = right - left;
            if (width < 0) width = 0;
            var height = bottom - top;
            if (height < 0) height = 0;

            return (right > left && bottom > top, new Rect(left, top, width, height));
        }

        public static (int Index, Rect Area) FindEnoughArea(IList<Rect> rects, Size size)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                if (rects[i].Contains(size))
                    return (i, rects[i]);
            }
            return (-1, default);
        }

        internal static Rect getOutBounds(IEnumerable rects)
        {
            double left = double.PositiveInfinity;
            double top = double.PositiveInfinity;
            double right = 0;
            double bottom = 0;
            foreach (var rect in rects)
            {
                var item = (Rect)rect;
                left = Math.Min(item.Left, left);
                top = Math.Min(item.Top, top);
                right = Math.Max(item.Right, right);
                bottom = Math.Max(item.Bottom, bottom);
            }
            if (right == 0 || bottom == 0 || left == double.PositiveInfinity || top == double.PositiveInfinity)
                return default;
            return new Rect(left, top, right - left, bottom - top);
        }

        public static Rect GetOutBounds(this Rect[,] rects) => getOutBounds(rects);
        public static Rect GetOutBounds(this IEnumerable<Rect> rects) => getOutBounds(rects);

        public static (Rect Target, Rect? ClipRight, Rect? ClipBottom) Clip(this Rect self, Size size)
        {
            var width = Math.Min(self.Width, size.Width);
            var height = Math.Min(self.Height, size.Height);
            var target = new Rect(self.Left, self.Top, size.Width, size.Height);

            double rightSpace = self.Width - width,
                   bottomSpace = self.Height - height;

            Rect? clipRight = null;
            if (rightSpace > 0)
                clipRight = new Rect(target.Right, self.Top, rightSpace, self.Height);
            Rect? clipBottom = null;
            if (bottomSpace > 0)
                clipBottom = new Rect(self.Left, target.Bottom, self.Width, bottomSpace);

            return (target, clipRight, clipBottom);
        }

        public static (bool Clipped, RectClip ClipResult) Clip(this Rect self, Rect rect)
        {
            var (clipped, target) = self.ClipOverlap(rect);

            if (!clipped)
                return (false, default);

            double leftSpace = target.Left - self.Left,
                rightSpace = self.Right - target.Right,
                topSpace = target.Top - self.Top,
                bottomSpace = self.Bottom - target.Bottom;

            RectClip result = new RectClip { Target = target };
            if (topSpace > 0)
                result.Top = (new Rect(self.Left, self.Top, self.Width, topSpace));
            if (rightSpace > 0)
                result.Right = (new Rect(target.Right, self.Top, rightSpace, self.Height));
            if (leftSpace > 0)
                result.Left = (new Rect(self.Left, self.Top, leftSpace, self.Height));
            if (bottomSpace > 0)
                result.Bottom = (new Rect(self.Left, target.Bottom, self.Width, bottomSpace));

            return (true, result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

namespace Leisn.Uwp.UI.Extensions
{
    public struct RectClip
    {
        public static RectClip Empty => new RectClip();

        public Rect Target;

        public Rect Left;
        public Rect Top;
        public Rect Right;
        public Rect Bottom;

        public bool IsClipsEmpty => Left.IsEmpty && Top.IsEmpty && Right.IsEmpty && Bottom.IsEmpty;

        public Rect[] Clips => new Rect[] { Left, Top, Right, Bottom };
    }
}

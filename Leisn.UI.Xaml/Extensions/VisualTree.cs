using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Leisn.UI.Xaml.Extensions
{
    public static class VisualTree
    {
        public static T FindDescendant<T>(this DependencyObject element)
            where T : DependencyObject
        {
            T retValue = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var type = child as T;
                if (type != null)
                {
                    retValue = type;
                    break;
                }

                retValue = FindDescendant<T>(child);

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }

        public static void PrintVisualTree(this DependencyObject element, int level = 0)
        {
            printSpace(level);
            System.Diagnostics.Debug.WriteLine(element.GetType().ToString());
            level++;
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                PrintVisualTree(child, level);
            }
        }

        private static void printSpace(int level)
        {
            string space = "";
            for (int i = 0; i < level; i++)
            {
                space += " ";
            }
            System.Diagnostics.Debug.Write(space);
        }

        public static T FindAncestor<T>(this DependencyObject element)
            where T : DependencyObject
        {
            T retValue = null;

            while (element != null)
            {
                var parent = VisualTreeHelper.GetParent(element);

                var type = parent as T;
                if (type != null)
                {
                    retValue = type;
                    break;
                }

                retValue = FindAncestor<T>(parent);
                if (retValue != null)
                    break;
            }
            return retValue;
        }
    }
}

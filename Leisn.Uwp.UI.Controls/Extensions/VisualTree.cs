using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Leisn.Uwp.UI.Extensions
{
    public static class VisualTree
    {
        /// <summary>
        /// Find first descendant control of a specified type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Parent element.</param>
        /// <returns>Descendant control or null if not found.</returns>
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

        /// <summary>
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

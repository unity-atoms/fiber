using System;
using System.Collections.Generic;

namespace FiberUtils
{
    public static class IListExtensions
    {
        public static void Swap<T>(this IList<T> list, T a, T b) where T : IEquatable<T>
        {
            var aIndex = -1;
            var bIndex = -1;

            for (var i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                if (item.Equals(a))
                {
                    aIndex = i;
                }
                else if (item.Equals(b))
                {
                    bIndex = i;
                }

                if (aIndex != -1 && bIndex != -1) break;
            }

            list.SwapByIndex(aIndex, bIndex);
        }

        public static void SwapByIndex<T>(this IList<T> list, int aIndex, int bIndex)
        {
            if (aIndex == bIndex) return;

            var oldA = list[aIndex];
            var oldB = list[bIndex];
            list[aIndex] = oldB;
            list[bIndex] = oldA;
        }

        public static void Move<T>(this List<T> list, int currentIndex, int newIndex)
        {
            var item = list[currentIndex];

            list.RemoveAt(currentIndex);

            if (newIndex > currentIndex)
            {
                newIndex--;
            }

            list.Insert(newIndex, item);
        }

        public static void AddRange<T>(this IList<T> list, IList<T> toAdd)
        {
            for (var i = 0; i < toAdd.Count; i++)
            {
                list.Add(toAdd[i]);
            }
        }
    }
}
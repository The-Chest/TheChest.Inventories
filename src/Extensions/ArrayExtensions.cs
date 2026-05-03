using System.Collections.Generic;

namespace TheChest.Inventories.Extensions
{
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Checks if the array contains any null values.
        /// </summary>
        /// <param name="array">An array of any type.</param>
        /// <returns>Returns true if the array contains at least one null value, otherwise false.</returns>
        internal static bool ContainsNull<T>(this T[] array) 
        {
            if (array.Length == 0)
                return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].IsNull())
                    return true;
            }

            return false;
        }

        internal static int GetAdjacentEqualCount<T>(this T[] array, int startIndex, int maxCount)
        {
            var index = startIndex;
            var amount = 1;
            var comparer = EqualityComparer<T>.Default;

            while (
                index + 1 < array.Length && amount < maxCount &&
                comparer.Equals(array[index + 1], array[startIndex])
            )
            {
                index++;
                amount++;
            }

            return index;
        }

        internal static bool HasAllEqual<T>(this T[] array)
        {
            if(array.Length == 0)
                return true;

            var first = array[0];
            var comparer = EqualityComparer<T>.Default;
            for (int i = 1; i < array.Length; i++)
            {
                if (!comparer.Equals(array[i], first))
                    return false;
            }
            return true;
        }

        internal static bool HasAllEqualAndNoNull<T>(this T[] array)
        {
            var first = array[0];
            if (first.IsNull())
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].IsNull())
                    return false;
                if (!comparer.Equals(array[i], first))
                    return false;
            }
            return true;
        }
    }
}

using System.Collections.Generic;

namespace TheChest.Inventories.Extensions
{
    /// <summary>
    /// Provides helper methods for array value inspection and comparison.
    /// </summary>
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

        /// <summary>
        /// Gets the last index in a contiguous sequence of values equal to the value at <paramref name="startIndex"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="array">The source array.</param>
        /// <param name="startIndex">The index where the comparison starts.</param>
        /// <param name="maxCount">The maximum number of equal adjacent values to inspect.</param>
        /// <returns>The last adjacent index that matches the start value, constrained by <paramref name="maxCount"/>.</returns>
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

        /// <summary>
        /// Determines whether all array values are equal.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="array">The source array.</param>
        /// <returns>True when all values are equal; otherwise false.</returns>
        internal static bool HasAllEqual<T>(this T[] array)
        {
            if (array.Length == 0)
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

        /// <summary>
        /// Determines whether all values are equal and none of them are null.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="array">The source array.</param>
        /// <returns>True when all values are equal and non-null; otherwise false.</returns>
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

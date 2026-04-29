using System;

namespace TheChest.Tests.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for working with arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Randomly reorders the elements of the specified array in place.
        /// </summary>
        /// <param name="items">The array whose elements will be shuffled.</param>
        public static void Shuffle(this Array items)
        {
            var rng = new Random();
            int n = items.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var item = items.GetValue(n);
                var item2 = items.GetValue(k);

                items.SetValue(item, k);
                items.SetValue(item2, n);
            }
        }
    }
}

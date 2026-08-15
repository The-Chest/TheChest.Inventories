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

        /// <summary>
        /// Randomly reorders the elements of the specified collection and returns a new array containing the shuffled elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="items">The collection of items to shuffle.</param>
        /// <param name="random">An optional random number generator.</param>
        /// <returns>A new array containing the shuffled elements.</returns>
        public static T[] ToShuffledArray<T>(this IEnumerable<T> items, Random? random = null)
        {
            random ??= new Random();
            var size = items.Count() * 4;
            return items
                .OrderBy(_ => random.Next(0, size))
                .ToArray();
        }
    }
}

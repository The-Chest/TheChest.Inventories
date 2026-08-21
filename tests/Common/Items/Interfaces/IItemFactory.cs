namespace TheChest.Tests.Common.Items.Interfaces
{
    /// <summary>
    /// A factory that creates items of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemFactory<T>
    {
        /// <summary>
        /// Creates an item
        /// </summary>
        /// <returns>Any item instance</returns>
        T CreateDefault();
        /// <summary>
        /// Creates an array of items using <see cref="CreateDefault"/>
        /// </summary>
        /// <param name="amount">Size of the returned array of items</param>
        /// <returns>An array with size <paramref name="amount"/></returns>
        T[] CreateMany(int amount);

        /// <summary>
        /// Creates an item with every property set to a random value
        /// </summary>
        /// <returns>Any item instance</returns>
        T CreateRandom();
        /// <summary>
        /// Creates an item with every property set to a random value, but different from the provided item
        /// </summary>
        /// <param name="item">The item to be different from</param>
        /// <returns>Any item instance different from <paramref name="item"/></returns>
        T CreateRandomDifferentFrom(T item);
        (T item1, T item2) CreateRandomDistinctPair();

        /// <summary>
        /// Creates an array of items using <see cref="CreateRandom"/>
        /// </summary>
        /// <param name="amount">Size of the returned array of items</param>
        /// <returns>An array with size <paramref name="amount"/></returns>
        T[] CreateManyRandom(int amount);
        (T[] items1, T[] items2) CreateManyRandomDistinctPairs(int amount);
        T[] CreateManyRandomDifferentFrom(T item, int amount);
    }
}

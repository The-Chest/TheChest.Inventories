namespace TheChest.Tests.Common.Items.Interfaces
{
    /// <summary>
    /// Generic item creation.
    /// There is nothing special about this interface. 
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
        /// Creates an item with every property set to a random value
        /// </summary>
        /// <returns>Any item instance</returns>
        T CreateRandom();
        /// <summary>
        /// Creates an array of items using <see cref="CreateDefault"/>
        /// </summary>
        /// <param name="amount">Size of the returned array of items</param>
        /// <returns>An array with size <paramref name="amount"/></returns>
        T[] CreateMany(int amount);
        /// <summary>
        /// Creates an array of items using <see cref="CreateRandom"/>
        /// </summary>
        /// <param name="amount">Size of the returned array of items</param>
        /// <returns>An array with size <paramref name="amount"/></returns>
        T[] CreateManyRandom(int amount);
    }
}

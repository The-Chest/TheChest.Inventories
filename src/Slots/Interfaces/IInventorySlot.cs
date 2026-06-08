using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Slot
    /// </summary>
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public interface IInventorySlot<T> : ISlot<T>
    {
        /// <summary>
        /// Checks whether the specified item can be added to the collection.
        /// </summary>
        /// <param name="item">The item to be evaluated.</param>
        /// <returns><see langword="true"/> if the item can be added; otherwise, <see langword="false"/>.</returns>
        bool CanAdd(T item);
        /// <summary>
        /// Adds the item in the current Slot if <see cref="ISlot{T}.IsFull"/> is <see langword="false"/>
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns><see langword="true"/> if the value is successful added</returns>
        bool Add(T item);
        /// <summary>
        /// Attempts to add the specified item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns><see langword="true"/> if the item was added; otherwise, <see langword="false"/>.</returns>
        bool TryAdd(T item);

        /// <summary>
        /// Checks whether the specified item can replace the content of the slot.
        /// </summary>
        /// <param name="item">The item to be evaluated.</param>
        /// <returns><see langword="true"/> if the item can be replaced; otherwise, <see langword="false"/>.</returns>
        bool CanReplace(T item);
        /// <summary>
        /// Attempts to replace the content of the slot with the specified item.
        /// </summary>
        /// <param name="item">The item to replace the current content with.</param>
        /// <param name="oldItem">When this method returns <see langword="true"/>, contains the item that was replaced, if the replacement was successful; otherwise, the default value for the type of the item.</param>
        /// <returns><see langword="true"/> if the item was successfully replaced; otherwise, <see langword="false"/>.</returns>
        bool TryReplace(T item, out T oldItem);
        /// <summary>
        /// Replaces the content of slot to item
        /// </summary>
        /// <param name="item">Item to replace</param>
        /// <returns>Old value of the slot</returns>
        T Replace(T item);

        /// <summary>
        /// Returns an item from slot
        /// </summary>
        /// <returns>Returns an item of the slot</returns>
        T Get();
    }
}
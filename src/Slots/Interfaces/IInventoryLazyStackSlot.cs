using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Stackable Slot with lazy behavior
    /// </summary>
    /// <para>
    /// This interface is still unstable. Some methods can be moved to a separated interface.
    /// </para>
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public interface IInventoryLazyStackSlot<T> : ILazyStackSlot<T>
    {
        #region Add
        /// <summary>
        /// Checks if the slot can add an amount of items
        /// </summary>
        /// <param name="item">item to be added to the slot</param>
        /// <param name="amount">the amount of the <paramref name="item"/> to be added to the slot</param>
        /// <returns>true if is possible to add the amount of the <paramref name="item"/> inside the slot</returns>
        bool CanAdd(T item, int amount = 1);
        /// <summary>
        /// Tries to add an amount of items to the current slot.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <param name="amount">The amount of items to be added.</param>
        /// <returns><see langword="true"/> when the items were added, otherwise <see langword="false"/>.</returns>
        bool TryAdd(T item, int amount = 1);
        /// <summary>
        /// Add an amount of items inside the current slot
        /// </summary>
        /// <param name="item">The item to be added </param>
        /// <param name="amount">The amount of items added</param>
        /// <returns>Return 0 if all items are fully added to slot, else will return the amount left</returns>
        int Add(T item, int amount = 1);
        #endregion

        #region Replace
        /// <summary>
        /// Checks if the slot can replace an item
        /// </summary>
        /// <param name="item">The item to be added and replace the old ones</param>
        /// <param name="amount">The amount of items that will replace</param>
        /// <returns>true if is possible to replace</returns>
        /// 
        bool CanReplace(T item, int amount = 1);
        /// <summary>
        /// Removes the current item of Slot and replace by new ones
        /// </summary>
        /// <param name="item">The item wich will replace the old one</param>
        /// <param name="amount">The amount of the New item</param>
        /// <returns>Returns an array of the old item</returns>
        T[] Replace(T item, int amount = 1);
        /// <summary>
        /// Removes the current item of Slot and replace by new ones
        /// </summary>
        /// <param name="item">The item in which elements are to be replaced.</param>
        /// <param name="amount">The amount of items that will be replaces.</param>
        /// <param name="oldItems">When this method returns <see langword="true"/>, contains the elements that were replaced.</param>
        /// <returns><see langword="true"/> if the replacement was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReplace(T item, int amount, out T[] oldItems);
        #endregion

        #region Get
        /// <summary>
        /// Gets an amount of items from the slot
        /// </summary>
        /// <param name="amount">Desired amount to be returned</param>
        /// <returns>an array items from the slot</returns>
        T[] Get(int amount = 1);
        /// <summary>
        /// Gets all items from inside the slot. 
        /// </summary>
        /// <returns>An array with all items from slot</returns>
        T[] GetAll();
        #endregion
    }
}

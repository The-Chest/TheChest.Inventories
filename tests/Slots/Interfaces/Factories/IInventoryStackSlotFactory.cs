using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Interfaces.Factories
{
    /// <summary>
    /// Factory interface to instantiate any <see cref="IInventoryStackSlot{T}"/>
    /// </summary>
    /// <typeparam name="T">Any type of item inside IInventorySlot</typeparam>
    public interface IInventoryStackSlotFactory<T>
    {
        /// <summary>
        /// Creates an empty <see cref="IInventoryStackSlot{T}"/> with a maximum amount of items.
        /// </summary>
        /// <param name="maxAmount">The maximum amount of items the slot can hold.</param>
        /// <returns>An empty <see cref="IInventoryStackSlot{T}"/> with the specified maximum amount.</returns>
        IInventoryStackSlot<T> Empty(int maxAmount);
        /// <summary>
        /// Creates a <see cref="IInventoryStackSlot{T}"/> with a specific item, amount, and maximum amount.
        /// </summary>
        /// <param name="item">The item to be added to the slot.</param>
        /// <param name="amount">The amount of the item to be added.</param>
        /// <param name="maxAmount">The maximum amount of items the slot can hold.</param>
        /// <returns>A <see cref="IInventoryStackSlot{T}"/> with the specified item, amount, and maximum amount.</returns>
        [Obsolete("Use WithItems instead.")]
        IInventoryStackSlot<T> WithItem(T item, int amount, int maxAmount);
        /// <summary>
        /// Creates a <see cref="IInventoryStackSlot{T}"/> with specific items and a maximum amount.
        /// </summary>
        /// <param name="items">The items to be added to the slot.</param>
        /// <param name="maxAmount">The maximum amount of items the slot can hold.</param>
        /// <returns>A <see cref="IInventoryStackSlot{T}"/> with the specified items and maximum amount.</returns>
        IInventoryStackSlot<T> WithItems(T[] items, int maxAmount);
        /// <summary>
        /// Creates a <see cref="IInventoryStackSlot{T}"/> that is full with the specified items.
        /// </summary>
        /// <param name="items">The items to be added to the slot.</param>
        /// <returns>A <see cref="IInventoryStackSlot{T}"/> that is full with the specified items.</returns>
        IInventoryStackSlot<T> Full(T[] items);
    }
}

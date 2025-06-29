﻿using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Stackable Slot 
    /// </summary>
    /// <typeparam name="T">Item the Slot Accept</typeparam>
    public interface IInventoryStackSlot<T> : IStackSlot<T>
    {
        /// <summary>
        /// Checks if is possible to add one item to the slot
        /// </summary>
        /// <param name="item">item to be checked to add</param>
        /// <returns>true if is possible to add the item</returns>
        bool CanAdd(T item);
        /// <summary>
        /// Checks if is possible to add an array of items to the slot
        /// </summary>
        /// <param name="items">items to be checked to add</param>
        /// <returns>true if is possible to add <paramref name="items"/></returns>
        bool CanAdd(T[] items);
        /// <summary>
        /// Adds an item to the slot
        /// </summary>
        /// <param name="item">The item to be added</param>
        [Obsolete("Use Add(T item) instead to avoid reference issues. This method will be removed.")]
        void Add(ref T item);
        /// <summary>
        /// Adds an item to the slot
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns>True if the value is successful added</returns>
        bool Add(T item);
        /// <summary>
        /// Adds an array of items to the slot.
        /// </summary>
        /// <param name="items">items to bem added to the slot</param>
        [Obsolete("Use Add(T[] items) instead to avoid reference issues. This method will be removed.")]
        void Add(ref T[] items);
        /// <summary>
        /// Adds an array of items to the slot.
        /// </summary>
        /// <param name="items">items to be added to the slot</param>
        /// <returns>The items that were not added to the slot</returns>
        T[] Add(T[] items);
        /// <summary>
        /// Checks if is possible to replace an item
        /// </summary>
        /// <param name="item">item to be checked if is possible to replace</param>
        /// <returns>returns true if is possible to replace</returns>
        bool CanReplace(T item);
        /// <summary>
        /// Checks if is possible to replace an array of items
        /// </summary>
        /// <param name="items">array of items to be checked if is possible to replace</param>
        /// <returns>returns true if is possible to replace</returns>
        bool CanReplace(T[] items);
        /// <summary>
        /// Replaces the items from the slot to <paramref name="items"/>
        /// </summary>
        /// <param name="items">items that will replace items from slots</param>
        /// <returns>old items from the slot if the replacement is possible</returns>
        [Obsolete("Use Replace(T[] items) instead to avoid reference issues. This method will be removed.")]
        T[] Replace(ref T[] items);
        /// <summary>
        /// Replaces the items from the slot to <paramref name="items"/>
        /// </summary>
        /// <param name="items">items that will replace items from slots</param>
        /// <returns>old items from the slot if the replacement is possible</returns>
        T[] Replace(T[] items);
        /// <summary>
        /// Replaces the items from the slot to one <paramref name="item"/>
        /// </summary>
        /// <param name="item">The item that will replace items from slots</param>
        /// <returns>the old items from the slot</returns>
        T[] Replace(T item);
        /// <summary>
        /// Replaces the items from the slot to one <paramref name="item"/>
        /// </summary>
        /// <param name="item">the item that will replace items from slots</param>
        /// <returns>returns the old items from the slot</returns>
        [Obsolete("Use Replace(T item) instead to avoid reference issues. This method will be removed.")]
        T[] Replace(ref T item);
        /// <summary>
        /// Gets a single item from the slot
        /// </summary>
        /// <returns>an item from the slot</returns>
        T? Get();
        /// <summary>
        /// Gets an amount of items from the slot.
        /// </summary>
        /// <param name="amount">Amount of items to get</param>
        /// <returns>an array with the size of the param <paramref name="amount"/></returns>
        T[] Get(int amount);
        /// <summary>
        /// Gets all items from inside the Slot. 
        /// </summary>
        /// <returns>an array with all items from slot</returns>
        T[] GetAll();
    }
}

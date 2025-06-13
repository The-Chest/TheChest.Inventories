﻿using TheChest.Core.Containers.Interfaces;
using TheChest.Inventories.Containers.Events.Stack;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Interface with methods for interaction with the Inventory using stacks
    /// <para>
    /// This interface is still unstable. Some methods can be moved to a separated interface.
    /// </para>
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public interface IStackInventory<T> : IStackContainer<T>
    {
        event StackInventoryAddEventHandler<T>? OnAdd;

        event StackInventoryGetEventHandler<T>? OnGet;

        #region IStackInventory
        /// <summary>
        /// Gets an <see cref="item"/> inside a slot
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>Returns the item inside <paramref name="index"/> Slot</returns>
        T? Get(int index);

        /// <summary>
        /// Search an Item from inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>Returns the first item found or null</returns>
        T? Get(T item);

        /// <summary>
        /// Search an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be found</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>An array with amount of items searched (or the max it can)</returns>
        T[] Get(T item, int amount);

        /// <summary>
        /// Get all Items of the selected type from all slots
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>An array with all items found in the inventory</returns>
        T[] GetAll(T item);

        /// <summary>
        /// Gets the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">The item to de counted</param>
        /// <returns>The current amount of the item in the Inventory</returns>
        int GetCount(T item);

        /// <summary>
        /// Adds and array of item in a avaliable slot
        /// </summary>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <returns></returns>
        T[] Add(T[] items);

        /// <summary>
        /// Adds an item in a avaliable slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>true if is possible to add <paramref name="item"/></returns>
        bool Add(T item);
        #endregion

        #region IInventory
        /// <summary>
        /// Gets an amount of items inside the Inventory Slot
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <param name="amount">Amount of the item to be returned</param>
        /// <returns>An array with the amount of items inside the slot (or the max it can)</returns>
        T[] Get(int index, int amount);

        /// <summary>
        /// Returns all the items from the selected slot 
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <returns>An array with of items</returns>
        T[] GetAll(int index);

        /// <summary>
        /// Adds an item in a specific slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <param name="index">slot where the item will be added</param>
        /// <param name="replace"></param>
        /// <returns>The item that couldn't be added or the replaced item if <paramref name="replace"/> is true</returns>
        T[] AddAt(T item, int index, bool replace = true);

        /// <summary>
        /// Adds an array of item inside the inventory
        /// </summary>
        /// <param name="items">Array of item of the same type wich will be added to inventory</param>
        /// <param name="index">Wich slot the items will be added</param>
        /// <param name="replace">Defines if the current Slot item will be replaced by the <paramref name="items"/> param</param>
        /// <returns>An array of items replaced or couldn't be added</returns>
        T[] AddAt(T[] items, int index, bool replace = true);
        #endregion

        #region IInteractive
        /// <summary>
        /// Move a item between two slots
        /// </summary>
        /// <param name="origin">Selected item</param>
        /// <param name="target">Where the item will be placed</param>
        void Move(int origin, int target);

        /// <summary>
        /// Returns every item from the inventory
        /// </summary>
        /// <returns>Returns an Array of <see cref="{T}"/></returns>
        T[] Clear();
        #endregion
    }
}

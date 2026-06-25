using System;
using TheChest.Core.Containers.Interfaces;
using TheChest.Inventories.Containers.Events;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Inventory Interface with add and get methods
    /// </summary>
    /// <remarks>
    /// This interface is still unstable. Some methods can be moved to separated interfaces.
    /// </remarks>
    /// <typeparam name="T">An item type</typeparam>
    public interface IInventory<T> : IContainer<T>
    {
        #region Get
        /// <summary>
        /// Raised when an amount of item is requested from an index of the inventory
        /// </summary>
        event InventoryGetEventHandler<T> OnGet;

        /// <summary>
        /// Gets an item inside a slot
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>An item inside of the <paramref name="index"/> Slot</returns>
        T Get(int index);
        /// <summary>
        /// Gets an item inside the inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>First item found that is equal <paramref name="item"/></returns>
        T Get(T item);
        /// <summary>
        /// Gets an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be found</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>An array with the <paramref name="amount"/> of items equal to <paramref name="item"/></returns>
        T[] Get(T item, int amount);
        /// <summary>
        /// Get all Item of the selected type from all slots
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>An array with all items that are equal to <paramref name="item"/> in the inventory</returns>
        T[] GetAll(T item);

        /// <summary>
        /// Gets every item from inventory
        /// </summary>
        /// <returns>Returns an Array of items</returns>
        T[] Clear();
        #endregion

        #region Count
        /// <summary>
        /// Get the amount of an <paramref name="item"/> inside the inventory
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>The current amount of the <paramref name="item"/> in the Inventory</returns>
        int GetCount(T item);
        #endregion

        /// <summary>
        /// Raised when an amount of item is added to an index
        /// </summary>
        event InventoryAddEventHandler<T> OnAdd;
        #region Add
        /// <summary>
        /// Checks if <paramref name="item"/> can be added to any slot.
        /// </summary>
        /// <param name="item">The item to evaluate for addition to the inventory.</param>
        /// <returns><see langword="true"/> if the <paramref name="item"/> can be added; otherwise, false.</returns>
        [Obsolete("Use CanAdd(params T[]) instead. This method will be removed in future versions.")]
        bool CanAdd(T item);
        /// <summary>
        /// Checks if <paramref name="items"/> can be added to any slot.
        /// </summary>
        /// <param name="items">An array of items to evaluate for addition to the inventory.</param>
        /// <returns><see langword="true"/> if ALL <paramref name="items"/> can be added; otherwise, <see langword="false"/>.</returns>
        bool CanAdd(params T[] items);

        /// <summary>
        /// Attempts to add the specified items to the Inventory.
        /// </summary>
        /// <param name="items">An array of items to add.</param>
        /// <returns><see langword="true"/> if all items were added successfully; otherwise, <see langword="false"/>.</returns>
        bool TryAdd(params T[] items);

        /// <summary>
        /// <para> Adds an item in a avaliable slot </para>
        /// <para> This method return will change to void in future versions. </para>
        /// <para> Use <see cref="CanAdd(T[])"/> to check if the item can be added before calling this method.</para>
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns><see langword="true"/> if the <paramref name="item"/> could be added</returns>
        [Obsolete("Use Add(params T[]) instead. This method will be removed in future versions.")]
        bool Add(T item);
        /// <summary>
        /// Adds and array of items in a avaliable slot
        /// </summary>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <returns>The items from param that were not possible to add</returns>
        T[] Add(params T[] items);
        #endregion

        #region AddAt
        /// <summary>
        /// Determines whether the specified item can be added at the given index.
        /// </summary>
        /// <param name="item">The item to evaluate for insertion at the specified index.</param>
        /// <param name="index">The zero-based index at which to check if the item can be added. Must be within the valid range of the collection.</param>
        /// <returns><see langword="true"/> if the item can be added at the specified index; otherwise, <see langword="false"/>.</returns>
        bool CanAddAt(T item, int index);

        /// <summary>
        /// Attempts to add the specified item at the given index in the collection.
        /// </summary>
        /// <remarks>
        /// This method does not throw an exception if the operation fails. Instead, it returns
        /// <see langword="false"/> to indicate failure.
        /// </remarks>
        /// <param name="item">The item to add to the inventory.</param>
        /// <param name="index">The zero-based index at which to insert the item.</param>
        /// <returns><see langword="true"/> if the <paramref name="item"/> was successfully added at the specified <paramref name="index"/>; otherwise, <see langword="false"/></returns>
        bool TryAddAt(T item, int index);

        /// <summary>
        /// <para>Adds an item in a specific slot </para>
        /// <para>This method return will change to void in future versions.</para>
        /// <para>Use <see cref="CanAddAt(T,int)"/> to check if the item can be added before calling this method.</para>
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <param name="index">Slot where the item will be added</param>
        /// <returns><see langword="true"/> if the <paramref name="item"/> could be added to the <paramref name="index"/></returns>
        bool AddAt(T item, int index);
        #endregion

        #region Replace
        /// <summary>
        /// Raised when an item is replaced in a specific slot
        /// </summary>
        event InventoryReplaceEventHandler<T> OnReplace;
        /// <summary>
        /// Checks if an item can be replaced in a specific slot
        /// </summary>
        /// <param name="item">Item to be checked to ocupy the slot on <paramref name="index"/></param>
        /// <param name="index">The index of the slot to be checked for replacement</param>
        /// <returns><see langword="true"/> if the <paramref name="item"/> can be replaced in the <paramref name="index"/> slot; otherwise, <see langword="false"/>.</returns>
        bool CanReplace(T item, int index);
        /// <summary>
        /// Tries to replace an item in a specific slot
        /// </summary>
        /// <param name="item">The item to be placed in the specified slot</param>
        /// <param name="index">The index of the slot where the item will be placed</param>
        /// <param name="oldItem">When this method returns <see langword="true"/>, contains the item that was replaced, if the replacement was successful; otherwise, the default value for the type of the item.</param>
        /// <returns><see langword="true"/> if the item was successfully replaced; otherwise, <see langword="false"/>.</returns>
        bool TryReplace(T item, int index, out T oldItem);
        /// <summary>
        /// Replaces an item in a specific slot
        /// </summary>
        /// <param name="item">The item that will now ocupy the slot on <paramref name="index"/></param>
        /// <param name="index">The index of the <paramref name="item"/> will ocupy</param>
        /// <returns>The old item from the slot on <paramref name="index"/></returns>
        T Replace(T item, int index);
        #endregion

        #region Move
        /// <summary>
        /// Raised when one item is moved from an index to other on the inventory
        /// </summary>
        event InventoryMoveEventHandler<T> OnMove;

        /// <summary>
        /// Checks if the specified item can be moved from the origin index to the target index.
        /// </summary>
        /// <param name="origin">The zero-based index representing the item's current position.</param>
        /// <param name="target">The zero-based index representing the desired target position.</param>
        /// <returns>true if the item can be moved to the target index; otherwise, false.</returns>
        bool CanMove(int origin, int target);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool TryMove(int origin, int target);
        /// <summary>
        /// Moves an item from one index to another in the inventory
        /// </summary>
        /// <param name="origin">Selected item</param>
        /// <param name="target">Where the item will be placed</param>
        void Move(int origin, int target);
        #endregion
    }
}

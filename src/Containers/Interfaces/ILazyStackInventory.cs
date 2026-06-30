using System;
using TheChest.Core.Containers.Interfaces;
using TheChest.Inventories.Containers.Events.Stack.Lazy;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Interface with methods for a basic Inventory Stackable with lazy behavior
    /// </summary>
    /// <typeparam name="T">Item the Inventory accept</typeparam>
    public interface ILazyStackInventory<T> : ILazyStackContainer<T>
    {
        #region Get
        /// <summary>
        /// Raised when an amount of item is requested from an index of the inventory
        /// </summary>
        event LazyStackInventoryGetEventHandler<T> OnGet;
        /// <summary>
        /// Gets an item from inside a slot
        /// </summary>
        /// <param name="index">Slot's inventory to be searched</param>
        /// <returns>Returns the item inside <paramref name="index"/> Slot</returns>
        T Get(int index);
        /// <summary>
        /// Returns an amount of items inside the Inventory Slot
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <param name="amount">Amount of the item to be returned</param>
        /// <returns>Returns the amount of items inside the slot (or the max it can)</returns>
        T[] Get(int index, int amount);
        /// <summary>
        /// Search an Item from inventory
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>Returns the first item found</returns>
        T Get(T item);
        /// <summary>
        /// Search an amount of items in the inventory
        /// </summary>
        /// <param name="item">Item to be founded</param>
        /// <param name="amount">Amount to be returned</param>
        /// <returns>Returns the amount of items searched (or the max it can)</returns>
        T[] Get(T item, int amount);
        /// <summary>
        /// Get all Item of the selected type from all slots
        /// </summary>
        /// <param name="item">Item to be search</param>
        /// <returns>Returns a list with all items founded in the inventory</returns>
        T[] GetAll(T item);
        /// <summary>
        /// Returns all the items from the selected slot 
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <returns>An array with of items</returns>
        T[] GetAll(int index);
        #endregion

        /// <summary>
        /// Raised when an amount of item is added to an index of the inventory
        /// </summary>
        event LazyStackInventoryAddEventHandler<T> OnAdd;

        #region Add
        /// <summary>
        /// Determines whether the specified amount of <paramref name="item"/> can be added to the inventory.
        /// </summary>
        /// <param name="item">The item to evaluate for addition.</param>
        /// <param name="amount">The number of units of the <paramref name="item"/> to check for addability.</param>
        /// <returns>true if the <paramref name="item"/> can be added in the specified <paramref name="amount"/>; otherwise, false.</returns>
        bool CanAdd(T item, int amount = 1);
        /// <summary>
        /// Attempts to add the specified amount of <paramref name="item"/> to the inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns><see langword="true"/> if the requested <paramref name="amount"/> was added; otherwise, <see langword="false"/>.</returns>
        bool TryAdd(T item, int amount);
        /// <summary>
        /// Adds items inside the inventory
        /// </summary>
        /// <param name="item">Array of item of the same type wich will be added to inventory</param>
        /// <param name="amount">Amount of the item to be returned</param>
        /// <returns>Returns the amount of items that couldn't be added</returns>
        int Add(T item, int amount);
        /// <summary>
        /// Adds an item in a avaliable slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <returns>returns true if the item could be added</returns>
        [Obsolete("Use Add(T item, int amount) instead. This method will be removed in future versions.")]
        bool Add(T item);
        #endregion

        #region AddAt
        /// <summary>
        /// Determines whether the specified <paramref name="item"/> can be added at the given index in the inventory, for the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="item">The item to evaluate for addition to the inventory.</param>
        /// <param name="index">The zero-based index at which to check if the <paramref name="item"/> can be added.</param>
        /// <param name="amount">The number of times the <paramref name="item"/> is intended to be added at the specified <paramref name="index"/>. Must be greater than zero.</param>
        /// <returns>true if the <paramref name="item"/> can be added at the specified <paramref name="index"/> for the given <paramref name="amount"/>; otherwise, false.</returns>
        bool CanAddAt(T item, int index, int amount = 1);
        /// <summary>
        /// Attempts to add the specified amount of <paramref name="item"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="index">The slot index where the item should be added.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns><see langword="true"/> if the requested <paramref name="amount"/> was added at <paramref name="index"/>; otherwise, <see langword="false"/>.</returns>
        bool TryAddAt(T item, int index, int amount);
        /// <summary>
        /// Adds an amount of item in a specific slot
        /// </summary>
        /// <param name="item">item to be added</param>
        /// <param name="index">slot where the item will be added</param>
        /// <param name="amount">amount of the item</param>
        /// <returns>Returns the amount of items that couldn't be added</returns>
        int AddAt(T item, int index, int amount);
        #endregion

        #region Move
        /// <summary>
        /// Raised when one item is moved from an index to other on the inventory
        /// </summary>
        event LazyStackInventoryMoveEventHandler<T> OnMove;

        /// <summary>
        /// Checks if the specified items can be moved from the origin index to the target index.
        /// </summary>
        /// <param name="origin">The zero-based index representing the items current position.</param>
        /// <param name="target">The zero-based index representing the desired target position.</param>
        /// <returns><see langword="true"/> if the item can be moved to the target index; otherwise, <see langword="false"/>.</returns>
        bool CanMove(int origin, int target);
        /// <summary>
        /// Tries to move all of the items from one index to another in the inventory
        /// </summary>
        /// <param name="origin">The zero-based index representing the items current position.</param>
        /// <param name="target">The zero-based index representing the desired target position.</param>
        /// <returns><see langword="true"/> if the items has been successfully moved; otherwise, <see langword="false"/>.</returns>
        bool TryMove(int origin, int target);
        /// <summary>
        /// Moves all of the items from one index to another in the inventory
        /// </summary>
        /// <param name="origin">The zero-based index representing the items current position.</param>
        /// <param name="target">The zero-based index representing the desired target position.</param>
        void Move(int origin, int target);
        #endregion

        #region Replace
        /// <summary>
        /// Raised when an amount of item is replaced on an index of the inventory
        /// </summary>
        event LazyStackInventoryReplaceEventHandler<T> OnReplace;

        /// <summary>
        /// Checks if an item can be replaced in a specific slot
        /// </summary>
        /// <param name="item">Item to check to replaced at the index <paramref name="index"/></param>
        /// <param name="index">Index of the slot where the item will be placed</param>
        /// <param name="amount">Amount of the item to be placed</param>
        /// <returns>True if the item can be replaced at the index <paramref name="index"/> for the given <paramref name="amount"/>; otherwise, false.</returns>
        bool CanReplace(T item, int index, int amount = 1);
        /// <summary>
        /// Replaces an amount of items in a specific slot
        /// </summary>
        /// <param name="item">Item to be replaced to the index <paramref name="index"/></param>
        /// <param name="index">Index of the slot where the item will be placed</param>
        /// <param name="amount">Amount of the item to be placed</param>
        /// <returns>The item that was replaced by <paramref name="item"/></returns>
        T[] Replace(T item, int index, int amount);
        /// <summary>
        /// Replaces elements in the specified array at the given index and outputs the replaced elements.
        /// </summary>
        /// <param name="item">The item in which elements are to be replaced.</param>
        /// <param name="index">The zero-based index at which to perform the replacement.</param>
        /// <param name="amount">The amount of items that will be added to the slot.</param>
        /// <param name="oldItems">When this method returns <see langword="true"/>, contains the elements that were replaced.</param>
        /// <returns><see langword="true"/> if the replacement was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReplace(T item, int index, int amount, out T[] oldItems);
        #endregion

        #region Count
        /// <summary>
        /// Returns the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">The item to de counted</param>
        /// <returns>Returns the current amount of the item in the Inventory</returns>
        int GetCount(T item);
        #endregion

        #region Clear
        /// <summary>
        /// Returns every item from the inventory
        /// </summary>
        /// <returns>Returns an Array of items</returns>
        T[] Clear();
        #endregion
    }
}

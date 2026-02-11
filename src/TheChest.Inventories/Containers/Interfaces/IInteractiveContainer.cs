using System;
using TheChest.Core.Containers.Interfaces;

namespace TheChest.Inventories.Containers.Interfaces
{
    /// <summary>
    /// Interface with methods for interaction with the Container 
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    [Obsolete("Do not inherit it directly, use IInventory<T> or IStackInventory<T> instead")]
    public interface IInteractiveContainer<T> : IContainer<T>
    {
        /// <summary>
        /// Moves an item from one index to another in the inventory
        /// </summary>
        /// <param name="origin">Selected item</param>
        /// <param name="target">Where the item will be placed</param>
        void Move(int origin, int target);

        /// <summary>
        /// Returns every item from inventory
        /// </summary>
        /// <returns>Returns an Array of items</returns>
        T[] Clear();
    }
}

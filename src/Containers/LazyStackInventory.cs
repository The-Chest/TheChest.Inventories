using System;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="ILazyStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public partial class LazyStackInventory<T> : LazyStackContainer<T>, ILazyStackInventory<T>
    {
        /// <summary>
        /// Slots of the inventory
        /// </summary>
        protected new readonly IInventoryLazyStackSlot<T>[] slots;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyStackInventory{T}"/> class with default size of 20 and max stack size of 1.
        /// </summary>
        public LazyStackInventory() : base() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyStackInventory{T}"/> class with the specified size and maximum stack size.
        /// </summary>
        /// <inheritdoc/>
        public LazyStackInventory(int size, int maxStackSize) : base(size, maxStackSize) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyStackInventory{T}"/> class with the specified items and maximum amount.
        /// </summary>
        /// <inheritdoc/>
        public LazyStackInventory((T item, int amount)[] items, int maxAmount) : base(items, maxAmount) { }

        /// <summary>
        /// Creates an Stackable Inventory with lazy behavior
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventoryLazyStackSlot{T}"/></param>
        /// <exception cref="ArgumentNullException">When <paramref name="slots"/> is <see langword="null"/></exception>
        public LazyStackInventory(IInventoryLazyStackSlot<T>[] slots) : base(slots)
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        /// <summary>
        /// Returns the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">Item to be searched</param>
        /// <returns>The amount of the <paramref name="item"/> in the Inventory </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual int GetCount(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var count = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    count += slot.Amount;
                }
            }
            return count;
        }
    }
}

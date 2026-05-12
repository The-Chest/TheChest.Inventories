using System;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public partial class Inventory<T> : Container<T>, IInventory<T>
    {
        /// <summary>
        /// An array of <see cref="IInventorySlot{T}"/> that holds the slots of this inventory
        /// </summary>
        protected new readonly IInventorySlot<T>[] slots;

        /// <summary>
        /// Creates an empty Inventory with a default size of 0
        /// </summary>
        public Inventory() 
        {
            this.slots = Array.Empty<IInventorySlot<T>>();
        }
        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation
        /// </summary>
        /// <param name="size">Number with the size of the Inventory</param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="size"/> is zero or smaller</exception>
        public Inventory(int size) : this(new T[size], size) { }
        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation and the size of the provided items array
        /// </summary>
        /// <param name="items">Items to be added to the slots on the inventory</param>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is null</exception>
        public Inventory(T[] items) : this(items, items.Length) { }
        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation, provided <paramref name="items"/> and the size of the inventory is defined by the <paramref name="size"/>
        /// </summary>
        /// <param name="items">Items to be added to the slots on the inventory</param>
        /// <param name="size">Number with the size of the inventory</param>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="size"/> is zero or smaller</exception>
        /// <exception cref="ArgumentException">When <paramref name="items"/> length is bigger than <paramref name="size"/></exception>
        public Inventory(T[] items, int size) : base(items, size) 
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));
            if (size < items.Length)
                throw new ArgumentException(
                    InventoryErrors.ItemsBiggerThanInventorySize,
                    nameof(size)
                );

            this.slots = new InventorySlot<T>[size];
            for (var i = 0; i < size; i++)
            {
                this.slots[i] = i < items.Length
                    ? new InventorySlot<T>(items[i])
                    : new InventorySlot<T>();
            }
        }
        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventorySlot{T}"/></param>
        public Inventory(IInventorySlot<T>[] slots) : base(slots) 
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual int GetCount(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var count = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    count++;
                }
            }
            return count;
        }
    }
}

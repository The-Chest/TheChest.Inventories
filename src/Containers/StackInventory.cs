using System;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Extensions;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public partial class StackInventory<T> : StackContainer<T>, IStackInventory<T>
    {
        /// <summary>
        /// Array of <see cref="IInventoryStackSlot{T}"/> slots in the inventory
        /// </summary>
        protected new readonly IInventoryStackSlot<T>[] slots;

        /// <summary>
        /// Creates an empty StackInventory with a default size of 0
        /// </summary>
        public StackInventory()
        {
            this.slots = Array.Empty<IInventoryStackSlot<T>>();
        }
        /// <summary>
        /// Creates an StackInventory and initializes it with the provided size and max stack size.
        /// </summary>
        /// <inheritdoc />
        public StackInventory(int size, int maxStackSize) : this(new T[size], maxStackSize) { }
        /// <summary>
        /// Creates an StackInventory and initializes it with the provided items and max stack size.
        /// </summary>
        /// <inheritdoc />
        public StackInventory(T[] items, int maxStackSize) : base(items, maxStackSize) 
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (maxStackSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxStackSize), StackInventoryErrors.MaxStackSizeMustBeGreaterThanZero);
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);

            this.slots = items.ToStackSlots(maxStackSize);
        }
        /// <summary>
        /// Creates an Inventory with <see cref="IInventoryStackSlot{T}"/> slots
        /// </summary>
        /// <inheritdoc />
        public StackInventory(IInventoryStackSlot<T>[] slots) : base(slots)
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual int GetCount(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var amount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                    amount += this.slots[i].Amount;
            }
            return amount;
        }
    }
}

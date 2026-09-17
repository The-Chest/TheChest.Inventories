using System;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots;
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
        /// Creates a StackInventory with the provided items, maximum stack size, and inventory size.
        /// </summary>
        /// <param name="items">Items to be added to the inventory slots.</param>
        /// <param name="maxStackSize">Maximum number of items allowed in each stack.</param>
        /// <param name="size">Number of slots in the inventory.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is null or contains a null item.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="maxStackSize"/> or <paramref name="size"/> is zero or smaller.</exception>
        /// <exception cref="ArgumentException">When the item stacks exceed <paramref name="size"/>.</exception>
        public StackInventory(T[] items, int maxStackSize, int size) : this(CreateSlots(items, maxStackSize, size)) { }
        /// <summary>
        /// Creates an Inventory with <see cref="IInventoryStackSlot{T}"/> slots
        /// </summary>
        /// <inheritdoc />
        public StackInventory(IInventoryStackSlot<T>[] slots) : base(slots)
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        private static IInventoryStackSlot<T>[] CreateSlots(T[] items, int maxStackSize, int size)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (maxStackSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxStackSize), StackInventoryErrors.MaxStackSizeMustBeGreaterThanZero);
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);

            var itemSlots = items.ToStackSlots(maxStackSize);
            if (itemSlots.Length > size)
                throw new ArgumentException(StackInventoryErrors.ItemsBiggerThanInventorySize, nameof(size));

            var slots = new IInventoryStackSlot<T>[size];
            Array.Copy(itemSlots, slots, itemSlots.Length);
            for (var index = itemSlots.Length; index < size; index++)
                slots[index] = new InventoryStackSlot<T>(maxStackSize);

            return slots;
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

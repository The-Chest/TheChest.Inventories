using System;
using TheChest.Core.Slots;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Exceptions;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Slots
{
    /// <summary>
    /// Generic Inventory Slot with with <see cref="Slot{T}"/> extension and <see cref="IInventorySlot{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">The item type the slot accepts</typeparam>
    public class InventorySlot<T> : Slot<T>, IInventorySlot<T>
    {
        /// <summary>
        /// Creates an empty inventory slot
        /// </summary>
        public InventorySlot() : base() {  }
        /// <summary>
        /// Creates a basic inventory slot with an item
        /// </summary>
        /// <param name="currentItem">item that belongs to this slot</param>
        public InventorySlot(T currentItem) : base(currentItem) 
        { 
            this.Content = currentItem;
        }

        /// <inheritdoc />
        public virtual bool CanAdd(T item)
        {
            return !this.IsFull && !item.IsNull();
        }
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">When the slot is full</exception>
        public virtual bool Add(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (this.IsFull)
                throw new InvalidOperationException(InventorySlotErrors.FullSlot);

            this.Content = item;
            return true;
        }

        /// <inheritdoc />
        public virtual T Get()
        {
            if (this.IsEmpty)
                return (T)(object)null;

            var content = this.Content;
            this.Content = (T)(object)null;
            return content;    
        }

        /// <inheritdoc />
        public virtual bool CanReplace(T item)
        {
            return !item.IsNull();
        }
        /// <inheritdoc />
        public virtual T Replace(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (this.IsEmpty)
            {
                this.Content = item;
                return (T)(object)null;
            }

            var content = this.Content;
            this.Content = item;
            return content;
        }
    }
}

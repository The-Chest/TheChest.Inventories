using System;
using System.Reflection;
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

        private void Clear()
        {
            // TODO: check how to handle this properly for value type without:
            // exposing the private content or creating a status change on `Slot<T>`
            // use a RawContent property to set the content to null for reference types and default for value types
            if (!typeof(T).IsValueType)
            {
                this.Content = default;
            }
            else
            {
                //TODO: avoid using reflection
                var field = typeof(Slot<T>).GetField(
                    "content", 
                    BindingFlags.NonPublic | BindingFlags.Instance
                );
                field.SetValue(this, null);
            }
        }
        
        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">When the slot is empty</exception>
        public virtual T Get()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException(InventorySlotErrors.EmptySlot);

            var content = this.Content;
            this.Clear();
            return content;
        }

        /// <inheritdoc />
        public virtual bool CanAdd(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            return !this.IsFull;
        }
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool TryAdd(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            
            if (this.IsFull)
                return false;

            this.Content = item;

            return true;
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
        public virtual bool CanReplace(T item)
        {
            return !this.IsEmpty && !item.IsNull();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool TryReplace(T item, out T oldItem)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            
            if (this.IsEmpty)
            {
                oldItem = default;
                return false;
            }

            oldItem = this.Content;
            this.Content = item;
            return true;
        }
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">When the slot is empty</exception>
        public virtual T Replace(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            if (this.IsEmpty)
                throw new InvalidOperationException(InventorySlotErrors.EmptySlot);

            var content = this.Content;
            this.Content = item;
            return content;
        }
    }
}

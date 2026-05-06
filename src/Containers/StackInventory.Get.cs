using System;
using System.Collections.Generic;
using TheChest.Inventories.Containers.Events.Stack;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class StackInventory<T>
    {
        /// <inheritdoc/>
        public event StackInventoryGetEventHandler<T> OnGet;

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when every item is retrieved from the inventory. 
        /// </remarks>
        public virtual T[] Clear()
        {
            if (this.IsEmpty)
                return Array.Empty<T>();

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();

            for (int i = 0; i < this.Size; i++)
            {
                var slotItems = this.slots[i].GetAll();
                if (slotItems.Length > 0)
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, i));

                items.AddRange(slotItems);
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when one item from <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T Get(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = this.slots[index].Get();
            if (!EqualityComparer<T>.Default.Equals(item, default))
                this.OnGet?.Invoke(this, (new[] { item }, index));

            return item;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when the first item from the inventory that is equal to <paramref name="item"/> is retrieved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T Get(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var result = slot.Get();
                    if (!EqualityComparer<T>.Default.Equals(item, default))
                        this.OnGet?.Invoke(this, (new[] { result }, index));
                    return result;
                }
            }
            return default;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items in an <paramref name="amount"/> from the inventory that contains <paramref name="item"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] Get(T item, int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();
            var remainingAmount = amount;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    var slotItems = slot.Get(remainingAmount);
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, i));
                    items.AddRange(slotItems);
                    remainingAmount -= slotItems.Length;
                    if (remainingAmount <= 0)
                        break;
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events.ToArray()));
            return items.ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items in an <paramref name="amount"/> from the <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Inventory Size or smaller than zero</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(int index, int amount)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = this.slots[index].Get(amount);
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items, index));

            return items;
        }

        /// <inheritdocs/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items from <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <param name="index">The zero-based index of the collection slot from which to retrieve items.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than Inventory's Size.</exception>
        public virtual T[] GetAll(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            var items = this.slots[index].GetAll();
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items, index));

            return items;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items from the inventory that contains <paramref name="item"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] GetAll(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.GetAll();
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, index));
                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack.Lazy;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class LazyStackInventory<T>
    {
        /// <inheritdoc/>
        public event LazyStackInventoryGetEventHandler<T> OnGet;

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when all items are returned from the inventory.
        /// </remarks>
        public virtual T[] Clear()
        {
            var events = new List<LazyStackInventoryGetItemEventData<T>>(this.Size / 4);
            var items = new List<T>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (!slot.IsEmpty)
                {
                    var slotItems = slot.GetAll();
                    if (slotItems.Length > 0)
                        events.Add(new LazyStackInventoryGetItemEventData<T>(slotItems[0], index, slotItems.Length));

                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new LazyStackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when an item is returned from <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T Get(int index)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = this.slots[index].Get().FirstOrDefault();
            if (!EqualityComparer<T>.Default.Equals(item, default))
                this.OnGet?.Invoke(this, (item, index, 1));

            return item;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when <paramref name="item"/> is returned from the inventory.
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
                    var foundItem = slot.Get().FirstOrDefault();
                    if (!EqualityComparer<T>.Default.Equals(foundItem, default))
                        this.OnGet?.Invoke(this, (foundItem, index, 1));

                    return foundItem;
                }
            }

            return default;
        }
        /// <summary>
        /// Gets an amount of items from the inventory
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when any amount of <paramref name="item"/> is returned from the inventory.
        /// </remarks>
        /// <param name="item">Item to be searched on the inventory</param>
        /// <param name="amount">Amount of <paramref name="item"/> to be returned</param>
        /// <returns>The amount of items searched (or the max it can return)</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = new List<T>();
            var events = new List<LazyStackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.Get(amount);
                    if (slotItems.Length > 0)
                        events.Add(new LazyStackInventoryGetItemEventData<T>(slotItems[0], index, slotItems.Length));

                    items.AddRange(slotItems);
                    amount -= slotItems.Length;
                    if (amount == 0)
                        break;
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new LazyStackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }
        /// <summary>
        /// Gets an amount of items from an specific slot the inventory
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when any amount of item is returned from <paramref name="index"/> of the inventory.
        /// </remarks>
        /// <param name="index">Slot item index to be returned</param>
        /// <param name="amount">Amount to be returned (or the max available)</param>
        /// <returns>An array of items</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T[] Get(int index, int amount)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = this.slots[index].Get(amount);
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items[0], index, items.Length));

            return items;
        }

        /// <summary>
        /// Gets all items of the selected type from all slots
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when every <paramref name="item"/> is returned from the inventory.
        /// </remarks> 
        /// <param name="item">Item to be searched</param>
        /// <returns>A list with all items founded in the inventory</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] GetAll(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<LazyStackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.GetAll();
                    if (slotItems.Length > 0)
                        events.Add(new LazyStackInventoryGetItemEventData<T>(slotItems[0], index, slotItems.Length));
                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new LazyStackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when all items from <paramref name="index"/> is returned from the inventory.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T[] GetAll(int index)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var items = this.slots[index].GetAll();
            if (items.Length > 0)
                this.OnGet?.Invoke(this, (items[0], index, items.Length));

            return items;
        }
    }
}

﻿using TheChest.Core.Containers;
using TheChest.Core.Slots.Extensions;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class Inventory<T> : Container<T>, IInventory<T>
    {
        protected new readonly IInventorySlot<T>[] slots;

        public event InventoryGetEventHandler<T>? OnGet;
        public event InventoryAddEventHandler<T>? OnAdd;
        public event InventoryMoveEventHandler<T>? OnMove;

        /// <summary>
        /// Creates an Inventory with <see cref="IInventorySlot{T}"/> implementation
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventorySlot{T}"/></param>
        public Inventory(IInventorySlot<T>[] slots) : base(slots) 
        {
            this.slots = slots;
        }

        public override IInventorySlot<T> this[int index] => this.slots[index];

        [Obsolete("This will be removed in the future versions. Use this[int index] instead")]
        public override IInventorySlot<T>[] Slots => this.slots.ToArray();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event after every possible <paramref name="items"/> is added. 
        /// </remarks>
        /// <returns>
        /// An array of <paramref name="items"/> that were not added to the inventory.
        /// </returns>
        public virtual T[] Add(params T[] items)
        {
            if (items.Length == 0) 
                return items;

            var addedAmount = 0;
            var addedItems = new Dictionary<int, T>();
            var index = 0;
            while (index < this.Size)
            {
                if(addedAmount >= items.Length)
                    break;

                var item = items[addedAmount];
                if (item is null)
                {
                    addedAmount++;
                    continue;
                }

                var added = this.slots[index].Add(item);
                if (added)
                {
                    addedItems.Add(index, item);
                    addedAmount++;
                }

                index++;
            }

            if(addedItems.Count > 0)
                this.OnAdd?.Invoke(this, (addedItems.Values.ToArray() , addedItems.Keys.ToArray()));

            if (addedAmount < items.Length)
                return items[addedAmount..];

            return Array.Empty<T>();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual bool Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size ; i ++)
            {
                var added = this.slots[i].Add(item);
                if (added)
                {
                    this.OnAdd?.Invoke(this, (item, i)); 
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added on <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Inventory{T}.Size"/></exception>
        public virtual T? AddAt(T item, int index, bool replace = true)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            T? result = default;
            if (replace)
            {
                result = this.slots[index].Replace(item);
                this.OnAdd?.Invoke(this, (item, index));//TODO: change it to OnReplace when #75 is implemented
            }
            else
            {
                var added = this.slots[index].Add(item);
                if (added)
                    this.OnAdd?.Invoke(this, (item, index));
                else
                    result = item;
            }

            return result;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event with every item returned from it.
        /// </remarks>
        public virtual T[] Clear()
        {
            var items = new List<T>();
            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                var item = this.slots[i].Get();
                if(item is not null)
                {
                    indexes.Add(i);
                    items.Add(item);
                }
            }

            if (items.Count > 0)
                this.OnGet?.Invoke(this, (items.ToArray(), indexes.ToArray()));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event when any amount of <paramref name="item"/> is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] GetAll(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    indexes.Add(i);
                    items.Add(this.slots[i].Get()!);
                }
            }

            if (items.Count > 0)
                this.OnGet?.Invoke(this, (items.ToArray(), indexes.ToArray()));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event if an item is found on <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Inventory{T}.Size"/></exception>
        public virtual T? Get(int index)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = this.slots[index].Get();

            if(item is not null)
                this.OnGet?.Invoke(this, (item, index));

            return item;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event when <paramref name="item"/> is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T? Get(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    this.OnGet?.Invoke(this, (item, i));
                    return this.slots[i].Get();
                }
            }
            
            return default;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event when the maximum possible <paramref name="amount"/> of <paramref name="item"/> is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var items = new List<T>();
            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                if (!this.slots[i].Contains(item))
                    continue;

                var slotItem = this.slots[i].Get();
                if(slotItem is null)
                    continue;

                items.Add(slotItem);
                indexes.Add(i);

                if (items.Count == amount)
                    break;
            }
            if (items.Count > 0)
                this.OnGet?.Invoke(this, (items.ToArray(), indexes.ToArray()));

            return items.ToArray();
        }
        
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual int GetCount(T item)
        {
            if (item is null)
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

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnMove"/> event.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are smaller than zero or bigger than the container size</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            var item = this.slots[origin].Get();
            var oldItem = this.slots[target].Replace(item);

            var events = new List<InventoryMoveItemEventData<T>>();
            if (item is not null)
                events.Add(new(item, origin, target));

            if (oldItem is not null)
            {
                this.slots[origin].Replace(oldItem);
                events.Add(new(oldItem, target, origin));
            }
            this.OnMove?.Invoke(this, new InventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}

﻿using TheChest.Core.Containers;
using TheChest.Core.Slots.Extensions;
using TheChest.Inventories.Containers.Events.Stack;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class StackInventory<T> : StackContainer<T>, IStackInventory<T>
    {
        protected new readonly IInventoryStackSlot<T>[] slots;

        public event StackInventoryAddEventHandler<T>? OnAdd;
        public event StackInventoryGetEventHandler<T>? OnGet;
        public event StackInventoryMoveEventHandler<T>? OnMove;

        public override IInventoryStackSlot<T> this[int index] => this.slots[index];

        [Obsolete("This will be removed in the future versions. Use this[int index] instead")]
        public override IInventoryStackSlot<T>[] Slots => this.slots.ToArray();

        /// <summary>
        /// Creates an Inventory with <see cref="IInventoryStackSlot{T}"/> slots
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventoryStackSlot{T}"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc/></exception>
        public StackInventory(IInventoryStackSlot<T>[] slots) : base(slots) 
        {
            this.slots = slots;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// It searches for an <see cref="IInventoryStackSlot{T}"/> that already contains the same type of <paramref name="item"/>, 
        /// if it finds it, it adds it to that slot, 
        /// else, it adds to the first empty slot.
        /// </para>
        /// <para>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the inventory. 
        /// </para>
        /// </remarks>
        /// <returns>true if is possible to add the items</returns>
        /// <exception cref="ArgumentNullException">When param <paramref name="item"/> is null</exception>
        public virtual bool Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var fallbackIndex = -1;
            for (var index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (!slot.CanAdd(item))
                    continue;
                
                if (slot.Contains(item))
                {
                    slot.Add(ref item);
                    this.OnAdd?.Invoke(this, (new[]{ item }, index));
                    return true; 
                }

                if(fallbackIndex == -1)
                    fallbackIndex = index;
            }

            if(fallbackIndex != -1)
            {
                this.slots[fallbackIndex].Add(ref item);
                this.OnAdd?.Invoke(this, (new[] { item }, fallbackIndex));
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// The method fires <see cref="OnAdd"/> event when every possible item is added to the inventory.
        /// </para>
        /// <para>
        /// Warning: this method does not accept different items in the same array. 
        /// This feature will be added in <see href="https://github.com/The-Chest/TheChest.Inventories/issues/42"/>
        /// </para>
        /// </remarks>
        /// <returns>Items from params that were not added to the inventory</returns>
        public virtual T[] Add(params T[] items)
        {
            if (items.Length == 0)
                return items;

            var fallbackIndexes = new List<int>();
            var events = new List<StackInventoryAddItemEventData<T>>();
            for (var index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (!slot.CanAdd(items))
                    continue;

                if (!slot.Contains(items[0]))
                {
                    if(fallbackIndexes.Count <= items.Length)
                        fallbackIndexes.Add(index);

                    continue;
                }

                var addedItems = items.ToArray();
                slot.Add(ref items);

                events.Add(new(addedItems[items.Length..], index));
                if (items.Length == 0)
                    break;
            }

            foreach (var index in fallbackIndexes)
            {
                var addedItems = items.ToArray();
                this.slots[index].Add(ref items);
                events.Add(new(addedItems[items.Length..], index));
                if (items.Length == 0)
                    break;
            }

            if(events.Count > 0)
                this.OnAdd?.Invoke(this, new StackInventoryAddEventArgs<T>(events));

            return items;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the <paramref name="index"/> of the inventory. 
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Inventory{T}.Size"/></exception>
        public virtual T[] AddAt(T item, int index, bool replace = true)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var slot = this.slots[index];

            if (slot.CanAdd(item)) 
            {
                slot.Add(ref item);
                this.OnAdd?.Invoke(this, (new[] { item }, index));

                return Array.Empty<T>();
            }
            
            if (slot.CanReplace(item) && replace)
            {
                //TODO: change it to OnReplace when <see href="https://github.com/The-Chest/TheChest.Inventories/issues/75"/> is implemented
                this.OnAdd?.Invoke(this, (new []{ item }, index));
                
                return slot.Replace(ref item);
            }

            return new T[1] { item };
        }

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// If <paramref name="replace"/> is true, it replaces the item from <paramref name="index"/>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">When <paramref name="items"/> is empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T[] AddAt(T[] items, int index, bool replace = true)
        {
            if(items.Length == 0)
                throw new ArgumentException("No items to be added", nameof(items));

            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];

            if (slot.CanAdd(items))
            {
                var addedItems = items.ToArray();
                slot.Add(ref items);
                this.OnAdd?.Invoke(this, (addedItems[items.Length..], index));
            }

            if (replace && slot.CanReplace(items))
            {
                var replacedItems = items.ToArray();
                var oldItems = slot.Replace(ref items);

                //TODO: change it to OnReplace when <see href="https://github.com/The-Chest/TheChest.Inventories/issues/75"/> is implemented
                this.OnAdd?.Invoke(this, (replacedItems[items.Length..], index));
                return oldItems;
            }

            return items;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnGet"/> event when every item is retrieved from the inventory. 
        /// </remarks>
        public virtual T[] Clear()
        {
            if(this.IsEmpty)
                return Array.Empty<T>();

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();

            for (int i = 0; i < this.Size; i++)
            {
                var slotItems = this.slots[i].GetAll();
                if(slotItems.Length > 0)
                    events.Add(new(slotItems, i));

                items.AddRange(slotItems);
            }
            if(events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items from <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Inventory Size or smaller than zero</exception>
        public virtual T[] GetAll(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            var items =  this.slots[index].GetAll();
            if(items.Length > 0)
                this.OnGet?.Invoke(this, (items, index));
            return items;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when all items from the inventory that contains <paramref name="item"/> are retrieved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] GetAll(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var events = new List<StackInventoryGetItemEventData<T>>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var slotItems = slot.GetAll();
                    events.Add(new(slotItems, index));
                    items.AddRange(slotItems);
                }
            }
            if(events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when one item from <paramref name="index"/> are retrieved.
        /// </remarks>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        public virtual T? Get(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var item = this.slots[index].Get();
            if(item is not null)
                this.OnGet?.Invoke(this, (new[]{ item }, index));
            return item;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="IStackInventory{T}.OnGet"/> when the first item from the inventory that is equal to <paramref name="item"/> is retrieved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T? Get(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (slot.Contains(item))
                {
                    var result = slot.Get();
                    if(result is not null)
                        this.OnGet?.Invoke(this, (new[]{ result }, index));
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
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (item is null)
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
                    events.Add(new(slotItems, i));
                    items.AddRange(slotItems);
                    remainingAmount -= slotItems.Length;
                    if (remainingAmount <= 0)
                        break;
                }
            }
            if(events.Count > 0)
                this.OnGet?.Invoke(this, new StackInventoryGetEventArgs<T>(events));    
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
            if(items.Length > 0)
                this.OnGet?.Invoke(this, (items, index));
            return items;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual int GetCount(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var amount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    amount += this.slots[i].StackAmount;
                }
            }
            return amount;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));

            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));

            if(origin == target)
                return;
            //TODO: compare the size of both slots are equivalent

            var items = this.slots[origin].GetAll();
            var oldItems = this.slots[target].Replace(ref items);

            var events = new List<StackInventoryMoveItemEventData<T>>();
            if (items is not null)
                events.Add(new(items, origin, target));

            if (oldItems is not null)
            {
                this.slots[origin].Replace(ref oldItems);
                events.Add(new(oldItems, target, origin));
            }
            this.OnMove?.Invoke(this, new StackInventoryMoveEventArgs<T>(events.ToArray()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Exceptions;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="IInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class Inventory<T> : Container<T>, IInventory<T>
    {
        /// <summary>
        /// An array of <see cref="IInventorySlot{T}"/> that holds the slots of this inventory
        /// </summary>
        protected new readonly IInventorySlot<T>[] slots;

        /// <inheritdoc/>
        public event InventoryGetEventHandler<T> OnGet;
        /// <inheritdoc/>
        public event InventoryAddEventHandler<T> OnAdd;
        /// <inheritdoc/>
        public event InventoryMoveEventHandler<T> OnMove;
        /// <inheritdoc/>
        public event InventoryReplaceEventHandler<T> OnReplace;

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

        /// <summary>
        /// Determines whether all specified items can be added to the available slots.
        /// </summary>
        /// <remarks>The method does not modify the slots or items.</remarks>
        /// <param name="items">An array of items to check for availability.</param>
        /// <returns><see langword="true"/> if all items in <paramref name="items"/> can be added to the slots; otherwise, <see langword="false"/>.</returns>
        protected bool CanAddAmount(T[] items)
        {
            var canAddAmount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                var item = items[canAddAmount];
                if (slot.CanAdd(item))
                {
                    canAddAmount++;
                    if (items.Length == canAddAmount)
                        return true;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool CanAdd(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].CanAdd(item))
                    return true;
            }

            return false;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one <see langword="null"/> item</exception>
        public virtual bool CanAdd(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.Length == 0)
                return false;
            if (items.Length > this.Size)
                return false;

            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), InventoryErrors.ItemArrayContainsNull);

            var canAddAmount = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                var item = items[canAddAmount];
                if (slot.CanAdd(item))
                {
                    canAddAmount++;
                    if (items.Length == canAddAmount)
                        return true;
                }
            }

            return false;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>"
        public virtual bool CanAddAt(T item, int index)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
        
            return this.slots[index].CanAdd(item);
        }

        /// <summary>
        /// Attempts to add the specified items to the available slots and returns any items that could not be added.
        /// </summary>
        /// <remarks>
        /// The method adds items to slots in order until either all items are added or no more slots are available.  
        /// The <c>OnAdd</c> event is invoked if any items are successfully added.
        /// </remarks>
        /// <param name="items">An array of items to add. 
        /// Items that are <see langword="null"/> or considered null by <c>IsNull()</c> are skipped.</param>
        /// <returns>An array containing the items that could not be added due to lack of available space. Returns an empty array if all items were successfully added.</returns>
        protected T[] AddItems(params T[] items)
        {
            var addedAmount = 0;
            var addedItems = new Dictionary<int, T>();

            var index = 0;
            while (index < this.Size)
            {
                if (addedAmount >= items.Length)
                    break;

                var item = items[addedAmount];
                if (item.IsNull())
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

            if (addedItems.Count > 0)
                this.OnAdd?.Invoke(this, (addedItems.Values.ToArray(), addedItems.Keys.ToArray()));

            if (addedAmount < items.Length)
                return items.Skip(addedAmount).ToArray();

            return Array.Empty<T>();
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">When the inventory is full</exception>
        public virtual bool Add(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if(this.IsFull)
                throw new InvalidOperationException("The inventory is full.");

            return this.AddItems(item).Length == 0;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event after every possible <paramref name="items"/> is added. 
        /// </remarks>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one <see langword="null"/> item</exception>
        /// <exception cref="InvalidOperationException">When there are not enough free slots to add all the items</exception>
        /// <returns>An array of <paramref name="items"/> that were not added to the inventory.</returns>
        public virtual T[] Add(params T[] items)
        {
            if (items.Length == 0)
                return items;
            if (items.Length > this.Size)
                throw new InvalidOperationException("The amount of items to be added cannot be bigger than the inventory size.");
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), "One of the items is null");
            if (!this.CanAddAmount(items))
                throw new InvalidOperationException("There are not enough free slots to add all the items.");

            return this.AddItems(items);
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added on <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        /// <exception cref="InvalidOperationException">When the item cannot be added to the slot at index <paramref name="index"/></exception>
        public virtual bool AddAt(T item, int index)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (!this.slots[index].CanAdd(item))
                throw new InvalidOperationException(InventoryErrors.CannotAddItemAtIndex);

            this.slots[index].Add(item);
            this.OnAdd?.Invoke(this, (item, index));

            return true;
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
                if(!item.IsNull())
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
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T[] GetAll(T item)
        {
            if(item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var items = new List<T>();
            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    indexes.Add(i);
                    items.Add(this.slots[i].Get());
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
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        public virtual T Get(int index)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = this.slots[index].Get();

            if(!EqualityComparer<T>.Default.Equals(item, default))
                this.OnGet?.Invoke(this, (item, index));

            return item;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event when <paramref name="item"/> is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual T Get(T item)
        {
            if (item.IsNull())
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
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (item.IsNull())
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
                if(slotItem.IsNull())
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

        /// <inheritdoc/>
        public virtual bool CanMove(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                return false;
            if (target < 0 || target >= this.Size)
                return false;
            if (this.slots[origin].IsEmpty && this.slots[target].IsEmpty)
                return false;

            return true;
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
            if (!EqualityComparer<T>.Default.Equals(item, default))
                events.Add(new InventoryMoveItemEventData<T>(item, origin, target));

            if (!EqualityComparer<T>.Default.Equals(oldItem, default))
            {
                this.slots[origin].Replace(oldItem);
                events.Add(new InventoryMoveItemEventData<T>(oldItem, target, origin));
            }
            this.OnMove?.Invoke(this, new InventoryMoveEventArgs<T>(events.ToArray()));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>"
        public virtual bool CanReplace(T item, int index)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].CanReplace(item);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        public virtual T Replace(T item, int index)
        {
            if(item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (slot.IsEmpty)
            {
                var added = slot.Add(item);
                if (added)
                {
                    this.OnReplace?.Invoke(this, (index, default, item));
                    return default;
                }
                
                return item;
            }

            var oldItem = slot.Replace(item);
            this.OnReplace?.Invoke(this, (index, oldItem, item));

            return oldItem;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack;
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
    public class StackInventory<T> : StackContainer<T>, IStackInventory<T>
    {
        /// <summary>
        /// Array of <see cref="IInventoryStackSlot{T}"/> slots in the inventory
        /// </summary>
        protected new readonly IInventoryStackSlot<T>[] slots;

        /// <inheritdoc/>
        public event StackInventoryAddEventHandler<T> OnAdd;
        /// <inheritdoc/>
        public event StackInventoryGetEventHandler<T> OnGet;
        /// <inheritdoc/>
        public event StackInventoryMoveEventHandler<T> OnMove;
        /// <inheritdoc/>
        public event StackInventoryReplaceEventHandler<T> OnReplace;

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

        /// <summary>
        /// Determines whether all specified items can be added to the collection without exceeding available capacity.
        /// </summary>
        /// <param name="items">An array of items to check for addability. The array must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if all items in the <paramref name="items"/> array can be added to the collection; otherwise, <see langword="false"/>.</returns>
        protected bool CanAddItems(T[] items)
        {
            var canAddAmount = 0;

            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                var toAddItems = items
                    .Skip(canAddAmount)
                    .Take(slot.AvailableAmount)
                    .ToArray();

                if (slot.CanAdd(toAddItems))
                {
                    canAddAmount += toAddItems.Length;
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
            if (this.IsFull)
                return false;

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].CanAdd(item))
                    return true;
            }

            return false;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one item <see langword="null"/></exception>
        public virtual bool CanAdd(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            
            if (items.Length == 0)
                return false;
            if (items.Length > this.slots.Sum(x => x.AvailableAmount)) // TODO: check if the this.CanAddItems(items) already does the job
                return false;

            return this.CanAddItems(items);
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
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one item <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>"
        public virtual bool CanAddAt(T[] items, int index)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (items.Length == 0)
                return false;

            return this.slots[index].CanAdd(items);
        }

        /// <summary>
        /// Attempts to add the specified items to the inventory and returns any items that could not be added.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method adds items to available inventory slots according to the inventory's add order logic. 
        /// </para>
        /// <para>
        /// The method fires <see cref="OnAdd"/> event when every possible item is added to the inventory.
        /// </para>
        /// </remarks>
        /// <param name="items">The items to add to the inventory. The order of items is preserved during the add operation.</param>
        /// <returns>An array containing the items that could not be added to the inventory.</returns>
        protected T[] AddItems(params T[] items)
        {
            var events = new List<StackInventoryAddItemEventData<T>>(items.Length);
            var indexes = this.slots.GetAddOrderIndexes(items);

            foreach (var index in indexes)
            {
                var slot = this.slots[index];

                var itemsToAdd = items.Take(slot.AvailableAmount).ToArray();
                var notAddedItems = slot.Add(itemsToAdd);
                var addedItemsCount = itemsToAdd.Length - notAddedItems.Length;

                if (addedItemsCount <= 0)
                    continue;

                events.Add(
                    new StackInventoryAddItemEventData<T>(
                        items.Take(addedItemsCount).ToArray(),
                        index
                    )
                );

                items = items.Skip(addedItemsCount).ToArray();
                if (items.Length == 0)
                    break;
            }

            if (events.Count > 0)
                this.OnAdd?.Invoke(this, new StackInventoryAddEventArgs<T>(events.ToArray()));

            return items;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the inventory. 
        /// </remarks>
        /// <returns><see langword="true"/> if is possible to add the items</returns>
        /// <exception cref="ArgumentNullException">When param <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool Add(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (this.IsFull)
                throw new InvalidOperationException(StackInventoryErrors.InventoryIsFull);

            return this.AddItems(item).Length == 0;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// The method fires <see cref="OnAdd"/> event when every possible item is added to the inventory.
        /// </para>
        /// <para>
        /// Warning: this method does not accept different items in the same array.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">When param <paramref name="items"/> is <see langword="null"/> or has one item <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">When the inventory is full or when there are not enough free slots to add all the items</exception>
        /// <exception cref="ArgumentException">When param <paramref name="items"/> length is zero</exception>
        /// <returns>Items from params that were not added to the inventory</returns>
        public virtual T[] Add(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.Length == 0)
                throw new ArgumentException(StackInventoryErrors.CannotAddEmptyArray, nameof(items));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (this.IsFull)
                throw new InvalidOperationException(StackInventoryErrors.InventoryIsFull);
            if (!this.CanAddItems(items))
                throw new InvalidOperationException(StackInventoryErrors.NotEnoughFreeSlots);

            return this.AddItems(items);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        /// <exception cref="InvalidOperationException">When the inventory is full or when the item cannot be added to the slot on <paramref name="index"/></exception>
        public virtual bool AddAt(T item, int index)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (!this.slots[index].CanAdd(item))
                throw new InvalidOperationException(StackInventoryErrors.NotPossibleToAddItem);

            var added = this.slots[index].Add(item);
            if (added)
                this.OnAdd?.Invoke(this, (new[] { item }, index));
            
            return added;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot or smaller than zero</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/> or has one item <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">When the items cannot be added to the slot on <paramref name="index"/></exception>
        public virtual T[] AddAt(T[] items, int index)
        {
            if (items.Length == 0)
                throw new ArgumentException(StackInventoryErrors.CannotAddEmptyArray, nameof(items));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (!items.HasAllEqual())
                throw new ArgumentException(StackInventoryErrors.CannotAddArrayWithDifferentItems, nameof(items));
            if (!this.slots[index].CanAdd(items))
                throw new InvalidOperationException(StackInventoryErrors.NotPossibleToAddAllItems);

            var notAddedItems = this.slots[index].Add(items);
            if (notAddedItems.Length != items.Length)
                this.OnAdd?.Invoke(this, (items.Skip(notAddedItems.Length).ToArray(), index));

            return notAddedItems;
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
                    events.Add(new StackInventoryGetItemEventData<T>(slotItems, i));

                items.AddRange(slotItems);
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
        public virtual T Get(int index)
        {
            if (index > this.Size || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var item = this.slots[index].Get();
            if(!EqualityComparer<T>.Default.Equals(item, default))
                this.OnGet?.Invoke(this, (new[]{ item }, index));
            
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
                    if(!EqualityComparer<T>.Default.Equals(item, default))
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
            if(events.Count > 0)
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
            if(items.Length > 0)
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

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual bool CanMove(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            
            if (origin == target)
                return false;

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return false;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return false;

            return true;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> are bigger than Slot or smaller than zero</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                return;

            var slotOrigin = this.slots[origin];
            var slotTarget = this.slots[target];

            //TODO: improve these checks
            if (slotOrigin.IsEmpty && slotTarget.IsEmpty)
                return;
            if (slotOrigin.MaxAmount != slotTarget.MaxAmount)
                return;

            var originItems = slotOrigin.GetAll();
            var targetItems = slotTarget.GetAll();

            var events = new List<StackInventoryMoveItemEventData<T>>();

            if (originItems.Length > 0 && slotTarget.CanAdd(originItems))
            {
                slotTarget.Add(originItems);
                events.Add(
                    new StackInventoryMoveItemEventData<T>(
                        originItems, 
                        origin, 
                        target
                    )
                );
            }

            if (targetItems.Length > 0 && slotOrigin.CanAdd(targetItems))
            {
                slotOrigin.Add(targetItems);
                events.Add(
                    new StackInventoryMoveItemEventData<T>(
                        targetItems, 
                        target,
                        origin
                    )
                );
            }

            if(events.Count > 0)
                this.OnMove?.Invoke(this, new StackInventoryMoveEventArgs<T>(events.ToArray()));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="items"/> length is zero</exception>"
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot size or smaller than zero</exception>
        /// <exception cref="InvalidOperationException">When the amount of <paramref name="items"/> to replace exceeds the stack size of the slot on <paramref name="index"/>.</exception>
        public virtual bool CanReplace(T[] items, int index)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (items.Length == 0)
                return false;
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);

            return this.slots[index].CanReplace(items);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="items"/> length is zero</exception>"
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> added is bigger than Slot size or smaller than zero</exception>
        /// <exception cref="InvalidOperationException">When the amount of <paramref name="items"/> to replace exceeds the stack size of the slot on <paramref name="index"/>.</exception>
        public virtual T[] Replace(T[] items, int index)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.Length == 0) 
                throw new ArgumentException(StackInventoryErrors.CannotReplaceEmptyArray, nameof(items)); // why not?
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            var slot = this.slots[index];
            if (items.Length > slot.MaxAmount)
                throw new InvalidOperationException(StackInventoryErrors.MaxStackSizeSmallerThanItemsToReplace);

            var oldItems = slot.Replace(items);
            this.OnReplace?.Invoke(this, (index, oldItems, items));

            return oldItems;
        }
    }
}

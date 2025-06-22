using TheChest.Core.Containers;
using TheChest.Core.Slots.Extensions;
using TheChest.Inventories.Containers.Events.Stack.Lazy;
using TheChest.Inventories.Containers.Interfaces;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Containers
{
    /// <summary>
    /// Generic Inventory with <see cref="ILazyStackInventory{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">An item type</typeparam>
    public class LazyStackInventory<T> : StackContainer<T>, ILazyStackInventory<T>
    {
        public event LazyStackInventoryGetEventHandler<T>? OnGet;

        protected new readonly IInventoryLazyStackSlot<T>[] slots;

        public override IInventoryLazyStackSlot<T> this[int index] => this.slots[index];
        
        [Obsolete("This will be removed in the future versions. Use this[int index] instead")]
        public override IInventoryLazyStackSlot<T>[] Slots => this.slots;

        /// <summary>
        /// Creates an Stackable Inventory with lazy behavior
        /// </summary>
        /// <param name="slots">An array of <see cref="IInventoryLazyStackSlot{T}"/></param>
        /// <exception cref="ArgumentNullException">When <paramref name="slots"/> is null</exception>
        public LazyStackInventory(IInventoryLazyStackSlot<T>[] slots) : base(slots)
        {
            this.slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }
        /// <summary>
        /// Adds an item to the first available slot
        /// </summary>
        /// <param name="item">Item to be added to the inventory</param>
        /// <returns>True if <paramref name="item"/> is possible to be added to the inventory</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual bool Add(T item)
        {
            if(item is null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.CanAdd(item))
                    return slot.Add(item) == 0;
            }

            return false;
        }
        /// <summary>
        /// Adds an amount of items to the first available slot
        /// </summary>
        /// <param name="item">Item to be added to the inventory</param>
        /// <param name="amount">Amount of <paramref name="item"/> to be added</param>
        /// <returns>Empty array when is succesfully added, otherwise it'll return an array with not added items</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual int Add(T item, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var notAdded = amount;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.CanAdd(item))
                {
                    notAdded = slot.Add(item, amount);
                    if (notAdded == 0)
                        break;
                }
            }

            return notAdded;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T[] AddAt(T item, int index, int amount, bool replace)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (slot.CanAdd(item, amount))
            {
                var notAdded = slot.Add(item, amount);
                return Enumerable.Repeat(item, notAdded).ToArray();
            }
            else if(replace && slot.CanReplace(item, amount))
            {
                return slot.Replace(item, amount);
            }

            return Enumerable.Repeat(item, amount).ToArray();
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual int AddAt(T item, int index, int amount)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (slot.CanAdd(item, amount))
                return slot.Add(item, amount);

            return amount;
        }
        /// <inheritdoc/>
        public virtual T[] Clear()
        {
            var events = new List<LazyStackInventoryGetItemEventData<T>>();
            var items = new List<T>();
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                if (!slot.IsEmpty)
                {
                    var slotItems = slot.GetAll();
                    if (slotItems.Length > 0)
                        events.Add(new(slotItems[0], index, slotItems.Length));

                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new(events));

            return items.ToArray();
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual T? Get(int index)
        {
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = this.slots[index].Get().FirstOrDefault();
            if (item is not null)
                this.OnGet?.Invoke(this, (item, index, 1));

            return item;
        }
        /// <inheritdoc/>
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
                    var foundItem = slot.Get().FirstOrDefault();
                    if(foundItem is not null)
                        this.OnGet?.Invoke(this, (foundItem, index, 1));

                    return foundItem;
                }
            }

            return default;
        }
        /// <summary>
        /// Gets an amount of items from the inventory
        /// </summary>
        /// <param name="item">Item to be searched on the inventory</param>
        /// <param name="amount">Amount of <paramref name="item"/> to be returned</param>
        /// <returns>The amount of items searched (or the max it can return)</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual T[] Get(T item, int amount)
        {
            if (item is null)
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
                        events.Add(new(slotItems[0], index, slotItems.Length));

                    items.AddRange(slotItems);
                    amount -= slotItems.Length;
                    if (amount == 0)
                        break;
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new(events));

            return items.ToArray();
        }
        /// <summary>
        /// Gets an amount of items from an specific slot the inventory
        /// </summary>
        /// <param name="index">Slot item index to be returned</param>
        /// <param name="amount">Amount to be returned (or the max available)</param>
        /// <returns>An array of <see cref="{T}"/></returns>
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
        /// <param name="item">Item to be searched</param>
        /// <returns>A list with all items founded in the inventory</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual T[] GetAll(T item)
        {
            if (item is null)
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
                        events.Add(new(slotItems[0], index, slotItems.Length));
                    items.AddRange(slotItems);
                }
            }
            if (events.Count > 0)
                this.OnGet?.Invoke(this, new(events));

            return items.ToArray();
        }
        /// <summary>
        /// Gets all items from an specific slot from the inventory
        /// </summary>
        /// <param name="index">Slot item index to be returned</param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the amount of an item inside the inventory
        /// </summary>
        /// <param name="item">Item to be searched</param>
        /// <returns>The amount of the <paramref name="item"/> in the Inventory </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null</exception>
        public virtual int GetCount(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var count = 0;
            for (int i = 0; i < this.Size; i++)
            {
                var slot = this.slots[i];
                if (slot.Contains(item))
                {
                    count += slot.StackAmount;
                }
            }
            return count;
        }
        /// <summary>
        /// Moves an item from one slot to another
        /// </summary>
        /// <param name="origin">Origin slot that will be moved to <paramref name="target"/></param>
        /// <param name="target">Target slot that will be moved to <paramref name="origin"/></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="origin"/> or <paramref name="target"/> index are smaller than zero or bigger than <see cref="StackContainer{T}.Size"/></exception>
        /// <exception cref="ArgumentException">When <paramref name="origin"/> and <paramref name="target"/> are equal</exception>
        public virtual void Move(int origin, int target)
        {
            if (origin < 0 || origin > this.Size)
                throw new ArgumentOutOfRangeException(nameof(origin));
            if (target < 0 || target > this.Size)
                throw new ArgumentOutOfRangeException(nameof(target));
            if (origin == target)
                throw new ArgumentException("Origin and target cannot be the same");

            var originSlot = this.slots[origin];
            var targetSlot = this.slots[target];
            if (originSlot.IsEmpty && targetSlot.IsEmpty)
                return;

            var originItems = originSlot.GetAll();
            var originItem = originItems.FirstOrDefault();

            if (originItem is null)
            {
                var targetItems = targetSlot.GetAll();
                var targetItem = targetItems.FirstOrDefault();
                originSlot.Add(targetItem!, targetItems.Length);
            }
            else
            {
                var targetItems = targetSlot.Replace(originItem!, originItems.Length);
                var targetItem = targetItems.FirstOrDefault();

                if(targetItem is not null)
                {
                    originSlot.Replace(targetItem!, targetItems.Length);
                }
            }
        }
    }
}

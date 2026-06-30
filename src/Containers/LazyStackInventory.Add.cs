using System;
using System.Collections.Generic;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack.Lazy;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class LazyStackInventory<T>
    {
        /// <inheritdoc/>
        public event LazyStackInventoryAddEventHandler<T> OnAdd;

        /// <summary>
        /// Determines whether the specified item can be added to the Inventory in the given quantity.
        /// </summary>
        /// <remarks>
        /// This method evaluates whether there is sufficient space in the Inventory to accommodate the specified item and quantity, 
        /// taking into account the current state of the Inventory's slots. 
        /// It does not modify the Inventory.
        /// </remarks>
        /// <param name="item">The item to check for addition to the Inventory.</param>
        /// <param name="amount">The quantity of the item to add. Must be a positive value.</param>
        /// <returns><see langword="true"/> if the specified item can be added to the Inventory in the given quantity; otherwise, <see langword="false"/>.</returns>
        protected bool CanAddItems(T item, int amount = 1)
        {
            var totalAmount = amount;
            for (int index = 0; index < this.Size; index++)
            {
                var slot = this.slots[index];
                var addAmount = Math.Min(totalAmount, slot.AvailableAmount);
                
                if (addAmount <= 0)
                    continue;
                if (!slot.CanAdd(item, addAmount))
                    continue;

                totalAmount -= addAmount;

                if (totalAmount == 0)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is less than or equal to zero.</exception>
        public virtual bool CanAdd(T item, int amount = 1)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (this.IsFull)
                return false;

            return this.CanAddItems(item, amount);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is less than or equal to zero, or if <paramref name="index"/> is less than 0 or greater than the current size of the inventory.</exception>
        public virtual bool CanAddAt(T item, int index, int amount = 1)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].CanAdd(item, amount);
        }

        /// <summary>
        /// Adds the specified amount of <paramref name="item"/> to the inventory using the slot add order.
        /// </summary>
        /// <param name="item">Item to be added to the inventory.</param>
        /// <param name="amount">Amount of the item to add.</param>
        /// <returns>Returns the amount of items that could not be added.</returns>
        protected int AddItem(T item, int amount)
        {
            var events = new List<LazyStackInventoryAddItemEventData<T>>(amount);
            var indexes = this.slots.GetAddOrderIndexes(item, amount);

            foreach (var index in indexes)
            {
                var slot = this.slots[index];

                var toAddAmount = Math.Min(amount, slot.AvailableAmount);

                if (toAddAmount <= 0)
                    continue;

                var notAddedAmount = slot.Add(item, toAddAmount);
                var addedItemsCount = toAddAmount - notAddedAmount;

                if (addedItemsCount <= 0)
                    continue;

                events.Add(new LazyStackInventoryAddItemEventData<T>(item, index, addedItemsCount));

                amount -= addedItemsCount;
                if (amount == 0)
                    break;
            }

            if (events.Count > 0)
                this.OnAdd?.Invoke(this, new LazyStackInventoryAddEventArgs<T>(events));

            return amount;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is less than or equal to zero.</exception>
        public virtual bool TryAdd(T item, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (!this.CanAdd(item, amount))
                return false;

            return this.AddItem(item, amount) == 0;
        }
        /// <summary>
        /// Adds an item to the first available slot
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the inventory.
        /// </remarks>
        /// <param name="item">Item to be added to the inventory</param>
        /// <returns><see langword="true" /> if <paramref name="item"/> is possible to be added to the inventory</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        public virtual bool Add(T item)
        {
            return this.Add(item, 1) == 0;
        }
        /// <summary>
        /// Adds an amount of items to the first available slot.
        /// </summary>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when any amount of <paramref name="item"/> are added to the inventory.
        /// </remarks>
        /// <param name="item">Item to be added to the inventory</param>
        /// <param name="amount">Amount of <paramref name="item"/> to be added</param>
        /// <returns>Empty array when is succesfully added, otherwise it'll return an array with not added items</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual int Add(T item, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (this.IsFull)
                throw new InvalidOperationException(LazyStackInventoryErrors.InventoryIsFull);
            //TODO: add check for available space and throw exception if there is not enough space to add the items

            return this.AddItem(item, amount);
        }

        /// <summary>
        /// Adds the specified amount of <paramref name="item"/> to the slot at <paramref name="index"/>.
        /// </summary>
        /// <param name="item">Item to be added to the slot.</param>
        /// <param name="index">Index of the slot that will receive the item.</param>
        /// <param name="amount">Amount of the item to add.</param>
        /// <returns>Returns the amount of items that could not be added to the slot.</returns>
        protected int AddItemAt(T item, int index, int amount)
        {
            var notAdded = this.slots[index].Add(item, amount);
            if (notAdded < amount)
                this.OnAdd?.Invoke(this, (item, index, amount - notAdded));

            return notAdded;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is less than or equal to zero, or <paramref name="index"/> is out of range.</exception>
        public virtual bool TryAddAt(T item, int index, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (!this.slots[index].TryAdd(item, amount))
                return false;

            this.OnAdd?.Invoke(this, (item, index, amount));

            return true;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added to the <paramref name="index"/> .
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual int AddAt(T item, int index, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.AddItemAt(item, index, amount);
        }
    }
}

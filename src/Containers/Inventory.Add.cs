using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class Inventory<T>
    {
        /// <inheritdoc/>
        public event InventoryAddEventHandler<T> OnAdd;

        /// <summary>
        /// Determines whether all specified items can be added to the available slots.
        /// </summary>
        /// <remarks>The method does not modify the slots or items.</remarks>
        /// <param name="items">An array of items to check for availability.</param>
        /// <returns><see langword="true"/> if all items in <paramref name="items"/> can be added to the slots; otherwise, <see langword="false"/>.</returns>
        protected bool CanAddItems(T[] items)
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
            //TODO: check if its better to return false instead of throw an exception when one of the items is null
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), InventoryErrors.ItemArrayContainsNull);

            if (items.Length == 0)
                return false;
            if (items.Length > this.Size)
                return false;

            return this.CanAddItems(items);
        }

        /// <summary>
        /// Attempts to add the specified items to the available slots and returns any items that could not be added.
        /// </summary>
        /// <remarks>
        /// The method adds items to slots in order until either all items are added or no more slots are available.  
        /// The <see cref="OnAdd"/> event is invoked if any items are successfully added.
        /// </remarks>
        /// <param name="items">An array of items to add. Items that are or considered <see langword="null"/> are skipped.</param>
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

                var slot = this.slots[index];
                if (slot.CanAdd(item))
                {
                    this.slots[index].Add(item);
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
        /// The method fires <see cref="OnAdd"/> event after every possible <paramref name="items"/> is added. 
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when <paramref name="items"/> is empty.</exception>
        public virtual bool TryAdd(params T[] items)
        {
            if (items.Length == 0)
                throw new ArgumentException(InventoryErrors.CannotAddEmptyArray, nameof(items));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), InventoryErrors.ItemArrayContainsNull);

            if (items.Length > this.Size)
                return false;
            if (!this.CanAddItems(items))
                return false;

            return this.AddItems(items).Length == 0;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event when <paramref name="item"/> is added.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the inventory has no available slot for <paramref name="item"/>.</exception>
        public virtual bool Add(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (this.IsFull)
                throw new InvalidOperationException(InventoryErrors.InventoryIsFull);

            return this.AddItems(item).Length == 0;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event after every possible <paramref name="items"/> is added. 
        /// </remarks>
        /// <param name="items">Array of items to be added to any avaliable slot found</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="items"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> contains one or more <see langword="null"/> entries.</exception>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="items"/> has more items than <see cref="Container{T}.Size"/> or when the inventory does not have enough available slots to add all items.</exception>
        /// <returns>An array of <paramref name="items"/> that were not added to the inventory.</returns>
        public virtual T[] Add(params T[] items)
        {
            if (items.Length == 0)
                throw new ArgumentException(InventoryErrors.CannotAddEmptyArray, nameof(items));
            if (items.Length > this.Size)
                throw new InvalidOperationException(InventoryErrors.ItemsBiggerThanInventorySize);
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), InventoryErrors.ItemArrayContainsNull);
            if (!this.CanAddItems(items))
                throw new InvalidOperationException(InventoryErrors.NotEnoughFreeSlots);

            return this.AddItems(items);
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

            this.slots[index].Add(item);
            this.OnAdd?.Invoke(this, (item, index));

            return true;
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires <see cref="OnAdd"/> event after the <paramref name="item"/> is added. 
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        /// <returns><see langword="true"/> if the <paramref name="item"/> was successfully added at the specified <paramref name="index"/>; <see langword="false"/> if the the slot at <paramref name="index"/> is occupied</returns>
        public virtual bool TryAddAt(T item, int index)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            try 
            {
                this.slots[index].Add(item);
                this.OnAdd?.Invoke(this, (item, index));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}

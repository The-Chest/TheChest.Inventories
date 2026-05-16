using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class StackInventory<T>
    {
        /// <inheritdoc/>
        public event StackInventoryAddEventHandler<T> OnAdd;

        /// <summary>
        /// Determines whether all specified items can be added to the collection without exceeding available capacity.
        /// </summary>
        /// <param name="items">An array of items to check for addability. The array must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if all items in the <paramref name="items"/> array can be added to the collection; otherwise, <see langword="false"/>.</returns>
        protected bool CanAddItems(T[] items)
        {
            if (items.Length == 0)
                return true;

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
            if (items.Length == 0)
                return true;
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (!items.HasAllEqual())
                throw new ArgumentException(StackInventoryErrors.CannotAddArrayWithDifferentItems, nameof(items));

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
            if (items.Length == 0)
                return true;
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);

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
                return Array.Empty<T>();
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);

            if (this.IsFull)
                throw new InvalidOperationException(StackInventoryErrors.InventoryIsFull);

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
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

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
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (items.Length == 0)
                return Array.Empty<T>();

            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (!items.HasAllEqual())
                throw new ArgumentException(StackInventoryErrors.CannotAddArrayWithDifferentItems, nameof(items));

            var notAddedItems = this.slots[index].Add(items);
            if (notAddedItems.Length != items.Length)
                this.OnAdd?.Invoke(this, (items.Skip(notAddedItems.Length).ToArray(), index));

            return notAddedItems;
        }
    }
}

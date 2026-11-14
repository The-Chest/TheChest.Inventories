using System;
using System.Collections.Generic;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class Inventory<T>
    {
        /// <inheritdoc/>
        public event InventoryGetEventHandler<T> OnGet;

        /// <summary>
        /// Gets the item from the slot at <paramref name="index"/> and fires the <see cref="OnGet"/> event.
        /// </summary>
        /// <param name="index">The index of the slot to get the item from.</param>
        /// <returns>The item from the slot at <paramref name="index"/>.</returns>
        protected T GetItem(int index)
        {
            var item = this.slots[index].Get();
            this.OnGet?.Invoke(this, (item, index));
            return item;
        }

        /// <summary>
        /// Gets the items from the slots at <paramref name="indexes"/> and fires the <see cref="OnGet"/> event.
        /// </summary>
        /// <param name="indexes">The indexes of the slots to get the items from.</param>
        /// <returns>The items from the slots at <paramref name="indexes"/>.</returns>
        protected T[] GetItems(int[] indexes)
        {
            var items = new List<T>();

            for (int i = 0; i < indexes.Length; i++)
            {
                var index = indexes[i];
                items.Add(this.slots[index].Get());
            }

            if (items.Count > 0)
                this.OnGet?.Invoke(this, (items.ToArray(), indexes));

            return items.ToArray();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event with every item returned from it.
        /// </remarks>
        public virtual T[] Clear()
        {
            var quarter = this.Size / 4;

            var items = new List<T>(quarter);
            var indexes = new List<int>(quarter);

            for (int i = 0; i < this.Size; i++)
            {
                if(this.slots[i].IsEmpty)
                    continue;

                var item = this.slots[i].Get(); 

                indexes.Add(i);
                items.Add(item);
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
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    indexes.Add(i);
                }
            }
            return this.GetItems(indexes.ToArray());
        }

        #region Get
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event if an item is found on <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        /// <exception cref="InvalidOperationException">When the slot in <paramref name="index"/> is empty</exception>
        public virtual T Get(int index)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (this.slots[index].IsEmpty)
                throw new InvalidOperationException(InventoryErrors.EmptySlot);

            return this.GetItem(index);
        }
        /// <inheritdoc/>
        /// <remarks>
        /// The method fires the <see cref="OnGet"/> event when <paramref name="item"/> is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">When the item is not found in the inventory</exception>
        public virtual T Get(T item)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));

            for (int index = 0; index < this.Size; index++)
            {
                if (this.slots[index].Contains(item))
                {
                    return this.GetItem(index);
                }
            }

            throw new InvalidOperationException(InventoryErrors.ItemNotFound);
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

            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                if (!this.slots[i].Contains(item) || !this.slots[i].CanGet())
                    continue;

                indexes.Add(i);

                if (indexes.Count == amount)
                    break;
            }

            return this.GetItems(indexes.ToArray());
        }
        #endregion

        #region TryGet
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        public virtual bool TryGet(int index, out T item)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (!this.slots[index].CanGet())
            {
                item = default;
                return false;
            }

            item = this.GetItem(index);
            return true;
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual bool TryGet(T item, int amount, out T[] items)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var indexes = new List<int>();
            for (int i = 0; i < this.Size; i++)
            {
                if (!this.slots[i].Contains(item) || !this.slots[i].CanGet())
                    continue;

                indexes.Add(i);

                if (indexes.Count == amount)
                    break;
            }
            items = this.GetItems(indexes.ToArray());

            return items.Length == amount;
        }
        #endregion

        #region CanGet
        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is smaller than zero or bigger than <see cref="Container{T}.Size"/></exception>
        public virtual bool CanGet(int index)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].CanGet();
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller</exception>
        public virtual bool CanGet(T item, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var count = 0;

            for (int i = 0; i < this.Size; i++)
            {
                if (this.slots[i].Contains(item))
                {
                    count++;
                    if (count == amount)
                        return true;
                }
            }

            return false;
        }
        #endregion
    }
}

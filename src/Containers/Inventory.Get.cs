using System;
using System.Collections.Generic;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class Inventory<T>
    {
        /// <inheritdoc/>
        public event InventoryGetEventHandler<T> OnGet;

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

            if (this.slots[index].IsEmpty)
                return default;

            var item = this.slots[index].Get();

            if (!item.IsNull())
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
                if (slotItem.IsNull())
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
    }
}

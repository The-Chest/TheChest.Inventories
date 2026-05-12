using System;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class Inventory<T>
    {
        /// <inheritdoc/>
        public event InventoryReplaceEventHandler<T> OnReplace;

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
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var oldItem = this.slots[index].Replace(item);
            this.OnReplace?.Invoke(this, (index, oldItem, item));

            return oldItem;
        }
    }
}

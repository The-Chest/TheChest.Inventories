using System;
using TheChest.Core.Containers;
using TheChest.Inventories.Containers.Events.Stack.Lazy;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class LazyStackInventory<T>
    {
        /// <inheritdoc/>
        public event LazyStackInventoryReplaceEventHandler<T> OnReplace;

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        public virtual bool CanReplace(T item, int index, int amount = 1)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.slots[index].CanReplace(item, amount);
        }
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="item"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="amount"/> is zero or smaller or <paramref name="index"/> is bigger than <see cref="StackContainer{T}.Size"/> or smaller than zero</exception>
        /// <exception cref="InvalidOperationException">When <paramref name="amount"/> exceeds the stack size of the slot on <paramref name="index"/>.</exception>
        public virtual T[] Replace(T item, int index, int amount)
        {
            if (item.IsNull())
                throw new ArgumentNullException(nameof(item));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var slot = this.slots[index];
            if (!slot.CanReplace(item, amount))
                throw new InvalidOperationException("The amount of items to replace exceeds the stack size of the slot.");

            var replacedItems = slot.Replace(item, amount);

            if (replacedItems.Length > 0)
                this.OnReplace?.Invoke(this, (replacedItems[0], replacedItems.Length, item, amount, index));
            else
                this.OnReplace?.Invoke(this, (default, 0, item, amount, index));

            return replacedItems;
        }
    }
}

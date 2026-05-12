using System;
using TheChest.Inventories.Containers.Events.Stack;
using TheChest.Inventories.Containers.Exceptions;
using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Containers
{
    public partial class StackInventory<T>
    {
        /// <inheritdoc/>
        public event StackInventoryReplaceEventHandler<T> OnReplace;

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
            if (!items.HasAllEqualAndNoNull())
                return false;

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
                throw new ArgumentException(StackInventoryErrors.CannotReplaceEmptyArray, nameof(items));
            if (items.ContainsNull())
                throw new ArgumentNullException(nameof(items), StackInventoryErrors.ItemArrayContainsNull);
            if (index < 0 || index > this.Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var oldItems = this.slots[index].Replace(items);
            this.OnReplace?.Invoke(this, (index, oldItems, items));

            return oldItems;
        }
    }
}

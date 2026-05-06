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

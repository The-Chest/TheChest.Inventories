using System;
using System.Collections.Generic;
using System.Linq;
using TheChest.Inventories.Extensions;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Slots.Extensions
{
    internal static class InventoryStackSlotExtensions
    {
        /// <summary>
        /// Determines the indexes of <see cref="IInventoryStackSlot{T}"/> where the specified <paramref name="items"/> can be added, prioritizing slots that already contain the first item.
        /// </summary>
        /// <remarks>
        /// Slots that already contain the first item in <paramref name="items"/> are prioritized in the returned list. 
        /// The method does not modify the slot array; it only provides slot indexes suitable for adding the items.
        /// </remarks>
        /// <param name="slots">An array of <see cref="IInventoryStackSlot{T}"/> to evaluate for item addition.</param>
        /// <param name="items">An array of items to be added to the slot arrays.</param>
        /// <returns>A interable indexes of slot, ordered with preferred slots first, indicating where the items can be added.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> is <see langword="null"/>.</exception>
        internal static IEnumerable<int> GetAddOrderIndexes<T>(this IInventoryStackSlot<T>[] slots, T[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var main = new List<int>(slots.Length);
            var fallback = new List<int>(slots.Length);

            for (var index = 0; index < slots.Length; index++)
            {
                var slot = slots[index];
                var itemsToAdd = items.Take(slot.AvailableAmount).ToArray();

                if (!slot.CanAdd(itemsToAdd))
                    continue;

                if (slot.Contains(items[0]))
                {
                    if (main.Count <= items.Length)
                        main.Add(index);

                    continue;
                }

                if (fallback.Count <= items.Length)
                    fallback.Add(index);
            }

            main.AddRange(fallback);
            return main;
        }

        /// <summary>
        /// Converts an array of items into inventory stack slots by grouping adjacent equal items,
        /// with each stack containing at most <paramref name="maxStackSize"/> items.
        /// </summary>
        /// <typeparam name="T">The item type stored in the stack slots.</typeparam>
        /// <param name="items">The source array of items to convert.</param>
        /// <param name="maxStackSize">The maximum number of items allowed in a single stack slot.</param>
        /// <returns>
        /// An array of <see cref="IInventoryStackSlot{T}"/> instances representing the grouped items.
        /// </returns>
        internal static IInventoryStackSlot<T>[] ToStackSlots<T>(this T[] items, int maxStackSize)
        {
            var index = 0;

            var slots = new List<IInventoryStackSlot<T>>(items.Length);

            while (index < items.Length)
            {
                var startIndex = index;
                var endIndex = items.GetAdjacentEqualCount(startIndex, maxStackSize) + 1;

                var amountToAdd = endIndex - startIndex;
                var itemsToAdd = new T[amountToAdd];

                Array.Copy(
                    items, startIndex,
                    itemsToAdd, 0, amountToAdd
                );

                slots.Add(new InventoryStackSlot<T>(itemsToAdd, maxStackSize));

                index = endIndex;
            }

            return slots.ToArray();
        }
    }
}

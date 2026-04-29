using System.Collections.Generic;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Extensions
{
    internal static class IInventoryStackSlotExtensions
    {
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

                slots.Add(new InventoryStackSlot<T>(items[startIndex..endIndex], maxStackSize));

                index = endIndex;
            }

            return slots.ToArray();
        }
    }
}

using System.Collections.Generic;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Extensions
{
    internal static class IInventoryStackSlotExtensions
    {
        internal static IInventoryStackSlot<T>[] ToStackSlots<T>(this T[] items, int maxStackSize)
        {
            var index = 0;

            var slots = new List<IInventoryStackSlot<T>>(items.Length);

            while (index < items.Length)
            {
                var startIndex = index;
                var endIndex = items.GetAdjacentEqualCount(startIndex, maxStackSize);

                slots.Add(new InventoryStackSlot<T>(items[startIndex..endIndex], maxStackSize));

                index = endIndex + 1;
            }

            return slots.ToArray();
        }
    }
}

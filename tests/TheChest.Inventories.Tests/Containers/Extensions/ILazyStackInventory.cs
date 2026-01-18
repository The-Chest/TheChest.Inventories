using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Extensions
{
    internal static class ILazyStackInventory
    {
        internal static ILazyStackSlot<T>[]? GetSlots<T>(this ILazyStackInventory<T> inventory)
            => inventory.GetSlotsFieldOrProperty() as ILazyStackSlot<T>[];

        internal static ILazyStackSlot<T>? GetSlot<T>(this ILazyStackInventory<T> inventory, int index)
            => inventory.GetSlots()![index];

        internal static T? GetItem<T>(this ILazyStackInventory<T> inventory, int index)
            => inventory.GetSlots()![index].GetContent();
    }
}

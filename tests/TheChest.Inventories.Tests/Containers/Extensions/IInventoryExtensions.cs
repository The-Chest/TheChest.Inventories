using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Extensions
{
    public static class InventorySlotsExtensions
    {
        internal static ISlot<T>[]? GetSlots<T>(this IInventory<T> inventory)
            => inventory.GetSlotsFieldOrProperty() as ISlot<T>[];

        internal static T? GetItem<T>(this IInventory<T> inventory, int index)
            => inventory.GetSlots()![index].GetContent();
    }
}

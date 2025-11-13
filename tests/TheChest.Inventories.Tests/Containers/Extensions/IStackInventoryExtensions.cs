using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Extensions
{
    internal static class IStackInventoryExtensions
    {
        internal static IStackSlot<T>[]? GetSlots<T>(this IStackInventory<T> inventory)
            => inventory.GetSlotsFieldOrProperty() as IStackSlot<T>[];

        internal static T[]? GetItems<T>(this IStackInventory<T> inventory, int index)
            => inventory.GetSlots()![index].GetContents();
    }
}

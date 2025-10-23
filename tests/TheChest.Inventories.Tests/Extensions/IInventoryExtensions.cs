using System.Reflection;
using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Extensions
{
    public static class InventorySlotsExtensions
    {
        private static object? GetSlotsFieldOrProperty<T>(this T inventory)
        {
            if (inventory is null)
                throw new ArgumentNullException(nameof(inventory));

            var type = inventory.GetType();

            var field = 
                type.GetField(
                    "slots", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy
                ) ??
                throw new InvalidOperationException("Field 'slots' not found.");
            
            return field.GetValue(inventory);
        }

        public static ISlot<T>[]? GetSlots<T>(this IInventory<T> inventory)
            => inventory.GetSlotsFieldOrProperty() as ISlot<T>[];

        public static IStackSlot<T>[]? GetSlots<T>(this IStackInventory<T> inventory)
            => inventory.GetSlotsFieldOrProperty() as IStackSlot<T>[];

        public static ILazyStackSlot<T>[]? GetSlots<T>(this ILazyStackInventory<T> inventory)
            => inventory.GetSlotsFieldOrProperty() as ILazyStackSlot<T>[];
    }
}

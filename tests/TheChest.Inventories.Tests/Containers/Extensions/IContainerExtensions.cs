using System.Reflection;

namespace TheChest.Inventories.Tests.Containers.Extensions
{
    internal static class IContainerExtensions
    {
        internal static object? GetSlotsFieldOrProperty<T>(this T inventory)
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
    }
}

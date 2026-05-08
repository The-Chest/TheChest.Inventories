using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Extensions
{
    internal static class IInventoryExtensions
    {
        internal static void Remove<T>(this IInventory<T> inventory, int amount = 1, Random? random = null)
        {
            random ??= new Random();
            for (int i = 0; i < amount; i++)
            {
                var randomIndex = random.Next(0, inventory.Size);
                inventory.Get(randomIndex);
            }
        }
    }
}

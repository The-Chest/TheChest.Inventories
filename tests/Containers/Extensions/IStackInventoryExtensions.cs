using TheChest.Inventories.Containers.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Extensions
{
    internal static class IStackInventoryExtensions
    {
        internal static void Remove<T>(this IStackInventory<T> inventory, int amount, Random? random = null)
        {
            random ??= new Random();
            for (int i = 0; i < amount; i++)
            {
                var randomIndex = random.Next(0, inventory.Size);
                inventory.Get(randomIndex);
            }
        }

        internal static int RemoveRandomAt<T>(this IStackInventory<T> inventory, int index, int maxSize, Random? random = null)
        {
            random ??= new Random(); 
            var randomAmount = random.Next(1, maxSize);
            inventory.Get(index, randomAmount);

            return randomAmount;
        }
    }
}

using TheChest.Core.Inventories.Containers.Interfaces;

namespace TheChest.Core.Inventories.Tests.Containers.Factories.Interfaces
{
    public interface IInventoryFactory<T>
    {
        IInventory<T> EmptyContainer(int size = 20);
        IInventory<T> FullContainer(int size, T item);
        IInventory<T> ShuffledItemContainer(int size, T item);
        IInventory<T> ShuffledItemsContainer(int size, params T[] items);
    }
}

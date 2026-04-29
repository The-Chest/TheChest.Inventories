using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Interfaces.Factories
{
    public interface IInventoryLazyStackSlotFactory<T>
    {
        IInventoryLazyStackSlot<T> Empty(int maxAmount);
        IInventoryLazyStackSlot<T> WithItem(T item, int amount, int maxAmount);
        IInventoryLazyStackSlot<T> FullSlot(T item, int maxAmount);
    }
}

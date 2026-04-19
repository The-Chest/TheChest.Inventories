using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Interfaces.Factories
{
    public interface IInventoryStackSlotFactory<T>
    {
        IInventoryStackSlot<T> EmptySlot(int maxAmount);
        IInventoryStackSlot<T> WithItem(T item, int amount, int maxAmount);
        IInventoryStackSlot<T> WithItems(T[] items, int maxAmount);
        IInventoryStackSlot<T> FullSlot(T item, int maxAmount);
        IInventoryStackSlot<T> FullSlot(T[] items);
    }
}

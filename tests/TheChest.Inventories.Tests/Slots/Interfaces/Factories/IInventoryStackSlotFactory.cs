using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Interfaces.Factories
{
    public interface IInventoryStackSlotFactory<T>
    {
        IInventoryStackSlot<T> Empty(int maxAmount);
        IInventoryStackSlot<T> WithItem(T item, int amount, int maxAmount);
        IInventoryStackSlot<T> WithItems(T[] items, int maxAmount);
        IInventoryStackSlot<T> Full(T[] items);
    }
}

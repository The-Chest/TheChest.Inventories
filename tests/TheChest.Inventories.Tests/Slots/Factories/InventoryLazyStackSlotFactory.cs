using TheChest.Inventories.Slots;
using TheChest.Inventories.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Factories
{
    public class InventoryLazyStackSlotFactory<Slot, Item> : IInventoryStackSlotFactory<Item>
        where Slot : InventoryLazyStackSlot<Item>
    {
        public virtual IInventoryStackSlot<Item> EmptySlot()
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, default(Item), 0 , 10);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> EmptySlot(int maxAmount = 10)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, default(Item), 0, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> FullSlot(Item item, int maxAmount = 10)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, item, maxAmount, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> FullSlot(params Item[] items)
        {
            if(items.Length != 1)
                throw new ArgumentException("InventoryLazyStackSlot only accepts one item", nameof(items));

            const int maxAmount = 10;
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, items[0], maxAmount, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> WithItem(Item item, int amount = 1, int maxAmount = 10)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, item, amount, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> WithItems(Item[] items, int maxAmount = 10)
        {
            if (items.Length != 1)
                throw new ArgumentException("InventoryLazyStackSlot only accepts one item", nameof(items));

            var amount = 10;
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, items[0], amount, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }
    }
}

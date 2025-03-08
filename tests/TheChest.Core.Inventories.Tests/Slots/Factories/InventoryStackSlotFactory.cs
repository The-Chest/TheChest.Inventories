using TheChest.Core.Tests.Slots.Factories;
using TheChest.Core.Inventories.Slots.Interfaces;
using TheChest.Core.Inventories.Slots;

namespace TheChest.Core.Inventories.Tests.Slots.Factories
{
    public class InventoryStackSlotFactory<Slot, Item> : StackSlotFactory<Slot, Item>, IInventoryStackSlotFactory<Item>
        where Slot : InventoryStackSlot<Item>
    {
        public IInventoryStackSlot<Item> EmptySlot(int maxAmount = 10)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, Array.Empty<Item>(), maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public IInventoryStackSlot<Item> FullSlot(Item[] items)
        {
            var type = typeof(Slot);

            var slot = Activator.CreateInstance(type, (Item[])items.Clone(), items.Length);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public IInventoryStackSlot<Item> WithItems(Item[] items, int maxAmount = 10)
        {
            var type = typeof(Slot);

            var slot = Activator.CreateInstance(type, (Item[])items.Clone(), maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public override IInventoryStackSlot<Item> EmptySlot()
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, Array.Empty<Item>(), 10);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> FullSlot(Item item, int maxAmount = 10)
        {
            if(maxAmount < 1)
                throw new ArgumentOutOfRangeException(nameof(maxAmount), "Max amount must be greater than 0");

            var type = typeof(Slot);

            var items = new Item[maxAmount];
            Array.Fill(items, item);

            var slot = Activator.CreateInstance(type, items, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public override IInventoryStackSlot<Item> WithItem(Item item, int amount, int maxAmount)
        {
            var type = typeof(Slot);
            var items = new Item[amount];
            Array.Fill(items, item);

            var slot = Activator.CreateInstance(type, items, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }
    }
}
        
using TheChest.Inventories.Slots.Interfaces;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Slots.Factories
{
    public class InventoryStackSlotFactory<Slot, Item> : IInventoryStackSlotFactory<Item>
        where Slot : InventoryStackSlot<Item>
    {
        public virtual IInventoryStackSlot<Item> Empty(int maxAmount)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> Full(Item[] items)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, (Item[])items.Clone());
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> WithItems(Item[] items, int maxAmount)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, (Item[])items.Clone(), maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }

        public virtual IInventoryStackSlot<Item> WithItem(Item item, int amount, int maxAmount)
        {
            var type = typeof(Slot);
            var items = new Item[amount];
            Array.Fill(items, item);

            var slot = Activator.CreateInstance(type, items, maxAmount);
            return (IInventoryStackSlot<Item>)slot!;
        }
    }
}
        
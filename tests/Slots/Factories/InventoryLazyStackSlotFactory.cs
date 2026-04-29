using TheChest.Inventories.Slots;
using TheChest.Inventories.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Slots.Factories
{
    public class InventoryLazyStackSlotFactory<Slot, Item> : IInventoryLazyStackSlotFactory<Item>
        where Slot : InventoryLazyStackSlot<Item>
    {
        public virtual IInventoryLazyStackSlot<Item> Empty(int maxAmount)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, default(Item), 0, maxAmount);
            return (IInventoryLazyStackSlot<Item>)slot!;
        }

        public virtual IInventoryLazyStackSlot<Item> FullSlot(Item item, int maxAmount)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, item, maxAmount, maxAmount);
            return (IInventoryLazyStackSlot<Item>)slot!;
        }

        public virtual IInventoryLazyStackSlot<Item> WithItem(Item item, int amount, int maxAmount)
        {
            var type = typeof(Slot);
            var slot = Activator.CreateInstance(type, item, amount, maxAmount);
            return (IInventoryLazyStackSlot<Item>)slot!;
        }
    }
}

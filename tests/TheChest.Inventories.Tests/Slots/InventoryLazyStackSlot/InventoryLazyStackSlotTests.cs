using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Slots
{
    public partial class InventoryLazyStackSlotTests<T> : IInventoryLazyStackSlotTests<T>
    {
        public InventoryLazyStackSlotTests() : base(configure =>
        {
            configure.Register<IInventoryLazyStackSlotFactory<T>, InventoryLazyStackSlotFactory<InventoryLazyStackSlot<T>, T>>();
        }) { }
    }
}

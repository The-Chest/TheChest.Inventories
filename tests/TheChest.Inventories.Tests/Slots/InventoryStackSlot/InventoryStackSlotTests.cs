using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common.Items.ReferenceType;
namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    [TestFixture(typeof(TestItem))]
    public partial class InventoryStackSlotTests<T> : IInventoryStackSlotTests<T>
    {
        public InventoryStackSlotTests() : base(configure => {
            configure.Register<IInventoryStackSlotFactory<T>, InventoryStackSlotFactory<InventoryStackSlot<T>, T>>();
        }) { }
    }
}

using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Slots
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestStructItem))]
    [TestFixture(typeof(TestEnumItem))]
    public partial class InventoryLazyStackSlotTests<T> : IInventoryLazyStackSlotTests<T>
    {
        public InventoryLazyStackSlotTests() : base(configure =>
        {
            configure.Register<IInventoryLazyStackSlotFactory<T>, InventoryLazyStackSlotFactory<InventoryLazyStackSlot<T>, T>>();
        }) { }
    }
}

using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestStructItem))]
    [TestFixture(typeof(TestEnumItem))]
    public partial class InventorySlotTests<T> : IInventorySlotTests<T>
    {
        public InventorySlotTests() : base(configure =>
        {
            configure.Register<IInventorySlotFactory<T>, InventorySlotFactory<InventorySlot<T>, T>>();
        }) { }
    }
}

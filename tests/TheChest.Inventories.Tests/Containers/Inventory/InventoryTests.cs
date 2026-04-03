using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;
using TheChest.Inventories.Tests.Containers.Factories;
using TheChest.Inventories.Tests.Containers.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Containers
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestStructItem))]
    [TestFixture(typeof(TestEnumItem))]
    public partial class InventoryTests<T> : IInventoryTests<T>
    {
        public InventoryTests() : base(configure =>
        {
            configure
                .Register<IInventorySlotFactory<T>, InventorySlotFactory<InventorySlot<T>, T>>()
                .Register<IInventoryFactory<T>, InventoryFactory<Inventory<T>, T>>();
        }) { }
    }
}

using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;
using TheChest.Inventories.Tests.Containers.Factories;
using TheChest.Inventories.Tests.Containers.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Factories.Interfaces;

namespace TheChest.Inventories.Tests.Containers
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestStructItem))]
    [TestFixture(typeof(TestEnumItem))]
    public partial class StackInventoryTests<T> : IStackInventoryTests<T>
    {
        public StackInventoryTests() : base(configure =>
        {
            configure
                .Register<IInventoryStackSlotFactory<T>, InventoryStackSlotFactory<InventoryStackSlot<T>, T>>()
                .Register<IStackInventoryFactory<T>, StackInventoryFactory<StackInventory<T>, T>>();
        }) { }
    }
}

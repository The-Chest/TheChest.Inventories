using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Containers.Factories;
using TheChest.Inventories.Tests.Containers.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Factories.Interfaces;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestStructItem))]
    [TestFixture(typeof(TestEnumItem))]
    public partial class LazyStackInventoryTests<T> : ILazyStackInventoryTests<T>
    {
        public LazyStackInventoryTests() : base(configure =>
        {
            configure
                .Register<IInventoryLazyStackSlotFactory<T>, InventoryLazyStackSlotFactory<InventoryLazyStackSlot<T>, T>>()
                .Register<ILazyStackInventoryFactory<T>, LazyStackInventoryFactory<LazyStackInventory<T>, T>>();
        })
        { }
    }
}

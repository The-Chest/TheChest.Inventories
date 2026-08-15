using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestEnumItem))]
    [TestFixture(typeof(TestStructItem))]
    public partial class InventorySlotTests<T> : BaseTest<T>
    {
        protected readonly IInventorySlotFactory<T> slotFactory;
        protected readonly IItemFactory<T> itemFactory;

        public InventorySlotTests() : 
            base(configure => configure.Register<IInventorySlotFactory<T>, InventorySlotFactory<InventorySlot<T>, T>>())
        {
            this.slotFactory = this.configurations.Resolve<IInventorySlotFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }
    }
}

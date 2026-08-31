using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestEnumItem))]
    [TestFixture(typeof(TestStructItem))]
    public partial class InventoryLazyStackSlotTests<T> : BaseTest<T>
    {
        protected readonly IInventoryLazyStackSlotFactory<T> slotFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_STACK_SIZE_TEST = 5;
        protected const int MAX_STACK_SIZE_TEST = 10;

        public InventoryLazyStackSlotTests() : base(configure =>
        {
            configure.Register<IInventoryLazyStackSlotFactory<T>, InventoryLazyStackSlotFactory<InventoryLazyStackSlot<T>, T>>();
        })
        {
            this.slotFactory = this.configurations.Resolve<IInventoryLazyStackSlotFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }
    }
}

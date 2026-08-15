using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestEnumItem))]
    [TestFixture(typeof(TestStructItem))]
    public partial class InventoryStackSlotTests<T> : BaseTest<T>
    {
        protected readonly IInventoryStackSlotFactory<T> slotFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_STACK_SIZE_TEST = 5;
        protected const int MAX_STACK_SIZE_TEST = 10;

        public InventoryStackSlotTests() : base(configure =>
        {
            configure.Register<IInventoryStackSlotFactory<T>, InventoryStackSlotFactory<InventoryStackSlot<T>, T>>();
        })
        {
            this.slotFactory = this.configurations.Resolve<IInventoryStackSlotFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }

        private int GetRandomStackSize() => this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
    }
}

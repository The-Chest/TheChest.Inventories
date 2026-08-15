using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common.Items.ReferenceType;
using TheChest.Tests.Common.Items.ValueType;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    [TestFixture(typeof(TestItem))]
    [TestFixture(typeof(TestEnumItem))]
    [TestFixture(typeof(TestStructItem))]
    public partial class InventoryStackSlotTests<T> : IInventoryStackSlotTests<T>
    {
        public InventoryStackSlotTests() : base(configure => {
            configure.Register<IInventoryStackSlotFactory<T>, InventoryStackSlotFactory<InventoryStackSlot<T>, T>>();
        }) { }

        private int GetRandomStackSize()
        {
            return this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
        }
    }
}

using TheChest.Inventories.Containers;
using TheChest.Inventories.Slots;
using TheChest.Inventories.Tests.Containers.Factories;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Inventories.Tests.Slots.Factories;
using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Tests.Common.Items.ReferenceType;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    [TestFixture(typeof(TestItem))]
    public partial class StackInventoryTests<T> : BaseTest<T>
    {
        protected readonly IStackInventoryFactory<T> inventoryFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_SIZE_TEST = 10;
        protected const int MAX_SIZE_TEST = 20;

        protected const int MIN_STACK_SIZE_TEST = 5;
        protected const int MAX_STACK_SIZE_TEST = 10;

        public StackInventoryTests() : base(configure =>
        {
            configure
                .Register<IInventoryStackSlotFactory<T>, InventoryStackSlotFactory<InventoryStackSlot<T>, T>>()
                .Register<IStackInventoryFactory<T>, StackInventoryFactory<StackInventory<T>, T>>();
        }) {

            this.inventoryFactory = this.configurations.Resolve<IStackInventoryFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }

        protected (int size, int stackSize) GenerateRandomSizeAndStackSize() =>
            (this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST), this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST));
    }
}

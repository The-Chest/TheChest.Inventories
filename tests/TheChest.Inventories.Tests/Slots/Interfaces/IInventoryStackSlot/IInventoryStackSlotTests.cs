using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Tests.Common.Items.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public abstract partial class IInventoryStackSlotTests<T> : BaseTest<T>
    {
        protected readonly IInventoryStackSlotFactory<T> slotFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_STACK_SIZE_TEST = 5;
        protected const int MAX_STACK_SIZE_TEST = 10;

        protected IInventoryStackSlotTests(Action<DIContainer> configure) : base(configure)
        {
            this.slotFactory = this.configurations.Resolve<IInventoryStackSlotFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }
    }
}

using TheChest.Inventories.Tests.Containers.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Tests.Common.Items.Interfaces;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T> : BaseTest<T>
    {
        protected readonly IStackInventoryFactory<T> inventoryFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_SIZE_TEST = 10;
        protected const int MAX_SIZE_TEST = 20;

        protected const int MIN_STACK_SIZE_TEST = 5;
        protected const int MAX_STACK_SIZE_TEST = 10;

        protected IStackInventoryTests(Action<DIContainer> configure) : base(configure)
        {
            this.inventoryFactory = this.configurations.Resolve<IStackInventoryFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }
    }
}

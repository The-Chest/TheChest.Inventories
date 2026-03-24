using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T> : BaseTest<T>
    {
        protected readonly IInventoryFactory<T> inventoryFactory;
        protected readonly IItemFactory<T> itemFactory;

        protected const int MIN_SIZE_TEST = 10;
        protected const int MAX_SIZE_TEST = 20;

        protected IInventoryTests(Action<DIContainer> configure) : base(configure)
        {
            this.inventoryFactory = this.configurations.Resolve<IInventoryFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }
    }
}

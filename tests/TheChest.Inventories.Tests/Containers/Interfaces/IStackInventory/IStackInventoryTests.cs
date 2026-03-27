using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T> : BaseTest<T>
    {
        protected readonly IStackInventoryFactory<T> inventoryFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected const int MIN_SIZE_TEST = 10;
        protected const int MAX_SIZE_TEST = 20;

        protected IStackInventoryTests(Action<DIContainer> configure) : base(configure)
        {
            this.inventoryFactory = this.configurations.Resolve<IStackInventoryFactory<T>>();
            this.itemFactory = this.configurations.Resolve<ISlotItemFactory<T>>();
        }
    }
}

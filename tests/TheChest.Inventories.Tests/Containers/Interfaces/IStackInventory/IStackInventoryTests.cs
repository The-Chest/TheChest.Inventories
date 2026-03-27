using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Inventories.Tests.Containers.Interfaces.Factories;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T> : BaseTest<T>
    {
        protected readonly IStackInventoryFactory<T> containerFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected IStackInventoryTests(Action<DIContainer> configure) : base(configure)
        {
            this.containerFactory = this.configurations.Resolve<IStackInventoryFactory<T>>();
            this.itemFactory = this.configurations.Resolve<ISlotItemFactory<T>>();
        }
    }
}

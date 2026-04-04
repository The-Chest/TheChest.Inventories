using TheChest.Inventories.Tests.Slots.Interfaces.Factories;
using TheChest.Tests.Common;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Tests.Common.Items.Interfaces;

namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventorySlotTests<T> : BaseTest<T>
    {
        protected readonly IInventorySlotFactory<T> slotFactory;
        protected readonly IItemFactory<T> itemFactory;

        public IInventorySlotTests(Action<DIContainer> configure) : base(configure)
        {
            this.slotFactory = this.configurations.Resolve<IInventorySlotFactory<T>>();
            this.itemFactory = this.configurations.Resolve<IItemFactory<T>>();
        }
    }
}

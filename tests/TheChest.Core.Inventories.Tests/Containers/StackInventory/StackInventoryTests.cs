using TheChest.Core.Tests.Containers;

namespace TheChest.Core.Inventories.Tests.Containers
{
    public abstract partial class StackInventoryTests<T> : IStackContainerTests<T>
    {
        protected new readonly IStackInventoryFactory<T> containerFactory;
        public StackInventoryTests(IStackInventoryFactory<T> containerFactory, ISlotItemFactory<T> itemFactory) : base(containerFactory, itemFactory) 
        { 
            this.containerFactory = containerFactory;
        }
    }
}

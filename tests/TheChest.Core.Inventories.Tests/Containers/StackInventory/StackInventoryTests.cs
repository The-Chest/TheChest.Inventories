namespace TheChest.Core.Inventories.Tests.Containers
{
    public abstract partial class StackInventoryTests<T> 
    {
        protected readonly IStackInventoryFactory<T> containerFactory;
        protected readonly ISlotItemFactory<T> itemFactory;
        public StackInventoryTests(IStackInventoryFactory<T> containerFactory, ISlotItemFactory<T> itemFactory)
        { 
            this.containerFactory = containerFactory;
            this.itemFactory = itemFactory;
        }
    }
}

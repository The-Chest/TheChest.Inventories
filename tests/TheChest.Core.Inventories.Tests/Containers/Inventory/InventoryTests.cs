namespace TheChest.Core.Inventories.Tests.Containers
{
    public abstract partial class InventoryTests<T>
    {
        protected readonly IInventoryFactory<T> containerFactory;
        protected readonly ISlotItemFactory<T> itemFactory;
        public InventoryTests(IInventoryFactory<T> containerFactory, ISlotItemFactory<T> itemFactory) 
        { 
            this.containerFactory = containerFactory;
            this.itemFactory = itemFactory;
        }
    }
}

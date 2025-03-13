namespace TheChest.Core.Inventories.Tests.Slots
{
    public abstract partial class IInventorySlotTests<T>
    {
        protected readonly IInventorySlotFactory<T> slotFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        public IInventorySlotTests(IInventorySlotFactory<T> slotFactory, ISlotItemFactory<T> itemFactory) 
        {
            this.slotFactory = slotFactory;
            this.itemFactory = itemFactory;
        }
    }
}

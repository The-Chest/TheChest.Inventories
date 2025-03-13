namespace TheChest.Core.Inventories.Tests.Slots
{
    public abstract partial class IInventoryStackSlotTests<T>
    {
        protected readonly IInventoryStackSlotFactory<T> slotFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        public IInventoryStackSlotTests(IInventoryStackSlotFactory<T> slotFactory, ISlotItemFactory<T> itemFactory)
        {
            this.slotFactory = slotFactory;
            this.itemFactory = itemFactory;
        }
    }
}

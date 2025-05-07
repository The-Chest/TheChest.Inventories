namespace TheChest.Inventories.Tests.Slots.IInventoryLazyStackSlot
{
    public abstract partial class IInventoryLazyStackSlotTests<T>
    {
        protected readonly IInventoryStackSlotFactory<T> slotFactory;
        protected readonly ISlotItemFactory<T> itemFactory;

        protected readonly Random random;

        public IInventoryLazyStackSlotTests(IInventoryStackSlotFactory<T> slotFactory, ISlotItemFactory<T> itemFactory)
        {
            this.slotFactory = slotFactory;
            this.itemFactory = itemFactory;

            this.random = new Random();
        }
    }
}

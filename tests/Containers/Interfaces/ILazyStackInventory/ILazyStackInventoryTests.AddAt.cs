namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void AddAt_AllItemsSuccessfullyAdded_ReturnsZero()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var notAddedCount = inventory.AddAt(item, 0, stackSize);
            
            Assert.That(notAddedCount, Is.Zero);
        }
    }
}

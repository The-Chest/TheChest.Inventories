namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItemsAt_EmptySlot_ReturnsEmpty()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var index = this.random.Next(0, size);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.Empty);
        }
    }
}

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

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var amount = stackSize;
            var items = this.itemFactory.CreateMany(amount);
            var index = this.random.Next(0, size); 
            Assert.That(() => inventory.AddAt(items, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemsAt_FullSlotSlotWithSameItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize);
            Assert.That(() => inventory.AddAt(items, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var index = this.random.Next(0, size);
            Assert.That(() => inventory.AddAt(items, index), Throws.InvalidOperationException);
        }
    }
}

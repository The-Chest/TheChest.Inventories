namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItemAt_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var index = this.random.Next(0, size);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItemAt_FullSlotWithSameItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size - 1);
            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
        }
    }
}

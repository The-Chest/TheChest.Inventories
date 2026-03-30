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
        public void AddItemAt_SlotWithDifferentItem_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.False);
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
        public void AddItemAt_FullSlotWithSameItem_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var index = this.random.Next(0, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.False);
        }
    }
}

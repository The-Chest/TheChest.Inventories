namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void CanReplace_MoreItemsThanStackSize_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);

            var canReplace = inventory.CanReplace(newItem, index, stackSize + 1);

            Assert.That(canReplace, Is.False);
        }

        [Test]
        public void CanReplace_SlotWithItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var canReplace = inventory.CanReplace(newItem, index, stackSize);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var canReplace = inventory.CanReplace(newItem, index, stackSize);

            Assert.That(canReplace, Is.True);
        }
    }
}

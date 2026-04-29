namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void CanReplace_EmptyItems_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var canReplace = inventory.CanReplace(Array.Empty<T>(), randomIndex);
            
            Assert.That(canReplace, Is.False);
        }

        [Test]
        public void CanReplace_SlotWithItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var canReplace = inventory.CanReplace(newItems, randomIndex);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var canReplace = inventory.CanReplace(newItems, randomIndex);

            Assert.That(canReplace, Is.True);
        }
    }
}

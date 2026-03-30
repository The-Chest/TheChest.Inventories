namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void CanAddItem_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_AvailableSlotOnInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex);

            var canAdd = inventory.CanAdd(item);
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_AvailableSlotWithDifferentItemOnInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex);

            var randomItem = this.itemFactory.CreateRandom();
            var canAdd = inventory.CanAdd(randomItem);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItem_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.False);
        }
    }
}

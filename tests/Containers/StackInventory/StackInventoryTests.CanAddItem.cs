namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            Assert.That(() => inventory.CanAdd(item: default!), Throws.ArgumentNullException);
        }


        [Test]
        public void CanAddItem_EmptyInventory_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_AvailableSlotOnInventory_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.False);
        }
    }
}

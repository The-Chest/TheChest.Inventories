namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void CanAddAt_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size - 1);
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddAt_AmountBiggerThanSlotSize_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size - 1);
            var amount = stackSize + this.random.Next(1, stackSize);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size - 1);
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size - 1);
            inventory.Get(randomIndex, stackSize - 1);

            var amount = this.random.Next(1, stackSize - 1);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddAt_SlotWithSameItemsAndNotEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size - 1);
            inventory.Get(randomIndex, stackSize - 1);

            var amount = stackSize;
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemAt_SlotWithDifferentItemsAndEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size - 1);
            inventory.Get(randomIndex, stackSize - 1);

            var randomItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAddAt(randomItem, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }
    }
}

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void CanAddItemsAt_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(5);

            var randomIndex = this.random.Next(0, size);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemsAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAddAt(items, randomIndex);
            
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemsAt_ItemsAmountBiggerThanSlotSize_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.GetAll(randomIndex);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemsAt_SlotWithDifferentItemsAndEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var items = this.itemFactory.CreateMany(stackSize - 1);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemsAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }
    }
}

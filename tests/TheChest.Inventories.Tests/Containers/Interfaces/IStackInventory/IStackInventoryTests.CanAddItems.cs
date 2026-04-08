namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void CanAddItems_EmptyItemsArray_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var items = Array.Empty<T>();
            
            var canAdd = inventory.CanAdd(items);
            
            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var items = this.itemFactory.CreateMany(10);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_InsufficientSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var randomAmount = this.random.Next(2, size);
            inventory.Get(item, randomAmount - 1);

            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_SufficientSpaceWithDifferentItem_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var randomAmount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_SufficientSpaceWithSameItem_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var items = this.itemFactory.CreateMany(stackSize - 1);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_SufficientSpaceWithSameItemInMultipleSlots_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            for (int i = 0; i < stackSize; i++)
            {
                var randomIndex = this.random.Next(0, size);
                inventory.Get(randomIndex, 1);
            }

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var items = this.itemFactory.CreateMany(size);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }
    }
}

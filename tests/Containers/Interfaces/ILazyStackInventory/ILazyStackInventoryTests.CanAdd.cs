namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void CanAdd_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAdd_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);
            
            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAdd_PartiallyFilledInventoryWithSpace_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItems = this.itemFactory.CreateManyRandom(MIN_SIZE_TEST / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);
           
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAdd_PartiallyFilledInventoryWithoutSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);
            
            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAdd_InventoryWithSameItemWithSpace_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 1).ToList();
            inventoryItems.Add(item);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            var amount = this.random.Next(1, stackSize);
            inventory.Get(item, amount);

            var canAdd = inventory.CanAdd(item, amount);

            Assert.That(canAdd, Is.True);
        }
    }
}

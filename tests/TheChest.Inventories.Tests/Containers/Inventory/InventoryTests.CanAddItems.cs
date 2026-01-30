namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddItems_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.CanAdd(items: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void CanAddItems_ArrayContainingNullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var items = this.itemFactory.CreateMany(5).ToList();
            items.Add(default!);

            Assert.That(() => inventory.CanAdd(items.ToArray()), Throws.ArgumentNullException);
        }

        [Test]
        public void CanAddItems_EmptyItemsArray_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var items = Array.Empty<T>();
            
            var canAdd = inventory.CanAdd(items);
            
            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_ItemsAmountBiggerThanInventorySize_ReturnsTrue()
        {
            var size = this.random.Next(5, 10);
            var inventory = this.containerFactory.EmptyContainer(size);
            var items = this.itemFactory.CreateMany(size + 1);

            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var items = this.itemFactory.CreateMany(10);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_PartiallyFullInventory_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomAmount = this.random.Next(1, size);
            inventory.Get(item, randomAmount);

            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_InsufficientSpace_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);
            var randomAmount = this.random.Next(1, size);
            inventory.Get(item, randomAmount - 1);

            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }
    }
}

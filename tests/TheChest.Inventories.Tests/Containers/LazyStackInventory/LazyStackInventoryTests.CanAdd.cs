namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void CanAdd_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.CanAdd(item: default!, amount: 1), Throws.ArgumentNullException);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanAdd_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.CanAdd(item, amount), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CanAdd_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);

            Assert.That(inventory.CanAdd(item, amount), Is.True);
        }

        [Test]
        public void CanAdd_FullInventory_ReturnsFalse()
        {
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, inventoryItem);
            
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            
            Assert.That(inventory.CanAdd(item, amount), Is.False);
        }

        [Test]
        public void CanAdd_PartiallyFilledInventoryWithSpace_ReturnsTrue()
        {
            var inventoryItems = this.itemFactory.CreateManyRandom(5);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, inventoryItems);
            
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            
            Assert.That(inventory.CanAdd(item, amount), Is.True);
        }

        [Test]
        public void CanAdd_PartiallyFilledInventoryWithoutSpace_ReturnsFalse()
        {
            var inventoryItems = this.itemFactory.CreateManyRandom(20);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, inventoryItems);
            
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            
            Assert.That(inventory.CanAdd(item, amount), Is.False);
        }

        [Test]
        public void CanAdd_InventoryWithSameItemWithSpace_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(10).ToList();
            inventoryItems.Add(item);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, inventoryItems.ToArray());
            
            var amount = this.random.Next(1, 5);
            
            Assert.That(inventory.CanAdd(item, amount), Is.True);
        }
    }
}

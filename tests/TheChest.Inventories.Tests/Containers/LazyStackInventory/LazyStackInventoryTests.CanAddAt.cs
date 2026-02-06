namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void CanAddAt_NullItem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size - 1);
            Assert.That(
                () => inventory.CanAddAt(item: default!, randomIndex, amount: 1), 
                Throws.ArgumentNullException
             );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanAddAt_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size - 1);
            Assert.That(
                () => inventory.CanAddAt(item, randomIndex, amount), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [TestCase(100)]
        [TestCase(-1)]
        public void CanAddAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.CanAddAt(item, index, 1), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void CanAddAt_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);

            var randomIndex = this.random.Next(0, size - 1);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddAt_AmountBiggerThanSlotSize_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size - 1);
            var amount = stackSize + this.random.Next(1, 5);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, 5, inventoryItem);
            
            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size - 1);
            var amount = this.random.Next(1, 5);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);
            
            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 5);

            var amount = this.random.Next(1, 5);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddAt_SlotWithSameItemsAndNotEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var amount = this.random.Next(2, 5);
            var canAdd = inventory.CanAddAt(item, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemAt_SlotWithDifferentItemsAndEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);
            
            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var randomItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 5);
            var canAdd = inventory.CanAddAt(randomItem, randomIndex, amount);

            Assert.That(canAdd, Is.False);
        }
    }
}

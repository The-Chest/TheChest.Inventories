namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void CanAddItemsAt_NullItems_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.CanAddAt(items: default!, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void CanAddItemsAt_ArrayContainingNullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var items = this.itemFactory.CreateMany(5).ToList();
            items.Add(default!);

            Assert.That(() => inventory.CanAddAt(items.ToArray(), 0), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(22)]
        public void CanAddItemsAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.CanAddAt(this.itemFactory.CreateMany(5), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void CanAddItemsAt_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var items = this.itemFactory.CreateMany(5);

            var canAdd = inventory.CanAddAt(items, 0);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemsAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAddAt(items, randomIndex);
            
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemsAt_ItemsAmountBiggerThanSlotSize_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.GetAll(randomIndex);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemsAt_SlotWithDifferentItemsAndEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(size, stackSize, randomItem);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var items = this.itemFactory.CreateMany(stackSize - 1);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemsAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }
    }
}

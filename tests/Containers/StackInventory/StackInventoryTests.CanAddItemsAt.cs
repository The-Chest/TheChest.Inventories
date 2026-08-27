using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void CanAddItemsAt_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            Assert.That(
                () => inventory.CanAddAt(items: null!, index),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void CanAddItemsAt_EmptyItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = Array.Empty<T>();
            var index = this.random.Next(0, size);
            var canAdd = inventory.CanAddAt(items, index);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        [IgnoreIfValueType]
        public void CanAddItemsAt_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize - 1).ToList();
            items.Add(default!);
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.CanAddAt(items.ToArray(), index),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanAddItemsAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            Assert.That(
                () => inventory.CanAddAt(items, index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanAddItemsAt_ItemsAmountExceedsStackSize_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemsAt_EmptyInventory_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);

            var randomIndex = this.random.Next(0, size);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemsAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItemsAt_ValueType_ItemsContainingDefault_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            var randomIndex = this.random.Next(0, size);
            var canAdd = inventory.CanAddAt(items, randomIndex);

            Assert.That(canAdd, Is.True);
        }
    }
}

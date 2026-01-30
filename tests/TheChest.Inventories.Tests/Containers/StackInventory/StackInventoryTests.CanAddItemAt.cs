using NUnit.Framework.Internal;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void CanAddItemAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.CanAddAt(item: default!, 0), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(22)]
        public void CanAddItemAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.CanAddAt(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void CanAddItemAt_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAddAt(item, 0);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var canAdd = inventory.CanAddAt(item, randomIndex);
            
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemAt_SlotWithSameItemsAndNotEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);

            var canAdd = inventory.CanAddAt(item, randomIndex);

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
            var canAdd = inventory.CanAddAt(randomItem, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex);

            var canAdd = inventory.CanAddAt(item, randomIndex);

            Assert.That(canAdd, Is.False);
        }
    }
}

using NUnit.Framework.Internal;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.CanAddAt(item: default!, 0), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(22)]
        public void CanAddAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.CanAddAt(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void CanAddAt_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAddAt(item, 0);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddAt_FullSlotWithSameItem_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var randomItem = this.itemFactory.CreateDefault();
            inventory.AddAt(randomItem, randomIndex);

            var canAdd = inventory.CanAddAt(randomItem, randomIndex);
            
            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex);

            var canAdd = inventory.CanAddAt(item, randomIndex);

            Assert.That(canAdd, Is.False);
        }
    }
}

using NUnit.Framework.Internal;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(
                () => inventory.CanAddAt(item: default!, 0), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanAddAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.CanAddAt(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanAddAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            var canAdd = inventory.CanAddAt(item, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_SlotWithDifferentItem_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var randomItem = this.itemFactory.CreateRandom();
            inventory.AddAt(randomItem, randomIndex);

            var item = this.itemFactory.CreateDefault();
            var canAdd = inventory.CanAddAt(item, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_SlotWithSameItem_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var randomItem = this.itemFactory.CreateDefault();
            inventory.AddAt(randomItem, randomIndex);

            var canAdd = inventory.CanAddAt(randomItem, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddAt_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAddAt(item, 0);

            Assert.That(canAdd, Is.True);
        }
    }
}

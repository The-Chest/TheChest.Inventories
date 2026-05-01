using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(
                () => inventory.CanAdd(item: default!),
                Throws.ArgumentNullException
                    .With.Message.EqualTo("Value cannot be null. (Parameter 'item')")
            );
        }

        [Test]
        public void CanAddItem_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_PartiallyFullInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex);

            var canAdd = inventory.CanAdd(item);
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.False);
        }
    }
}

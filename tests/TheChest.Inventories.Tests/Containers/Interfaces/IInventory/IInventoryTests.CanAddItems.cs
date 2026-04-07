using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        [Ignore("This test is not valid.")]
        public void CanAddItems_EmptyItemsArray_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var items = Array.Empty<T>();

            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(10);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItems_FullInventoryValueType_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }
    }
}

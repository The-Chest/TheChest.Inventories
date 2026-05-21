using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            Assert.That(() => inventory.GetCount(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetCount_DoesNotRemoveItems()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            inventory.GetCount(items[0]);

            Assert.That(inventory.GetItem(0), Is.EqualTo(items[0]));
        }

        [Test]
        public void GetCount_NoItems_ReturnsZero()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var count = inventory.GetCount(this.itemFactory.CreateRandom());

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void GetCount_ReturnsItemCount()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var count = inventory.GetCount(items[0]);

            Assert.That(count, Is.EqualTo(size));
        }
    }
}

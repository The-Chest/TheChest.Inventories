using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.GetCount(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetCount_DoesNotRemoveItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            inventory.GetCount(items[0]);

            Assert.That(inventory.GetItem<T>(0), Is.EqualTo(items[0]));
        }
    }
}

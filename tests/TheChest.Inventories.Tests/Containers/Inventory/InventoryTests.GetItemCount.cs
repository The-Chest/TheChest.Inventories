using TheChest.Tests.Common.Extensions.Containers;

using TheChest.Tests.Common.Attributes;
namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
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

            Assert.That(inventory.GetItem(0), Is.EqualTo(items[0]));
        }
    }
}

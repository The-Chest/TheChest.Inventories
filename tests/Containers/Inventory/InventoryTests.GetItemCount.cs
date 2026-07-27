using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            Assert.That(
                () => inventory.GetCount(default!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetCount_DefaultValue_EmptyContainer_ReturnsZero()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var result = inventory.GetCount(default!);

            Assert.That(result, Is.Zero);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetCount_DefaultValue_FullContainer_ReturnsItemsAmount()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, default!);

            var result = inventory.GetCount(default!);

            Assert.That(result, Is.EqualTo(size));
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

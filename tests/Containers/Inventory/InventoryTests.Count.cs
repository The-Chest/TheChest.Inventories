using NUnit.Framework;
using System.Linq;
using System;
using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Count_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            Assert.That(
                () => inventory.Count(default!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Count_DefaultValue_EmptyContainer_ReturnsZero()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var result = inventory.Count(default!);

            Assert.That(result, Is.Zero);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Count_DefaultValue_FullContainer_ReturnsItemsAmount()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, default!);

            var result = inventory.Count(default!);

            Assert.That(result, Is.EqualTo(size));
        }

        [Test]
        public void Count_DoesNotRemoveItems()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            inventory.Count(items[0]);

            Assert.That(inventory.GetItem(0), Is.EqualTo(items[0]));
        }

        [Test]
        public void Count_NoItems_ReturnsZero()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var count = inventory.Count(this.itemFactory.CreateRandom());

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void Count_ReturnsItemCount()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var count = inventory.Count(items[0]);

            Assert.That(count, Is.EqualTo(size));
        }
    }
}

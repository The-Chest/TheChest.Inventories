using NUnit.Framework;
using System.Linq;
using System;
namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Count_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.Count(default!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void Count_NotFoundItem_ReturnsZero()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var wrongItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, wrongItem);

            var item = this.itemFactory.CreateDefault();
            var count = inventory.Count(item);

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void Count_ExistingItem_ReturnsItemCount()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var count = size * stackSize;
            var result = inventory.Count(item);

            Assert.That(result, Is.EqualTo(count));
        }
    }
}

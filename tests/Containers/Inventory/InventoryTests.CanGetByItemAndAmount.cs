using System;
using NUnit.Framework;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void CanGetByItemAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.CanGet(item, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void CanGetByItemAndAmount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());

            Assert.That(
                () => inventory.CanGet(default!, 1),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void CanGetByItemAndAmount_NoMatchingItems_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateDefault();

            var result = inventory.CanGet(item, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByItemAndAmount_NotEnoughMatchingItems_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var result = inventory.CanGet(item, size + 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByItemAndAmount_EnoughMatchingItems_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, size);
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var result = inventory.CanGet(item, amount);

            Assert.That(result, Is.True);
        }
    }
}

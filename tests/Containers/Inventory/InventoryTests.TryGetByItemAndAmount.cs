using System;
using System.Linq;
using NUnit.Framework;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void TryGetByItemAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.TryGet(item, amount, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void TryGetByItemAndAmount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());

            Assert.That(
                () => inventory.TryGet(default!, 1, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void TryGetByItemAndAmount_NoMatchingItems_ReturnsFalseAndEmptyArray()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateDefault();

            var result = inventory.TryGet(item, 1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
            });
        }

        [Test]
        public void TryGetByItemAndAmount_NotEnoughMatchingItems_ReturnsFalseAndAvailableItems()
        {
            var size = this.GenerateRandomSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, expectedItem);

            var result = inventory.TryGet(expectedItem, size + 1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.EqualTo(Enumerable.Repeat(expectedItem, size)));
            });
        }

        [Test]
        public void TryGetByItemAndAmount_EnoughMatchingItems_ReturnsTrueAndRequestedItems()
        {
            var size = this.GenerateRandomSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var expectedAmount = this.random.Next(1, size);
            var inventory = this.inventoryFactory.FullContainer(size, expectedItem);

            var result = inventory.TryGet(expectedItem, expectedAmount, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(items, Is.EqualTo(Enumerable.Repeat(expectedItem, expectedAmount)));
            });
        }

        [Test]
        public void TryGetByItemAndAmount_MatchingItems_CallsOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, expectedItem);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(expectedItem, 1, out _);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TryGetByItemAndAmount_NoMatchingItems_DoesNotCallOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateDefault();
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(item, 1, out _);

            Assert.That(called, Is.False);
        }
    }
}

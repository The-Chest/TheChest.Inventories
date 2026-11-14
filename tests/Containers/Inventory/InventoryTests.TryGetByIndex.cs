using System;
using NUnit.Framework;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryGetByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryGet(index, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryGetByIndex_EmptySlot_ReturnsFalseAndDefaultItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, out var item);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(item, Is.EqualTo(default(T)));
            });
        }

        [Test]
        public void TryGetByIndex_EmptySlot_DoesNotCallOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var index = this.random.Next(0, size);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(index, out _);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TryGetByIndex_FullSlot_ReturnsTrueAndItem()
        {
            var size = this.GenerateRandomSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, expectedItem);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, out var item);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(item, Is.EqualTo(expectedItem));
            });
        }

        [Test]
        public void TryGetByIndex_FullSlot_RemovesItemFromSlot()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);

            inventory.TryGet(index, out _);

            Assert.That(inventory.GetSlot<T>(index).IsEmpty, Is.True);
        }

        [Test]
        public void TryGetByIndex_FullSlot_CallsOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(index, out _);

            Assert.That(called, Is.True);
        }
    }
}

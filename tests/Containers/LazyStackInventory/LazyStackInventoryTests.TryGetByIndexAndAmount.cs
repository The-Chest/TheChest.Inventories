using NUnit.Framework;
using System;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryGetByIndexAndAmount_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet(index, 1, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TryGetByIndexAndAmount_NonPositiveAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.TryGet(index, amount, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryGetByIndexAndAmount_EmptySlot_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, 1, out var items);

            Assert.That(result, Is.False);
            Assert.That(items, Is.Empty);
        }

        [Test]
        public void TryGetByIndexAndAmount_AmountExceedsAvailableItems_ReturnsFalseAndAvailableItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, stackSize + 1, out var items);

            Assert.That(result, Is.False);
            Assert.That(items, Has.Length.EqualTo(stackSize).And.All.EqualTo(item));
        }

        [Test]
        public void TryGetByIndexAndAmount_EnoughItems_ReturnsRequestedItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            var result = inventory.TryGet(index, amount, out var items);

            Assert.That(result, Is.True);
            Assert.That(items, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }
    }
}

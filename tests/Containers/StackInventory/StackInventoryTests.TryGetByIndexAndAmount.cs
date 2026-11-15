using System;
using NUnit.Framework;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryGetByIndexAndAmount_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet(index, 1, out _),
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TryGetByIndexAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet(this.random.Next(0, size), amount, out _),
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryGetByIndexAndAmount_NotEnoughItems_ReturnsFalseAndDoesNotChangeSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateRandom());
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, stackSize + 1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
                Assert.That(inventory.GetSlot(index)!.Amount, Is.EqualTo(stackSize));
            });
        }

        [Test]
        public void TryGetByIndexAndAmount_NotEnoughItems_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateRandom());
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(this.random.Next(0, size), stackSize + 1, out _);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TryGetByIndexAndAmount_EnoughItems_ReturnsRequestedItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            var result = inventory.TryGet(index, amount, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(items, Has.Length.EqualTo(amount));
                Assert.That(items, Has.All.EqualTo(item));
                Assert.That(inventory.GetSlot(index)!.Amount, Is.EqualTo(stackSize - amount));
            });
        }
    }
}

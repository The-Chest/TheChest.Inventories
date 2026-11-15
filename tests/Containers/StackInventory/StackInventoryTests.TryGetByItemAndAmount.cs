using System;
using NUnit.Framework;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void TryGetByItemAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.TryGet(item, amount, out _),
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void TryGetByItemAndAmount_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet((T)default!, 1, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void TryGetByItemAndAmount_NoMatchingItems_ReturnsFalseAndEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var result = inventory.TryGet(item, 1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
            });
        }

        [Test]
        public void TryGetByItemAndAmount_NotEnoughItems_ReturnsFalseWithoutRemovingItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var currentAmount = inventory.Count(item);

            var result = inventory.TryGet(item, currentAmount + 1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
                Assert.That(inventory.Count(item), Is.EqualTo(currentAmount));
            });
        }

        [Test]
        public void TryGetByItemAndAmount_NotEnoughItems_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(item, size * stackSize + 1, out _);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TryGetByItemAndAmount_EnoughItemsAcrossSlots_ReturnsRequestedItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var amount = stackSize + 1;

            var result = inventory.TryGet(item, amount, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(items, Has.Length.EqualTo(amount));
                Assert.That(items, Has.All.EqualTo(item));
                Assert.That(inventory.Count(item), Is.EqualTo(size * stackSize - amount));
            });
        }

        [Test]
        public void TryGetByItemAndAmount_EnoughItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(item, 1, out _);

            Assert.That(called, Is.True);
        }
    }
}

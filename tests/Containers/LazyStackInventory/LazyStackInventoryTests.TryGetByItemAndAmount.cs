using NUnit.Framework;
using System;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void TryGetByItemAndAmount_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet((T)default!, 1, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TryGetByItemAndAmount_NonPositiveAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.TryGet(item, amount, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryGetByItemAndAmount_ItemNotFound_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var existingItem = this.itemFactory.CreateRandom();
            var searchedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, existingItem);

            var result = inventory.TryGet(searchedItem, 1, out var items);

            Assert.That(result, Is.False);
            Assert.That(items, Is.Empty);
        }

        [Test]
        public void TryGetByItemAndAmount_AmountExceedsAvailableItems_ReturnsFalseAndAvailableItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var amount = size * stackSize + 1;

            var result = inventory.TryGet(item, amount, out var items);

            Assert.That(result, Is.False);
            Assert.That(items, Has.Length.EqualTo(size * stackSize).And.All.EqualTo(item));
        }

        [Test]
        public void TryGetByItemAndAmount_EnoughItemsAcrossSlots_ReturnsRequestedItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var amount = stackSize + 1;

            var result = inventory.TryGet(item, amount, out var items);

            Assert.That(result, Is.True);
            Assert.That(items, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }
    }
}

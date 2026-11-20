using NUnit.Framework;
using System;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryGetByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet(index, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryGetByIndex_EmptySlot_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, out var items);

            Assert.That(result, Is.False);
            Assert.That(items, Is.Empty);
        }

        [Test]
        public void TryGetByIndex_SlotWithItems_ReturnsAllItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, out var items);

            Assert.That(result, Is.True);
            Assert.That(items, Has.Length.EqualTo(stackSize).And.All.EqualTo(item));
        }
    }
}

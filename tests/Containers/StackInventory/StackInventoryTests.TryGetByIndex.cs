using System;
using NUnit.Framework;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryGetByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryGet(index, out _),
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryGetByIndex_EmptySlot_ReturnsFalseAndEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
            });
        }

        [Test]
        public void TryGetByIndex_EmptySlot_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(this.random.Next(0, size), out _);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TryGetByIndex_SlotWithItems_ReturnsAllItemsAndEmptiesSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var index = this.random.Next(0, size);

            var result = inventory.TryGet(index, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(item));
                Assert.That(inventory.GetSlot(index)!.IsEmpty, Is.True);
            });
        }

        [Test]
        public void TryGetByIndex_SlotWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateRandom());
            var index = this.random.Next(0, size);
            var called = false;
            inventory.OnGet += (_, _) => called = true;

            inventory.TryGet(index, out _);

            Assert.That(called, Is.True);
        }
    }
}

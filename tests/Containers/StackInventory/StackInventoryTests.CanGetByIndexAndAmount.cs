using System;
using NUnit.Framework;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanGetByIndexAndAmount_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanGet(index, 1),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanGetByIndexAndAmount_NonPositiveAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanGet(0, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void CanGetByIndexAndAmount_EmptySlot_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByIndexAndAmount_EmptySlot_DefaultAmount_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByIndexAndAmount_InsufficientItems_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index, stackSize + 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByIndexAndAmount_SufficientItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            var result = inventory.CanGet(index, amount);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanGetByIndexAndAmount_FullSlot_DefaultAmount_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index);

            Assert.That(result, Is.True);
        }
    }
}

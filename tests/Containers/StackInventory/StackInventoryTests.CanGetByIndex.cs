using System;
using NUnit.Framework;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanGetByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanGet(index),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanGetByIndex_EmptySlot_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByIndex_FullSlot_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index);

            Assert.That(result, Is.True);
        }
    }
}

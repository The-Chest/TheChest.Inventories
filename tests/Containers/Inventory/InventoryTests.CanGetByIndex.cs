using System;
using NUnit.Framework;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanGetByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.CanGet(index),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanGetByIndex_EmptySlot_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGetByIndex_FullSlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, this.itemFactory.CreateDefault());
            var index = this.random.Next(0, size);

            var result = inventory.CanGet(index);

            Assert.That(result, Is.True);
        }
    }
}

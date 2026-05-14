using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1, 1)]
        [TestCase(MAX_SIZE_TEST + 1, 1)]
        public void CanReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.CanReplace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("index")
            );
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        public void CanReplace_InvalidAmount_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.CanReplace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("amount")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void CanReplace_NullItem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size - 1);
            var randomAmount = this.random.Next(1, stackSize);
            Assert.That(
                () => inventory.CanReplace(default!, randomIndex, randomAmount),
                Throws.TypeOf<ArgumentNullException>().And.With.Message.Contains("item")
            );
        }

        [Test]
        public void CanReplace_MoreItemsThanStackSize_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);

            var canReplace = inventory.CanReplace(newItem, index, stackSize + 1);

            Assert.That(canReplace, Is.False);
        }

        [Test]
        public void CanReplace_SlotWithItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var canReplace = inventory.CanReplace(newItem, index, stackSize);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var canReplace = inventory.CanReplace(newItem, index, stackSize);

            Assert.That(canReplace, Is.True);
        }
    }
}

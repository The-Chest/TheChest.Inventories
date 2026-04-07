using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void CanAddAt_NullItem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size - 1);
            Assert.That(() => inventory.CanAddAt(item: default!, randomIndex, amount: 1), Throws.ArgumentNullException);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanAddAt_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size - 1);
            Assert.That(
                () => inventory.CanAddAt(item, randomIndex, amount), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void CanAddAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.CanAddAt(item, index, 1), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }
    }
}

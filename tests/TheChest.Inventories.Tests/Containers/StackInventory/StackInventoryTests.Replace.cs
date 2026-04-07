using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void CanReplace_NullItems_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);

            Assert.That(() => inventory.CanReplace(null!, randomIndex), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfValueTypeAttribute]
        public void CanReplace_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var index = this.random.Next(0, size);
            var items = this.itemFactory
                .CreateManyRandom(stackSize)
                .Append(default)
                .ToArray();
            
            Assert.That(() => inventory.CanReplace(items!, index), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void CanReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.CanReplace(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CanReplace_MoreItemsThanStackSize_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var index = this.random.Next(0, size);
            Assert.That(() =>
                inventory.Replace(items, index),
                Throws.InvalidOperationException
                    .With.Message.EqualTo("The amount of items to replace exceeds the stack size of the slot.")
            );
        }
    }
}

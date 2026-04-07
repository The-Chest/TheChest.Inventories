namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void CanAddItemsAt_NullItems_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.CanAddAt(items: default!, 0), Throws.ArgumentNullException);
        }

        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void CanAddItemsAt_ArrayContainingNullItem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize - 1).ToList();
            items.Add(default!);

            Assert.That(() => inventory.CanAddAt(items.ToArray(), 0), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(MAX_STACK_SIZE_TEST + 1)]
        public void CanAddItemsAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.CanAddAt(this.itemFactory.CreateMany(stackSize), index), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void Replace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(20);
            Assert.That(() => inventory.Replace(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Replace_EmptyItems_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            Assert.That(() => inventory.Replace(Array.Empty<T>(), randomIndex), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Replace_MoreItemsThanStackSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize + 1);

            Assert.That(() =>
                inventory.Replace(items, randomIndex),
                Throws.InvalidOperationException
                    .With.Message.EqualTo("The amount of items to replace exceeds the stack size of the slot.")
            );
        }

        [Test]
        public void Replace_NullItems_DoesNotReplace()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            Assert.That(() => inventory.Replace(null!, randomIndex), Throws.TypeOf<ArgumentNullException>());
        }
    }
}

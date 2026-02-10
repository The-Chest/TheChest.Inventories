namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void CanReplace_NullItems_ThrowsArgumentNullException()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);

            Assert.That(() => inventory.CanReplace(null!, randomIndex), Throws.ArgumentNullException);
        }

        [Test]
        public void CanReplace_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

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
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.CanReplace(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CanReplace_EmptyItems_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var canReplace = inventory.CanReplace(Array.Empty<T>(), randomIndex);
            
            Assert.That(canReplace, Is.False);
        }

        [Test]
        public void CanReplace_MoreItemsThanStackSize_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var index = this.random.Next(0, size);
            Assert.That(() =>
                inventory.Replace(items, index),
                Throws.InvalidOperationException
                    .With.Message.EqualTo("The amount of items to replace exceeds the stack size of the slot.")
            );
        }

        [Test]
        public void CanReplace_SlotWithItems_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var canReplace = inventory.CanReplace(newItems, randomIndex);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var canReplace = inventory.CanReplace(newItems, randomIndex);

            Assert.That(canReplace, Is.True);
        }
    }
}

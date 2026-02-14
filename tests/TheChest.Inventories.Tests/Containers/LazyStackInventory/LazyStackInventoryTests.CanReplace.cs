namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1, 1)]
        [TestCase(2000, 1)]
        public void CanReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.CanReplace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("index")
            );
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        public void CanReplace_InvalidAmount_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.CanReplace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("amount")
            );
        }

        [Test]
        public void CanReplace_InvalidAmount_ThrowsArgumentOutOfRangeException()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.CanReplace(default! , 1, 1),
                Throws.TypeOf<ArgumentNullException>().And.With.Message.Contains("item")
            );
        }

        [Test]
        public void CanReplace_MoreItemsThanStackSize_ReturnsFalse()
        {
            var oldItem = this.itemFactory.CreateDefault();
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);

            var canReplace = inventory.CanReplace(newItem, index, stackSize + 1);

            Assert.That(canReplace, Is.False);
        }

        [Test]
        public void CanReplace_SlotWithItems_ReturnsTrue()
        {
            var oldItem = this.itemFactory.CreateDefault();
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var canReplace = inventory.CanReplace(newItem, index, stackSize);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var canReplace = inventory.CanReplace(newItem, index, stackSize);

            Assert.That(canReplace, Is.True);
        }
    }
}

namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void GetAmountFrom_InvalidIndex_ThrowsIndexOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(index, 10), Throws.InstanceOf<IndexOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetAmountFrom_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(0, amount), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAmountFrom_EmptySlot_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = inventory.Get(0, 10);
            Assert.That(item, Is.Empty);
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_ReturnsItems()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(10, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var items = inventory.Get(index, 10);

            Assert.That(items, Is.Not.Empty);
            Assert.That(items.Count, Is.EqualTo(10));
            Assert.That(items, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_RemovesItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(10, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var removeAmount = this.random.Next(1, stackSize);
            inventory.Get(index, removeAmount);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize - removeAmount));
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_ReturnsTheMaximumAmountPossible()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 10);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var items = inventory.Get(index, 100);

            Assert.That(items.Count, Is.EqualTo(stackSize));
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_RemovesAllItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 10);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.Get(index, 20);

            Assert.That(inventory[index].StackAmount, Is.Zero);
        }
    }
}

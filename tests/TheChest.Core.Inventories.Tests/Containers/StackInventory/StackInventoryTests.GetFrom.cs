namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void GetFrom_InvalidIndex_ThrowsIndexOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(index), Throws.InstanceOf<IndexOutOfRangeException>());
        }

        [Test]
        public void GetFrom_EmptySlot_ReturnsNull()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = inventory.Get(0);
            Assert.That(item, Is.Null);
        }

        [Test]
        public void GetFrom_SlotWithItems_ReturnsItem()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var item = inventory.Get(index);

            Assert.That(item, Is.EqualTo(slotItem));
        }

        [Test]
        public void GetFrom_SlotWithItems_RemovesItemFromSlot()
        {
            var index = this.random.Next(0, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.Get(index);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize - 1));
        }
    }
}

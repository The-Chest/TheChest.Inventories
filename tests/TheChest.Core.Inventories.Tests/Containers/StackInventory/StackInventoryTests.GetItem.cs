namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetItem_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.Get(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItem_EmptySlot_ReturnsNull()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer();

            var foundItem = inventory.Get(item);
            
            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public void GetItem_SlotWithItems_ReturnsItem()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var item = inventory.Get(slotItem);

            Assert.That(item, Is.EqualTo(slotItem));
        }

        [Test]
        public void GetItem_SlotWithItems_RemovesItemFromFirstFoundSlot()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            inventory.Get(slotItem);

            Assert.That(inventory[0].StackAmount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void GetItem_SlotWithDifferentItems_ReturnsNull()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }
    }
}

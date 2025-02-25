namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void GetAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(item, amount), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAmount_InvalidItem_ThrowsIndexOutOfRangeException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(default(T)!, 10), Throws.InstanceOf<IndexOutOfRangeException>());
        }

        [Test]
        public void GetAmount_EmptyInventory_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.EmptyContainer();
            var amount = inventory.Get(item, 10);
            Assert.That(amount, Is.Empty);
        }

        [Test]
        public void GetAmount_InventoryWithItems_ReturnsSearchedItems()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);

            var items = inventory.Get(slotItems[0], stackSize);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Is.EqualTo(slotItems));
            });
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromSlot()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);

            inventory.Get(stackSize, stackSize);

            Assert.That(inventory[0].IsEmpty, Is.True);
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromMultipleSlotInOrder()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);

            inventory.Get(stackSize, stackSize + (stackSize - 2));
            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].StackAmount, Is.EqualTo(0));
                Assert.That(inventory[1].StackAmount, Is.EqualTo(2));
            });
        }
    }
}

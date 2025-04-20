namespace TheChest.Inventories.Tests.Containers
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
        public void GetAmount_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Get(default(T)!, 10), Throws.InstanceOf<ArgumentNullException>());
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
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(inventorySize / 2)
                .Append(item)
                .Append(item)
                .ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);

            var items = inventory.Get(item, stackSize);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(item));
            });
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromSlot()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);

            inventory.Get(slotItem, stackSize);

            Assert.That(inventory[0].IsEmpty, Is.True);
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromMultipleSlotsInOrder()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(2, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);

            inventory.Get(slotItem, stackSize + (stackSize - 2));
            
            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].StackAmount, Is.EqualTo(0));
                Assert.That(inventory[1].StackAmount, Is.EqualTo(2));
            });
        }
    }
}

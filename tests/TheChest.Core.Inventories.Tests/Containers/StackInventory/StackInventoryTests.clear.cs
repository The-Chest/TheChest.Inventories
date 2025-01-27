namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_InventoryWithItems_ReturnsAllItems()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var items = this.itemFactory.CreateManyRandom(inventorySize * stackSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, items);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void Clear_InventoryWithItems_RemoveItemsFromInventory()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var items = this.itemFactory.CreateManyRandom(inventorySize * stackSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, items);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }

        [Test]
        public void Clear_FullInventory_ReturnsAllItems()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, item);

            var expectedArraySize = inventorySize * stackSize;
            var expectedArray = new T[expectedArraySize];
            Array.Fill(expectedArray, item);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(expectedArray));
        }

        [Test]
        public void Clear_FullInventory_RemoveItemsFromInventory()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, item);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }
    }
}

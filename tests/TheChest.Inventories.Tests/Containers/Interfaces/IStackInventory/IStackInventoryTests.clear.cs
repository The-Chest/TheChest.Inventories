namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_InventoryWithItems_ReturnsAllItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size * stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void Clear_FullInventory_ReturnsAllItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var expectedArraySize = size * stackSize;
            var expectedArray = new T[expectedArraySize];
            Array.Fill(expectedArray, item);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(expectedArray));
        }
    }
}

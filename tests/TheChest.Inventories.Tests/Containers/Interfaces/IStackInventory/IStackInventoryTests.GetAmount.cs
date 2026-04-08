namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void GetAmount_EmptyInventory_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.EmptyContainer();
            var amount = inventory.Get(item, 10);
            Assert.That(amount, Is.Empty);
        }

        [Test]
        public void GetAmount_InventoryWithItems_ReturnsSearchedItems()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(inventorySize / 2)
                .Append(item)
                .Append(item)
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);

            var items = inventory.Get(item, stackSize);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(item));
            });
        }
    }
}

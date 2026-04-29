namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void GetAllItems_EmptyInventory_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.EmptyContainer();

            var items = inventory.GetAll(item);

            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetAllItems_InventoryWithItems_ReturnSearchedItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size / 2)
                .Append(item)
                .Append(item)
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var items = inventory.GetAll(item);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize * 2));
                Assert.That(items, Has.All.EqualTo(item));
            });
        }
    }
}

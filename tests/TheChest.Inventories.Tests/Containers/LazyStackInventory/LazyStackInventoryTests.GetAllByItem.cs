namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void GetAll_ByItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.GetAll(item: default!));
        }

        [Test]
        public void GetAll_ByItem_ExistingItem_ReturnsAllMatchingItems()
        {
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventoryRandomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, inventoryItem, inventoryRandomItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.GetAll(item);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void GetAll_ByItem_ExistingItem_ReMovesAllMatchingItemsFromInventory()
        {
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventoryRandomItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, inventoryItem, inventoryRandomItem);

            var item = this.itemFactory.CreateDefault();
            inventory.GetAll(item);

            Assert.That(inventory.Slots, Has.None.EqualTo(item));
        }
    }
}

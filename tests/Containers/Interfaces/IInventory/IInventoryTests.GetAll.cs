namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void GetAll_FoundItems_ReturnsSearchingItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size / 2);
            var sameItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items.Concat(sameItems).ToArray());

            var randomItem = sameItems[0];
            var result = inventory.GetAll(randomItem);

            Assert.That(result, Is.EqualTo(sameItems));
        }

        [Test]
        public void GetAll_NotFoundItemm_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var randomItem = this.itemFactory.CreateRandom();
            var result = inventory.GetAll(randomItem);

            Assert.That(result, Is.Empty);
        }
    }
}

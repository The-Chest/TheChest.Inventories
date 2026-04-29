namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void GetCount_ReturnsItemCount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var count = inventory.GetCount(items[0]);

            Assert.That(count, Is.EqualTo(size));
        }

        [Test]
        public void GetCount_NoItems_ReturnsZero()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var count = inventory.GetCount(this.itemFactory.CreateRandom());

            Assert.That(count, Is.Zero);
        }
    }
}

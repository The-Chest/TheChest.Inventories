namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void AddItems_NoItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(0);

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_SuccessAdding_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_FullInventory_DoesNotAddItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateManyRandom(size);
            inventory.Add(items);

            Assert.That(inventory.GetCount(items[0]), Is.EqualTo(0));
        }
    }
}

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_FullInventory_ReturnsEveryItemFromInventory()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size / 2)
                .Concat(this.itemFactory.CreateMany(size / 2))
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(items));
        }
    }
}

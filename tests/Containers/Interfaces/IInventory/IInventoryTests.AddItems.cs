namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
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
    }
}
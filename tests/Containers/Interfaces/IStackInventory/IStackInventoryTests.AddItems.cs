namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItems_EmptyInventory_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }
    }
}

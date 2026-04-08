namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItems_EmptyInventory_ReturnsEmptyArray()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_FullInventory_ReturnsNotAddedItems()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.inventoryFactory.FullContainer(20, 2, slotItem);

            var result = inventory.Add(items);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}

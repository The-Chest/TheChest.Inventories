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
        public void AddItems_FullInventory_ThrowsInvalidOperationException()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.inventoryFactory.FullContainer(20, 2, slotItem);

            Assert.That(() => inventory.Add(items), Throws.InvalidOperationException);
        }
    }
}

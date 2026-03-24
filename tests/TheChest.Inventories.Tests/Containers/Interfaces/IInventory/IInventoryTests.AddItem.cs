namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void AddItem_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItem_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, items);

            var item = this.itemFactory.CreateDefault();

            var result = inventory.Add(item);

            Assert.That(result, Is.False);
        }
    }
}

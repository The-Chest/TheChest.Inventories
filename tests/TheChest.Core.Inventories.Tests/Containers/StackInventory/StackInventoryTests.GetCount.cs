namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetCount_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.GetCount(default!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetCount_EmptyInventory_ReturnsZero()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var count = inventory.GetCount(item);

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void GetCount_InventoryWithItems_ReturnsItemCount()
        {
            var amount = 10;
            var stackSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateMany(amount);
            var inventoryItems = this.itemFactory.CreateManyRandom(10).ToList();
            inventoryItems.AddRange(items);

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());

            var count = inventory.GetCount(items[0]);

            Assert.That(count, Is.EqualTo(amount));
        }
    }
}

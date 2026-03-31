namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void GetCount_EmptyInventory_ReturnsZero()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var count = inventory.GetCount(item);

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void GetCount_InventoryWithItems_ReturnsItemCount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(10)
                .Append(item)
                .Append(item)
                .ToList();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            var count = inventory.GetCount(item);

            Assert.That(count, Is.EqualTo(stackSize * 2));
        }
    }
}

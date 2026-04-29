namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_InventoryWithItems_ReturnsAllItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var result = inventory.Clear();

            Assert.That(result, Has.Length.EqualTo(size * stackSize).And.All.EqualTo(item));
        }
    }
}

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void CanAdd_EmptyInventory_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAdd_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);
            
            Assert.That(canAdd, Is.False);
        }
    }
}

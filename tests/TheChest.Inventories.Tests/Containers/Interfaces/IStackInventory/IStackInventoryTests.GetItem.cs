namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void GetItem_EmptyInventory_ReturnsNull()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var foundItem = inventory.Get(item);
            
            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public void GetItem_InventoryWithItems_ReturnsItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = inventory.Get(slotItem);

            Assert.That(item, Is.EqualTo(slotItem));
        }

        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void GetItem_InventoryWithDifferentItems_ReturnsNull()
        {
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(20, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }
    }
}

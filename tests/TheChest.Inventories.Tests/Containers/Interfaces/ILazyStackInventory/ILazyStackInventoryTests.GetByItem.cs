namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByItem_ExistingItem_ReturnsItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var expectedItem = this.itemFactory.CreateDefault();
            var result = inventory.Get(expectedItem);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public void Get_ByItem_NotFoundItem_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var item = this.itemFactory.CreateRandom();
            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }
    }
}

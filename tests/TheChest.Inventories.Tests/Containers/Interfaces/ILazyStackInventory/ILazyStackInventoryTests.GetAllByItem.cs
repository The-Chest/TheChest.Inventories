namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void GetAllByItem_ExistingItem_ReturnsAllMatchingItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventoryRandomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItem, inventoryRandomItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.GetAll(item);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void GetAllByItem_DifferentItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var invalidRandomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, invalidRandomItems);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.GetAll(item);

            Assert.That(result, Is.Empty);
        }
    }
}

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void GetAll_ByIndex_ExistingIndex_ReturnsAllItemsFromSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size);
            var result = inventory.GetAll(index);

            Assert.That(result, Is.Not.Empty.And.Length.EqualTo(stackSize).And.All.EqualTo(expectedItem));
        }

        [Test]
        public void GetAll_ByIndex_NotExistingIndex_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            var result = inventory.GetAll(index);

            Assert.That(result, Is.Empty);
        }
    }
}

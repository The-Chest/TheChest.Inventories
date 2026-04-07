namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByIndex_ValidIndex_ReturnsItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var randomIndex = this.random.Next(0, size - 1);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void Get_ByIndex_EmptySlot_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size - 1);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.Null);
        }
    }
}
